using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.EditorTools
{
    /// <summary>
    /// Re-runnable authoring tool for Task A2 (arena visual hierarchy) against
    /// the authored Arena_VerticalSlice_01 scene. Idempotent: finds existing
    /// named objects/materials and updates them in place rather than
    /// duplicating, so it is safe to re-run after tuning. Creates only the
    /// bounded set of content Task A2 requires (ground hierarchy, readable
    /// Water, one landmark, one chokepoint, visible boundary walls, hazard
    /// contrast, lighting contrast) - not a generic level-building framework.
    /// </summary>
    public static class StageABArenaVisualBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Arena_VerticalSlice_01.unity";
        const string MaterialFolder = "Assets/_Project/Materials";
        static readonly Shader StandardShader = Shader.Find("Standard");

        [MenuItem("Tools/Stage AB/Build Arena Visual Hierarchy (A2)")]
        public static void BuildArenaVisuals()
        {
            Scene scene = EnsureSceneOpen();

            Material rimFloorMat = EnsureMaterial("Arena_RimFloor", new Color(0.30f, 0.31f, 0.34f), 0.15f);
            Material innerFloorMat = EnsureMaterial("Arena_InnerFloor", new Color(0.62f, 0.55f, 0.42f), 0.25f);
            Material waterMat = EnsureMaterial("Arena_Water", new Color(0.16f, 0.42f, 0.78f), 0.85f);
            Material wallMat = EnsureMaterial("Arena_WallStone", new Color(0.24f, 0.23f, 0.25f), 0.1f);
            Material hazardMat = EnsureMaterial("Arena_Hazard", new Color(0.75f, 0.18f, 0.12f), 0.2f);
            Material landmarkMat = EnsureMaterial("Arena_Landmark", new Color(0.85f, 0.74f, 0.35f), 0.5f);
            Material chokepointMat = EnsureMaterial("Arena_Chokepoint", new Color(0.35f, 0.33f, 0.30f), 0.1f);

            ApplyGroundHierarchy(rimFloorMat, innerFloorMat);
            ApplyWaterMaterial(waterMat);
            ApplyHazardMaterial(hazardMat);
            BuildLandmark(landmarkMat);
            BuildChokepoint(chokepointMat);
            BuildWallVisuals(wallMat);
            TuneLighting();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            Debug.Log("[STAGE_AB_A2] Arena visual hierarchy build complete.");
        }

        static Scene EnsureSceneOpen()
        {
            Scene active = SceneManager.GetActiveScene();
            if (active.path == ScenePath)
            {
                return active;
            }

            return EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        }

        static Material EnsureMaterial(string name, Color color, float smoothness)
        {
            if (!Directory.Exists(MaterialFolder))
            {
                Directory.CreateDirectory(MaterialFolder);
                AssetDatabase.Refresh();
            }

            string path = $"{MaterialFolder}/{name}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null)
            {
                mat = new Material(StandardShader);
                AssetDatabase.CreateAsset(mat, path);
            }

            mat.color = color;
            mat.SetFloat("_Glossiness", smoothness);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        static void ApplyGroundHierarchy(Material rimFloorMat, Material innerFloorMat)
        {
            GameObject surface = GameObject.Find("GameplaySurface");
            if (surface != null)
            {
                var renderer = surface.GetComponent<Renderer>();
                renderer.sharedMaterial = rimFloorMat;
            }

            GameObject inner = GameObject.Find("InnerCombatFloor");
            if (inner == null)
            {
                inner = GameObject.CreatePrimitive(PrimitiveType.Plane);
                inner.name = "InnerCombatFloor";
                Object.DestroyImmediate(inner.GetComponent<Collider>());
            }

            inner.transform.position = new Vector3(0f, 0.02f, 0f);
            inner.transform.localScale = new Vector3(1.7f, 1f, 1.7f);
            inner.GetComponent<Renderer>().sharedMaterial = innerFloorMat;
        }

        static void ApplyWaterMaterial(Material waterMat)
        {
            GameObject water = GameObject.Find("WaterZone");
            if (water != null)
            {
                water.GetComponent<Renderer>().sharedMaterial = waterMat;
            }
        }

        static void ApplyHazardMaterial(Material hazardMat)
        {
            GameObject hazard = GameObject.Find("HazardObstacle");
            if (hazard != null)
            {
                hazard.GetComponent<Renderer>().sharedMaterial = hazardMat;
            }
        }

        static void BuildLandmark(Material landmarkMat)
        {
            GameObject landmark = GameObject.Find("ArenaLandmark");
            if (landmark == null)
            {
                landmark = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                landmark.name = "ArenaLandmark";
            }

            landmark.transform.position = new Vector3(0f, 1.2f, 2.5f);
            landmark.transform.localScale = new Vector3(1.1f, 1.2f, 1.1f);
            landmark.GetComponent<Renderer>().sharedMaterial = landmarkMat;

            var collider = landmark.GetComponent<CapsuleCollider>();
            if (collider != null)
            {
                collider.radius = 0.55f;
            }
        }

        static void BuildChokepoint(Material chokepointMat)
        {
            BuildRock("ChokepointRock_A", new Vector3(-3f, 0.9f, 1.5f), chokepointMat);
            BuildRock("ChokepointRock_B", new Vector3(-1f, 0.9f, 1.5f), chokepointMat);
        }

        static void BuildRock(string name, Vector3 position, Material mat)
        {
            GameObject rock = GameObject.Find(name);
            if (rock == null)
            {
                rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rock.name = name;
            }

            rock.transform.position = position;
            rock.transform.localScale = new Vector3(1.4f, 1.8f, 1.4f);
            rock.transform.rotation = Quaternion.Euler(0f, 20f, 0f);
            rock.GetComponent<Renderer>().sharedMaterial = mat;
        }

        static void BuildWallVisuals(Material wallMat)
        {
            BuildWallVisual("ArenaWall_North", new Vector3(22f, 4f, 1f), wallMat);
            BuildWallVisual("ArenaWall_South", new Vector3(22f, 4f, 1f), wallMat);
            BuildWallVisual("ArenaWall_East", new Vector3(1f, 4f, 20f), wallMat);
            BuildWallVisual("ArenaWall_West", new Vector3(1f, 4f, 20f), wallMat);
        }

        static void BuildWallVisual(string wallName, Vector3 size, Material mat)
        {
            GameObject wall = GameObject.Find(wallName);
            if (wall == null)
            {
                Debug.LogWarning($"[STAGE_AB_A2] Wall '{wallName}' not found; skipping visual.");
                return;
            }

            Transform existing = wall.transform.Find("Visual");
            GameObject visual;
            if (existing == null)
            {
                visual = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visual.name = "Visual";
                Object.DestroyImmediate(visual.GetComponent<Collider>());
                visual.transform.SetParent(wall.transform, false);
            }
            else
            {
                visual = existing.gameObject;
            }

            visual.transform.localPosition = Vector3.zero;
            visual.transform.localRotation = Quaternion.identity;
            visual.transform.localScale = size;
            visual.GetComponent<Renderer>().sharedMaterial = mat;
        }

        static void TuneLighting()
        {
            GameObject lightGo = GameObject.Find("Directional Light");
            if (lightGo == null)
            {
                return;
            }

            Light light = lightGo.GetComponent<Light>();
            light.intensity = 1.15f;
            light.shadows = LightShadows.Soft;
        }
    }
}
