using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Solo-play IPlayerActionGateway: resolves every request immediately
    /// against the local PlayerActionExecutor. UI (ProductionHud's skill
    /// buttons) depends on IPlayerActionGateway, not on this concrete type,
    /// so it needs no changes when a scene wires NetworkPlayerActionGateway
    /// instead (Task B2).
    /// </summary>
    public sealed class LocalPlayerActionGateway : MonoBehaviour, IPlayerActionGateway
    {
        PlayerActionExecutor executor;

        public void Initialize(PlayerActionExecutor playerActionExecutor) => executor = playerActionExecutor;

        public bool RequestBasicAttack() => executor != null && executor.ExecuteBasicAttack();
        public bool RequestLoiTram() => executor != null && executor.ExecuteLoiTram();
        public bool RequestPhongBo() => executor != null && executor.ExecutePhongBo();
        public bool RequestHoThe() => executor != null && executor.ExecuteHoThe();
    }
}
