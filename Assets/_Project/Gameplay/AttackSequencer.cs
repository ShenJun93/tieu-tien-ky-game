namespace TieuTienKy.Gameplay
{
    public enum AttackPhase
    {
        Idle,
        Anticipation,
        Recovery
    }

    /// <summary>
    /// Pure anticipation -> impact -> recovery timing for the single Basic
    /// Attack, kept separate from MonoBehaviour/Time so the sequencing is
    /// unit-testable without a scene. No combo/cancel system by design.
    /// </summary>
    public sealed class AttackSequencer
    {
        readonly float anticipationSeconds;
        readonly float recoverySeconds;
        float phaseEndTime;

        public AttackSequencer(float anticipationSeconds, float recoverySeconds)
        {
            this.anticipationSeconds = anticipationSeconds;
            this.recoverySeconds = recoverySeconds;
        }

        public AttackPhase Phase { get; private set; } = AttackPhase.Idle;

        /// <summary>Starts a swing if and only if currently Idle.</summary>
        public bool TryBeginAttack(float currentTime)
        {
            if (Phase != AttackPhase.Idle)
            {
                return false;
            }

            Phase = AttackPhase.Anticipation;
            phaseEndTime = currentTime + anticipationSeconds;
            return true;
        }

        /// <summary>
        /// Advances the sequencer. Returns true exactly once, on the frame
        /// anticipation ends and the hit should be applied. Sets
        /// <paramref name="recoveryEnded"/> true exactly once, on the frame
        /// recovery ends and the next attack becomes available.
        /// </summary>
        public bool Tick(float currentTime, out bool recoveryEnded)
        {
            recoveryEnded = false;

            switch (Phase)
            {
                case AttackPhase.Anticipation when currentTime >= phaseEndTime:
                    Phase = AttackPhase.Recovery;
                    phaseEndTime = currentTime + recoverySeconds;
                    return true;

                case AttackPhase.Recovery when currentTime >= phaseEndTime:
                    Phase = AttackPhase.Idle;
                    recoveryEnded = true;
                    return false;

                default:
                    return false;
            }
        }
    }
}
