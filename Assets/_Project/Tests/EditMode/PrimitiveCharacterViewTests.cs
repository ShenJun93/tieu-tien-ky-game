using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    public class PrimitiveCharacterViewTests
    {
        [Test]
        public void Build_ArmedCharacter_CreatesFullBodyAndWeaponSocket()
        {
            var root = new GameObject("ActorRoot");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.yellow, Color.cyan, armed: true, visualScale: 1f);

            Assert.NotNull(root.transform.Find("CharacterView/Body"));
            Assert.NotNull(root.transform.Find("CharacterView/Head"));
            Assert.NotNull(root.transform.Find("CharacterView/LeftArm"));
            Assert.NotNull(root.transform.Find("CharacterView/RightArm"));
            Assert.NotNull(root.transform.Find("CharacterView/LeftLeg"));
            Assert.NotNull(root.transform.Find("CharacterView/RightLeg"));
            Assert.NotNull(root.transform.Find("CharacterView/WeaponSocket/Sword"));

            Object.DestroyImmediate(root);
        }

        [Test]
        public void Build_UnarmedCharacter_CreatesNoSword()
        {
            var root = new GameObject("ActorRoot");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.yellow, Color.cyan, armed: false, visualScale: 1f);

            Assert.NotNull(root.transform.Find("CharacterView/WeaponSocket"));
            Assert.IsNull(root.transform.Find("CharacterView/WeaponSocket/Sword"));

            Object.DestroyImmediate(root);
        }

        [Test]
        public void Build_VisualChildren_HaveNoColliders()
        {
            var root = new GameObject("ActorRoot");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.yellow, Color.cyan, armed: true, visualScale: 1f);

            foreach (Collider collider in root.GetComponentsInChildren<Collider>())
            {
                Assert.Fail($"Visual child '{collider.gameObject.name}' must not own a gameplay collider.");
            }

            Object.DestroyImmediate(root);
        }

        [Test]
        public void Build_NamedPlayer_UsesChibiSpriteInsteadOfPrimitiveLimbs()
        {
            var root = new GameObject("Player");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.yellow, Color.cyan, armed: true, visualScale: 1f);

            Transform spriteChild = root.transform.Find("CharacterView/ChibiSprite");
            Assert.NotNull(spriteChild, "Player must render via its chibi sprite.");

            var renderer = spriteChild.GetComponent<SpriteRenderer>();
            Assert.NotNull(renderer);
            Assert.NotNull(renderer.sprite);
            Assert.AreEqual("Player_Chibi_01", renderer.sprite.name);

            Assert.IsNull(root.transform.Find("CharacterView/Head"), "Sprite mode must not also build primitive limbs.");
            Assert.NotNull(root.transform.Find("CharacterView/WeaponSocket/Sword"), "WeaponSocket/Sword must still build unconditionally for SwordAttackView's thunder-stack feedback.");

            Object.DestroyImmediate(root);
        }

        [TestCase("Pursuer")]
        [TestCase("Lancer")]
        public void Build_NamedEnemyActor_UsesChibiSprite(string actorName)
        {
            var root = new GameObject(actorName);
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.gray, Color.gray, armed: false, visualScale: 1f);

            Transform spriteChild = root.transform.Find("CharacterView/ChibiSprite");
            Assert.NotNull(spriteChild, $"{actorName} must render via its chibi sprite.");
            Assert.AreEqual($"{actorName}_Chibi_01", spriteChild.GetComponent<SpriteRenderer>().sprite.name);
            Assert.IsNull(root.transform.Find("CharacterView/Head"));

            Object.DestroyImmediate(root);
        }

        [Test]
        public void Build_UnknownActorName_FallsBackToPrimitiveLimbs()
        {
            var root = new GameObject("MiniBoss");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.red, Color.red, armed: true, visualScale: 1.35f);

            Assert.NotNull(root.transform.Find("CharacterView/Head"), "Unmatched actor names must keep the primitive fallback.");
            Assert.IsNull(root.transform.Find("CharacterView/ChibiSprite"));

            Object.DestroyImmediate(root);
        }

        [Test]
        public void Build_ChibiSpriteChild_HasNoCollider()
        {
            var root = new GameObject("Lancer");
            var view = root.AddComponent<PrimitiveCharacterView>();
            view.Build(Color.gray, Color.gray, armed: false, visualScale: 1f);

            foreach (Collider collider in root.GetComponentsInChildren<Collider>())
            {
                Assert.Fail($"Visual child '{collider.gameObject.name}' must not own a gameplay collider.");
            }

            Object.DestroyImmediate(root);
        }
    }
}
