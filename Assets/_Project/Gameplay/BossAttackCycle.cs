namespace TieuTienKy.Gameplay
{
    public enum BossPattern
    {
        ArcStrike,
        Charge,
        RadialPulse
    }

    /// <summary>
    /// Tiny deterministic three-pattern cycle for the mini-boss. No random
    /// planner, no phases, no behavior tree.
    /// </summary>
    public sealed class BossAttackCycle
    {
        public BossPattern CurrentPattern { get; private set; } = BossPattern.ArcStrike;

        public BossPattern AdvancePattern()
        {
            CurrentPattern = CurrentPattern switch
            {
                BossPattern.ArcStrike => BossPattern.Charge,
                BossPattern.Charge => BossPattern.RadialPulse,
                _ => BossPattern.ArcStrike
            };

            return CurrentPattern;
        }

        public void Reset()
        {
            CurrentPattern = BossPattern.ArcStrike;
        }
    }
}
