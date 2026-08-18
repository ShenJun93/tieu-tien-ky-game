using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    public enum BlessingId
    {
        ThunderSword,
        WindStride,
        BodyWard
    }

    public readonly struct RunCombatModifiers
    {
        public readonly float ConductiveMultiplier;
        public readonly float MoveSpeedMultiplier;
        public readonly float AttackRecoveryMultiplier;
        public readonly int MaxHealthBonus;
        public readonly float PhongBoCooldownMultiplier;
        public readonly float HoTheWindowBonusSeconds;

        public RunCombatModifiers(float conductiveMultiplier, float moveSpeedMultiplier, float attackRecoveryMultiplier, int maxHealthBonus, float phongBoCooldownMultiplier, float hoTheWindowBonusSeconds)
        {
            ConductiveMultiplier = conductiveMultiplier;
            MoveSpeedMultiplier = moveSpeedMultiplier;
            AttackRecoveryMultiplier = attackRecoveryMultiplier;
            MaxHealthBonus = maxHealthBonus;
            PhongBoCooldownMultiplier = phongBoCooldownMultiplier;
            HoTheWindowBonusSeconds = hoTheWindowBonusSeconds;
        }
    }

    /// <summary>
    /// Pure in-run Cơ Duyên stacks and the resulting combat modifiers. No
    /// persistence, no inventory, no permanent skill tree: Reset returns to
    /// the run's starting state (called on run restart only).
    /// </summary>
    public sealed class RunBlessingState
    {
        public const float BaseConductiveMultiplier = 2.5f;
        const float ThunderSwordConductiveBonusPerStack = 0.75f;
        const float WindStrideMoveSpeedMultiplierPerStack = 1.12f;
        const float WindStrideRecoveryMultiplierPerStack = 0.92f;
        const float WindStridePhongBoCooldownMultiplierPerStack = 0.85f;
        const int BodyWardMaxHealthBonusPerStack = 2;
        const float BodyWardHoTheWindowBonusSecondsPerStack = 0.15f;
        const int MaxStacksPerBlessing = 3;

        readonly Dictionary<BlessingId, int> stacks = new Dictionary<BlessingId, int>
        {
            { BlessingId.ThunderSword, 0 },
            { BlessingId.WindStride, 0 },
            { BlessingId.BodyWard, 0 }
        };

        public void Apply(BlessingId id)
        {
            stacks[id] = Mathf.Min(MaxStacksPerBlessing, stacks[id] + 1);
        }

        public int StackCount(BlessingId id) => stacks[id];

        public RunCombatModifiers CurrentModifiers
        {
            get
            {
                int thunderStacks = stacks[BlessingId.ThunderSword];
                int windStacks = stacks[BlessingId.WindStride];
                int wardStacks = stacks[BlessingId.BodyWard];

                float conductiveMultiplier = BaseConductiveMultiplier + thunderStacks * ThunderSwordConductiveBonusPerStack;
                float moveSpeedMultiplier = Mathf.Pow(WindStrideMoveSpeedMultiplierPerStack, windStacks);
                float attackRecoveryMultiplier = Mathf.Pow(WindStrideRecoveryMultiplierPerStack, windStacks);
                int maxHealthBonus = wardStacks * BodyWardMaxHealthBonusPerStack;
                float phongBoCooldownMultiplier = Mathf.Pow(WindStridePhongBoCooldownMultiplierPerStack, windStacks);
                float hoTheWindowBonusSeconds = wardStacks * BodyWardHoTheWindowBonusSecondsPerStack;

                return new RunCombatModifiers(conductiveMultiplier, moveSpeedMultiplier, attackRecoveryMultiplier, maxHealthBonus, phongBoCooldownMultiplier, hoTheWindowBonusSeconds);
            }
        }

        public void Reset()
        {
            stacks[BlessingId.ThunderSword] = 0;
            stacks[BlessingId.WindStride] = 0;
            stacks[BlessingId.BodyWard] = 0;
        }
    }
}
