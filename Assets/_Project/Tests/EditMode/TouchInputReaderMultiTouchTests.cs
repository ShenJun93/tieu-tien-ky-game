using NUnit.Framework;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TieuTienKy.Gameplay.Tests
{
    public class TouchInputReaderMultiTouchTests : InputTestFixture
    {
        GameObject go;
        TouchInputReader reader;
        float leftX;
        float rightX;
        float midY;

        public override void Setup()
        {
            base.Setup();

            leftX = Screen.width * 0.25f;
            rightX = Screen.width * 0.75f;
            midY = Screen.height * 0.5f;

            go = new GameObject("TouchInputReaderUnderTest");
            reader = go.AddComponent<TouchInputReader>();
        }

        public override void TearDown()
        {
            if (go != null)
            {
                Object.DestroyImmediate(go);
            }

            base.TearDown();
        }

        [Test]
        public void LeftTouchBegin_OwnsMovement_DoesNotTriggerAttack()
        {
            BeginTouch(1, new Vector2(leftX, midY));
            reader.Update();

            Assert.IsFalse(reader.AttackTriggeredThisFrame);

            MoveTouch(1, new Vector2(leftX + 40f, midY));
            reader.Update();

            Assert.Greater(reader.MoveInput.x, 0f);
        }

        [Test]
        public void RightTouchBegin_WhileLeftMoveHeld_TriggersAttackWithoutInterruptingMovement()
        {
            BeginTouch(1, new Vector2(leftX, midY));
            reader.Update();

            MoveTouch(1, new Vector2(leftX + 40f, midY));
            reader.Update();
            Vector2 moveInputWhileHeld = reader.MoveInput;
            Assert.AreNotEqual(Vector2.zero, moveInputWhileHeld);

            BeginTouch(2, new Vector2(rightX, midY));
            reader.Update();

            Assert.IsTrue(reader.AttackTriggeredThisFrame);
            Assert.AreEqual(moveInputWhileHeld, reader.MoveInput);
        }

        [Test]
        public void SecondLeftTouch_WhileFirstOwnsMovement_DoesNotStealOwnership()
        {
            BeginTouch(1, new Vector2(leftX, midY));
            reader.Update();

            BeginTouch(2, new Vector2(leftX - 20f, midY));
            reader.Update();

            Assert.IsFalse(reader.AttackTriggeredThisFrame);

            EndTouch(1, new Vector2(leftX, midY));
            reader.Update();

            Assert.AreEqual(Vector2.zero, reader.MoveInput);
        }

        [Test]
        public void TwoRightTouchesBeginningSameFrame_TriggerAttackOnlyOnce()
        {
            SetTouch(101, TouchPhase.Began, new Vector2(rightX, midY), queueEventOnly: true);
            SetTouch(102, TouchPhase.Began, new Vector2(rightX + 10f, midY), queueEventOnly: false);
            reader.Update();

            Assert.IsTrue(reader.AttackTriggeredThisFrame);
        }
    }
}
