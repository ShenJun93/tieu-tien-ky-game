namespace TieuTienKy.Gameplay
{
    /// <summary>The one HUD callback ArenaRunDirector needs, shared by RunHud (P0A_Greybox sandbox) and ProductionHud (Arena_VerticalSlice_01) so the director does not depend on which HUD implementation is wired.</summary>
    public interface IBossArrivalCueDisplay
    {
        void ShowBossArrivalCue();
    }
}
