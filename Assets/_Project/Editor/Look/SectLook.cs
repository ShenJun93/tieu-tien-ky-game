using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TieuTienKy.EditorTools.Look
{
    /// <summary>
    /// LOOK-1e: sect identity and xianxia loadouts for the KayKit cast.
    /// - Loadouts show one weapon per fighter. The FBXs ship every weapon and shield enabled.
    ///   Western tells (wizard hat, bear hat, knight helm, crossbows, spellbooks, mug) are hidden.
    /// - Sect palettes are derived textures. Saturated clothing hues are rotated to the sect hue;
    ///   skin, hair and low-saturation metal keep their colours.
    /// </summary>
    public static class SectLook
    {
        public enum Sect { Jade, Crimson, Azure }

        const string CastRoot = "Assets/ThirdParty/KayKit/Adventurers";
        const string PaletteFolder = "Assets/_Project/Art/Palettes/Sects";
        const string MaterialFolder = "Assets/_Project/Materials/Look";

        static readonly Dictionary<Sect, float> SectHue = new Dictionary<Sect, float>
        {
            { Sect.Jade, 170f / 360f },    // #3FD1C0 family (hero sect)
            { Sect.Crimson, 348f / 360f }, // #C0395A family
            { Sect.Azure, 222f / 360f },   // #5B8CFF family
        };

        static readonly Dictionary<string, string[]> Keep = new Dictionary<string, string[]>
        {
            { "Knight", new[] { "Knight_ArmLeft", "Knight_ArmRight", "Knight_Body", "Knight_Head", "Knight_LegLeft", "Knight_LegRight", "Knight_Cape", "1H_Sword" } },
            { "Barbarian", new[] { "Barbarian_ArmLeft", "Barbarian_ArmRight", "Barbarian_Body", "Barbarian_Head", "Barbarian_LegLeft", "Barbarian_LegRight", "Barbarian_Cape", "2H_Axe" } },
            { "Mage", new[] { "Mage_ArmLeft", "Mage_ArmRight", "Mage_Body", "Mage_Head", "Mage_LegLeft", "Mage_LegRight", "Mage_Cape", "2H_Staff" } },
            { "Rogue", new[] { "Rogue_ArmLeft", "Rogue_ArmRight", "Rogue_Body", "Rogue_Head", "Rogue_LegLeft", "Rogue_LegRight", "Rogue_Cape", "Knife", "Knife_Offhand" } },
            { "RogueHooded", new[] { "Rogue_ArmLeft", "Rogue_ArmRight", "Rogue_Body", "Rogue_Head_Hooded", "Rogue_LegLeft", "Rogue_LegRight", "Rogue_Cape", "Knife", "Knife_Offhand" } },
        };

        public static void ApplyLoadout(GameObject character, string castName)
        {
            if (!Keep.TryGetValue(castName, out var keep)) return;
            foreach (var r in character.GetComponentsInChildren<Renderer>(true))
                r.gameObject.SetActive(keep.Contains(r.name));
        }

        /// <summary>Toon material with the character's palette re-hued to the sect colour.</summary>
        public static Material SectMaterial(string castName, Sect sect, Material toonTemplate)
        {
            string texName = castName.StartsWith("Rogue") ? "rogue_texture" : $"{castName.ToLowerInvariant()}_texture";
            string matPath = $"{MaterialFolder}/Toon_{castName}_{sect}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (material == null)
            {
                material = new Material(toonTemplate);
                AssetDatabase.CreateAsset(material, matPath);
            }
            material.SetTexture("_BaseMap", SectTexture(texName, sect));
            EditorUtility.SetDirty(material);
            return material;
        }

        static Texture2D SectTexture(string texName, Sect sect)
        {
            string target = $"{PaletteFolder}/{texName}_{sect.ToString().ToLowerInvariant()}.png";
            if (!File.Exists(target))
            {
                var src = new Texture2D(2, 2);
                src.LoadImage(File.ReadAllBytes($"{CastRoot}/Textures/{texName}.png"));
                var px = src.GetPixels();
                float hue = SectHue[sect];
                for (int i = 0; i < px.Length; i++)
                {
                    Color.RGBToHSV(px[i], out float h, out float s, out float v);
                    bool skinOrHair = h > 0.02f && h < 0.12f && s < 0.75f; // warm oranges and browns
                    if (s < 0.28f || skinOrHair) continue;               // metal, cloth whites, skin, hair keep
                    var c = Color.HSVToRGB(hue, Mathf.Clamp01(s * 0.95f), v);
                    c.a = px[i].a;
                    px[i] = c;
                }
                var outTex = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
                outTex.SetPixels(px);
                Directory.CreateDirectory(PaletteFolder);
                File.WriteAllBytes(target, outTex.EncodeToPNG());
                AssetDatabase.ImportAsset(target);
                var importer = (TextureImporter)AssetImporter.GetAtPath(target);
                importer.filterMode = FilterMode.Bilinear;
                importer.mipmapEnabled = false;
                importer.SaveAndReimport();
            }
            return AssetDatabase.LoadAssetAtPath<Texture2D>(target);
        }
    }
}
