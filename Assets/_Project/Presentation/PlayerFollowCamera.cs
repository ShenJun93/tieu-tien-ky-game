using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Smallest possible camera-follow for P0A physical playtesting: keeps the
    /// yellow Player visible by tracking X/Z only, while retaining the
    /// scene-authored camera height and look angle untouched. Attached and
    /// initialized by GreyboxSceneBootstrapper using its existing Player
    /// reference - no GameObject.Find, no Cinemachine, no camera state
    /// machine. Camera follows Player; Player transform is never written by
    /// this component.
    /// </summary>
    public sealed class PlayerFollowCamera : MonoBehaviour
    {
        [SerializeField] float smoothTime = 0.15f;
        [SerializeField] float impulseDurationSeconds = 0.18f;

        Transform target;
        float cameraHeight;
        Vector2 horizontalOffset;
        Vector3 velocity;
        float impulseMagnitude;
        float impulseStartTime;

        public void Initialize(Transform playerTarget)
        {
            target = playerTarget;
            cameraHeight = transform.position.y;
            horizontalOffset = PlayerFollowCameraMath.ComputeHorizontalOffset(transform.position, target.position);
        }

        /// <summary>
        /// Bounded one-shot camera dip on a meaningful impact (landed hit,
        /// player taking damage, boss arrival) - never lateral shake, so it
        /// reads as weight without becoming disorienting. A larger magnitude
        /// arriving mid-impulse extends/strengthens it rather than resetting,
        /// so back-to-back impacts don't visually stutter.
        /// </summary>
        public void ApplyImpulse(float magnitude)
        {
            float currentFalloff = PlayerFollowCameraMath.ComputeImpulseFalloff(Time.time - impulseStartTime, impulseDurationSeconds);
            impulseMagnitude = Mathf.Max(impulseMagnitude * currentFalloff, magnitude);
            impulseStartTime = Time.time;
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            float falloff = PlayerFollowCameraMath.ComputeImpulseFalloff(Time.time - impulseStartTime, impulseDurationSeconds);
            float dippedHeight = cameraHeight - impulseMagnitude * falloff;
            Vector3 desired = PlayerFollowCameraMath.ComputeDesiredPosition(target.position, dippedHeight, horizontalOffset);
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
