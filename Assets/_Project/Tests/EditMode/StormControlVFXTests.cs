using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// StartCoroutine runs a coroutine's body synchronously up to its first
    /// yield even outside Play Mode, so an EditMode [Test] can assert Beat 1
    /// (Ignition) was spawned correctly by StormControlVFX.SpawnAt without
    /// needing to pump subsequent yields - the same limitation the rest of
    /// the 5-beat sequence needs PlayMode to exercise (see
    /// StormControlVFXPlayModeTests).
    /// </summary>
    public class StormControlVFXTests
    {
        const string RunnerName = "StormControlVFX_Runner";
        const string IgnitionName = "StormControlVFX_Ignition";

        [TearDown]
        public void TearDown()
        {
            foreach (string name in new[] { RunnerName, IgnitionName })
            {
                GameObject found = GameObject.Find(name);
                if (found != null)
                {
                    Object.DestroyImmediate(found);
                }
            }
        }

        [Test]
        public void SpawnAt_ImmediatelyCreatesIgnitionLayer_WithAdditiveMaterialAndTexture()
        {
            Vector3 origin = new Vector3(1f, 0f, 2f);
            var color = new Color(0.35f, 0.8f, 1f, 1f);

            StormControlVFX.SpawnAt(origin, pulseRadius: 2.1f, residualLifetimeSeconds: 0.5f, residualColor: color);

            Assert.IsNotNull(GameObject.Find(RunnerName), "SpawnAt must create a runner host GameObject at the origin.");

            GameObject ignition = GameObject.Find(IgnitionName);
            Assert.IsNotNull(ignition, "Beat 1 (Ignition) must spawn synchronously, before any beat delay.");

            var renderer = ignition.GetComponent<Renderer>();
            Assert.IsNotNull(renderer, "Ignition must be a real renderable quad.");
            Assert.AreEqual("P0A_StormControlAdditive", renderer.sharedMaterial.name,
                "Ignition is a CAUSE/energy beat and must use the additive material per the visual pipeline contract.");

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            Texture assignedTexture = block.GetTexture(Shader.PropertyToID("_MainTex"));
            Assert.IsNotNull(assignedTexture, "Ignition's texture must be assigned via MaterialPropertyBlock, not the shared material asset.");
            Assert.AreEqual("StormControl_IgnitionFlash_01", assignedTexture.name);
        }
    }
}
