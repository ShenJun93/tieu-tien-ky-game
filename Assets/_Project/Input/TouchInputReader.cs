using UnityEngine;
using UnityEngine.InputSystem;

namespace TieuTienKy.Input
{
    /// <summary>
    /// Minimal touch/pointer reader: left half of the screen is a drag-to-move
    /// zone, right half is tap-to-attack. Uses Pointer.current so the same
    /// code path works for touchscreen and editor mouse input.
    /// </summary>
    public sealed class TouchInputReader : MonoBehaviour
    {
        [SerializeField] float moveDragPixelsForFullSpeed = 120f;

        bool moveTouchActive;
        int moveTouchId = -1;
        Vector2 moveOrigin;

        public Vector2 MoveInput { get; private set; }
        public bool AttackTriggeredThisFrame { get; private set; }

        void Update()
        {
            AttackTriggeredThisFrame = false;

            if (Pointer.current == null)
            {
                MoveInput = Vector2.zero;
                return;
            }

            Vector2 screenPos = Pointer.current.position.ReadValue();
            bool pressedThisFrame = Pointer.current.press.wasPressedThisFrame;
            bool releasedThisFrame = Pointer.current.press.wasReleasedThisFrame;
            bool isPressed = Pointer.current.press.isPressed;
            bool isLeftHalf = screenPos.x < Screen.width * 0.5f;

            if (pressedThisFrame)
            {
                if (isLeftHalf)
                {
                    moveTouchActive = true;
                    moveOrigin = screenPos;
                }
                else
                {
                    AttackTriggeredThisFrame = true;
                }
            }

            if (moveTouchActive && isPressed)
            {
                Vector2 delta = screenPos - moveOrigin;
                MoveInput = Vector2.ClampMagnitude(delta / moveDragPixelsForFullSpeed, 1f);
            }
            else
            {
                MoveInput = Vector2.zero;
            }

            if (releasedThisFrame)
            {
                moveTouchActive = false;
            }
        }
    }
}
