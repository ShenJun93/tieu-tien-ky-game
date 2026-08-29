using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Regression/documentation coverage for a Director-observed behavior:
    /// several physical-device relaunches ended in Defeat at 00:03 with
    /// Kills: 0, before any player input was given. Static analysis of Wave
    /// 1's two-Pursuer pincer timing against a zero-input player predicted
    /// this is not a code defect - it's the wave's designed difficulty
    /// converging on a target that never intentionally moves or fights back.
    /// This test drives the real GreyboxSceneBootstrapper object graph
    /// through real Unity time with zero simulated input, the same way a
    /// device left idle would, to confirm that prediction deterministically
    /// (see docs/tasks/TASK-TIEU-TIEN-KY-PRODUCT-PROOF-SLICE-008-FOLLOWUP-FIXES.md).
    /// </summary>
    public class ArenaAfkDefeatInvestigationTests
    {
        static readonly string[] TrackedNames =
        {
            "Ground", "Player", "WaterZone", "HazardObstacle",
            "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West",
            "P0A_RunHud", "P0A_BlessingChoiceHud", "P0A_OnboardingHud", "P0A_ArenaEventDirector", "P0A_ArenaRunDirector",
            "P0A_DiagnosticOverlay", "Pursuer", "Lancer", "MiniBoss"
        };

        GameObject bootstrapRoot;

        [TearDown]
        public void TearDown()
        {
            foreach (string name in TrackedNames)
            {
                var go = GameObject.Find(name);
                if (go != null)
                {
                    Object.Destroy(go);
                }
            }

            if (bootstrapRoot != null)
            {
                Object.Destroy(bootstrapRoot);
            }
        }

        [UnityTest]
        public IEnumerator Wave1_WithZeroPlayerInput_PlayerIsDefeatedWithinBoundedPincerTiming()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();

            yield return null;
            yield return null;

            GameObject directorObject = GameObject.Find("P0A_ArenaRunDirector");
            Assert.IsNotNull(directorObject, "ArenaRunDirector must exist after bootstrap.");
            var director = directorObject.GetComponent<ArenaRunDirector>();

            // No touch input is ever simulated in this test - PlayerController.Update()
            // reads TouchInputReader.MoveInput, which defaults to Vector2.zero. Enemy
            // hits can still knock the player around, so exact hit timing depends on
            // pincer/knockback interaction. Wait for Defeat within a bounded timeout.
            float timeoutAt = Time.time + 6f;
            while (director.Stage != ArenaRunStage.Defeat && Time.time < timeoutAt)
            {
                yield return null;
            }

            Assert.AreEqual(ArenaRunStage.Defeat, director.Stage,
                "A fully idle player must be defeated by Wave 1's two-Pursuer pincer - " +
                "this is expected AI behavior against a non-responding target, not a bug.");
            Assert.AreEqual(0, director.KillCount,
                "An idle player lands no hits, so Kills must be 0 - matching the Director's device observation.");
            Assert.That(director.ElapsedSeconds, Is.InRange(2.0f, 6.0f),
                $"Expected bounded early-wave Defeat timing for a zero-input player; got {director.ElapsedSeconds:F2}s.");
        }
    }
}
