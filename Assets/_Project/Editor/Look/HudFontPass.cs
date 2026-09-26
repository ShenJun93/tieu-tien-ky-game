using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1d font pass. It creates dynamic TMP font assets for the OFL Vietnamese fonts and
    /// points the BrawlHud prefab's text at them, then adds Vietnamese labels: a realm title and
    /// a kill feed. The pass is idempotent and edits the prefab through PrefabUtility, not YAML.
    /// Dynamic atlases add Vietnamese glyphs on demand. Shipping builds should bake a static atlas
    /// from the full Vietnamese character list (research 02 §12).
    /// </summary>
    public static class HudFontPass
    {
        const string FontRoot = "Assets/_Project/UI/Fonts";
        public const string BodyFontPath = FontRoot + "/BeVietnamPro-Bold SDF.asset";
        public const string TitleFontPath = FontRoot + "/Philosopher-Bold SDF.asset";

        public static TMP_FontAsset Body => FontAsset("BeVietnamPro-Bold", BodyFontPath);
        public static TMP_FontAsset Title => FontAsset("Philosopher-Bold", TitleFontPath);

        public static void Apply()
        {
            var body = Body;
            var title = Title;
            var root = PrefabUtility.LoadPrefabContents(BrawlHudBuilder.PrefabPath);
            try
            {
                foreach (var tmp in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                    tmp.font = body;

                var name = root.transform.Find("PlayerPanel/Name")?.GetComponentInChildren<TextMeshProUGUI>();
                if (name != null)
                {
                    name.font = title;
                    name.text = "Thanh Vân <size=70%><color=#9FB8AE>· Trúc Cơ III</color></size>";
                }

                if (root.transform.Find("KillFeed") == null)
                {
                    var feed = new GameObject("KillFeed", typeof(RectTransform));
                    var rt = (RectTransform)feed.transform;
                    rt.SetParent(root.transform, false);
                    rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(1f, 1f);
                    rt.anchoredPosition = new Vector2(-150f, -40f);
                    rt.sizeDelta = new Vector2(520f, 40f);
                    var text = feed.AddComponent<TextMeshProUGUI>();
                    text.font = body;
                    text.fontSize = 24;
                    text.alignment = TextAlignmentOptions.Right;
                    text.raycastTarget = false;
                    text.outlineWidth = 0.2f;
                    text.outlineColor = new Color32(10, 10, 14, 255);
                    text.text = "<color=#3FD1C0>Thanh Vân</color> hạ gục <color=#C0395A>Hắc Hổ</color>";
                }
                PrefabUtility.SaveAsPrefabAsset(root, BrawlHudBuilder.PrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        static TMP_FontAsset FontAsset(string fontName, string assetPath)
        {
            var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
            if (existing != null) return existing;
            var font = AssetDatabase.LoadAssetAtPath<Font>($"{FontRoot}/{fontName}.ttf");
            var asset = TMP_FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic, true);
            asset.name = $"{fontName} SDF";
            AssetDatabase.CreateAsset(asset, assetPath);
            asset.atlasTextures[0].name = $"{fontName} Atlas";
            AssetDatabase.AddObjectToAsset(asset.atlasTextures[0], asset);
            asset.material.name = $"{fontName} Material";
            AssetDatabase.AddObjectToAsset(asset.material, asset);
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}
