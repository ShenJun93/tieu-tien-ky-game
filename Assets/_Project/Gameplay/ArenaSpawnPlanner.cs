using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Pure spawn-position math. Root-cause fix for the P0A+ physical-device
    /// boss blocker: every spawn (wave enemy or boss) used to be offset from
    /// the run's frozen start-of-run anchor, so by the time the Boss stage
    /// spawned, accumulated combat/knockback/arena-event drift could put the
    /// player far enough away (and off the follow-camera) that the boss
    /// looked stuck/unreachable even though it was chasing correctly. This
    /// offsets from whatever anchor position the caller supplies (the
    /// player's CURRENT position) instead, then clamps into the arena's
    /// usable interior via ArenaBounds so a spawn can never land outside the
    /// walls even when the anchor itself is near an edge.
    /// </summary>
    public static class ArenaSpawnPlanner
    {
        public static Vector3 ComputeSpawnPosition(Vector3 anchorPosition, float offsetX, float offsetZ, ArenaBounds bounds)
        {
            Vector3 desired = new Vector3(anchorPosition.x + offsetX, anchorPosition.y, anchorPosition.z + offsetZ);
            return bounds.Clamp(desired);
        }
    }
}
