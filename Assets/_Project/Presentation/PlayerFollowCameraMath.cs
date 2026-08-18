using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Pure, dependency-free camera-follow math for PlayerFollowCamera and its
    /// EditMode tests. Tracks the Player's X/Z only - camera height and look
    /// angle are never derived from Player state, so the scene-authored
    /// camera angle is retained exactly.
    /// </summary>
    public static class PlayerFollowCameraMath
    {
        public static Vector2 ComputeHorizontalOffset(Vector3 initialCameraPosition, Vector3 initialPlayerPosition)
        {
            return new Vector2(
                initialCameraPosition.x - initialPlayerPosition.x,
                initialCameraPosition.z - initialPlayerPosition.z);
        }

        public static Vector3 ComputeDesiredPosition(Vector3 playerPosition, float cameraHeight, Vector2 horizontalOffset)
        {
            return new Vector3(
                playerPosition.x + horizontalOffset.x,
                cameraHeight,
                playerPosition.z + horizontalOffset.y);
        }

        /// <summary>
        /// Linear 1-to-0 falloff for a bounded camera impulse (a small dip in
        /// camera height on meaningful impact). Never negative, never exceeds
        /// 1, so an impulse always decays to exactly no effect - it can only
        /// ever subtract a small amount from the tracked height, never add
        /// lateral shake, which is what keeps it from becoming disorienting.
        /// </summary>
        public static float ComputeImpulseFalloff(float elapsedSeconds, float durationSeconds)
        {
            if (durationSeconds <= 0f)
            {
                return 0f;
            }

            return Mathf.Clamp01(1f - elapsedSeconds / durationSeconds);
        }
    }
}
