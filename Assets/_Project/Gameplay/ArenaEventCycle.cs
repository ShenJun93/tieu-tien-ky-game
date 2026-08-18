namespace TieuTienKy.Gameplay
{
    public enum ArenaEventPhase
    {
        Inactive,
        Warning,
        Active,
        Cooldown
    }

    /// <summary>
    /// Pure WARNING -> ACTIVE -> COOLDOWN -> INACTIVE timing for one arena
    /// event occurrence, kept separate from MonoBehaviour/Time so it is
    /// unit-testable without a scene. Mirrors AttackSequencer/
    /// EnemyAttackCycle's proven style.
    /// </summary>
    public sealed class ArenaEventCycle
    {
        readonly float warningSeconds;
        readonly float activeSeconds;
        readonly float cooldownSeconds;
        float phaseEndTime;

        public ArenaEventCycle(float warningSeconds, float activeSeconds, float cooldownSeconds)
        {
            this.warningSeconds = warningSeconds;
            this.activeSeconds = activeSeconds;
            this.cooldownSeconds = cooldownSeconds;
        }

        public ArenaEventPhase Phase { get; private set; } = ArenaEventPhase.Inactive;

        /// <summary>Begins a warning if and only if currently Inactive.</summary>
        public bool Begin(float currentTime)
        {
            if (Phase != ArenaEventPhase.Inactive)
            {
                return false;
            }

            Phase = ArenaEventPhase.Warning;
            phaseEndTime = currentTime + warningSeconds;
            return true;
        }

        /// <summary>
        /// Advances the cycle. Returns true - and sets
        /// <paramref name="becameActive"/> true - exactly once, on the frame
        /// the warning ends and the event becomes Active. Sets
        /// <paramref name="finished"/> true exactly once, on the frame
        /// cooldown ends and the cycle returns to Inactive.
        /// </summary>
        public bool Tick(float currentTime, out bool becameActive, out bool finished)
        {
            becameActive = false;
            finished = false;

            switch (Phase)
            {
                case ArenaEventPhase.Warning when currentTime >= phaseEndTime:
                    Phase = ArenaEventPhase.Active;
                    phaseEndTime = currentTime + activeSeconds;
                    becameActive = true;
                    return true;

                case ArenaEventPhase.Active when currentTime >= phaseEndTime:
                    Phase = ArenaEventPhase.Cooldown;
                    phaseEndTime = currentTime + cooldownSeconds;
                    return false;

                case ArenaEventPhase.Cooldown when currentTime >= phaseEndTime:
                    Phase = ArenaEventPhase.Inactive;
                    finished = true;
                    return false;

                default:
                    return false;
            }
        }

        public void Reset()
        {
            Phase = ArenaEventPhase.Inactive;
            phaseEndTime = 0f;
        }
    }
}
