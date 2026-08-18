using TieuTienKy.Gameplay;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.EditorTools
{
    /// <summary>
    /// Builds the Task B2 authored network duel content: the NetworkPlayer
    /// prefab and a separate Arena_Network_01 scene, rather than networking
    /// the entire solo run scene. Re-runnable, like the project's other
    /// content-builder tools.
    /// </summary>
    public static class StageABNetworkBuilder
    {
        const string NetworkPrefabDir = "Assets/_Project/Prefabs/Network";
        const string NetworkPlayerPrefabPath = NetworkPrefabDir + "/NetworkPlayer.prefab";
        const string ScenesDir = "Assets/_Project/Scenes";
        const string NetworkArenaScenePath = ScenesDir + "/Arena_Network_01.unity";
        const string CultivatorProxyPath = "Assets/_Project/Prefabs/Characters/CultivatorProxy.prefab";
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";

        const float ArenaWallThickness = 1f;
        const float ArenaWallHeight = 4f;

        [MenuItem("Tools/Stage AB/Build Network Player Prefab (B2)")]
        public static void BuildNetworkPlayerPrefab()
        {
            EnsureFolder(NetworkPrefabDir);

            var player = new GameObject("NetworkPlayer");

            var controller = player.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            player.AddComponent<KnockbackReceiver>();
            player.AddComponent<Combatant>();
            player.AddComponent<TieuTienKy.Input.TouchInputReader>();
            player.AddComponent<PlayerController>();
            var basicAttack = player.AddComponent<BasicAttack>();
            player.AddComponent<LoiTramSkill>();
            var phongBo = player.AddComponent<PhongBoSkill>();
            player.AddComponent<HoTheSkill>();
            player.AddComponent<PlayerSkillController>();

            player.AddComponent<NetworkObject>();
            var networkTransform = player.AddComponent<NetworkTransform>();
            networkTransform.Interpolate = true;
            player.AddComponent<NetworkPlayerActionGateway>();
            player.AddComponent<NetworkedCombatantSync>();
            player.AddComponent<NetworkPlayerMovement>();

            GameObject cultivatorProxy = AssetDatabase.LoadAssetAtPath<GameObject>(CultivatorProxyPath);
            if (cultivatorProxy != null)
            {
                GameObject presentationGO = (GameObject)PrefabUtility.InstantiatePrefab(cultivatorProxy, player.transform);
                presentationGO.transform.localPosition = Vector3.zero;
                presentationGO.transform.localRotation = Quaternion.identity;

                var presentation = presentationGO.GetComponent<CharacterPresentation>();
                if (presentation != null)
                {
                    var movementDriver = presentationGO.AddComponent<PresentationMovementDriver>();
                    movementDriver.Initialize(player.GetComponent<PlayerController>(), presentation);
                }
            }

            _ = phongBo;
            _ = basicAttack;

            PrefabUtility.SaveAsPrefabAsset(player, NetworkPlayerPrefabPath);
            Object.DestroyImmediate(player);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[STAGE_AB_B2] NetworkPlayer prefab built at " + NetworkPlayerPrefabPath);
        }

        [MenuItem("Tools/Stage AB/Build Network Arena Scene (B2)")]
        public static void BuildNetworkArenaScene()
        {
            EnsureFolder(ScenesDir);
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildCamera();
            BuildLight();

            GameObject ground = BuildGround();
            BuildArenaBoundaries(ground);
            BuildWaterZone(new Vector3(0f, 0.5f, 0f));

            Transform spawnA = CreateMarker("PlayerSpawn_A", new Vector3(-6f, 1f, 0f));
            Transform spawnB = CreateMarker("PlayerSpawn_B", new Vector3(6f, 1f, 0f));

            var networkManagerGO = new GameObject("NetworkManager");
            var networkManager = networkManagerGO.AddComponent<NetworkManager>();
            var transport = networkManagerGO.AddComponent<UnityTransport>();
            transport.ConnectionData.Address = "127.0.0.1";
            transport.ConnectionData.Port = 7777;
            transport.ConnectionData.ServerListenAddress = "0.0.0.0";

            networkManager.NetworkConfig.NetworkTransport = transport;
            networkManager.NetworkConfig.ConnectionApproval = false;
            networkManager.NetworkConfig.PlayerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(NetworkPlayerPrefabPath);

            var sessionDirectorGO = new GameObject("NetworkArenaSessionDirector");
            sessionDirectorGO.AddComponent<NetworkObject>();
            var sessionDirector = sessionDirectorGO.AddComponent<NetworkArenaSessionDirector>();

            var bootstrapGO = new GameObject("NetworkArenaSceneBootstrap");
            var bootstrap = bootstrapGO.AddComponent<NetworkArenaSceneBootstrap>();
            bootstrapGO.AddComponent<NetworkSmokeTestDriver>();

            var so = new SerializedObject(bootstrap);
            SerializedProperty spawnPointsProp = so.FindProperty("spawnPoints");
            spawnPointsProp.arraySize = 2;
            spawnPointsProp.GetArrayElementAtIndex(0).objectReferenceValue = spawnA;
            spawnPointsProp.GetArrayElementAtIndex(1).objectReferenceValue = spawnB;
            so.FindProperty("sessionDirector").objectReferenceValue = sessionDirector;
            so.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, NetworkArenaScenePath);
            Debug.Log("[STAGE_AB_B2] Arena_Network_01 scene built at " + NetworkArenaScenePath);
        }

        [MenuItem("Tools/Stage AB/Build All Network Content (B2)")]
        public static void BuildAllNetworkContent()
        {
            BuildNetworkPlayerPrefab();
            BuildNetworkArenaScene();
        }

        [MenuItem("Tools/Stage AB/Build Windows Smoke Test Player (B6)")]
        public static void BuildWindowsSmokeTestPlayer()
        {
            var options = new BuildPlayerOptions
            {
                scenes = new[] { NetworkArenaScenePath },
                locationPathName = "Builds/Windows/NetworkSmokeTest.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);
            UnityEditor.Build.Reporting.BuildSummary summary = report.summary;
            Debug.Log($"[STAGE_AB_B6_BUILD] result={summary.result} totalErrors={summary.totalErrors} totalWarnings={summary.totalWarnings} outputPath={summary.outputPath}");
        }

        static void BuildCamera()
        {
            var cameraGO = new GameObject("Main Camera");
            cameraGO.tag = "MainCamera";
            cameraGO.transform.position = new Vector3(0f, 14f, -10f);
            cameraGO.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
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
            light.intensity = 1.15f;
            light.shadows = LightShadows.Soft;
        }

        static GameObject BuildGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "GameplaySurface";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            TintPrimitive(ground, new Color(0.35f, 0.36f, 0.4f));
            return ground;
        }

        static void BuildArenaBoundaries(GameObject ground)
        {
            var boundariesRoot = new GameObject("Boundaries");
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;
            float halfThickness = ArenaWallThickness * 0.5f;
            float centerY = ArenaWallHeight * 0.5f;

            BuildWall(boundariesRoot.transform, "ArenaWall_North",
                new Vector3(groundBounds.center.x, centerY, groundBounds.max.z + halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildWall(boundariesRoot.transform, "ArenaWall_South",
                new Vector3(groundBounds.center.x, centerY, groundBounds.min.z - halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildWall(boundariesRoot.transform, "ArenaWall_East",
                new Vector3(groundBounds.max.x + halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));

            BuildWall(boundariesRoot.transform, "ArenaWall_West",
                new Vector3(groundBounds.min.x - halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));
        }

        static void BuildWall(Transform parent, string name, Vector3 center, Vector3 size)
        {
            var wall = new GameObject(name);
            wall.transform.SetParent(parent, true);
            wall.transform.position = center;
            var collider = wall.AddComponent<BoxCollider>();
            collider.size = size;
        }

        static void BuildWaterZone(Vector3 position)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = "WaterZone";
            zone.transform.position = position;
            zone.transform.localScale = new Vector3(4f, 1f, 4f);
            TintPrimitive(zone, new Color(0.16f, 0.42f, 0.78f));

            var boxCollider = zone.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;
            zone.AddComponent<WaterZone>();
        }

        static Transform CreateMarker(string name, Vector3 position)
        {
            var go = new GameObject(name);
            go.transform.position = position;
            return go.transform;
        }

        static Material s_primitiveMaterial;
        static void TintPrimitive(GameObject go, Color color)
        {
            if (s_primitiveMaterial == null)
            {
                s_primitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            var renderer = go.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            renderer.sharedMaterial = s_primitiveMaterial;
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(Shader.PropertyToID("_Color"), color);
            renderer.SetPropertyBlock(block);
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
    }
}
