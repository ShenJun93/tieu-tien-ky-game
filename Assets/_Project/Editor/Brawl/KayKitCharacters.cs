using System.Linq;
using TieuTienKy.Brawl;
using TieuTienKy.Combat;
using TieuTienKy.EditorTools.Look;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace TieuTienKy.EditorTools.Brawl
{
    /// <summary>
    /// Placeholder <see cref="CharacterDefinition"/>s for the KayKit cast (ADR 008). All the KayKit
    /// specifics live here: loadout (child visibility), palette re-hue per sect, generic-rig clips.
    /// A pipeline hero gets its own definition; nothing outside this file changes.
    /// </summary>
    public static class KayKitCharacters
    {
        const string CastRoot = "Assets/ThirdParty/KayKit/Adventurers";
        const string LookMaterials = "Assets/_Project/Materials/Look";
        const string ControllerFolder = "Assets/_Project/Animation/Brawl";
        const string CharacterFolder = "Assets/_Project/Characters/KayKit";

        /// <summary>Creates or refreshes the definition for <paramref name="cast"/>, with a skin for each team given.</summary>
        public static CharacterDefinition Ensure(string cast, params Team[] teams)
        {
            EnsureFolder(CharacterFolder);
            string path = $"{CharacterFolder}/{cast}.asset";
            var def = AssetDatabase.LoadAssetAtPath<CharacterDefinition>(path);
            if (def == null)
            {
                def = ScriptableObject.CreateInstance<CharacterDefinition>();
                AssetDatabase.CreateAsset(def, path);
            }

            def.CharacterId = $"kaykit.{cast.ToLowerInvariant()}";
            def.Prefab = EnsurePrefab(cast);
            def.Animations = LocomotionController(cast);
            def.Height = HeightOf(def.Prefab);
            def.BodyRadius = 0.9f;
            def.WeaponSocket = FindPath(def.Prefab.transform, "handslot.r");
            def.Source = "KayKit Character Pack: Adventurers 1.0 (Kay Lousberg), placeholder until the ADR 008 pipeline hero";
            def.License = "CC0-1.0";

            var template = AssetDatabase.LoadAssetAtPath<Material>($"{LookMaterials}/Toon_KayKit_{cast}.mat");
            var skins = def.SectSkins.ToList();
            foreach (var team in teams)
            {
                skins.RemoveAll(s => s.Team == team);
                skins.Add(new CharacterDefinition.SectSkin { Team = team, Material = SectLook.SectMaterial(cast, (SectLook.Sect)(int)team, template) });
            }
            def.SectSkins = skins.OrderBy(s => s.Team).ToArray();
            EditorUtility.SetDirty(def);
            AssetDatabase.SaveAssets();
            return def;
        }

        /// <summary>A prefab variant of the FBX with the xianxia loadout applied (one weapon, no Western hats).</summary>
        static GameObject EnsurePrefab(string cast)
        {
            string path = $"{CharacterFolder}/{cast}.prefab";
            var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (existing != null) return existing;

            var model = AssetDatabase.LoadAssetAtPath<GameObject>($"{CastRoot}/Characters/{cast}.fbx");
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(model);
            SectLook.ApplyLoadout(instance, cast);
            var prefab = PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);
            return prefab;
        }

        /// <summary>Idle ↔ Running_A driven by Speed, the animation contract of <see cref="CharacterDefinition"/>.</summary>
        static AnimatorController LocomotionController(string cast)
        {
            string path = $"{ControllerFolder}/{cast}_Locomotion.controller";
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (existing != null) return existing;
            EnsureFolder(ControllerFolder);

            var clips = AssetDatabase.LoadAllAssetsAtPath($"{CastRoot}/Characters/{cast}.fbx").OfType<AnimationClip>().ToList();
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            var state = controller.CreateBlendTreeInController("Locomotion", out BlendTree tree);
            tree.blendParameter = "Speed";
            tree.useAutomaticThresholds = false;
            tree.AddChild(clips.First(c => c.name == "Idle"), 0f);
            tree.AddChild(clips.First(c => c.name == "Running_A"), 1f);
            controller.layers[0].stateMachine.defaultState = state;
            return controller;
        }

        static string FindPath(Transform root, string name)
        {
            var hit = root.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t.name == name);
            return hit == null ? null : AnimationUtility.CalculateTransformPath(hit, root);
        }

        /// <summary>Measured on a scene instance: renderer bounds on a prefab asset are not reliable.</summary>
        static float HeightOf(GameObject prefab)
        {
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            try
            {
                var renderers = instance.GetComponentsInChildren<Renderer>(false);
                if (renderers.Length == 0) return 2.5f;
                var b = renderers[0].bounds;
                foreach (var r in renderers) b.Encapsulate(r.bounds);
                return b.size.y;
            }
            finally { Object.DestroyImmediate(instance); }
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path)) return;
            int slash = path.LastIndexOf('/');
            EnsureFolder(path.Substring(0, slash));
            AssetDatabase.CreateFolder(path.Substring(0, slash), path.Substring(slash + 1));
        }
    }
}
