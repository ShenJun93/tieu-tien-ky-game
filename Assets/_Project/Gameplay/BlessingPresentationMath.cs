using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Pure per-stack presentation scaling for the three Cơ Duyên blessings.
    /// Kept separate from the MonoBehaviours that actually build/tint
    /// primitives (PlayerBlessingPresentation, SwordAttackView, BasicAttack)
    /// so the escalation curve itself is unit-testable without a scene. Every
    /// blessing must visibly escalate stack I -> II -> III; this is the one
    /// place that escalation curve is defined.
    /// </summary>
    public static class BlessingPresentationMath
    {
        const float SwordAccentScalePerStack = 0.18f;
        const float WindRingBaseScale = 0.7f;
        const float WindRingScalePerStack = 0.25f;
        const float WardAuraBaseScale = 1.15f;
        const float WardAuraScalePerStack = 0.18f;
        const float LightningFlashBaseRadius = 0.35f;
        const float LightningFlashRadiusPerStack = 0.18f;
        const float LightningFlashBaseLifetime = 0.12f;
        const float LightningFlashLifetimePerStack = 0.03f;

        /// <summary>Sword weapon visual scale multiplier. Always >= 1, even at 0 stacks (base weapon size).</summary>
        public static float SwordAccentScale(int thunderStacks) => 1f + Mathf.Max(0, thunderStacks) * SwordAccentScalePerStack;

        /// <summary>Sword weapon lightning-flash impact radius. Present even at 0 stacks (baseline attack readability), escalates with stacks.</summary>
        public static float LightningFlashPeakRadius(int thunderStacks) => LightningFlashBaseRadius + Mathf.Max(0, thunderStacks) * LightningFlashRadiusPerStack;

        public static float LightningFlashLifetimeSeconds(int thunderStacks) => LightningFlashBaseLifetime + Mathf.Max(0, thunderStacks) * LightningFlashLifetimePerStack;

        /// <summary>Wind-accent ring scale. 0 (hidden) until the blessing is actually taken.</summary>
        public static float WindRingScale(int windStacks) => windStacks <= 0 ? 0f : WindRingBaseScale + windStacks * WindRingScalePerStack;

        public static bool WindRingVisible(int windStacks) => windStacks > 0;

        /// <summary>Body-ward aura scale. 0 (hidden) until the blessing is actually taken.</summary>
        public static float WardAuraScale(int wardStacks) => wardStacks <= 0 ? 0f : WardAuraBaseScale + wardStacks * WardAuraScalePerStack;

        public static bool WardAuraVisible(int wardStacks) => wardStacks > 0;
    }
}
