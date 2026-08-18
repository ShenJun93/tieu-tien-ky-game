namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// The single UI/input -> gameplay execution boundary for player actions
    /// (Task B1). LocalPlayerActionGateway executes immediately against a
    /// local PlayerActionExecutor; NetworkPlayerActionGateway forwards the
    /// request to the authoritative host, which executes against the same
    /// kind of PlayerActionExecutor on its own copy of the player. UI/HUD
    /// code should depend only on this interface, never directly on
    /// BasicAttack/skills, so the same button works unchanged in either
    /// mode. Each method returns whether the request was actually sent/
    /// resolved (e.g. false on cooldown), never whether it dealt damage -
    /// outcomes are not this boundary's concern.
    /// </summary>
    public interface IPlayerActionGateway
    {
        bool RequestBasicAttack();
        bool RequestLoiTram();
        bool RequestPhongBo();
        bool RequestHoThe();
    }
}
