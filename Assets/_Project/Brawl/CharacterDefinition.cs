using System;
using System.Collections.Generic;
using TieuTienKy.Combat;
using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Everything the game needs to know about one fighter body (ADR 008 swap seam). Gameplay code
    /// only sees definitions, never a specific model, so a placeholder (KayKit) and a pipeline hero
    /// (concept → 3D → Humanoid rig) are interchangeable data. Field names follow the character
    /// platform's Character Definition contract so its GLB + sidecar output can fill them later.
    /// </summary>
    [CreateAssetMenu(menuName = "Tieu Tien Ky/Character Definition", fileName = "Character")]
    public sealed class CharacterDefinition : ScriptableObject
    {
        [Serializable]
        public struct SectSkin
        {
            public Team Team;
            public Material Material;
        }

        [Tooltip("Stable id, e.g. hero.jade_swordsman. Never reuse it for a different body.")]
        public string CharacterId;

        [Tooltip("Visual root: model, loadout and renderers. No gameplay components.")]
        public GameObject Prefab;

        [Tooltip("Controller or AnimatorOverrideController. Contract: a float 'Speed' (0 idle .. 1 full run) and the states in FighterView.RequiredStates.")]
        public RuntimeAnimatorController Animations;

        [Tooltip("Humanoid or generic avatar. Leave empty to keep the one on the prefab's Animator.")]
        public Avatar Avatar;

        [Tooltip("Standing height in arena units. Camera framing and VFX anchors use it.")]
        public float Height = 2.5f;

        [Tooltip("Body radius in the simulation (arena units).")]
        public float BodyRadius = 0.9f;

        [Tooltip("Transform path under the prefab root where the main weapon and its trail attach.")]
        public string WeaponSocket;

        [Tooltip("One toon material per sect; every renderer slot uses it.")]
        public SectSkin[] SectSkins = Array.Empty<SectSkin>();

        [Tooltip("Where the body came from (pack, tool chain, author).")]
        public string Source;

        [Tooltip("Licence of the shipped body, e.g. CC0-1.0. Unknown blocks release.")]
        public string License;

        public Material SkinFor(Team team)
        {
            foreach (var skin in SectSkins)
                if (skin.Team == team) return skin.Material;
            return null;
        }

        /// <summary>Problems that would break a match or a release. Empty means usable.</summary>
        public List<string> Validate()
        {
            var problems = new List<string>();
            if (string.IsNullOrWhiteSpace(CharacterId)) problems.Add("CharacterId is empty");
            if (Prefab == null) problems.Add("Prefab is missing");
            if (Animations == null) problems.Add("Animations is missing");
            if (Height <= 0f) problems.Add("Height must be positive");
            if (BodyRadius <= 0f) problems.Add("BodyRadius must be positive");
            if (string.IsNullOrWhiteSpace(License)) problems.Add("License is empty");
            if (Prefab != null && !string.IsNullOrEmpty(WeaponSocket) && Prefab.transform.Find(WeaponSocket) == null)
                problems.Add($"WeaponSocket '{WeaponSocket}' not found under the prefab");
            if (SectSkins.Length == 0) problems.Add("No sect skins");
            return problems;
        }
    }
}
