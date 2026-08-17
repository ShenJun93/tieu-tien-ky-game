namespace TieuTienKy.Gameplay
{
    public enum EnemyAttackPhase
    {
        Chase,
        Telegraph,
        Recovery
    }

    /// <summary>
    /// Pure CHASE -> TELEGRAPH -> ATTACK signal -> RECOVERY timing, kept
    /// separate from MonoBehaviour/Time so it is unit-testable without a
    /// scene. Mirrors AttackSequencer's proven style. No behavior
    /// tree/generic AI system.
    /// </summary>
    public sealed class EnemyAttackCycle
    {
        readonly float telegraphSeconds;
        readonly float recoverySeconds;
        float phaseEndTime;

        public EnemyAttackCycle(float telegraphSeconds, float recoverySeconds)
        {
            this.telegraphSeconds = telegraphSeconds;
            this.recoverySeconds = recoverySeconds;
        }

        public EnemyAttackPhase Phase { get; private set; } = EnemyAttackPhase.Chase;

        /// <summary>Begins a telegraph if and only if currently Chasing.</summary>
        public bool TryBeginTelegraph(float currentTime)
        {
            if (Phase != EnemyAttackPhase.Chase)
            {
                return false;
            }

            Phase = EnemyAttackPhase.Telegraph;
            phaseEndTime = currentTime + telegraphSeconds;
            return true;
        }

        /// <summary>
        /// Advances the cycle. Returns true exactly once, on the frame the
        /// telegraph ends and the attack should fire. Sets
        /// <paramref name="recoveryEnded"/> true exactly once, on the frame
        /// recovery ends and chase resumes.
        /// </summary>
        public bool Tick(float currentTime, out bool recoveryEnded)
        {
            recoveryEnded = false;

            switch (Phase)
            {
                case EnemyAttackPhase.Telegraph when currentTime >= phaseEndTime:
                    Phase = EnemyAttackPhase.Recovery;
                    phaseEndTime = currentTime + recoverySeconds;
                    return true;

                case EnemyAttackPhase.Recovery when currentTime >= phaseEndTime:
                    Phase = EnemyAttackPhase.Chase;
                    recoveryEnded = true;
                    return false;

                default:
                    return false;
            }
        }

        public void Reset()
        {
            Phase = EnemyAttackPhase.Chase;
            phaseEndTime = 0f;
        }
    }
}
