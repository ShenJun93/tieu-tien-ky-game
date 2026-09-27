using System.IO;
using System.Linq;
using TieuTienKy.Brawl;
using TieuTienKy.Combat;
using TieuTienKy.EditorTools.Look;
using Unity.Cinemachine;
using Unity.Cinemachine.TargetTracking;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering.Universal;

namespace TieuTienKy.EditorTools.Brawl
{
    /// <summary>
    /// Builds the playable brawl scene: the LOOK-1 arena, a BrawlMatch whose roster is a list of
    /// CharacterDefinitions (KayKit placeholders today, ADR 008), a Cinemachine follow camera at the
    /// teardown framing that tracks a camera anchor, and the HUD with a floating OnScreenStick.
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Brawl.BrawlArenaBuilder.Build -quit
    /// </summary>
    public static class BrawlArenaBuilder
    {
        public const string ScenePath = "Assets/_Project/Scenes/Brawl/Arena_Brawl_01.unity";
        const string CastRoot = "Assets/ThirdParty/KayKit/Adventurers";
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
            // NewScene unloads unused assets, so the definitions are loaded after it.
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var entries = Roster.Select(r => new BrawlMatch.Entry
            {
                Character = KayKitCharacters.Ensure(r.cast, r.team),
                Team = r.team,
                Spawn = r.spawn,
            }).ToArray();
            ArenaLabBuilder.BuildEnvironment(out Rect playArea);

            // Fighters spawn at runtime from their definitions; the camera tracks this anchor, not a model.
            var hero = entries[0];
            var cameraTarget = new GameObject("CameraTarget").transform;
            cameraTarget.position = new Vector3(hero.Spawn.x, 0f, hero.Spawn.y);

            var match = new GameObject("BrawlMatch").AddComponent<BrawlMatch>();
            match.Configure(entries, 0, playArea, cameraTarget, FeedbackVfxLibrary.HeroRing);

            var mainCamera = BuildCameras(cameraTarget, hero.Character.Height);
            BuildHud(mainCamera);

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);

            // Preview only, after saving: spawn the roster as the match will, then capture.
            var preview = new GameObject("Preview").transform;
            foreach (var e in entries)
            {
                var view = FighterSpawner.Spawn(e.Character, e.Team, e.Spawn, preview);
                // Edit mode does not run the Animator; sample the idle clip so the capture is not a T-pose.
                e.Character.Animations.animationClips.FirstOrDefault()?.SampleAnimation(view.gameObject, 0.3f);
            }
            Capture(mainCamera, Path.Combine(Directory.GetCurrentDirectory(), "Logs", "look", "brawl-start.png"));
            Object.DestroyImmediate(preview.gameObject);
            Debug.Log($"[BrawlArena] {entries.Length} fighters, play area {playArea}, hero height {hero.Character.Height:F2}");
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
