using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1d icon pass: swaps the placeholder skill glyphs in BrawlHud.prefab for the
    /// Director's ChatGPT icons in UI/HUD/Icons. The icons carry their own colour, so the
    /// element tint is cleared. Idempotent; edits the prefab through PrefabUtility.
    /// </summary>
    public static class HudIconPass
    {
        const string IconRoot = "Assets/_Project/UI/HUD/Icons";

        static readonly (string button, string file)[] Icons =
        {
            ("Btn_Attack", "icon_attack_sword"),
            ("Btn_PhongBo", "icon_phongbo_wind"),
            ("Btn_LoiTram", "icon_loitram_bolt"),
            ("Btn_HoThe", "icon_hothe_shield"),
        };

        public static void Apply()
        {
            var root = PrefabUtility.LoadPrefabContents(BrawlHudBuilder.PrefabPath);
            try
            {
                foreach (var (button, file) in Icons)
                {
                    var sprite = ImportSprite($"{IconRoot}/{file}.png");
                    var icon = root.transform.Find($"{button}/Icon")?.GetComponent<Image>();
                    if (sprite == null || icon == null || icon.sprite == sprite) continue; // already applied: avoid float churn
                    icon.sprite = sprite;
                    icon.color = Color.white;
                    icon.preserveAspect = true;
                    // The painted icons fill their canvas more than the glyphs did: a slightly smaller inset.
                    var rt = icon.rectTransform;
                    float inset = ((RectTransform)rt.parent).sizeDelta.x * 0.14f;
                    rt.offsetMin = new Vector2(inset, inset);
                    rt.offsetMax = new Vector2(-inset, -inset);
                }
                PrefabUtility.SaveAsPrefabAsset(root, BrawlHudBuilder.PrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(root);
            }
        }

        static Sprite ImportSprite(string path)
        {
            if (!(AssetImporter.GetAtPath(path) is TextureImporter importer)) return null;
            if (importer.textureType != TextureImporterType.Sprite || importer.maxTextureSize != 256)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.alphaIsTransparency = true;
                importer.mipmapEnabled = false;
                importer.maxTextureSize = 256; // shown at <= 205 px on a 1080p screen
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
    }
}
