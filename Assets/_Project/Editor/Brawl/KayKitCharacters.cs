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
            def.Animations = FighterController(cast);
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

        static readonly string[] OneHanded = { "1H_Melee_Attack_Slice_Diagonal", "1H_Melee_Attack_Slice_Horizontal", "1H_Melee_Attack_Chop" };
        static readonly string[] TwoHanded = { "2H_Melee_Attack_Slice", "2H_Melee_Attack_Stab", "2H_Melee_Attack_Chop" };

        /// <summary>
        /// The <see cref="FighterView"/> animator contract for a KayKit body: a Speed blend for
        /// Locomotion, three combo swings, Hit and Death. Swing clips are sped up so each fills
        /// its combo step's length in the sim.
        /// </summary>
        static AnimatorController FighterController(string cast)
        {
            string path = $"{ControllerFolder}/{cast}_Fighter.controller";
            var existing = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (existing != null) return existing;
            EnsureFolder(ControllerFolder);
            AssetDatabase.DeleteAsset($"{ControllerFolder}/{cast}_Locomotion.controller"); // CORE-1a controller, superseded

            var clips = AssetDatabase.LoadAllAssetsAtPath($"{CastRoot}/Characters/{cast}.fbx").OfType<AnimationClip>()
                .Where(c => !c.name.StartsWith("__preview__")).ToDictionary(c => c.name);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
            var machine = controller.layers[0].stateMachine;

            var locomotion = controller.CreateBlendTreeInController(FighterView.LocomotionState, out BlendTree tree);
            tree.blendParameter = "Speed";
            tree.useAutomaticThresholds = false;
            tree.AddChild(clips["Idle"], 0f);
            tree.AddChild(clips["Running_A"], 1f);
            machine.defaultState = locomotion;

            var combo = new CombatConfig().Combo;
            var swings = cast == "Barbarian" || cast == "Mage" ? TwoHanded : OneHanded;
            for (int i = 0; i < combo.Length; i++)
            {
                var clip = clips[swings[i]];
                var state = machine.AddState(FighterView.AttackState(i + 1));
                state.motion = clip;
                state.speed = clip.length / (combo[i].Total / (float)new CombatConfig().TickRate);
            }
            machine.AddState(FighterView.HitState).motion = clips["Hit_A"];
            machine.AddState(FighterView.DeathState).motion = clips["Death_A"];
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
