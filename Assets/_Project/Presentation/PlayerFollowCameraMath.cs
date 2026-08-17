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
    }
}
