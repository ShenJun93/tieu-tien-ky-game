using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1e xianxia landmarks. No CC0 Asian temple kit exists (research 02 §9), so these are
    /// modest procedural pieces: a paifang gate, red paper lanterns and a bronze incense burner
    /// (ding). Meshes are saved once under Assets/_Project/Art/Xianxia/ and can be replaced by
    /// modelled art later without touching the arena layout.
    /// </summary>
    public static class XianxiaDressing
    {
        const string Root = "Assets/_Project/Art/Xianxia";

        public static readonly Color Lacquer = Hex("#9E2B25");
        public static readonly Color DarkWood = Hex("#3A2A22");
        public static readonly Color RoofJade = Hex("#2F5E55");
        public static readonly Color Gold = Hex("#C9A45C");
        public static readonly Color Bronze = Hex("#7A5A2E");

        public static GameObject Paifang(Transform parent, Vector3 position, float width, float height)
        {
            var gate = new GameObject("Paifang").transform;
            gate.SetParent(parent, false);
            gate.localPosition = position;
            var lacquer = Mat("Xianxia_Lacquer", Lacquer, 0.35f);
            var wood = Mat("Xianxia_DarkWood", DarkWood, 0.1f);
            var roof = Mat("Xianxia_RoofJade", RoofJade, 0.25f);
            var gold = Mat("Xianxia_Gold", Gold, 0.55f, metallic: 0.6f);

            float half = width * 0.5f - 0.5f;
            foreach (float x in new[] { -half, half })
            {
                Part(gate, "Pillar", Lathe("Pillar", new[] { (0.42f, 0f), (0.36f, 0.25f), (0.3f, 0.3f), (0.3f, 1f) }), new Vector3(x, 0f, 0f), new Vector3(1f, height, 1f), lacquer);
                Part(gate, "Base", Lathe("PillarBase", new[] { (0.55f, 0f), (0.55f, 0.4f), (0.4f, 0.55f), (0f, 0.55f) }), new Vector3(x, 0f, 0f), Vector3.one, gold);
            }
            Box(gate, "BeamLow", new Vector3(0f, height * 0.72f, 0f), new Vector3(width - 0.6f, 0.35f, 0.45f), lacquer);
            Box(gate, "Plaque", new Vector3(0f, height * 0.83f, -0.05f), new Vector3(width * 0.28f, 0.7f, 0.2f), gold);
            Box(gate, "BeamHigh", new Vector3(0f, height * 0.93f, 0f), new Vector3(width + 0.2f, 0.45f, 0.6f), wood);
            // Roof: a flat slab plus upturned eaves at both ends.
            Box(gate, "Roof", new Vector3(0f, height * 1.02f, 0f), new Vector3(width + 1.2f, 0.35f, 1.6f), roof);
            foreach (float s in new[] { -1f, 1f })
            {
                var eave = Box(gate, "Eave", new Vector3(s * (width * 0.5f + 0.9f), height * 1.08f, 0f), new Vector3(1.2f, 0.3f, 1.5f), roof);
                eave.transform.localRotation = Quaternion.Euler(0f, 0f, s * 18f);
            }
            Box(gate, "Ridge", new Vector3(0f, height * 1.12f, 0f), new Vector3(width * 0.9f, 0.22f, 0.3f), gold);
            return gate.gameObject;
        }

        public static GameObject Lantern(Transform parent, Vector3 position, float scale)
        {
            var root = new GameObject("Lantern").transform;
            root.SetParent(parent, false);
            root.localPosition = position;
            root.localScale = Vector3.one * scale;
            var paper = Mat("Xianxia_LanternPaper", Hex("#C8322A"), 0.2f, emission: Hex("#FF5A2A") * 1.6f);
            var gold = Mat("Xianxia_Gold", Gold, 0.55f, metallic: 0.6f);
            var body = new List<(float, float)>();
            for (int i = 0; i <= 10; i++)
            {
                float t = i / 10f;
                body.Add((0.12f + 0.33f * Mathf.Sin(Mathf.PI * t), t * 0.8f));
            }
            Part(root, "Paper", Lathe("LanternBody", body.ToArray()), Vector3.zero, Vector3.one, paper);
            Part(root, "CapTop", Lathe("LanternCap", new[] { (0.18f, 0f), (0.18f, 0.08f), (0f, 0.1f) }), new Vector3(0f, 0.78f, 0f), Vector3.one, gold);
            Part(root, "CapBottom", Lathe("LanternCap", new[] { (0.18f, 0f), (0.18f, 0.08f), (0f, 0.1f) }), new Vector3(0f, 0.02f, 0f), new Vector3(1f, -1f, 1f), gold);
            Box(root, "Tassel", new Vector3(0f, -0.25f, 0f), new Vector3(0.05f, 0.4f, 0.05f), Mat("Xianxia_Lacquer", Lacquer, 0.35f));
            var glow = new GameObject("Glow").AddComponent<Light>();
            glow.transform.SetParent(root, false);
            glow.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            glow.type = LightType.Point;
            glow.color = Hex("#FF7A3A");
            glow.intensity = 2.2f;
            glow.range = 6f;
            glow.shadows = LightShadows.None;
            return root.gameObject;
        }

        public static GameObject IncenseBurner(Transform parent, Vector3 position, float scale)
        {
            var root = new GameObject("IncenseBurner_Ding").transform;
            root.SetParent(parent, false);
            root.localPosition = position;
            root.localScale = Vector3.one * scale;
            var bronze = Mat("Xianxia_Bronze", Bronze, 0.45f, metallic: 0.5f);
            Part(root, "Bowl", Lathe("DingBowl", new[] { (0.05f, 0f), (0.55f, 0.1f), (0.7f, 0.45f), (0.66f, 0.7f), (0.74f, 0.78f), (0.6f, 0.8f), (0.5f, 0.55f), (0f, 0.5f) }), new Vector3(0f, 0.35f, 0f), Vector3.one, bronze);
            for (int i = 0; i < 3; i++)
            {
                float a = i * Mathf.PI * 2f / 3f;
                Box(root, "Leg", new Vector3(Mathf.Cos(a) * 0.38f, 0.2f, Mathf.Sin(a) * 0.38f), new Vector3(0.14f, 0.45f, 0.14f), bronze);
            }
            foreach (float s in new[] { -1f, 1f })
                Box(root, "Handle", new Vector3(s * 0.5f, 1.25f, 0f), new Vector3(0.1f, 0.35f, 0.35f), bronze);
            // Three incense sticks with glowing tips.
            var stick = Mat("Xianxia_DarkWood", DarkWood, 0.1f);
            var ember = Mat("Xianxia_Ember", Hex("#FF8A3A"), 0f, emission: Hex("#FF6A20") * 3f);
            for (int i = -1; i <= 1; i++)
            {
                Box(root, "Stick", new Vector3(i * 0.12f, 1.35f, 0f), new Vector3(0.03f, 0.9f, 0.03f), stick);
                Box(root, "Ember", new Vector3(i * 0.12f, 1.82f, 0f), new Vector3(0.05f, 0.06f, 0.05f), ember);
            }
            return root.gameObject;
        }

        // ---------- mesh helpers ----------

        static GameObject Part(Transform parent, string name, Mesh mesh, Vector3 pos, Vector3 scale, Material mat)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            go.transform.localPosition = pos;
            go.transform.localScale = scale;
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            go.AddComponent<MeshRenderer>().sharedMaterial = mat;
            return go;
        }

        static GameObject Box(Transform parent, string name, Vector3 pos, Vector3 size, Material mat)
            => Part(parent, name, Resources.GetBuiltinResource<Mesh>("Cube.fbx"), pos, size, mat);

        /// <summary>Revolves a (radius, height) profile around Y into a closed mesh, saved once as an asset.</summary>
        static Mesh Lathe(string name, (float r, float y)[] profile, int segments = 24)
        {
            string path = $"{Root}/Meshes/{name}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing != null) return existing;
            var verts = new List<Vector3>();
            var tris = new List<int>();
            int rows = profile.Length;
            for (int s = 0; s <= segments; s++)
            {
                float a = s * Mathf.PI * 2f / segments;
                for (int k = 0; k < rows; k++)
                    verts.Add(new Vector3(Mathf.Cos(a) * profile[k].r, profile[k].y, Mathf.Sin(a) * profile[k].r));
            }
            for (int s = 0; s < segments; s++)
            for (int k = 0; k < rows - 1; k++)
            {
                int a = s * rows + k, b = (s + 1) * rows + k;
                tris.AddRange(new[] { a, a + 1, b, b, a + 1, b + 1 });
            }
            var mesh = new Mesh { name = name };
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            Folder($"{Root}/Meshes");
            AssetDatabase.CreateAsset(mesh, path);
            return mesh;
        }

        static Material Mat(string name, Color color, float smoothness, float metallic = 0f, Color? emission = null)
        {
            string path = $"{Root}/Materials/{name}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;
            Folder($"{Root}/Materials");
            var m = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            m.SetColor("_BaseColor", color);
            m.SetFloat("_Smoothness", smoothness);
            m.SetFloat("_Metallic", metallic);
            if (emission.HasValue)
            {
                m.EnableKeyword("_EMISSION");
                m.SetColor("_EmissionColor", emission.Value);
                m.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
            }
            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        static void Folder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
            Folder(parent);
            AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
        }

        static Color Hex(string hex) => ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.magenta;
    }
}
