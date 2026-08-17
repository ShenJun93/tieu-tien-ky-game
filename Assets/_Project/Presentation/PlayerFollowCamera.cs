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

        Transform target;
        float cameraHeight;
        Vector2 horizontalOffset;
        Vector3 velocity;

        public void Initialize(Transform playerTarget)
        {
            target = playerTarget;
            cameraHeight = transform.position.y;
            horizontalOffset = PlayerFollowCameraMath.ComputeHorizontalOffset(transform.position, target.position);
        }

        void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desired = PlayerFollowCameraMath.ComputeDesiredPosition(target.position, cameraHeight, horizontalOffset);
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
        }
    }
}
