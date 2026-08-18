namespace TieuTienKy.Gameplay
{
    public enum ArenaRunStage
    {
        Wave1,
        Blessing1,
        Wave2,
        Blessing2,
        EliteWave,
        Blessing3,
        Boss,
        Victory,
        Defeat
    }

    /// <summary>
    /// Pure stage-progression state machine for one arena run. Explicit
    /// switch transitions only - no state-machine framework/graph.
    /// </summary>
    public sealed class ArenaRunProgression
    {
        public ArenaRunStage Stage { get; private set; } = ArenaRunStage.Wave1;

        /// <summary>Advances from a combat stage to its following gate/end. Returns false (no change) if not currently in a combat stage.</summary>
        public bool AdvanceAfterCombatClear()
        {
            switch (Stage)
            {
                case ArenaRunStage.Wave1:
                    Stage = ArenaRunStage.Blessing1;
                    return true;
                case ArenaRunStage.Wave2:
                    Stage = ArenaRunStage.Blessing2;
                    return true;
                case ArenaRunStage.EliteWave:
                    Stage = ArenaRunStage.Blessing3;
                    return true;
                case ArenaRunStage.Boss:
                    Stage = ArenaRunStage.Victory;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>Advances from a blessing gate to the next combat stage. Returns false (no change) if not currently at a blessing gate.</summary>
        public bool AdvanceAfterBlessingChoice()
        {
            switch (Stage)
            {
                case ArenaRunStage.Blessing1:
                    Stage = ArenaRunStage.Wave2;
                    return true;
                case ArenaRunStage.Blessing2:
                    Stage = ArenaRunStage.EliteWave;
                    return true;
                case ArenaRunStage.Blessing3:
                    Stage = ArenaRunStage.Boss;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>Ends the run in defeat from any non-Victory stage.</summary>
        public void MarkDefeat()
        {
            if (Stage != ArenaRunStage.Victory)
            {
                Stage = ArenaRunStage.Defeat;
            }
        }

        public void Reset()
        {
            Stage = ArenaRunStage.Wave1;
        }
    }
}
