using UnityEngine;

namespace TieuTienKy.Diagnostics
{
    public enum BoundarySide
    {
        None,
        North,
        South,
        East,
        West,
        Corner
    }

    /// <summary>
    /// Pure, dependency-free classification of a player's physical bounds
    /// against the arena's actual runtime Bounds (never a hardcoded arena
    /// size) for the P0A diagnostic overlay and its EditMode tests. Corner
    /// means the player is outside on both the X and Z axes at once.
    /// </summary>
    public static class BoundaryClassifier
    {
        public static BoundarySide Classify(Bounds arenaBounds, Bounds playerBounds)
        {
            bool outWest = playerBounds.min.x < arenaBounds.min.x;
            bool outEast = playerBounds.max.x > arenaBounds.max.x;
            bool outSouth = playerBounds.min.z < arenaBounds.min.z;
            bool outNorth = playerBounds.max.z > arenaBounds.max.z;

            bool outOnX = outWest || outEast;
            bool outOnZ = outSouth || outNorth;

            if (outOnX && outOnZ)
            {
                return BoundarySide.Corner;
            }

            if (outWest)
            {
                return BoundarySide.West;
            }

            if (outEast)
            {
                return BoundarySide.East;
            }

            if (outSouth)
            {
                return BoundarySide.South;
            }

            if (outNorth)
            {
                return BoundarySide.North;
            }

            return BoundarySide.None;
        }
    }
}
