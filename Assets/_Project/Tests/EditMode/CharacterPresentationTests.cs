using NUnit.Framework;
using TieuTienKy.Gameplay;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    public class CharacterPresentationTests
    {
        GameObject root;
        CharacterPresentation presentation;
        Animator animator;
        Transform weapon;
        Transform bodyVfx;
        Transform feetVfx;
        Transform castVfx;
        Renderer[] renderers;

        [SetUp]
        public void SetUp()
        {
            root = new GameObject("PresentationRoot");
            presentation = root.AddComponent<CharacterPresentation>();
            animator = root.AddComponent<Animator>();

            weapon = new GameObject("WeaponSocket").transform;
            bodyVfx = new GameObject("BodyVfxSocket").transform;
            feetVfx = new GameObject("FeetVfxSocket").transform;
            castVfx = new GameObject("CastVfxSocket").transform;
            weapon.SetParent(root.transform);
            bodyVfx.SetParent(root.transform);
            feetVfx.SetParent(root.transform);
            castVfx.SetParent(root.transform);

            var rendererGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rendererGO.transform.SetParent(root.transform);
            renderers = new[] { rendererGO.GetComponent<Renderer>() };
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(root);
        }

        [Test]
        public void IsConfigured_FalseBeforeConfigure_TrueAfter()
        {
            Assert.IsFalse(presentation.IsConfigured);
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);
            Assert.IsTrue(presentation.IsConfigured);
        }

        [Test]
        public void Configure_ExposesSocketsExactlyAsProvided()
        {
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);

            Assert.AreSame(weapon, presentation.WeaponSocket);
            Assert.AreSame(bodyVfx, presentation.BodyVfxSocket);
            Assert.AreSame(feetVfx, presentation.FeetVfxSocket);
            Assert.AreSame(castVfx, presentation.CastVfxSocket);
        }

        [Test]
        public void SemanticPlayMethods_DoNotThrow_EvenWithoutAnimatorController()
        {
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);

            Assert.DoesNotThrow(() =>
            {
                presentation.SetMovement(0.5f);
                presentation.PlayBasicAttack();
                presentation.PlayCast();
                presentation.PlayMobility();
                presentation.PlayHit();
                presentation.PlayDeath();
            });
        }

        [Test]
        public void PlayImpact_RaisesImpactRequested_WithGivenWorldPoint()
        {
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);
            Vector3? received = null;
            presentation.ImpactRequested += point => received = point;

            var expected = new Vector3(1f, 2f, 3f);
            presentation.PlayImpact(expected);

            Assert.IsTrue(received.HasValue);
            Assert.AreEqual(expected, received.Value);
        }

        [Test]
        public void SetBlessingVisual_TintsAssignedRenderers()
        {
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);

            presentation.SetBlessingVisual(Color.red, 1f);

            var block = new MaterialPropertyBlock();
            renderers[0].GetPropertyBlock(block);
            Color applied = block.GetColor(Shader.PropertyToID("_Color"));
            Assert.AreEqual(Color.red.r, applied.r, 0.001f);
            Assert.AreEqual(Color.red.g, applied.g, 0.001f);
            Assert.AreEqual(Color.red.b, applied.b, 0.001f);
        }

        [Test]
        public void SetBlessingVisual_ZeroIntensity_LeavesRenderersWhite()
        {
            presentation.Configure(animator, weapon, bodyVfx, feetVfx, castVfx, renderers);

            presentation.SetBlessingVisual(Color.red, 0f);

            var block = new MaterialPropertyBlock();
            renderers[0].GetPropertyBlock(block);
            Color applied = block.GetColor(Shader.PropertyToID("_Color"));
            Assert.AreEqual(Color.white.r, applied.r, 0.001f);
            Assert.AreEqual(Color.white.g, applied.g, 0.001f);
            Assert.AreEqual(Color.white.b, applied.b, 0.001f);
        }
    }
}
