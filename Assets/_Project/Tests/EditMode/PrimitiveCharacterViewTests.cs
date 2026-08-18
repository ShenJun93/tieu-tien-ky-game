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
    }
}
