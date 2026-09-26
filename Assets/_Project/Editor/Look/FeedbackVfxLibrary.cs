using System.IO;
using UnityEditor;
using UnityEngine;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1c feedback VFX kit. Creates the textures, materials, meshes and prefabs under
    /// Assets/_Project/VFX/ only when they are missing, so hand edits in the Editor survive re-runs.
    /// Colours follow teardown §3.3: red is reserved for must-dodge telegraphs, gold for parry cues,
    /// jade-cyan for the hero, lavender for Lôi (lightning).
    /// </summary>
    public static class FeedbackVfxLibrary
    {
        const string Root = "Assets/_Project/VFX";
        const string AdditiveShader = "TieuTienKy/P0A_UnlitTexturedAdditive";
        const string AlphaShader = "TieuTienKy/P0A_UnlitTexturedAlpha";

        public static readonly Color HeroJade = Hex("#3FD1C0");
        public static readonly Color DodgeRed = Hex("#E8322E");
        public static readonly Color ParryGold = Hex("#F5C542");
        public static readonly Color LoiLavender = Hex("#B9A6FF");

        public static GameObject HeroRing => Prefab("HeroRing", () => GroundQuad("HeroRing", Tex("ring", RingAlpha), AdditiveShader, HeroJade * 1.6f));
        public static GameObject TelegraphEdge => Prefab("Telegraph_CircleEdge", () => GroundQuad("Telegraph_CircleEdge", Tex("telegraph_edge", TelegraphEdgeAlpha, premultiply: false), AlphaShader, DodgeRed));
        public static GameObject TelegraphFill => Prefab("Telegraph_CircleFill", () => GroundQuad("Telegraph_CircleFill", Tex("telegraph_fill", TelegraphFillAlpha, premultiply: false), AlphaShader, DodgeRed));
        public static GameObject ParryGlint => Prefab("ParryGlint", () => Billboard("ParryGlint", Tex("glint", GlintAlpha), ParryGold * 2.2f));
        public static GameObject HitSpark => Prefab("HitSpark", () => Billboard("HitSpark", Tex("spark", SparkAlpha), Hex("#FFF4DC") * 2f));
        public static GameObject HeroSlash => Prefab("Slash_Hero", () => SlashArc("Slash_Hero", HeroJade));
        public static Material LightningMaterial => Mat("Lightning_Loi", Tex("bolt", BoltAlpha), AdditiveShader, LoiLavender * 1.1f);
        public static Material LightningCoreMaterial => Mat("Lightning_LoiCore", Tex("bolt", BoltAlpha), AdditiveShader, Color.white * 1.2f);

        // ---------- prefabs ----------

        static GameObject Prefab(string name, System.Func<GameObject> build)
        {
            string path = $"{Root}/Prefabs/{name}.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return existing;
            Folder($"{Root}/Prefabs");
            var go = build();
            var prefab = PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
            return prefab;
        }

        static GameObject GroundQuad(string name, Texture2D tex, string shader, Color color)
        {
            var go = new GameObject(name);
            var quad = new GameObject("Quad");
            quad.transform.SetParent(go.transform, false);
            quad.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            quad.transform.localPosition = new Vector3(0f, 0.03f, 0f);
            quad.AddComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            quad.AddComponent<MeshRenderer>().sharedMaterial = Mat(name, tex, shader, color);
            return go;
        }

        static GameObject Billboard(string name, Texture2D tex, Color color)
        {
            var go = new GameObject(name);
            go.AddComponent<MeshFilter>().sharedMesh = Resources.GetBuiltinResource<Mesh>("Quad.fbx");
            go.AddComponent<MeshRenderer>().sharedMaterial = Mat(name, tex, AdditiveShader, color);
            return go;
        }

        /// <summary>A flat 150° crescent: u runs along the arc, v from outer (0) to inner (1) edge.</summary>
        static GameObject SlashArc(string name, Color edge)
        {
            string meshPath = $"{Root}/Meshes/SlashArc.asset";
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if (mesh == null)
            {
                Folder($"{Root}/Meshes");
                mesh = BuildArcMesh(150f, 0.55f, 1.6f, 32);
                AssetDatabase.CreateAsset(mesh, meshPath);
            }
            var go = new GameObject(name);
            go.AddComponent<MeshFilter>().sharedMesh = mesh;
            var tex = Tex($"slash_{ColorUtility.ToHtmlStringRGB(edge)}", (u, v) => SlashPixel(u, v, edge));
            go.AddComponent<MeshRenderer>().sharedMaterial = Mat(name, tex, AdditiveShader, Color.white * 1.4f);
            return go;
        }

        static Mesh BuildArcMesh(float degrees, float inner, float outer, int segments)
        {
            var vertices = new Vector3[(segments + 1) * 2];
            var uvs = new Vector2[vertices.Length];
            var triangles = new int[segments * 6];
            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                float a = Mathf.Deg2Rad * (-degrees * 0.5f + degrees * t);
                var dir = new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a));
                // Taper: the tail of the swing is thinner than the head.
                float width = Mathf.Lerp(0.35f, 1f, t);
                vertices[i * 2] = dir * outer;
                vertices[i * 2 + 1] = dir * Mathf.Lerp(outer, inner, width);
                uvs[i * 2] = new Vector2(t, 0f);
                uvs[i * 2 + 1] = new Vector2(t, 1f);
                if (i == segments) continue;
                int k = i * 6, v = i * 2;
                triangles[k] = v; triangles[k + 1] = v + 2; triangles[k + 2] = v + 1;
                triangles[k + 3] = v + 1; triangles[k + 4] = v + 2; triangles[k + 5] = v + 3;
            }
            var mesh = new Mesh { name = "SlashArc", vertices = vertices, uv = uvs, triangles = triangles };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        // ---------- textures (procedural, 256 px) ----------

        delegate Color Pixel(float u, float v);

        static Texture2D Tex(string name, System.Func<float, float, float> alpha, bool premultiply = true) => Tex(name, (u, v) => new Color(1f, 1f, 1f, alpha(u, v)), premultiply);

        static Texture2D Tex(string name, Pixel pixel, bool premultiply = true)
        {
            string path = $"{Root}/Textures/vfx_{name}.png";
            var existing = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (existing != null) return existing;
            Folder($"{Root}/Textures");
            const int size = 256;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                var c = pixel((x + 0.5f) / size, (y + 0.5f) / size);
                // Additive shaders ignore alpha, so their textures carry it premultiplied into the colour.
                tex.SetPixel(x, y, premultiply ? new Color(c.r * c.a, c.g * c.a, c.b * c.a, c.a) : c);
            }
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
            AssetDatabase.ImportAsset(path);
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            importer.alphaIsTransparency = true;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        static float Radius(float u, float v) => Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f)) * 2f;

        static float RingAlpha(float u, float v)
        {
            float r = Radius(u, v);
            float band = Mathf.Exp(-Mathf.Pow((r - 0.86f) / 0.05f, 2f));
            return Mathf.Clamp01(band + (r < 0.86f ? 0.07f : 0f));
        }

        static float TelegraphEdgeAlpha(float u, float v)
        {
            float r = Radius(u, v);
            return r > 0.93f && r < 0.99f ? 1f : 0f;
        }

        static float TelegraphFillAlpha(float u, float v) => Radius(u, v) < 0.99f ? 0.42f : 0f;

        static float GlintAlpha(float u, float v)
        {
            float x = Mathf.Abs(u - 0.5f) * 2f, y = Mathf.Abs(v - 0.5f) * 2f;
            float cross = Mathf.Exp(-x * 22f) * Mathf.Exp(-y * 2.4f) + Mathf.Exp(-y * 22f) * Mathf.Exp(-x * 2.4f);
            float core = Mathf.Exp(-(x * x + y * y) * 30f);
            return Mathf.Clamp01(cross + core);
        }

        static float SparkAlpha(float u, float v)
        {
            float x = u - 0.5f, y = v - 0.5f;
            float r = Mathf.Sqrt(x * x + y * y) * 2f;
            float a = Mathf.Atan2(y, x);
            float rays = Mathf.Pow(Mathf.Abs(Mathf.Cos(a * 4f)), 18f) * Mathf.Clamp01(1f - r);
            return Mathf.Clamp01(rays + Mathf.Exp(-r * r * 40f));
        }

        static float BoltAlpha(float u, float v) => Mathf.Exp(-Mathf.Pow((v - 0.5f) / 0.12f, 2f));

        static Color SlashPixel(float u, float v, Color edge)
        {
            float along = Mathf.Pow(Mathf.Sin(Mathf.PI * Mathf.Clamp01(u * 1.08f)), 0.7f) * Mathf.SmoothStep(0f, 1f, u * 3f);
            float core = Mathf.Exp(-Mathf.Pow((v - 0.25f) / 0.14f, 2f));
            float body = Mathf.Clamp01(1f - v) * 0.9f;
            var c = Color.Lerp(edge, Color.white, core);
            c.a = Mathf.Clamp01((core + body) * along);
            return c;
        }

        // ---------- shared ----------

        static Material Mat(string name, Texture2D tex, string shader, Color color)
        {
            string path = $"{Root}/Materials/{name}.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;
            Folder($"{Root}/Materials");
            var material = new Material(Shader.Find(shader));
            material.SetTexture("_MainTex", tex);
            material.SetColor("_Color", color);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        static void Folder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            string parent = Path.GetDirectoryName(path).Replace('\\', '/');
            Folder(parent);
            AssetDatabase.CreateFolder(parent, Path.GetFileName(path));
        }

        static Color Hex(string hex) => ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.magenta;
    }
}
