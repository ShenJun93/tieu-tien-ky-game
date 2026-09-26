using System.Collections.Generic;
using System.IO;
using System.Linq;
using TieuTienKy.Brawl;
using TieuTienKy.Combat;
using TieuTienKy.EditorTools.Look;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;

namespace TieuTienKy.EditorTools.Brawl
{
    /// <summary>
    /// CORE-1a: builds the playable brawl scene. It has the LOOK-1 arena, a KayKit cast with sect
    /// palettes and locomotion controllers, a BrawlMatch driving the pure CombatSim, a Cinemachine
    /// follow camera at the teardown framing, and the HUD with a floating OnScreenStick.
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Brawl.BrawlArenaBuilder.Build -quit
    /// </summary>
    public static class BrawlArenaBuilder
    {
        public const string ScenePath = "Assets/_Project/Scenes/Brawl/Arena_Brawl_01.unity";
        const string CastRoot = "Assets/ThirdParty/KayKit/Adventurers";
        const string LookMaterials = "Assets/_Project/Materials/Look";
        const string ControllerFolder = "Assets/_Project/Animation/Brawl";
        static readonly string[] LoopClips = { "Idle", "Running_A" };

        static readonly (string cast, Team team, Vector2 spawn)[] Roster =
        {
            ("Knight", Team.Jade, new Vector2(0f, -6f)),       // local player
            ("Mage", Team.Jade, new Vector2(-5f, -8f)),
            ("Barbarian", Team.Crimson, new Vector2(8f, 4f)),
            ("RogueHooded", Team.Crimson, new Vector2(12f, -1f)),
            ("Rogue", Team.Azure, new Vector2(-10f, 5f)),
        };

        [MenuItem("Tieu Tien Ky/Brawl/Build Brawl Arena Scene")]
        public static void Build()
        {
            ShaderUtil.allowAsyncCompilation = false;
            EnsureLoopingClips();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ArenaLabBuilder.BuildEnvironment(out Rect playArea);

            var entries = new List<BrawlMatch.Entry>();
            var castRoot = new GameObject("Fighters").transform;
            foreach (var (cast, team, spawn) in Roster)
                entries.Add(new BrawlMatch.Entry { View = SpawnFighter(castRoot, cast, team, spawn), Team = team });

            var hero = entries[0].View;
            var ring = (GameObject)PrefabUtility.InstantiatePrefab(FeedbackVfxLibrary.HeroRing, hero.transform);
            ring.transform.localScale = Vector3.one * 2.6f / hero.transform.lossyScale.x;

            var match = new GameObject("BrawlMatch").AddComponent<BrawlMatch>();
            match.Configure(entries.ToArray(), 0, playArea);

            float heroHeight = HeightOf(hero.gameObject);
            var mainCamera = BuildCameras(hero.transform, heroHeight);
            BuildHud(mainCamera);

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);
            Capture(mainCamera, Path.Combine(Directory.GetCurrentDirectory(), "Logs", "look", "brawl-start.png"));
            Debug.Log($"[BrawlArena] {entries.Count} fighters, play area {playArea}, hero height {heroHeight:F2}");
        }

        static FighterView SpawnFighter(Transform parent, string cast, Team team, Vector2 spawn)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>($"{CastRoot}/Characters/{cast}.fbx");
            var go = (GameObject)PrefabUtility.InstantiatePrefab(model, parent);
            go.name = $"{cast}_{team}";
            go.transform.SetPositionAndRotation(new Vector3(spawn.x, 0f, spawn.y), Quaternion.Euler(0f, 180f, 0f));
            SectLook.ApplyLoadout(go, cast);
            var template = AssetDatabase.LoadAssetAtPath<Material>($"{LookMaterials}/Toon_KayKit_{cast}.mat");
            var material = SectLook.SectMaterial(cast, (SectLook.Sect)(int)team, template);
            foreach (var r in go.GetComponentsInChildren<Renderer>(true))
                r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();

            var animator = go.GetComponent<Animator>();
            if (animator == null) animator = go.AddComponent<Animator>(); // Unity fake-null: no ?? here
            animator.applyRootMotion = false;
            animator.runtimeAnimatorController = LocomotionController(cast);
            var view = go.AddComponent<FighterView>();
            view.Bind(animator);
            return view;
        }

        /// <summary>Idle ↔ Running_A driven by the Speed parameter, one controller per KayKit character.</summary>
        static AnimatorController LocomotionController(string cast)
        {
            string path = $"{ControllerFolder}/{cast}_Locomotion.controller";
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (existing != null) return existing;
            if (!AssetDatabase.IsValidFolder("Assets/_Project/Animation")) AssetDatabase.CreateFolder("Assets/_Project", "Animation");
            if (!AssetDatabase.IsValidFolder(ControllerFolder)) AssetDatabase.CreateFolder("Assets/_Project/Animation", "Brawl");

            var clips = AssetDatabase.LoadAllAssetsAtPath($"{CastRoot}/Characters/{cast}.fbx").OfType<AnimationClip>().ToList();
            var idle = clips.First(c => c.name == "Idle");
            var run = clips.First(c => c.name == "Running_A");

            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            var state = controller.CreateBlendTreeInController("Locomotion", out BlendTree tree);
            tree.blendParameter = "Speed";
            tree.useAutomaticThresholds = false;
            tree.AddChild(idle, 0f);
            tree.AddChild(run, 1f);
            controller.layers[0].stateMachine.defaultState = state;
            return controller;
        }

        /// <summary>KayKit clips import as play-once; locomotion clips must loop (edited through ModelImporter).</summary>
        static void EnsureLoopingClips()
        {
            foreach (var (cast, _, _) in Roster)
            {
                string path = $"{CastRoot}/Characters/{cast}.fbx";
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                var clips = importer.clipAnimations.Length > 0 ? importer.clipAnimations : importer.defaultClipAnimations;
                bool changed = false;
                foreach (var clip in clips)
                {
                    if (!LoopClips.Contains(clip.name) || clip.loopTime) continue;
                    clip.loopTime = true;
                    changed = true;
                }
                if (!changed) continue;
                importer.clipAnimations = clips;
                importer.SaveAndReimport();
            }
        }

        static Camera BuildCameras(Transform hero, float heroHeight)
        {
            // Teardown 3.1: pitch 55, vertical FOV 32, hero at ~10% of screen height.
            float distance = heroHeight / (0.10f * 2f * Mathf.Tan(16f * Mathf.Deg2Rad));
            var rotation = Quaternion.Euler(55f, 0f, 0f);
            Vector3 offset = -(rotation * Vector3.forward) * distance + Vector3.up * heroHeight * 0.5f;

            var camera = new GameObject("Main Camera").AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.gameObject.AddComponent<AudioListener>();
            camera.gameObject.AddComponent<CinemachineBrain>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.043f, 0.051f, 0.07f);
            camera.fieldOfView = 32f;
            camera.farClipPlane = 300f;
            camera.transform.SetPositionAndRotation(hero.position + offset, rotation);
            var data = camera.GetUniversalAdditionalCameraData();
            data.renderPostProcessing = true;
            data.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

            var cm = new GameObject("GameplayCamera").AddComponent<CinemachineCamera>();
            cm.transform.SetPositionAndRotation(hero.position + offset, rotation);
            cm.Target.TrackingTarget = hero;
            var lens = cm.Lens;
            lens.FieldOfView = 32f;
            lens.FarClipPlane = 300f;
            cm.Lens = lens;
            var follow = cm.gameObject.AddComponent<CinemachineFollow>();
            follow.FollowOffset = offset;
            var tracker = follow.TrackerSettings;
            tracker.BindingMode = BindingMode.WorldSpace;
            tracker.PositionDamping = new Vector3(0.35f, 0.35f, 0.35f);
            follow.TrackerSettings = tracker;
            return camera;
        }

        static void BuildHud(Camera camera)
        {
            var prefab = BrawlHudBuilder.Prefab;
            HudFontPass.Apply();
            HudIconPass.Apply();
            var hud = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            hud.GetComponent<Canvas>().worldCamera = camera;
            // A touch anywhere in the lower-left zone grabs the floating stick (OnScreenStick dynamic origin).
            foreach (var stick in hud.GetComponentsInChildren<OnScreenStick>(true))
                stick.dynamicOriginRange = 320f;

            var events = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            events.transform.SetAsLastSibling();
        }

        static float HeightOf(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            var b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return b.size.y;
        }

        static void Capture(Camera camera, string file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            var rt = new RenderTexture(1920, 1080, 24);
            camera.targetTexture = rt;
            Canvas.ForceUpdateCanvases();
            camera.Render(); // warm-up: the first batch-mode frame renders additional lights incorrectly
            camera.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            tex.Apply();
            File.WriteAllBytes(file, tex.EncodeToPNG());
            RenderTexture.active = null;
            camera.targetTexture = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
        }
    }
}
