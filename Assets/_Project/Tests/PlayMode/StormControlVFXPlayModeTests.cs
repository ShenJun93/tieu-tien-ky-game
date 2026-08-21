using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    public class StormControlVFXPlayModeTests
    {
        const string RunnerName = "StormControlVFX_Runner";
        const string IgnitionName = "StormControlVFX_Ignition";
        const string WaterRippleName = "StormControlVFX_WaterRipple";
        const string LightningName = "StormControlVFX_Lightning";
        const string ShockRingName = "StormControlVFX_ShockRing";
        const string ResidualBurstName = "ConductiveBurstVFX_Primitive";

        [TearDown]
        public void TearDown()
        {
            foreach (string name in new[] { RunnerName, IgnitionName, WaterRippleName, LightningName, ShockRingName, ResidualBurstName })
            {
                GameObject found = GameObject.Find(name);
                if (found != null)
                {
                    Object.Destroy(found);
                }
            }
        }

        [UnityTest]
        public IEnumerator SpawnAt_PlaysAllFiveBeatsInOrder_ThenCleansUpTheRunner()
        {
            const float pulseRadius = 2.4f;
            Vector3 origin = new Vector3(0.5f, 0f, -1f);
            var color = new Color(0.35f, 0.8f, 1f, 1f);

            StormControlVFX.SpawnAt(origin, pulseRadius, residualLifetimeSeconds: 0.4f, residualColor: color);

            Assert.IsNotNull(GameObject.Find(IgnitionName), "Beat 1 (Ignition) must appear at cast time.");
            Assert.IsNull(GameObject.Find(WaterRippleName), "Beat 2 must not appear before its own start delay.");

            yield return new WaitForSecondsRealtime(0.06f);
            Assert.IsNotNull(GameObject.Find(WaterRippleName), "Beat 2 (Water response) must appear shortly after ignition.");

            yield return new WaitForSecondsRealtime(0.06f);
            Assert.IsNotNull(GameObject.Find(LightningName), "Beat 3 (Lightning identity) must appear next in the sequence.");

            yield return new WaitForSecondsRealtime(0.06f);
            GameObject shockRing = GameObject.Find(ShockRingName);
            Assert.IsNotNull(shockRing, "Beat 4 (Spatial payoff) must appear after lightning.");

            // Let the shock ring grow most of the way toward its authored
            // end-of-life scale before sampling it, but stay comfortably
            // inside its own ShockRingDurationSeconds (0.22s) window - it
            // self-destroys right after that, and sampling past that point
            // would read a destroyed GameObject instead of a "too small" one.
            yield return new WaitForSecondsRealtime(0.15f);
            float expectedDiameter = pulseRadius * 2f;
            Assert.AreEqual(expectedDiameter, shockRing.transform.localScale.x, expectedDiameter * 0.15f,
                "Beat 4's shock ring must visually scale to runStyle.StormPulseRadius, not an invented fixed size.");

            yield return new WaitForSecondsRealtime(0.15f);
            Assert.IsNotNull(GameObject.Find(ResidualBurstName), "Beat 5 (Residual) must still reuse PrimitiveBurstVFX.SpawnAt.");

            yield return new WaitForSecondsRealtime(0.05f);
            Assert.IsNull(GameObject.Find(RunnerName), "The runner must destroy itself once the full sequence has fired.");
        }
    }
}
