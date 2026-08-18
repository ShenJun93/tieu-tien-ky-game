using System.Linq;
using TieuTienKy.Gameplay;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.Gameplay.EditorTools
{
    /// <summary>
    /// Generates the Vertical Slice v0.1 local proxy character content
    /// (rig hierarchy + AnimationClips + AnimatorController + prefab) that
    /// cannot be hand-authored reliably as raw Unity YAML. Re-runnable: each
    /// call overwrites the same asset paths. Not a generic character
    /// pipeline - this builds exactly one proxy for exactly this project.
    /// </summary>
    public static class VerticalSliceContentBuilder
    {
        const string CharactersDir = "Assets/_Project/Prefabs/Characters";
        const string AnimDir = CharactersDir + "/Animations";
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");

        static readonly Color BodyColor = new Color(0.55f, 0.75f, 0.95f);
        static readonly Color AccentColor = new Color(1f, 0.95f, 0.4f);

        const string EnemiesDir = "Assets/_Project/Prefabs/Enemies";

        static readonly Color PursuerColor = new Color(0.8f, 0.3f, 0.3f);
        static readonly Color LancerColor = new Color(0.55f, 0.25f, 0.65f);
        static readonly Color BossColor = new Color(0.85f, 0.65f, 0.15f);
        static readonly Color BossAccentColor = new Color(1f, 0.9f, 0.4f);
        const float BossVisualScale = 1.35f;

        /// <summary>
        /// Authored reusable prefabs for Pursuer/Lancer/MiniBoss, replacing
        /// ArenaRunDirector's inline new GameObject()+AddComponent() chain
        /// (Work Package 4/migration map: MIGRATE spawn construction). Each
        /// prefab holds every component ArenaRunDirector previously built
        /// inline except archetype-Initialize(), which still requires a live
        /// player reference supplied at spawn time.
        /// </summary>
        [MenuItem("Tools/Vertical Slice/Build Enemy Prefabs")]
        public static void BuildEnemyPrefabs()
        {
            EnsureFolder(EnemiesDir);

            BuildEnemyPrefab("Pursuer", PursuerColor, addEnemyController: true);
            BuildEnemyPrefab("Lancer", LancerColor, addEnemyController: true);
            BuildBossPrefab();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[VerticalSliceContentBuilder] Enemy/boss prefabs built at " + EnemiesDir);
        }

        static void BuildEnemyPrefab(string name, Color tint, bool addEnemyController)
        {
            var enemy = new GameObject(name);
            var controller = enemy.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            enemy.AddComponent<KnockbackReceiver>();
            enemy.AddComponent<Combatant>();

            var view = enemy.AddComponent<PrimitiveCharacterView>();
            view.Build(tint, tint, armed: false, visualScale: 1f);

            enemy.AddComponent<PrimitiveTelegraphVFX>();

            if (addEnemyController)
            {
                enemy.AddComponent<EnemyCombatController>();
            }

            string path = EnemiesDir + "/" + name + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(enemy, path);
            Object.DestroyImmediate(enemy);
        }

        static void BuildBossPrefab()
        {
            var boss = new GameObject("MiniBoss");
            var controller = boss.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f * BossVisualScale;
            controller.radius = 0.5f * BossVisualScale;

            boss.AddComponent<KnockbackReceiver>();
            boss.AddComponent<Combatant>();

            var view = boss.AddComponent<PrimitiveCharacterView>();
            view.Build(BossColor, BossAccentColor, armed: true, visualScale: BossVisualScale);

            boss.AddComponent<PrimitiveTelegraphVFX>();
            boss.AddComponent<MiniBossController>();

            string path = EnemiesDir + "/MiniBoss.prefab";
            PrefabUtility.SaveAsPrefabAsset(boss, path);
            Object.DestroyImmediate(boss);
        }

        [MenuItem("Tools/Vertical Slice/Build Cultivator Proxy")]
        public static void BuildCultivatorProxy()
        {
            EnsureFolder(CharactersDir);
            EnsureFolder(AnimDir);

            var root = new GameObject("PresentationRoot");
            var presentation = root.AddComponent<CharacterPresentation>();
            var animator = root.AddComponent<Animator>();

            Transform rig = CreateChild(root.transform, "Rig", Vector3.zero);

            Transform bodyAnchor = CreateChild(rig, "BodyAnchor", new Vector3(0f, 0.15f, 0f));
            Transform bodyBob = CreateChild(bodyAnchor, "BodyBob", Vector3.zero);
            Renderer bodyR = BuildMesh(bodyBob, "BodyMesh", PrimitiveType.Capsule, Vector3.zero, new Vector3(0.42f, 0.32f, 0.32f), BodyColor);

            Transform headAnchor = CreateChild(bodyBob, "HeadAnchor", new Vector3(0f, 0.67f, 0f));
            Renderer headR = BuildMesh(headAnchor, "HeadMesh", PrimitiveType.Cube, Vector3.zero, new Vector3(0.42f, 0.4f, 0.4f), BodyColor);
            Transform castVfxSocket = CreateChild(headAnchor, "CastVfxSocket", new Vector3(0f, 0.55f, 0f));

            Transform rightArmAnchor = CreateChild(bodyBob, "RightArmAnchor", new Vector3(0.4f, 0.25f, 0f));
            Transform rightArmPivot = CreateChild(rightArmAnchor, "RightArmPivot", Vector3.zero);
            Renderer rightArmR = BuildMesh(rightArmPivot, "RightArmMesh", PrimitiveType.Capsule, new Vector3(0f, -0.15f, 0f), new Vector3(0.16f, 0.3f, 0.16f), BodyColor);
            Transform weaponSocket = CreateChild(rightArmPivot, "WeaponSocket", new Vector3(0f, -0.3f, 0.1f));
            Renderer swordR = BuildMesh(weaponSocket, "Sword", PrimitiveType.Cube, new Vector3(0f, 0.35f, 0f), new Vector3(0.06f, 0.55f, 0.1f), AccentColor);

            Transform leftArmAnchor = CreateChild(bodyBob, "LeftArmAnchor", new Vector3(-0.4f, 0.25f, 0f));
            Transform leftArmPivot = CreateChild(leftArmAnchor, "LeftArmPivot", Vector3.zero);
            Renderer leftArmR = BuildMesh(leftArmPivot, "LeftArmMesh", PrimitiveType.Capsule, new Vector3(0f, -0.15f, 0f), new Vector3(0.16f, 0.3f, 0.16f), BodyColor);

            Transform rightLegAnchor = CreateChild(bodyBob, "RightLegAnchor", new Vector3(0.18f, -0.55f, 0f));
            Transform rightLegPivot = CreateChild(rightLegAnchor, "RightLegPivot", Vector3.zero);
            Renderer rightLegR = BuildMesh(rightLegPivot, "RightLegMesh", PrimitiveType.Capsule, new Vector3(0f, -0.18f, 0f), new Vector3(0.18f, 0.35f, 0.18f), BodyColor);

            Transform leftLegAnchor = CreateChild(bodyBob, "LeftLegAnchor", new Vector3(-0.18f, -0.55f, 0f));
            Transform leftLegPivot = CreateChild(leftLegAnchor, "LeftLegPivot", Vector3.zero);
            Renderer leftLegR = BuildMesh(leftLegPivot, "LeftLegMesh", PrimitiveType.Capsule, new Vector3(0f, -0.18f, 0f), new Vector3(0.18f, 0.35f, 0.18f), BodyColor);

            Transform bodyVfxSocket = CreateChild(bodyBob, "BodyVfxSocket", new Vector3(0f, 0.2f, 0.22f));
            Transform feetVfxSocket = CreateChild(rig, "FeetVfxSocket", new Vector3(0f, -0.95f, 0f));

            var renderers = new[] { bodyR, headR, rightArmR, leftArmR, rightLegR, leftLegR, swordR };

            AnimationClip idle = BuildIdleClip();
            AnimationClip run = BuildRunClip();
            AnimationClip basicAttack = BuildBasicAttackClip();
            AnimationClip cast = BuildCastClip();
            AnimationClip hit = BuildHitClip();
            AnimationClip death = BuildDeathClip();
            AnimationClip mobility = BuildMobilityClip();

            animator.runtimeAnimatorController = BuildController(idle, run, basicAttack, cast, hit, death, mobility);

            presentation.Configure(animator, weaponSocket, bodyVfxSocket, feetVfxSocket, castVfxSocket, renderers);

            string prefabPath = CharactersDir + "/CultivatorProxy.prefab";
            PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
            Object.DestroyImmediate(root);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[VerticalSliceContentBuilder] CultivatorProxy prefab built at " + prefabPath);
        }

        static Transform CreateChild(Transform parent, string name, Vector3 localPosition)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            return go.transform;
        }

        static Renderer BuildMesh(Transform parent, string name, PrimitiveType type, Vector3 localPosition, Vector3 localScale, Color color)
        {
            GameObject go = GameObject.CreatePrimitive(type);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.localPosition = localPosition;
            go.transform.localScale = localScale;

            var collider = go.GetComponent<Collider>();
            if (collider != null)
            {
                Object.DestroyImmediate(collider);
            }

            var renderer = go.GetComponent<Renderer>();
            renderer.sharedMaterial = LoadPrimitiveMaterial();
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(TintColorPropertyId, color);
            renderer.SetPropertyBlock(block);
            return renderer;
        }

        static Material s_primitiveMaterial;
        static Material LoadPrimitiveMaterial()
        {
            if (s_primitiveMaterial == null)
            {
                s_primitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_primitiveMaterial;
        }

        static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            string[] parts = assetPath.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }

        static AnimationClip NewClip(string name, bool loop)
        {
            var clip = new AnimationClip { name = name };
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = loop;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            return clip;
        }

        static void SaveClip(AnimationClip clip)
        {
            string path = AnimDir + "/" + clip.name + ".anim";
            AnimationClip existing = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (existing != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            AssetDatabase.CreateAsset(clip, path);
        }

        static AnimationCurve Curve(params Keyframe[] keys) => new AnimationCurve(keys);

        static AnimationClip BuildIdleClip()
        {
            AnimationClip clip = NewClip("Idle", loop: true);
            clip.SetCurve("Rig/BodyAnchor/BodyBob", typeof(Transform), "localPosition.y",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.6f, 0.03f), new Keyframe(1.2f, 0f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildRunClip()
        {
            AnimationClip clip = NewClip("Run", loop: true);
            clip.SetCurve("Rig/BodyAnchor/BodyBob", typeof(Transform), "localPosition.y",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.2f, 0.06f), new Keyframe(0.4f, 0f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob/RightArmAnchor/RightArmPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, -35f), new Keyframe(0.2f, 35f), new Keyframe(0.4f, -35f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob/LeftArmAnchor/LeftArmPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 35f), new Keyframe(0.2f, -35f), new Keyframe(0.4f, 35f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob/RightLegAnchor/RightLegPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 30f), new Keyframe(0.2f, -30f), new Keyframe(0.4f, 30f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob/LeftLegAnchor/LeftLegPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, -30f), new Keyframe(0.2f, 30f), new Keyframe(0.4f, -30f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildBasicAttackClip()
        {
            AnimationClip clip = NewClip("BasicAttack", loop: false);
            clip.SetCurve("Rig/BodyAnchor/BodyBob/RightArmAnchor/RightArmPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.12f, -35f), new Keyframe(0.18f, 60f), new Keyframe(0.4f, 0f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildCastClip()
        {
            AnimationClip clip = NewClip("Cast", loop: false);
            clip.SetCurve("Rig/BodyAnchor/BodyBob/RightArmAnchor/RightArmPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.2f, -100f), new Keyframe(0.5f, 0f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob/LeftArmAnchor/LeftArmPivot", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.2f, -100f), new Keyframe(0.5f, 0f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildHitClip()
        {
            AnimationClip clip = NewClip("Hit", loop: false);
            clip.SetCurve("Rig/BodyAnchor/BodyBob", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.08f, -18f), new Keyframe(0.25f, 0f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildDeathClip()
        {
            AnimationClip clip = NewClip("Death", loop: false);
            clip.SetCurve("Rig", typeof(Transform), "localEulerAngles.z",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.6f, 78f)));
            SaveClip(clip);
            return clip;
        }

        static AnimationClip BuildMobilityClip()
        {
            AnimationClip clip = NewClip("Mobility", loop: false);
            clip.SetCurve("Rig/BodyAnchor/BodyBob", typeof(Transform), "localPosition.z",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.12f, 0.18f), new Keyframe(0.3f, 0f)));
            clip.SetCurve("Rig/BodyAnchor/BodyBob", typeof(Transform), "localEulerAngles.x",
                Curve(new Keyframe(0f, 0f), new Keyframe(0.12f, 20f), new Keyframe(0.3f, 0f)));
            SaveClip(clip);
            return clip;
        }

        static AnimatorController BuildController(AnimationClip idle, AnimationClip run, AnimationClip basicAttack, AnimationClip cast, AnimationClip hit, AnimationClip death, AnimationClip mobility)
        {
            string path = AnimDir + "/CultivatorAnimator.controller";
            if (AssetDatabase.LoadAssetAtPath<AnimatorController>(path) != null)
            {
                AssetDatabase.DeleteAsset(path);
            }

            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("MoveSpeed", AnimatorControllerParameterType.Float);
            controller.AddParameter("BasicAttack", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Cast", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Mobility", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Hit", AnimatorControllerParameterType.Trigger);
            controller.AddParameter("Death", AnimatorControllerParameterType.Trigger);

            AnimatorStateMachine sm = controller.layers[0].stateMachine;

            AnimatorState idleState = sm.AddState("Idle");
            idleState.motion = idle;
            sm.defaultState = idleState;

            AnimatorState runState = sm.AddState("Run");
            runState.motion = run;

            AnimatorState attackState = sm.AddState("BasicAttack");
            attackState.motion = basicAttack;

            AnimatorState castState = sm.AddState("Cast");
            castState.motion = cast;

            AnimatorState mobilityState = sm.AddState("Mobility");
            mobilityState.motion = mobility;

            AnimatorState hitState = sm.AddState("Hit");
            hitState.motion = hit;

            AnimatorState deathState = sm.AddState("Death");
            deathState.motion = death;

            AnimatorStateTransition toRun = idleState.AddTransition(runState);
            toRun.hasExitTime = false;
            toRun.duration = 0.1f;
            toRun.AddCondition(AnimatorConditionMode.Greater, 0.1f, "MoveSpeed");

            AnimatorStateTransition toIdle = runState.AddTransition(idleState);
            toIdle.hasExitTime = false;
            toIdle.duration = 0.1f;
            toIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "MoveSpeed");

            AddOneShot(sm, attackState, "BasicAttack", idleState);
            AddOneShot(sm, castState, "Cast", idleState);
            AddOneShot(sm, mobilityState, "Mobility", idleState);
            AddOneShot(sm, hitState, "Hit", idleState);

            AnimatorStateTransition toDeath = sm.AddAnyStateTransition(deathState);
            toDeath.hasExitTime = false;
            toDeath.duration = 0.05f;
            toDeath.AddCondition(AnimatorConditionMode.If, 0, "Death");

            return controller;
        }

        static void AddOneShot(AnimatorStateMachine sm, AnimatorState target, string trigger, AnimatorState returnState)
        {
            AnimatorStateTransition enter = sm.AddAnyStateTransition(target);
            enter.hasExitTime = false;
            enter.duration = 0.05f;
            enter.canTransitionToSelf = false;
            enter.AddCondition(AnimatorConditionMode.If, 0, trigger);

            AnimatorStateTransition exit = target.AddTransition(returnState);
            exit.hasExitTime = true;
            exit.exitTime = 0.95f;
            exit.hasFixedDuration = true;
            exit.duration = 0.1f;
        }

        // ------------------------------------------------------------------
        // Work Package 1 / 5: production game-flow scenes + authored arena.
        // ------------------------------------------------------------------

        const string ScenesDir = "Assets/_Project/Scenes";
        const string BootScenePath = ScenesDir + "/Boot.unity";
        const string MainMenuScenePath = ScenesDir + "/MainMenu.unity";
        const string ArenaScenePath = ScenesDir + "/Arena_VerticalSlice_01.unity";

        static readonly Color GroundColor = new Color(0.5f, 0.62f, 0.5f);
        static readonly Color WaterZoneColor = new Color(0.2f, 0.5f, 0.9f, 0.6f);
        static readonly Color HazardColor = new Color(0.3f, 0.3f, 0.3f);
        static readonly Color SpawnMarkerColor = new Color(1f, 0.4f, 0.4f, 0.5f);
        static readonly Color BossMarkerColor = new Color(0.9f, 0.7f, 0.1f, 0.6f);

        const float ArenaWallHeight = 4f;
        const float ArenaWallThickness = 1f;
        const float ArenaWallBelowGroundMargin = 1f;

        [MenuItem("Tools/Vertical Slice/Build All Production Content")]
        public static void BuildAllProductionContent()
        {
            BuildCultivatorProxy();
            BuildEnemyPrefabs();
            BuildBootScene();
            BuildMainMenuScene();
            BuildArenaVerticalSliceScene();
            RegisterProductionBuildSettings();
            Debug.Log("[VerticalSliceContentBuilder] All production content built.");
        }

        [MenuItem("Tools/Vertical Slice/Build Boot Scene")]
        public static void BuildBootScene()
        {
            EnsureFolder(ScenesDir);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildCamera();
            BuildLight();

            new GameObject("BootLoader").AddComponent<BootLoader>();

            EditorSceneManager.SaveScene(scene, BootScenePath);
        }

        [MenuItem("Tools/Vertical Slice/Build Main Menu Scene")]
        public static void BuildMainMenuScene()
        {
            EnsureFolder(ScenesDir);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildCamera();
            BuildLight();

            new GameObject("MainMenuController").AddComponent<MainMenuController>();

            EditorSceneManager.SaveScene(scene, MainMenuScenePath);
        }

        [MenuItem("Tools/Vertical Slice/Build Arena Vertical Slice Scene")]
        public static void BuildArenaVerticalSliceScene()
        {
            EnsureFolder(ScenesDir);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildCamera(new Vector3(0f, 9f, -7f), Quaternion.Euler(52f, 0f, 0f));
            BuildLight();

            GameObject ground = BuildGround();
            BuildArenaBoundaries(ground);

            Transform playerSpawn = CreateMarker("PlayerSpawn", new Vector3(0f, 1f, -2f), SpawnMarkerColor, buildVisual: false);

            Transform waterMarkerA = CreateMarker("WaterZonePosition_A", new Vector3(3f, 0.5f, 0f), WaterZoneColor, buildVisual: false);
            Transform waterMarkerB = CreateMarker("WaterZonePosition_B", new Vector3(-3f, 0.5f, 3f), WaterZoneColor, buildVisual: false);
            Transform waterMarkerC = CreateMarker("WaterZonePosition_C", new Vector3(3f, 0.5f, -4f), WaterZoneColor, buildVisual: false);
            WaterZone waterZone = BuildWaterZone(waterMarkerA.position, new Vector3(3f, 1f, 3f));

            BuildHazardObstacle(new Vector3(5.5f, 1f, 0f));

            Transform wave1Zone = CreateMarker("EnemySpawnZone_Wave1", new Vector3(4f, 0.1f, 2f), SpawnMarkerColor, buildVisual: true);
            Transform wave2ZoneA = CreateMarker("EnemySpawnZone_Wave2_A", new Vector3(4f, 0.1f, 4f), SpawnMarkerColor, buildVisual: true);
            Transform wave2ZoneB = CreateMarker("EnemySpawnZone_Wave2_B", new Vector3(-4f, 0.1f, -2f), SpawnMarkerColor, buildVisual: true);
            Transform eliteZone = CreateMarker("EnemySpawnZone_Elite", new Vector3(-5f, 0.1f, 3f), SpawnMarkerColor, buildVisual: true);
            Transform bossZone = CreateMarker("BossSpawnZone", new Vector3(0f, 0.1f, 6f), BossMarkerColor, buildVisual: true);
            Transform cameraBoundsMarker = CreateMarker("CameraBounds", Vector3.zero, Color.clear, buildVisual: false);
            cameraBoundsMarker.localScale = new Vector3(10f, 1f, 10f);
            _ = new[] { wave1Zone, wave2ZoneA, wave2ZoneB, eliteZone, bossZone };

            var bootstrapper = new GameObject("ArenaVerticalSliceBootstrapper").AddComponent<ArenaVerticalSliceBootstrapper>();

            GameObject cultivatorProxy = AssetDatabase.LoadAssetAtPath<GameObject>(CharactersDir + "/CultivatorProxy.prefab");
            GameObject pursuer = AssetDatabase.LoadAssetAtPath<GameObject>(EnemiesDir + "/Pursuer.prefab");
            GameObject lancer = AssetDatabase.LoadAssetAtPath<GameObject>(EnemiesDir + "/Lancer.prefab");
            GameObject boss = AssetDatabase.LoadAssetAtPath<GameObject>(EnemiesDir + "/MiniBoss.prefab");

            bootstrapper.ConfigureAuthoring(
                ground.transform,
                playerSpawn,
                waterZone,
                new[] { waterMarkerA, waterMarkerB, waterMarkerC },
                cultivatorProxy,
                pursuer,
                lancer,
                boss);

            EditorSceneManager.SaveScene(scene, ArenaScenePath);
        }

        [MenuItem("Tools/Vertical Slice/Register Production Build Settings")]
        public static void RegisterProductionBuildSettings()
        {
            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(BootScenePath, true),
                new EditorBuildSettingsScene(MainMenuScenePath, true),
                new EditorBuildSettingsScene(ArenaScenePath, true)
            };
        }

        static void BuildCamera() => BuildCamera(new Vector3(0f, 8f, -6f), Quaternion.Euler(50f, 0f, 0f));

        static void BuildCamera(Vector3 position, Quaternion rotation)
        {
            var cameraGO = new GameObject("Main Camera");
            cameraGO.tag = "MainCamera";
            cameraGO.transform.position = position;
            cameraGO.transform.rotation = rotation;
            var camera = cameraGO.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.1f, 0.12f);
        }

        static void BuildLight()
        {
            var lightGO = new GameObject("Directional Light");
            lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
            var light = lightGO.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
        }

        static GameObject BuildGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "GameplaySurface";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            TintPrimitive(ground, GroundColor);
            return ground;
        }

        static void BuildArenaBoundaries(GameObject ground)
        {
            var boundariesRoot = new GameObject("Boundaries");
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;
            float halfThickness = ArenaWallThickness * 0.5f;
            float wallBottomY = ground.transform.position.y - ArenaWallBelowGroundMargin;
            float centerY = wallBottomY + ArenaWallHeight * 0.5f;

            BuildArenaWall(boundariesRoot.transform, "ArenaWall_North",
                new Vector3(groundBounds.center.x, centerY, groundBounds.max.z + halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildArenaWall(boundariesRoot.transform, "ArenaWall_South",
                new Vector3(groundBounds.center.x, centerY, groundBounds.min.z - halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildArenaWall(boundariesRoot.transform, "ArenaWall_East",
                new Vector3(groundBounds.max.x + halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));

            BuildArenaWall(boundariesRoot.transform, "ArenaWall_West",
                new Vector3(groundBounds.min.x - halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));
        }

        static void BuildArenaWall(Transform parent, string name, Vector3 center, Vector3 size)
        {
            var wall = new GameObject(name);
            wall.transform.SetParent(parent, true);
            wall.transform.position = center;

            var collider = wall.AddComponent<BoxCollider>();
            collider.size = size;
        }

        static WaterZone BuildWaterZone(Vector3 position, Vector3 size)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = "WaterZone";
            zone.transform.position = position;
            zone.transform.localScale = size;
            TintPrimitive(zone, WaterZoneColor);

            var boxCollider = zone.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            return zone.AddComponent<WaterZone>();
        }

        static void BuildHazardObstacle(Vector3 position)
        {
            GameObject hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hazard.name = "HazardObstacle";
            hazard.transform.position = position;
            hazard.transform.localScale = new Vector3(1f, 2f, 3f);
            TintPrimitive(hazard, HazardColor);

            hazard.AddComponent<HazardObstacle>();
        }

        static Transform CreateMarker(string name, Vector3 position, Color color, bool buildVisual)
        {
            if (!buildVisual)
            {
                var empty = new GameObject(name);
                empty.transform.position = position;
                return empty.transform;
            }

            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = name;
            marker.transform.position = position;
            marker.transform.localScale = new Vector3(1f, 0.05f, 1f);
            Object.DestroyImmediate(marker.GetComponent<Collider>());
            TintPrimitive(marker, color);
            return marker.transform;
        }

        static void TintPrimitive(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            renderer.sharedMaterial = LoadPrimitiveMaterial();
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(TintColorPropertyId, color);
            renderer.SetPropertyBlock(block);
        }
    }
}
