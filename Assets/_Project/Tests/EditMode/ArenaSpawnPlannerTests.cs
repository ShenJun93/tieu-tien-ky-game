using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Regression coverage for the physical-device Boss-stage blocker: a
    /// spawn offset must track the player's CURRENT position (this test's
    /// "anchor"), not a frozen run-start position, and must never land
    /// outside the arena's usable interior even when the anchor itself has
    /// drifted near an edge.
    /// </summary>
    public class ArenaSpawnPlannerTests
    {
        static readonly ArenaBounds Bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

        [Test]
        public void ComputeSpawnPosition_AnchorNearCenter_ReturnsExactOffset()
        {
            var anchor = new Vector3(0f, 1f, 0f);

            Vector3 result = ArenaSpawnPlanner.ComputeSpawnPosition(anchor, 0f, 6f, Bounds);

            Assert.AreEqual(new Vector3(0f, 1f, 6f), result);
        }

        [Test]
        public void ComputeSpawnPosition_TracksMovedAnchor_NotAFrozenOrigin()
        {
            // Simulates a player who has drifted far from the run's original
            // spawn point by the time the boss is due to spawn.
            var driftedAnchor = new Vector3(-7f, 1f, -6f);

            Vector3 result = ArenaSpawnPlanner.ComputeSpawnPosition(driftedAnchor, 0f, 6f, Bounds);

            Assert.AreEqual(-7f, result.x, 1e-4f);
            Assert.AreEqual(0f, result.z, 1e-4f);
        }

        [Test]
        public void ComputeSpawnPosition_OffsetPastEdge_ClampsInsideBounds()
        {
            var anchor = new Vector3(8f, 1f, 8f);

            Vector3 result = ArenaSpawnPlanner.ComputeSpawnPosition(anchor, 0f, 6f, Bounds);

            Assert.IsTrue(Bounds.Contains(result), "Spawn position must always be inside the usable arena.");
            Assert.AreEqual(9f, result.z, 1e-4f);
        }

        [Test]
        public void ComputeSpawnPosition_ClampingNeverIncreasesDistanceFromAnchor()
        {
            var anchor = new Vector3(8.5f, 1f, 8.5f);

            Vector3 unclamped = new Vector3(anchor.x, anchor.y, anchor.z + 6f);
            Vector3 result = ArenaSpawnPlanner.ComputeSpawnPosition(anchor, 0f, 6f, Bounds);

            float unclampedDistance = Vector3.Distance(anchor, unclamped);
            float clampedDistance = Vector3.Distance(anchor, result);

            Assert.LessOrEqual(clampedDistance, unclampedDistance);
        }
    }
}
