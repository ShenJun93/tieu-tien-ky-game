using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1d brawl HUD (uGUI + TextMeshPro, Unity's 6.3 runtime-UI recommendation). It builds
    /// Assets/_Project/UI/HUD/BrawlHud.prefab and its sprites once. After that the prefab is
    /// edited in the Editor, and re-runs never overwrite it. The layout follows teardown §3.7:
    /// positions are (x%, y% from top) of a 1920x1080 reference, and sizes are % of screen height.
    /// The skill icons are placeholder glyphs until the Director's ChatGPT icons replace them.
    /// </summary>
    public static class BrawlHudBuilder
    {
        const string Root = "Assets/_Project/UI/HUD";
        public const string PrefabPath = Root + "/BrawlHud.prefab";
        const float RefW = 1920f, RefH = 1080f;

        static readonly Color Panel = Hex("#12151CCC");
        static readonly Color RingTop = Hex("#E2C27A"), RingBottom = Hex("#6A5530");
        static readonly Color Jade = Hex("#3FD1C0"), Crimson = Hex("#C0395A"), Azure = Hex("#5B8CFF");
        static readonly Color Lavender = Hex("#B9A6FF"), Gold = Hex("#F5C542");

        public static GameObject Prefab
        {
            get
            {
                var existing = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
                if (existing != null) return existing;
                Folder(Root);
                var go = Build();
                var prefab = PrefabUtility.SaveAsPrefabAsset(go, PrefabPath);
                Object.DestroyImmediate(go);
                return prefab;
            }
        }

        static GameObject Build()
        {
            var root = new GameObject("BrawlHud", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.planeDistance = 1f;
            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(RefW, RefH);
            scaler.matchWidthOrHeight = 1f;

            // Floating joystick with a ghost ring (14%, 76%), 22% of height. OnScreenStick drives <Gamepad>/leftStick.
            var stickBase = Circle(root.transform, "Joystick", 14f, 76f, 22f, MakeSprite("joy_ring", RingOnly(0.9f, 0.06f)), new Color(1f, 1f, 1f, 0.35f));
            var knob = Circle(stickBase.transform, "Knob", 0f, 0f, 9f, MakeSprite("disc", Disc()), new Color(1f, 1f, 1f, 0.55f), local: true);
            var stick = knob.AddComponent<OnScreenStick>();
            stick.controlPath = "<Gamepad>/leftStick";
            stick.movementRange = 90f;
            stick.behaviour = OnScreenStick.Behaviour.ExactPositionWithDynamicOrigin;

            SkillButton(root.transform, "Btn_Attack", 90f, 79f, 19f, MakeSprite("icon_sword", Sword()), Color.white, "<Gamepad>/buttonSouth", 0f, null);
            SkillButton(root.transform, "Btn_PhongBo", 80f, 88f, 13f, MakeSprite("icon_wind", Wind()), Jade, "<Gamepad>/buttonEast", 0f, null);
            SkillButton(root.transform, "Btn_LoiTram", 80f, 64f, 13f, MakeSprite("icon_bolt", Bolt()), Lavender, "<Gamepad>/buttonNorth", 0.62f, "4");
            SkillButton(root.transform, "Btn_HoThe", 91f, 56f, 13f, MakeSprite("icon_shield", Shield()), Gold, "<Gamepad>/buttonWest", 0f, null);

            BuildPlayerPanel(root.transform);
            BuildMatchPanel(root.transform);
            var pause = Circle(root.transform, "Btn_Pause", 96.5f, 6.5f, 6.5f, MakeSprite("disc", Disc()), Panel);
            Label(pause.transform, "II", 30, Color.white);
            return root;
        }

        // ---------- widgets ----------

        static void SkillButton(Transform parent, string name, float x, float y, float sizePct, Sprite icon, Color tint, string control, float cooldown, string seconds)
        {
            var button = Circle(parent, name, x, y, sizePct, MakeSprite("disc", Disc()), Panel);
            button.AddComponent<OnScreenButton>().controlPath = control;
            Stretch(AddImage(button.transform, "Ring", MakeSprite("metal_ring", MetalRing())), 0f);
            var glyph = Stretch(AddImage(button.transform, "Icon", icon), sizePct * RefH * 0.01f * 0.2f);
            glyph.color = tint;
            // Radial cooldown sweep with seconds in the centre (teardown 3.7).
            var sweep = Stretch(AddImage(button.transform, "Cooldown", MakeSprite("disc", Disc())), sizePct * RefH * 0.01f * 0.06f);
            sweep.color = new Color(0f, 0f, 0f, 0.62f);
            sweep.type = UnityEngine.UI.Image.Type.Filled;
            sweep.fillMethod = UnityEngine.UI.Image.FillMethod.Radial360;
            sweep.fillOrigin = (int)UnityEngine.UI.Image.Origin360.Top;
            sweep.fillClockwise = false;
            sweep.fillAmount = cooldown;
            sweep.gameObject.SetActive(cooldown > 0f);
            if (seconds != null)
                Label(button.transform, seconds, Mathf.RoundToInt(sizePct * RefH * 0.01f * 0.42f), Color.white);
        }

        static void BuildPlayerPanel(Transform parent)
        {
            var panel = MakeRect(parent, "PlayerPanel", new Vector2(0f, 1f), new Vector2(36f, -30f), new Vector2(430f, 96f), pivotX: 0f);
            Stretch(AddImage(panel, "Bg", MakeSprite("rounded", Rounded())), 0f).color = Panel;
            var portrait = MakeRect(panel, "Portrait", new Vector2(0f, 0.5f), new Vector2(14f, 0f), new Vector2(72f, 72f), pivotX: 0f);
            Stretch(AddImage(portrait, "Disc", MakeSprite("disc", Disc())), 0f).color = Jade * 0.6f + Color.black * 0.4f;
            Stretch(AddImage(portrait, "Ring", MakeSprite("metal_ring", MetalRing())), 0f);
            var barBg = MakeRect(panel, "HpBar", new Vector2(0f, 0.5f), new Vector2(100f, -14f), new Vector2(310f, 22f), pivotX: 0f);
            Stretch(AddImage(barBg, "Back", MakeSprite("rounded", Rounded())), 0f).color = new Color(0f, 0f, 0f, 0.7f);
            var fill = Stretch(AddImage(barBg, "Fill", MakeSprite("rounded", Rounded())), 3f);
            fill.color = Jade;
            fill.type = UnityEngine.UI.Image.Type.Filled;
            fill.fillMethod = UnityEngine.UI.Image.FillMethod.Horizontal;
            fill.fillAmount = 0.82f;
            var hp = Label(barBg, "820 / 1000", 18, Color.white);
            hp.fontStyle = FontStyles.Bold;
            var nameLabel = Label(MakeRect(panel, "Name", new Vector2(0f, 0.5f), new Vector2(100f, 20f), new Vector2(310f, 32f), pivotX: 0f), "Lv 7", 24, Hex("#E2C27A"));
            nameLabel.alignment = TextAlignmentOptions.Left;
        }

        static void BuildMatchPanel(Transform parent)
        {
            var panel = MakeRect(parent, "MatchPanel", new Vector2(0.5f, 1f), new Vector2(0f, -24f), new Vector2(420f, 118f));
            Stretch(AddImage(panel, "Bg", MakeSprite("rounded", Rounded())), 0f).color = Panel;
            var timer = Label(MakeRect(panel, "Timer", new Vector2(0.5f, 1f), new Vector2(0f, -8f), new Vector2(200f, 52f)), "3:48", 44, Color.white);
            timer.fontStyle = FontStyles.Bold;
            var sects = new[] { (Jade, "12"), (Crimson, "9"), (Azure, "7") };
            for (int i = 0; i < sects.Length; i++)
            {
                var pill = MakeRect(panel, $"Sect{i}", new Vector2(0.5f, 0f), new Vector2((i - 1) * 130f, 30f), new Vector2(112f, 42f));
                Stretch(AddImage(pill, "Bg", MakeSprite("rounded", Rounded())), 0f).color = new Color(0f, 0f, 0f, 0.55f);
                var dot = MakeRect(pill, "Dot", new Vector2(0f, 0.5f), new Vector2(10f, 0f), new Vector2(26f, 26f), pivotX: 0f);
                Stretch(AddImage(dot, "Disc", MakeSprite("disc", Disc())), 0f).color = sects[i].Item1;
                var score = Label(MakeRect(pill, "Score", new Vector2(0f, 0.5f), new Vector2(44f, 0f), new Vector2(60f, 42f), pivotX: 0f), sects[i].Item2, 28, Color.white);
                score.alignment = TextAlignmentOptions.Left;
                score.fontStyle = FontStyles.Bold;
            }
        }

        // ---------- rect helpers ----------

        static GameObject Circle(Transform parent, string name, float xPct, float yPct, float sizePct, Sprite sprite, Color color, bool local = false)
        {
            float size = sizePct * 0.01f * RefH;
            var rt = local
                ? MakeRect(parent, name, new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(size, size))
                : MakeRect(parent, name, new Vector2(xPct * 0.01f, 1f - yPct * 0.01f), Vector2.zero, new Vector2(size, size));
            var img = rt.gameObject.AddComponent<Image>();
            img.sprite = sprite;
            img.color = color;
            return rt.gameObject;
        }

        static RectTransform MakeRect(Transform parent, string name, Vector2 anchor, Vector2 pos, Vector2 size, float pivotX = 0.5f)
        {
            var rt = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = rt.anchorMax = anchor;
            rt.pivot = new Vector2(pivotX, anchor.y >= 1f ? 1f : anchor.y <= 0f ? 0f : 0.5f);
            rt.anchoredPosition = pos;
            rt.sizeDelta = size;
            return rt;
        }

        static Image AddImage(Transform parent, string name, Sprite sprite)
        {
            var rt = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            var img = rt.gameObject.AddComponent<Image>();
            img.sprite = sprite;
            img.raycastTarget = false;
            if (sprite != null && sprite.border != Vector4.zero) img.type = UnityEngine.UI.Image.Type.Sliced;
            return img;
        }

        static Image Stretch(Image img, float inset)
        {
            var rt = img.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = new Vector2(inset, inset);
            rt.offsetMax = new Vector2(-inset, -inset);
            return img;
        }

        static TextMeshProUGUI Label(Transform parent, string text, int size, Color color)
        {
            var rt = new GameObject("Label", typeof(RectTransform)).GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            var tmp = rt.gameObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;
            tmp.outlineWidth = 0.18f;
            tmp.outlineColor = new Color32(10, 10, 14, 255);
            return tmp;
        }

        // ---------- procedural sprites (256 px, 2x2 supersampled) ----------

        delegate float Shape(Vector2 p); // p in [-1,1]^2, y up; returns coverage 0..1
        delegate Color ColorShape(Vector2 p);

        static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        static Sprite MakeSprite(string name, Shape shape) => MakeSprite(name, p => new Color(1f, 1f, 1f, shape(p)));

        static Sprite MakeSprite(string name, ColorShape shape)
        {
            if (Cache.TryGetValue(name, out var cached) && cached != null) return cached;
            string path = $"{Root}/Sprites/hud_{name}.png";
            if (!File.Exists(path))
            {
                Folder($"{Root}/Sprites");
                const int size = 256;
                var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    Color acc = Color.clear;
                    for (int sy = 0; sy < 2; sy++)
                    for (int sx = 0; sx < 2; sx++)
                    {
                        var p = new Vector2((x + 0.25f + sx * 0.5f) / size * 2f - 1f, (y + 0.25f + sy * 0.5f) / size * 2f - 1f);
                        acc += shape(p);
                    }
                    tex.SetPixel(x, y, acc / 4f);
                }
                File.WriteAllBytes(path, tex.EncodeToPNG());
                Object.DestroyImmediate(tex);
                AssetDatabase.ImportAsset(path);
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                if (name == "rounded") importer.spriteBorder = new Vector4(48, 48, 48, 48);
                importer.SaveAndReimport();
            }
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            Cache[name] = sprite;
            return sprite;
        }

        static Shape Disc() => p => p.magnitude <= 0.98f ? 1f : 0f;

        static Shape RingOnly(float radius, float width) => p => Mathf.Abs(p.magnitude - radius) <= width ? 1f : 0f;

        static ColorShape MetalRing() => p =>
        {
            float r = p.magnitude;
            if (r > 0.98f || r < 0.86f) return Color.clear;
            var c = Color.Lerp(RingBottom, RingTop, (p.y + 1f) * 0.5f);
            if (r > 0.95f || r < 0.885f) c *= 0.55f; // dark bevel edges
            c.a = 1f;
            return c;
        };

        static Shape Rounded() => p =>
        {
            var q = new Vector2(Mathf.Max(Mathf.Abs(p.x) - 0.6f, 0f), Mathf.Max(Mathf.Abs(p.y) - 0.6f, 0f));
            return q.magnitude <= 0.38f ? 1f : 0f;
        };

        static Shape Poly(params Vector2[] pts) => p => Inside(p, pts) ? 1f : 0f;

        static Shape Union(params Shape[] shapes) => p => { float a = 0f; foreach (var s in shapes) a = Mathf.Max(a, s(p)); return a; };

        static Shape Bolt() => Poly(new Vector2(0.15f, 0.85f), new Vector2(-0.45f, 0.0f), new Vector2(-0.02f, 0.02f), new Vector2(-0.2f, -0.85f), new Vector2(0.45f, 0.1f), new Vector2(0.02f, 0.08f));

        static Shape Wind() => Union(
            Poly(new Vector2(-0.75f, 0.55f), new Vector2(-0.45f, 0.55f), new Vector2(-0.05f, 0f), new Vector2(-0.45f, -0.55f), new Vector2(-0.75f, -0.55f), new Vector2(-0.35f, 0f)),
            Poly(new Vector2(-0.15f, 0.55f), new Vector2(0.15f, 0.55f), new Vector2(0.55f, 0f), new Vector2(0.15f, -0.55f), new Vector2(-0.15f, -0.55f), new Vector2(0.25f, 0f)),
            Poly(new Vector2(0.45f, 0.3f), new Vector2(0.62f, 0.3f), new Vector2(0.85f, 0f), new Vector2(0.62f, -0.3f), new Vector2(0.45f, -0.3f), new Vector2(0.68f, 0f)));

        static Shape Shield() => p =>
        {
            float outer = Inside(p, new[] { new Vector2(-0.62f, 0.7f), new Vector2(0.62f, 0.7f), new Vector2(0.62f, 0.05f), new Vector2(0f, -0.82f), new Vector2(-0.62f, 0.05f) }) ? 1f : 0f;
            float inner = Inside(p, new[] { new Vector2(-0.42f, 0.52f), new Vector2(0.42f, 0.52f), new Vector2(0.42f, 0.08f), new Vector2(0f, -0.56f), new Vector2(-0.42f, 0.08f) }) ? 1f : 0f;
            float boss = Mathf.Abs(p.x) < 0.07f && p.y < 0.52f && p.y > -0.5f ? 1f : 0f;
            return Mathf.Max(outer - inner, boss);
        };

        static Shape Sword() => p =>
        {
            var q = new Vector2((p.x - p.y) * 0.7071f, (p.x + p.y) * 0.7071f); // rotate 45 degrees
            bool blade = Mathf.Abs(q.x) < 0.11f && q.y > -0.25f && q.y < 0.78f - Mathf.Abs(q.x) * 1.5f;
            bool guard = Mathf.Abs(q.x) < 0.36f && q.y > -0.36f && q.y < -0.25f;
            bool grip = Mathf.Abs(q.x) < 0.06f && q.y > -0.72f && q.y < -0.36f;
            bool pommel = (q - new Vector2(0f, -0.78f)).magnitude < 0.1f;
            return blade || guard || grip || pommel ? 1f : 0f;
        };

        static bool Inside(Vector2 p, Vector2[] poly)
        {
            bool inside = false;
            for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++)
                if ((poly[i].y > p.y) != (poly[j].y > p.y) &&
                    p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
                    inside = !inside;
            return inside;
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
