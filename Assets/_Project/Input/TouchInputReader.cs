using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TieuTienKy.Input
{
    /// <summary>
    /// Touch/mouse reader: left half of the screen is a drag-to-move zone, right
    /// half is tap-to-attack. Reads real independent touches via EnhancedTouch so
    /// a left-side move touch and a right-side attack touch can be held at the
    /// same time; falls back to Mouse.current when no touchscreen is present
    /// (Editor testing).
    /// </summary>
    public sealed class TouchInputReader : MonoBehaviour
    {
        [SerializeField] float moveDragPixelsForFullSpeed = 120f;

        bool moveTouchActive;
        int moveTouchId = -1;
        Vector2 moveOrigin;
        bool enhancedTouchOwnedByThis;

        public Vector2 MoveInput { get; private set; }
        public bool AttackTriggeredThisFrame { get; private set; }

        /// <summary>Test seam: overrides the EventSystem UI-hit probe so tests can simulate a touch beginning on interactive UI without a live Canvas.</summary>
        public System.Func<int, bool> IsPointerOverUiOverride { get; set; }

        void OnEnable()
        {
            EnableEnhancedTouchIfNeeded();
        }

        void OnDisable()
        {
            if (enhancedTouchOwnedByThis)
            {
                EnhancedTouchSupport.Disable();
                enhancedTouchOwnedByThis = false;
            }
        }

        void EnableEnhancedTouchIfNeeded()
        {
            if (!enhancedTouchOwnedByThis)
            {
                EnhancedTouchSupport.Enable();
                enhancedTouchOwnedByThis = true;
            }
        }

        public void Update()
        {
            AttackTriggeredThisFrame = false;

            if (Touchscreen.current != null)
            {
                // OnEnable normally already did this; guarded re-check covers the
                // rare case where Update runs before OnEnable has taken effect
                // (observed under the Editor's EditMode test runner).
                EnableEnhancedTouchIfNeeded();
                ReadTouches();
            }
            else
            {
                ReadMouseFallback();
            }
        }

        void ReadTouches()
        {
            bool attackThisFrame = false;
            ReadOnlyArray<Touch> touches = Touch.activeTouches;

            for (int i = 0; i < touches.Count; i++)
            {
                Touch touch = touches[i];
                bool isLeftHalf = touch.screenPosition.x < Screen.width * 0.5f;
                bool began = touch.phase == TouchPhase.Began;
                bool endedOrCanceled = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;

                if (began && isLeftHalf && !moveTouchActive)
                {
                    moveTouchActive = true;
                    moveTouchId = touch.touchId;
                    moveOrigin = touch.screenPosition;
                }
                else if (began && !isLeftHalf && !IsPointerOverUi(touch.touchId))
                {
                    attackThisFrame = true;
                }

                if (moveTouchActive && touch.touchId == moveTouchId)
                {
                    if (endedOrCanceled)
                    {
                        moveTouchActive = false;
                        moveTouchId = -1;
                    }
                    else
                    {
                        Vector2 delta = touch.screenPosition - moveOrigin;
                        MoveInput = Vector2.ClampMagnitude(delta / moveDragPixelsForFullSpeed, 1f);
                    }
                }
            }

            if (!moveTouchActive)
            {
                MoveInput = Vector2.zero;
            }

            AttackTriggeredThisFrame = attackThisFrame;
        }

        void ReadMouseFallback()
        {
            if (Mouse.current == null)
            {
                MoveInput = Vector2.zero;
                return;
            }

            Vector2 screenPos = Mouse.current.position.ReadValue();
            bool pressedThisFrame = Mouse.current.leftButton.wasPressedThisFrame;
            bool releasedThisFrame = Mouse.current.leftButton.wasReleasedThisFrame;
            bool isPressed = Mouse.current.leftButton.isPressed;
            bool isLeftHalf = screenPos.x < Screen.width * 0.5f;

            if (pressedThisFrame)
            {
                if (isLeftHalf)
                {
                    moveTouchActive = true;
                    moveOrigin = screenPos;
                }
                else if (!IsPointerOverUi(-1))
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

        bool IsPointerOverUi(int pointerId)
        {
            if (IsPointerOverUiOverride != null)
            {
                return IsPointerOverUiOverride(pointerId);
            }

            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }
    }
}
