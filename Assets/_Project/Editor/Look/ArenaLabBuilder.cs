using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1b arena lab: a dark jade sect arena built from the CC0 KayKit Dungeon kit.
    /// It has floor detail at 2 scales, low edge walls, edge prop clusters, warm torch
    /// accents and a KayKit cast mid-fight, all judged against teardown §3.3–3.4.
    /// It renders screenshots to Logs/look/. Batch usage (no -nographics, it renders):
    ///   Unity -batchmode -projectPath . -executeMethod TieuTienKy.EditorTools.Look.ArenaLabBuilder.Build -quit
    /// </summary>
    public static class ArenaLabBuilder
    {
        const string ScenePath = "Assets/_Project/Scenes/Look/ArenaLab.unity";
        const string MaterialFolder = "Assets/_Project/Materials/Look";
        const string PaletteFolder = "Assets/_Project/Art/Palettes";
        const string KitRoot = "Assets/ThirdParty/KayKit/DungeonRemastered";
        const string CastRoot = "Assets/ThirdParty/KayKit/Adventurers";
        const int Columns = 12;
        const int Rows = 8;

        static readonly List<PlayableGraph> Graphs = new List<PlayableGraph>();
        static Material stoneFloor, stoneWall, props;
        static float cell;
        static GameObject hud;
        static readonly List<(Transform target, string text, Color color, float sizePct)> DamageNumbers = new List<(Transform, string, Color, float)>();

        [MenuItem("Tieu Tien Ky/Look/Build Arena Lab")]
        public static void Build()
        {
            ShaderUtil.allowAsyncCompilation = false;
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var jade = BuildJadePalette();
            var original = AssetDatabase.LoadAssetAtPath<Texture2D>($"{KitRoot}/Textures/dungeon_texture.png");
            stoneFloor = LitMaterial("Arena_StoneFloor", jade, Hex("#56675F"), 0.12f);
            stoneWall = LitMaterial("Arena_StoneWall", jade, Hex("#3A4744"), 0.08f);
            props = LitMaterial("Arena_Props", original, Color.white, 0.1f);

            var root = new GameObject("Arena_Sect_01").transform;
            cell = SizeOf("floor_tile_large").x;
            BuildFloor(root);
            BuildEdges(root);
            BuildPropClusters(root);
            var hero = BuildCast();
            BuildFeedbackMoment();
            hud = (GameObject)PrefabUtility.InstantiatePrefab(BrawlHudBuilder.Prefab);
            BuildLighting();
            BuildPostProcessing();

            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            EditorSceneManager.SaveScene(scene, ScenePath);

            string outDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "look");
            Directory.CreateDirectory(outDir);
            float heroHeight = Bounds(hero).size.y;
            // Teardown 3.1: pitch 55, vFOV 32, hero at ~10% of screen height.
            float distance = heroHeight / (0.10f * 2f * Mathf.Tan(16f * Mathf.Deg2Rad));
            Vector3 focus = hero.transform.position + Vector3.up * heroHeight * 0.5f;
            Capture(null, 55f, 32f, distance, focus);
            Capture(Path.Combine(outDir, "arena-gameplay.png"), 55f, 32f, distance, focus);
            Capture(Path.Combine(outDir, "hud-gameplay.png"), 55f, 32f, distance, focus, withHud: true);
            Capture(Path.Combine(outDir, "arena-closeup.png"), 42f, 32f, distance * 0.45f, focus);

            foreach (var g in Graphs) g.Destroy();
            Graphs.Clear();
            Debug.Log($"[ArenaLab] cell {cell:F2}, hero height {heroHeight:F2}, camera distance {distance:F1}; screenshots in {outDir}");
        }

        // ---------- palette ----------

        /// <summary>Derives a jade stone palette from the kit texture: low-saturation swatches (stone, plaster) become jade, keeping each gradient's value.</summary>
        static Texture2D BuildJadePalette()
        {
            string target = $"{PaletteFolder}/dungeon_texture_jade.png";
            var src = new Texture2D(2, 2);
            src.LoadImage(File.ReadAllBytes($"{KitRoot}/Textures/dungeon_texture.png"));
            var pixels = src.GetPixels();
            Color dark = Hex("#0E1B17"), light = Hex("#9CC7B4");
            for (int i = 0; i < pixels.Length; i++)
            {
                Color.RGBToHSV(pixels[i], out _, out float s, out float v);
                if (s < 0.22f)
                    pixels[i] = Color.Lerp(dark, light, Mathf.SmoothStep(0f, 1f, v));
            }
            var outTex = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
            outTex.SetPixels(pixels);
            Directory.CreateDirectory(PaletteFolder);
            File.WriteAllBytes(target, outTex.EncodeToPNG());
            AssetDatabase.Refresh();
            var importer = (TextureImporter)AssetImporter.GetAtPath(target);
            importer.filterMode = FilterMode.Bilinear;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
            return AssetDatabase.LoadAssetAtPath<Texture2D>(target);
        }

        // ---------- layout ----------

        static void BuildFloor(Transform root)
        {
            var rng = new System.Random(7);
            var floor = new GameObject("Floor").transform;
            floor.SetParent(root);
            for (int x = 0; x < Columns; x++)
            for (int z = 0; z < Rows; z++)
            {
                // Rock tiles only on the outer ring, so the central combat area stays clear (teardown 3.4).
                bool ring = x == 0 || z == 0 || x == Columns - 1 || z == Rows - 1;
                string piece = ring && rng.NextDouble() < 0.35 ? "floor_tile_large_rocks" : "floor_tile_large";
                var go = Place(piece, floor, CellCenter(x, z), stoneFloor);
                go.transform.rotation = Quaternion.Euler(0f, 90f * rng.Next(4), 0f);
            }

            // Second detail scale: a decorated sect seal in the centre and a few cracked patches.
            Place("floor_tile_small_decorated", floor, new Vector3(0f, 0.02f, 0f), stoneFloor);
            foreach (var p in new[] { new Vector3(-cell * 3.5f, 0.02f, cell * 1.5f), new Vector3(cell * 4f, 0.02f, -cell * 2f), new Vector3(cell * 1.5f, 0.02f, cell * 2.6f) })
                Place(rng.NextDouble() < 0.5 ? "floor_tile_small_broken_A" : "floor_tile_small_broken_B", floor, p, stoneFloor);
        }

        static void BuildEdges(Transform root)
        {
            var edges = new GameObject("Edges").transform;
            edges.SetParent(root);
            var rng = new System.Random(11);
            float halfW = Columns * cell * 0.5f, halfD = Rows * cell * 0.5f;
            string[] back = { "wall_half", "wall_half", "wall_half", "wall_half", "wall_half" };
            for (int x = 0; x < Columns; x++)
            {
                float px = -halfW + cell * (x + 0.5f);
                Place(back[rng.Next(back.Length)], edges, new Vector3(px, 0f, halfD), stoneWall, 0f);
                Place("barrier", edges, new Vector3(px, 0f, -halfD), stoneWall, 180f);
            }
            for (int z = 0; z < Rows; z++)
            {
                float pz = -halfD + cell * (z + 0.5f);
                Place("wall_half", edges, new Vector3(-halfW, 0f, pz), stoneWall, 90f);
                Place("wall_half", edges, new Vector3(halfW, 0f, pz), stoneWall, -90f);
            }
            foreach (var c in new[] { new Vector3(-halfW, 0f, halfD), new Vector3(halfW, 0f, halfD), new Vector3(-halfW, 0f, -halfD), new Vector3(halfW, 0f, -halfD) })
                Place("pillar_decorated", edges, c, stoneWall);
        }

        static void BuildPropClusters(Transform root)
        {
            var clusters = new GameObject("PropClusters").transform;
            clusters.SetParent(root);
            float halfW = Columns * cell * 0.5f, halfD = Rows * cell * 0.5f;
            // Back wall: three sect banners between torches.
            string[] banners = { "banner_patternA_green", "banner_patternA_red", "banner_patternA_blue" };
            for (int i = 0; i < 3; i++)
            {
                float x = (i - 1) * halfW * 0.6f;
                Place(banners[i], clusters, new Vector3(x, SizeOf("wall_half").y * 0.95f, halfD - 0.6f), props);
                Place("torch_lit", clusters, new Vector3(x + cell * 0.9f, 0f, halfD - cell * 0.35f), props);
            }
            // Corner clusters keep the centre 70% clear.
            foreach (var (pos, rot) in new[] { (new Vector3(-halfW + cell * 0.8f, 0f, halfD - cell * 0.8f), 20f), (new Vector3(halfW - cell * 0.8f, 0f, -halfD + cell * 0.9f), -30f) })
            {
                Place("rubble_large", clusters, pos, stoneWall, rot);
                Place("candle_triple", clusters, pos + new Vector3(cell * 0.6f, 0f, -cell * 0.3f), props);
            }
            Place("pillar", clusters, new Vector3(halfW - cell * 1.2f, 0f, halfD - cell * 1.1f), stoneWall);
            Place("rubble_half", clusters, new Vector3(-halfW + cell * 1.3f, 0f, -halfD + cell * 1.0f), stoneWall, 60f);
        }

        /// <summary>A KayKit cast frozen mid-fight near the centre; returns the hero.</summary>
        static GameObject BuildCast()
        {
            var cast = new GameObject("Cast").transform;
            var hero = SpawnCharacter("Knight", cast, new Vector3(0f, 0f, -cell * 0.4f), 52f, "Block_Attack");
            SpawnCharacter("Barbarian", cast, new Vector3(cell * 0.9f, 0f, cell * 0.3f), 230f, "2H_Melee_Attack_Chop");
            SpawnCharacter("Rogue", cast, new Vector3(-cell * 1.1f, 0f, cell * 0.5f), 140f, "Dodge_Left");
            SpawnCharacter("Mage", cast, new Vector3(-cell * 2.4f, 0f, -cell * 0.9f), 80f, "Spellcast_Shoot");
            SpawnCharacter("RogueHooded", cast, new Vector3(cell * 2.6f, 0f, -cell * 1.2f), 250f, "1H_Melee_Attack_Chop", 0.2f);
            return hero;
        }

        static readonly Dictionary<string, GameObject> CastByName = new Dictionary<string, GameObject>();

        static GameObject SpawnCharacter(string name, Transform parent, Vector3 position, float yaw, string clip, float phase = 0.45f)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>($"{CastRoot}/Characters/{name}.fbx");
            var go = (GameObject)PrefabUtility.InstantiatePrefab(model, parent);
            go.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, yaw, 0f));
            var material = AssetDatabase.LoadAssetAtPath<Material>($"{MaterialFolder}/Toon_KayKit_{name}.mat");
            if (material != null)
                foreach (var r in go.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();
            Pose(go, AssetDatabase.GetAssetPath(model), clip, phase);
            CastByName[name] = go;
            return go;
        }

        static void Pose(GameObject go, string clipSourcePath, string clipName, float phase)
        {
            var clip = AssetDatabase.LoadAllAssetsAtPath(clipSourcePath).OfType<AnimationClip>().FirstOrDefault(c => c.name == clipName);
            if (clip == null) { Debug.LogWarning($"[ArenaLab] no clip {clipName}"); return; }
            var animator = go.GetComponent<Animator>();
            if (animator == null) animator = go.AddComponent<Animator>(); // Unity fake-null: no ?? here
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.applyRootMotion = false;
            var graph = PlayableGraph.Create($"Pose_{go.name}");
            graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            var playable = AnimationClipPlayable.Create(graph, clip);
            playable.SetTime(clip.length * phase);
            AnimationPlayableOutput.Create(graph, "Pose", animator).SetSourcePlayable(playable);
            graph.Evaluate();
            Graphs.Add(graph);
        }

        /// <summary>
        /// One readable combat beat for the screenshot. The hero (jade ring) counters the Barbarian with a
        /// jade slash, and the Barbarian flashes white with a hit spark. The Mage's Lôi bolt chases the Rogue
        /// out of a red must-dodge circle, while the hooded rogue winds up a parryable chop (gold glint).
        /// </summary>
        static void BuildFeedbackMoment()
        {
            var fx = new GameObject("Feedback").transform;
            var hero = CastByName["Knight"].transform;
            var barbarian = CastByName["Barbarian"].transform;
            var rogue = CastByName["Rogue"].transform;
            var mage = CastByName["Mage"].transform;
            var hooded = CastByName["RogueHooded"].transform;
            var cameraFacing = Quaternion.Euler(55f, 0f, 0f);

            Spawn(FeedbackVfxLibrary.HeroRing, fx, hero.position, Quaternion.identity, 2.6f);

            Spawn(FeedbackVfxLibrary.HeroSlash, fx, hero.position + Vector3.up * 1.1f, Quaternion.Euler(0f, 52f, 0f), 2.1f);
            Vector3 contact = Vector3.Lerp(hero.position, barbarian.position, 0.72f) + Vector3.up * 1.3f;
            Spawn(FeedbackVfxLibrary.HitSpark, fx, contact, cameraFacing, 2.2f);
            var flash = new MaterialPropertyBlock();
            flash.SetFloat("_FlashAmount", 0.7f);
            foreach (var r in barbarian.GetComponentsInChildren<Renderer>())
                r.SetPropertyBlock(flash);

            Vector3 zone = rogue.position + new Vector3(cell * 0.35f, 0f, -cell * 0.1f);
            Spawn(FeedbackVfxLibrary.TelegraphEdge, fx, zone, Quaternion.identity, cell * 1.5f);
            Spawn(FeedbackVfxLibrary.TelegraphFill, fx, zone, Quaternion.identity, cell * 1.5f * 0.7f);

            Spawn(FeedbackVfxLibrary.ParryGlint, fx, hooded.position + Vector3.up * 2.5f + hooded.forward * 0.4f, cameraFacing, 1.8f);

            Vector3 from = mage.position + Vector3.up * 1.9f + mage.forward * 0.8f;
            Vector3 to = rogue.position + Vector3.up * 1.2f;
            Bolt(fx, from, to, 0.5f, 7);
            Bolt(fx, Vector3.Lerp(from, to, 0.45f), to + new Vector3(-0.8f, -0.6f, 1.4f), 0.22f, 11);
            Spawn(FeedbackVfxLibrary.HitSpark, fx, to, cameraFacing, 1.4f);

            DamageNumbers.Clear();
            DamageNumbers.Add((barbarian, "128", Color.white, 2.8f));
            DamageNumbers.Add((rogue, "342!", Hex("#FF9A3C"), 4.2f));
        }

        static void Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, float scale)
        {
            var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
            go.transform.SetPositionAndRotation(position, rotation);
            go.transform.localScale = Vector3.one * scale;
        }

        static void Bolt(Transform parent, Vector3 from, Vector3 to, float width, int seed)
        {
            var rng = new System.Random(seed);
            var line = new GameObject("LoiBolt").AddComponent<LineRenderer>();
            line.transform.SetParent(parent, false);
            const int points = 9;
            line.positionCount = points;
            Vector3 side = Vector3.Cross((to - from).normalized, Vector3.up);
            for (int i = 0; i < points; i++)
            {
                float t = i / (points - 1f);
                float jitter = (i == 0 || i == points - 1) ? 0f : (float)(rng.NextDouble() - 0.5) * 1.1f;
                line.SetPosition(i, Vector3.Lerp(from, to, t) + side * jitter + Vector3.up * jitter * 0.4f);
            }
            line.widthMultiplier = width;
            line.alignment = LineAlignment.View;
            line.textureMode = LineTextureMode.Stretch;
            line.sharedMaterial = FeedbackVfxLibrary.LightningMaterial;
            line.shadowCastingMode = ShadowCastingMode.Off;

            // A thin white core over the lavender glow keeps the bolt readable as Lôi, not generic white.
            var core = Object.Instantiate(line.gameObject, parent).GetComponent<LineRenderer>();
            core.name = "LoiBoltCore";
            core.widthMultiplier = width * 0.3f;
            core.sharedMaterial = FeedbackVfxLibrary.LightningCoreMaterial;
        }

        // ---------- light and post ----------

        static void BuildLighting()
        {
            var key = new GameObject("Key_Warm").AddComponent<Light>();
            key.type = LightType.Directional;
            key.color = Hex("#FFE3C0");
            key.intensity = 1.25f;
            key.shadows = LightShadows.Soft;
            key.transform.rotation = Quaternion.Euler(52f, -30f, 0f);

            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = Hex("#34495A");
            RenderSettings.ambientEquatorColor = Hex("#243238");
            RenderSettings.ambientGroundColor = Hex("#10161A");
            RenderSettings.skybox = null;

            float halfW = Columns * cell * 0.5f, halfD = Rows * cell * 0.5f;
            foreach (var p in new[] { new Vector3(-halfW * 0.6f + cell * 0.9f, 1.8f, halfD - 1.5f), new Vector3(cell * 0.9f, 1.8f, halfD - 1.5f), new Vector3(halfW * 0.6f + cell * 0.9f, 1.8f, halfD - 1.5f), new Vector3(-halfW + 2f, 1.2f, -halfD + 2f) })
            {
                var torch = new GameObject("Accent_Torch").AddComponent<Light>();
                torch.type = LightType.Point;
                torch.color = Hex("#FF9A4A");
                torch.intensity = 3f;
                torch.range = cell * 2.2f;
                torch.transform.position = p;
            }
        }

        static void BuildPostProcessing()
        {
            // Created once; later tuning happens on the asset in the Editor and survives re-runs.
            string path = $"{MaterialFolder}/ArenaLab_Volume.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                var bloom = profile.Add<Bloom>(true);
                bloom.threshold.Override(1.05f);
                bloom.intensity.Override(0.6f);
                var vignette = profile.Add<Vignette>(true);
                vignette.intensity.Override(0.22f);
                var tonemap = profile.Add<Tonemapping>(true);
                tonemap.mode.Override(TonemappingMode.Neutral);
                AssetDatabase.CreateAsset(profile, path);
                foreach (var c in profile.components) AssetDatabase.AddObjectToAsset(c, profile);
                AssetDatabase.SaveAssets();
            }

            var volume = new GameObject("PostProcess").AddComponent<Volume>();
            volume.isGlobal = true;
            volume.sharedProfile = profile;
        }

        // ---------- helpers ----------

        static Vector3 CellCenter(int x, int z) => new Vector3((x - (Columns - 1) * 0.5f) * cell, 0f, (z - (Rows - 1) * 0.5f) * cell);

        static GameObject Place(string piece, Transform parent, Vector3 position, Material material, float yaw = 0f)
        {
            var model = AssetDatabase.LoadAssetAtPath<GameObject>($"{KitRoot}/Models/{piece}.fbx");
            if (model == null) { Debug.LogWarning($"[ArenaLab] missing piece {piece}"); return new GameObject(piece); }
            var go = (GameObject)PrefabUtility.InstantiatePrefab(model, parent);
            go.transform.SetPositionAndRotation(position, Quaternion.Euler(0f, yaw, 0f));
            foreach (var r in go.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(material, r.sharedMaterials.Length).ToArray();
            return go;
        }

        static Vector3 SizeOf(string piece)
        {
            var go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>($"{KitRoot}/Models/{piece}.fbx"));
            var size = Bounds(go).size;
            Object.DestroyImmediate(go);
            return size;
        }

        static Bounds Bounds(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            var b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return b;
        }

        static void Capture(string file, float pitch, float fov, float distance, Vector3 focus, bool withHud = false)
        {
            var cam = new GameObject("CaptureCamera").AddComponent<Camera>();
            cam.fieldOfView = fov;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Hex("#0B0D12");
            cam.transform.rotation = Quaternion.Euler(pitch, 0f, 0f);
            cam.transform.position = focus - cam.transform.forward * distance;
            cam.farClipPlane = 300f;
            var data = cam.GetUniversalAdditionalCameraData();
            data.renderPostProcessing = true;
            data.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;

            var rt = new RenderTexture(1920, 1080, 24);
            cam.targetTexture = rt;
            var labels = new List<GameObject>();
            if (hud != null)
            {
                hud.SetActive(withHud);
                var canvas = hud.GetComponent<Canvas>();
                canvas.worldCamera = cam;
                if (withHud)
                    foreach (var (target, text, color, sizePct) in DamageNumbers)
                        labels.Add(DamageLabel(hud.transform, cam, target, text, color, sizePct));
                Canvas.ForceUpdateCanvases();
            }
            cam.Render();
            foreach (var label in labels) Object.DestroyImmediate(label);
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

        /// <summary>A screen-space damage number above a character's head (teardown 3.6).</summary>
        static GameObject DamageLabel(Transform canvas, Camera cam, Transform target, string text, Color color, float sizePct)
        {
            var go = new GameObject($"Damage_{text}", typeof(RectTransform));
            var rt = (RectTransform)go.transform;
            rt.SetParent(canvas, false);
            Vector3 vp = cam.WorldToViewportPoint(target.position + Vector3.up * 3.1f);
            rt.anchorMin = rt.anchorMax = new Vector2(vp.x, vp.y);
            rt.sizeDelta = new Vector2(300f, 80f);
            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = sizePct * 0.01f * 1080f;
            tmp.fontStyle = FontStyles.Bold;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.outlineWidth = 0.25f;
            tmp.outlineColor = new Color32(12, 10, 16, 255);
            return go;
        }

        static Material LitMaterial(string name, Texture texture, Color tint, float smoothness)
        {
            if (!AssetDatabase.IsValidFolder(MaterialFolder))
                AssetDatabase.CreateFolder("Assets/_Project/Materials", "Look");
            string path = $"{MaterialFolder}/{name}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(material, path);
            }
            material.SetTexture("_BaseMap", texture);
            material.SetColor("_BaseColor", tint);
            material.SetFloat("_Smoothness", smoothness);
            material.SetFloat("_Metallic", 0f);
            EditorUtility.SetDirty(material);
            return material;
        }

        static Color Hex(string hex) => ColorUtility.TryParseHtmlString(hex, out var c) ? c : Color.magenta;
    }
}
