using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Keeps the gameplay view inside the arena. Given how the camera sits relative to its anchor,
    /// it finds the anchor positions for which no screen edge shows the void beyond the walls.
    /// Pure math; the match clamps its camera anchor with it every frame.
    /// </summary>
    public static class CameraFraming
    {
        /// <param name="offset">Camera position minus anchor position.</param>
        /// <param name="rotation">Camera rotation (pitched down, looking along +z).</param>
        /// <param name="shown">Arena rectangle on the ground plane (x, z) that may be on screen, walls included.</param>
        /// <param name="wallTop">Height of the back wall's top; the top screen edge may reach it but not beyond.</param>
        /// <returns>
        /// Allowed anchor rectangle (x, z). An axis collapses to the centre when the view is larger
        /// than the arena along it.
        /// </returns>
        public static Rect AnchorRange(Vector3 offset, Quaternion rotation, float verticalFovDegrees, float aspect, Rect shown, float wallTop)
        {
            float tanV = Mathf.Tan(verticalFovDegrees * 0.5f * Mathf.Deg2Rad);
            float tanH = tanV * aspect;

            Vector3 bottom = rotation * new Vector3(0f, -tanV, 1f);
            Vector3 top = rotation * new Vector3(0f, tanV, 1f);
            Vector3 side = rotation * new Vector3(tanH, 0f, 1f);

            // Ground offsets (anchor at the origin) where each screen edge lands.
            float bottomZ = offset.z + (-offset.y / bottom.y) * bottom.z;
            float topZ = top.y < -1e-4f ? offset.z + ((wallTop - offset.y) / top.y) * top.z : float.PositiveInfinity;
            float sideX = side.y < -1e-4f ? Mathf.Abs(offset.x + (-offset.y / side.y) * side.x) : float.PositiveInfinity;

            (float zMin, float zMax) = Collapse(shown.yMin - bottomZ, shown.yMax - topZ, shown.center.y);
            (float xMin, float xMax) = Collapse(shown.xMin + sideX, shown.xMax - sideX, shown.center.x);
            return Rect.MinMaxRect(xMin, zMin, xMax, zMax);
        }

        public static Vector3 Clamp(Vector3 anchor, Rect range) => new Vector3(
            Mathf.Clamp(anchor.x, range.xMin, range.xMax),
            anchor.y,
            Mathf.Clamp(anchor.z, range.yMin, range.yMax));

        static (float, float) Collapse(float min, float max, float centre)
        {
            if (float.IsInfinity(min) || float.IsInfinity(max) || min > max)
            {
                float mid = float.IsInfinity(min) || float.IsInfinity(max) ? centre : (min + max) * 0.5f;
                return (mid, mid);
            }
            return (min, max);
        }
    }
}
