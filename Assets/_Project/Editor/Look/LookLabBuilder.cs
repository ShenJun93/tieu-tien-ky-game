using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1 look lab: the KayKit cast on the dark jade floor (ADR 006) under the
    /// gameplay camera, with the shared toon material, plus screenshots in Logs/look/.
    /// This is a review scene, not game content. Batch usage (no -nographics, it renders):
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Look.LookLabBuilder.Build -quit
    /// </summary>
    public static class LookLabBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Look/LookLab.unity";
        const string MaterialFolder = "Assets/_Project/Materials/Look";
        const string KayKitRoot = "Assets/ThirdParty/KayKit/Adventurers";
        const float TargetHeight = 1.7f;

        static readonly string[] Cast = { "Barbarian", "Knight", "Mage", "Rogue", "RogueHooded" };
        static readonly Color FloorColor = Hex("#24372F");
        static readonly Color WallColor = Hex("#14151B");
        static readonly List<PlayableGraph> Graphs = new List<PlayableGraph>();

        [MenuItem("Tieu Tien Ky/Look/Build Look Lab")]
        public static void Build()
        {
            ShaderUtil.allowAsyncCompilation = false;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            BuildEnvironment();

            var lineup = Cast.Select(Spawn).Where(go => go != null).ToList();
            for (int i = 0; i < lineup.Count; i++)
            {
                NormalizeHeight(lineup[i]);
                lineup[i].transform.SetPositionAndRotation(
                    new Vector3((i - (lineup.Count - 1) * 0.5f) * 1.6f, 0f, 0f),
                    Quaternion.Euler(0f, 180f, 0f));
            }

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);

            string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "look");
            Directory.CreateDirectory(outDir);
            Capture(null, 55f, 32f, 29f, Vector3.zero); // warm-up frame
            // Gameplay framing (teardown 3.1): 55 deg pitch, vFOV 32, hero ~10% of screen height.
            Capture(Path.Combine(outDir, "looklab-gameplay.png"), 55f, 32f, 29f, new Vector3(0f, 0.8f, 0f));
            Capture(Path.Combine(outDir, "looklab-closeup.png"), 30f, 32f, 8.5f, new Vector3(0f, 0.9f, 0f));

            foreach (var g in Graphs) g.Destroy();
            Graphs.Clear();
            Debug.Log($"[LookLab] {lineup.Count} characters; screenshots in {outDir}");
        }

        static GameObject Spawn(string name)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>($"{KayKitRoot}/Characters/{name}.fbx");
            if (model == null) { Debug.LogWarning($"[LookLab] missing KayKit {name}"); return null; }
            var go = (GameObject)PrefabUtility.InstantiatePrefab(model);
            string textureName = name.StartsWith("Rogue") ? "rogue_texture" : $"{name.ToLowerInvariant()}_texture";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{KayKitRoot}/Textures/{textureName}.png");
            var material = ToonMaterial($"Toon_KayKit_{name}", texture);
            foreach (var r in go.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();
            Pose(go, AssetDatabase.GetAssetPath(model), "Idle");
            return go;
        }

        /// <summary>Evaluates one frame of a clip through a PlayableGraph so the pose shows in edit-mode renders.</summary>
        static void Pose(GameObject go, string clipSourcePath, string clipName)
        {
            var clip = AssetDatabase.LoadAllAssetsAtPath(clipSourcePath).OfType<AnimationClip>()
                .FirstOrDefault(c => c.name == clipName);
            if (clip == null) return;
            var animator = go.GetComponent<Animator>();
            if (animator == null) animator = go.AddComponent<Animator>(); // Unity fake-null: no ?? here
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.applyRootMotion = false;
            var graph = PlayableGraph.Create($"Pose_{go.name}");
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            var playable = AnimationClipPlayable.Create(graph, clip);
            playable.SetTime(clip.length * 0.3f);
            AnimationPlayableOutput.Create(graph, "Pose", animator).SetSourcePlayable(playable);
            graph.Evaluate();
            Graphs.Add(graph);
        }

        static void NormalizeHeight(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return;
            var b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            if (b.size.y > 0.01f)
                go.transform.localScale *= TargetHeight / b.size.y;
        }

        static void BuildEnvironment()
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor_DarkJade";
            floor.transform.localScale = new Vector3(6f, 1f, 6f);
            floor.GetComponent<Renderer>().sharedMaterial = LitMaterial("Floor_DarkJade", FloorColor, 0.25f);

            var wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "Wall_Ink";
            wall.transform.position = new Vector3(0f, 0.5f, 6f);
            wall.transform.localScale = new Vector3(30f, 1f, 0.8f);
            wall.GetComponent<Renderer>().sharedMaterial = LitMaterial("Wall_Ink", WallColor, 0.1f);

            var sun = new GameObject("Key_Warm").AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.color = Hex("#FFE8C8");
            sun.intensity = 1.3f;
            sun.shadows = LightShadows.Soft;
            sun.transform.rotation = Quaternion.Euler(50f, -35f, 0f);

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Hex("#4A5A6E");
            RenderSettings.ambientEquatorColor = Hex("#2E3A40");
            RenderSettings.ambientGroundColor = Hex("#1A1F22");
            RenderSettings.skybox = null;
        }

        /// <summary>Renders one frame to a PNG; a null path renders a warm-up frame only.</summary>
        static void Capture(string file, float pitch, float fov, float distance, Vector3 focus)
        {
            var cam = new GameObject("CaptureCamera").AddComponent<Camera>();
            cam.fieldOfView = fov;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = WallColor;
            cam.transform.rotation = Quaternion.Euler(pitch, 0f, 0f);
            cam.transform.position = focus - cam.transform.forward * distance;
            cam.farClipPlane = 200f;

            var rt = new RenderTexture(1920, 1080, 24) { antiAliasing = 4 };
            cam.targetTexture = rt;
            cam.Render();
            if (file != null)
            {
                RenderTexture.active = rt;
                var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                tex.Apply();
                File.WriteAllBytes(file, tex.EncodeToPNG());
                RenderTexture.active = null;
                Object.DestroyImmediate(tex);
            }
            cam.targetTexture = null;
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(cam.gameObject);
        }

        static Material ToonMaterial(string name, Texture texture)
        {
            var material = LoadOrCreate(name, "TieuTienKy/ToonPrototype");
            material.SetTexture("_BaseMap", texture);
            material.SetColor("_BaseColor", Color.white);
            material.SetColor("_ShadowColor", Hex("#6E6A8C"));
            material.SetColor("_RimColor", Hex("#FFF1DC"));
            material.SetFloat("_RimStrength", 0.45f);
            material.SetColor("_OutlineColor", Hex("#1A1622"));
            material.SetFloat("_OutlineWidth", 0.012f);
            EditorUtility.SetDirty(material);
            return material;
        }

        static Material LitMaterial(string name, Color color, float smoothness)
        {
            var material = LoadOrCreate(name, "Universal Render Pipeline/Lit");
            material.SetColor("_BaseColor", color);
            material.SetFloat("_Smoothness", smoothness);
            material.SetFloat("_Metallic", 0f);
            EditorUtility.SetDirty(material);
            return material;
        }

        static Material LoadOrCreate(string name, string shaderName)
        {
            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder("Assets/_Project/Materials", "Look");
            string path = $"{MaterialFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find(shaderName));
                AssetDatabase.CreateAsset(material, path);
            }
            return material;
        }

        static Color Hex(string hex) => ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.magenta;
    }
}
