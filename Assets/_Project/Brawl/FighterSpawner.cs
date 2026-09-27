using System.Linq;
using TieuTienKy.Combat;
using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>Builds a fighter's presentation from its <see cref="CharacterDefinition"/>.</summary>
    public static class FighterSpawner
    {
        public static FighterView Spawn(CharacterDefinition character, Team team, Vector2 position, Transform parent)
        {
            var go = Object.Instantiate(character.Prefab, new Vector3(position.x, 0f, position.y), Quaternion.Euler(0f, 180f, 0f), parent);
            go.name = $"{character.CharacterId}_{team}";

            var skin = character.SkinFor(team);
            if (skin != null)
                foreach (var r in go.GetComponentsInChildren<Renderer>(true))
                    r.sharedMaterials = Enumerable.Repeat(skin, r.sharedMaterials.Length).ToArray();

            var animator = go.GetComponentInChildren<Animator>();
            if (animator == null) animator = go.AddComponent<Animator>(); // Unity fake-null: no ?? here
            if (character.Avatar != null) animator.avatar = character.Avatar;
            animator.applyRootMotion = false;
            animator.runtimeAnimatorController = character.Animations;

            var view = go.AddComponent<FighterView>();
            view.Bind(animator);
            return view;
        }
    }
}
