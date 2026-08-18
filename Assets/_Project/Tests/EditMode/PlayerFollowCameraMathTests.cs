using NUnit.Framework;
using TieuTienKy.Gameplay;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Deterministic coverage for the P0A camera-follow math. Exercises the
    /// pure functions directly - no scene, no GameObjects - so it pins that
    /// the camera retains its authored height/offset and tracks Player X/Z
    /// only, independent of any Transform mutation.
    /// </summary>
    public class PlayerFollowCameraMathTests
    {
        [Test]
        public void ComputeHorizontalOffset_ReturnsCameraMinusPlayer_OnXZ()
        {
            var initialCamera = new Vector3(1.5f, 6.5f, -6f);
            var initialPlayer = new Vector3(0f, 1f, 0f);

            Vector2 offset = PlayerFollowCameraMath.ComputeHorizontalOffset(initialCamera, initialPlayer);

            Assert.AreEqual(1.5f, offset.x, 1e-5f);
            Assert.AreEqual(-6f, offset.y, 1e-5f);
        }

        [Test]
        public void ComputeDesiredPosition_TracksPlayerXZPlusOffset()
        {
            var offset = new Vector2(1.5f, -6f);
            var playerPosition = new Vector3(4f, 1f, 3f);

            Vector3 desired = PlayerFollowCameraMath.ComputeDesiredPosition(playerPosition, cameraHeight: 6.5f, horizontalOffset: offset);

            Assert.AreEqual(5.5f, desired.x, 1e-5f);
            Assert.AreEqual(-3f, desired.z, 1e-5f);
        }

        [Test]
        public void ComputeDesiredPosition_IgnoresPlayerY_KeepsFixedCameraHeight()
        {
            var offset = new Vector2(0f, 0f);

            // Player Y changing (e.g. minor gravity/knockback bob) must not
            // move the camera vertically - only the scene-authored height is
            // ever used.
            Vector3 desiredLow = PlayerFollowCameraMath.ComputeDesiredPosition(new Vector3(0f, 1f, 0f), cameraHeight: 6.5f, horizontalOffset: offset);
            Vector3 desiredHigh = PlayerFollowCameraMath.ComputeDesiredPosition(new Vector3(0f, 9f, 0f), cameraHeight: 6.5f, horizontalOffset: offset);

            Assert.AreEqual(6.5f, desiredLow.y, 1e-5f);
            Assert.AreEqual(6.5f, desiredHigh.y, 1e-5f);
        }

        [Test]
        public void ComputeImpulseFalloff_IsFullAtZeroElapsed_AndZeroAtOrPastDuration()
        {
            Assert.AreEqual(1f, PlayerFollowCameraMath.ComputeImpulseFalloff(0f, 0.2f), 1e-5f);
            Assert.AreEqual(0f, PlayerFollowCameraMath.ComputeImpulseFalloff(0.2f, 0.2f), 1e-5f);
            Assert.AreEqual(0f, PlayerFollowCameraMath.ComputeImpulseFalloff(5f, 0.2f), 1e-5f);
        }

        [Test]
        public void ComputeImpulseFalloff_NeverNegative_NeverExceedsOne()
        {
            Assert.AreEqual(0.5f, PlayerFollowCameraMath.ComputeImpulseFalloff(0.1f, 0.2f), 1e-5f);
            Assert.GreaterOrEqual(PlayerFollowCameraMath.ComputeImpulseFalloff(-1f, 0.2f), 0f);
            Assert.LessOrEqual(PlayerFollowCameraMath.ComputeImpulseFalloff(-1f, 0.2f), 1f);
        }

        [Test]
        public void ComputeImpulseFalloff_ZeroDuration_ReturnsZero()
        {
            Assert.AreEqual(0f, PlayerFollowCameraMath.ComputeImpulseFalloff(0f, 0f), 1e-5f);
        }
    }
}
