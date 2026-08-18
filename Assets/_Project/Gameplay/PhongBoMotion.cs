using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Pure destination math for Phong Bộ: a flat-plane forward offset, always clamped into the authoritative ArenaBounds so the reposition can never leave the arena.</summary>
    public static class PhongBoMotion
    {
        public static Vector3 ComputeDestination(Vector3 origin, Vector3 forward, float distanceMeters, ArenaBounds bounds)
        {
            Vector3 flatForward = new Vector3(forward.x, 0f, forward.z);
            flatForward = flatForward.sqrMagnitude > 0.0001f ? flatForward.normalized : Vector3.forward;
            Vector3 raw = origin + flatForward * Mathf.Max(0f, distanceMeters);
            return bounds.Clamp(raw);
        }
    }
}
