using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Pure knockback math, kept separate from CharacterController application
    /// so the bound check is unit-testable without a scene.
    /// </summary>
    public static class KnockbackCalculator
    {
        public static Vector3 ClampToBound(Vector3 rawImpulse, float maxMagnitude)
        {
            maxMagnitude = Mathf.Max(0f, maxMagnitude);

            float magnitude = rawImpulse.magnitude;
            if (magnitude <= maxMagnitude || magnitude <= Mathf.Epsilon)
            {
                return rawImpulse;
            }

            return rawImpulse * (maxMagnitude / magnitude);
        }

        /// <summary>Scales a knockback impulse when an elemental reaction (e.g. Conductive Burst) triggered, so the reaction reads as clearly stronger than a normal hit.</summary>
        public static Vector3 ApplyReactionMultiplier(Vector3 baseImpulse, bool reactionTriggered, float multiplier)
        {
            return reactionTriggered ? baseImpulse * multiplier : baseImpulse;
        }
    }
}
