using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Applies a bounded knockback impulse to a CharacterController over a short,
    /// decelerating window. Deliberately simple: no physics-combat framework.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class KnockbackReceiver : MonoBehaviour
    {
        [SerializeField] float maxKnockbackMagnitude = 16f;
        [SerializeField] float decayPerSecond = 18f;

        CharacterController controller;
        Vector3 currentVelocity;

        public bool IsBeingKnockedBack => currentVelocity.sqrMagnitude > 0.01f;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        public void ApplyKnockback(Vector3 rawImpulse)
        {
            currentVelocity = KnockbackCalculator.ClampToBound(rawImpulse, maxKnockbackMagnitude);
        }

        void Update()
        {
            if (!IsBeingKnockedBack)
            {
                return;
            }

            controller.Move(currentVelocity * Time.deltaTime);
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, decayPerSecond * Time.deltaTime);
        }
    }
}
