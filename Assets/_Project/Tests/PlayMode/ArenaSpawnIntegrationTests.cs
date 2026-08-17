using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Regression coverage for the physical-device Boss-stage blocker: the
    /// Human reported the mini-boss "appeared to become stuck/outside usable
    /// arena" and unreachable. Root cause (see docs/evidence/P0A_EVIDENCE_REPORT.md):
    /// every spawn offset (including the boss's) was computed from the
    /// run's frozen start-of-run player position, not the player's current
    /// position - so after several waves/events of drift, the boss could
    /// spawn far outside the follow-camera's view and far from the player.
    /// These tests exercise the real bootstrapped object graph (Awake,
    /// physics, real ArenaBounds derived from the actual Ground) rather than
    /// the pure ArenaSpawnPlanner/ArenaBounds unit tests alone.
    /// </summary>
    public class ArenaSpawnIntegrationTests
    {
        static readonly string[] TrackedNames =
        {
            "Ground", "Player", "WaterZone", "HazardObstacle",
            "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West",
            "P0A_RunHud", "P0A_BlessingChoiceHud", "P0A_OnboardingHud", "P0A_ArenaEventDirector", "P0A_ArenaRunDirector",
            "P0A_DiagnosticOverlay", "Pursuer", "Lancer", "MiniBoss"
        };

        GameObject bootstrapRoot;
        GameObject bossProbe;

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

            if (bossProbe != null)
            {
                Object.Destroy(bossProbe);
            }
        }

        [UnityTest]
        public IEnumerator SpawnOffset_TracksCurrentPlayerPosition_NotFrozenRunStartAnchor()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();

            yield return null;
            yield return null;

            GameObject player = GameObject.Find("Player");
            GameObject directorObject = GameObject.Find("P0A_ArenaRunDirector");
            Assert.IsNotNull(player);
            Assert.IsNotNull(directorObject);

            var director = directorObject.GetComponent<ArenaRunDirector>();

            // Simulate the player having drifted away from the run's
            // original spawn point (0,1,0) across several waves of
            // combat/knockback, exactly as would happen by the time the
            // Boss stage is due to spawn - but still inside the arena's
            // real usable interior (a real player can never leave it).
            var controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.position = new Vector3(3f, 1f, -3f);
            controller.enabled = true;

            Vector3 spawnPosition = InvokeSpawnOffset(director, 0f, 6f);

            var bounds = (ArenaBounds)typeof(ArenaRunDirector)
                .GetField("arenaBounds", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(director);

            // Offset X is 0, so an anchor tracking the CURRENT player must
            // reproduce the player's current X exactly; the old, frozen-anchor
            // behavior would have returned the run-start X (0) instead of 3.
            Assert.AreEqual(player.transform.position.x, spawnPosition.x, 0.01f,
                "A spawn offset must track the player's CURRENT position, not the frozen run-start position, or the boss can spawn unreachable/off-camera.");
            Assert.IsTrue(bounds.Contains(spawnPosition), "A spawn position must always be inside the arena's usable interior.");
        }

        [UnityTest]
        public IEnumerator SpawnOffset_PlayerNearArenaEdge_NeverPlacesSpawnOutsideUsableBounds()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();

            yield return null;
            yield return null;

            GameObject player = GameObject.Find("Player");
            GameObject directorObject = GameObject.Find("P0A_ArenaRunDirector");
            var director = directorObject.GetComponent<ArenaRunDirector>();

            var controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.position = new Vector3(8.7f, 1f, 8.7f);
            controller.enabled = true;

            Vector3 spawnPosition = InvokeSpawnOffset(director, 0f, 6f);

            var bounds = (ArenaBounds)typeof(ArenaRunDirector)
                .GetField("arenaBounds", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(director);

            Assert.IsTrue(bounds.Contains(spawnPosition), "A spawn position must never land outside the arena's usable interior.");
        }

        [Test]
        public void BossController_ClampIntoArenaIfOutside_SnapsBackInsideUsableBounds()
        {
            bossProbe = new GameObject("MiniBossProbe");
            var controller = bossProbe.AddComponent<CharacterController>();
            bossProbe.AddComponent<KnockbackReceiver>();
            bossProbe.AddComponent<Combatant>();
            var bossController = bossProbe.AddComponent<MiniBossController>();

            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);
            var playerProbe = new GameObject("PlayerProbe");
            bossController.Initialize(playerProbe.transform, playerProbe.AddComponent<Combatant>(), bounds);

            controller.enabled = false;
            bossProbe.transform.position = new Vector3(15f, 1f, 15f);
            controller.enabled = true;

            typeof(MiniBossController)
                .GetMethod("ClampIntoArenaIfOutside", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(bossController, null);

            Assert.IsTrue(bounds.Contains(bossProbe.transform.position),
                "A boss stranded outside the usable arena (e.g. wedged at a corner by a Charge lunge) must be snapped back inside so the encounter cannot be permanently stranded.");

            Object.DestroyImmediate(playerProbe);
        }

        static Vector3 InvokeSpawnOffset(ArenaRunDirector director, float x, float z)
        {
            MethodInfo method = typeof(ArenaRunDirector).GetMethod("SpawnOffset", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, "Expected private instance method 'SpawnOffset' on ArenaRunDirector.");
            return (Vector3)method.Invoke(director, new object[] { x, z });
        }
    }
}
