using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Single reusable source of the arena's usable interior (X/Z), derived
    /// once from the actual Ground collider bounds minus a margin (actor
    /// radius/skin-width headroom) - never an independently hardcoded
    /// coordinate. Perimeter containment already blocks movement via the
    /// arena walls; this exists so spawn/positioning logic (enemy/boss
    /// spawns, future arena-event placement) can compute a position that is
    /// guaranteed reachable without re-deriving Ground bounds itself.
    /// </summary>
    public readonly struct ArenaBounds
    {
        public readonly float MinX;
        public readonly float MaxX;
        public readonly float MinZ;
        public readonly float MaxZ;

        public ArenaBounds(float minX, float maxX, float minZ, float maxZ)
        {
            MinX = minX;
            MaxX = maxX;
            MinZ = minZ;
            MaxZ = maxZ;
        }

        /// <summary>Derives usable bounds from the Ground's actual collider bounds, inset by margin on every side.</summary>
        public static ArenaBounds FromGroundBounds(Bounds groundBounds, float margin)
        {
            return new ArenaBounds(
                groundBounds.min.x + margin,
                groundBounds.max.x - margin,
                groundBounds.min.z + margin,
                groundBounds.max.z - margin);
        }

        /// <summary>Clamps a position's X/Z into the usable interior, leaving Y untouched.</summary>
        public Vector3 Clamp(Vector3 position)
        {
            return new Vector3(
                Mathf.Clamp(position.x, MinX, MaxX),
                position.y,
                Mathf.Clamp(position.z, MinZ, MaxZ));
        }

        public bool Contains(Vector3 position)
        {
            return position.x >= MinX && position.x <= MaxX && position.z >= MinZ && position.z <= MaxZ;
        }
    }
}
