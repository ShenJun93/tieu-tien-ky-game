using TieuTienKy.Input;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Touch-driven movement + facing for the greybox player capsule.</summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    [RequireComponent(typeof(TouchInputReader))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float turnSpeedDegreesPerSecond = 720f;
        [SerializeField] float gravity = -9.81f;

        CharacterController controller;
        KnockbackReceiver knockbackReceiver;
        TouchInputReader inputReader;
        float verticalVelocity;
        float runMoveSpeedMultiplier = 1f;

        /// <summary>0..1 input-driven move intensity this frame, for CharacterPresentation.SetMovement (Idle/Run blend). Presentation-facing only - never drives gameplay motion itself.</summary>
        public float NormalizedMoveSpeed { get; private set; }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
            inputReader = GetComponent<TouchInputReader>();
        }

        /// <summary>Run-blessing-driven move speed multiplier (e.g. Phong Hành). Scales the base moveSpeed at motion time; never overwrites it.</summary>
        public void SetRunMoveSpeedMultiplier(float multiplier)
        {
            runMoveSpeedMultiplier = multiplier;
        }

        void Update()
        {
            Vector2 moveInput = inputReader.MoveInput;
            Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            NormalizedMoveSpeed = Mathf.Clamp01(moveInput.magnitude);

            if (!knockbackReceiver.IsBeingKnockedBack && moveDirection.sqrMagnitude > 0.0001f)
            {
                Vector3 motion = moveDirection * moveSpeed * runMoveSpeedMultiplier * Time.deltaTime;
                controller.Move(motion);

                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeedDegreesPerSecond * Time.deltaTime);
            }

            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            controller.Move(new Vector3(0f, verticalVelocity * Time.deltaTime, 0f));
        }
    }
}
