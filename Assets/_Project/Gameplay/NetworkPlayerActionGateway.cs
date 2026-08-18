using Unity.Netcode;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Client-owned request -> server-authoritative execution (Task B1/B3),
    /// using the exact same PlayerActionExecutor a local player resolves
    /// through - never a second attack/skill implementation. The client-side
    /// Request* call only asks; it never grants itself an outcome. Each
    /// ServerRpc defaults to RequireOwnership = true, so only this
    /// connection's own player can drive it - satisfies "remote participant
    /// never drives local input" once wired into the network player prefab
    /// (Task B2).
    /// </summary>
    public sealed class NetworkPlayerActionGateway : NetworkBehaviour, IPlayerActionGateway
    {
        PlayerActionExecutor executor;

        /// <summary>Wired on the server/host side only, where the authoritative BasicAttack/skill components live for this player.</summary>
        public void Initialize(PlayerActionExecutor playerActionExecutor) => executor = playerActionExecutor;

        public bool RequestBasicAttack()
        {
            RequestBasicAttackServerRpc();
            return true;
        }

        public bool RequestLoiTram()
        {
            RequestLoiTramServerRpc();
            return true;
        }

        public bool RequestPhongBo()
        {
            RequestPhongBoServerRpc();
            return true;
        }

        public bool RequestHoThe()
        {
            RequestHoTheServerRpc();
            return true;
        }

        [ServerRpc]
        void RequestBasicAttackServerRpc() => executor?.ExecuteBasicAttack();

        [ServerRpc]
        void RequestLoiTramServerRpc() => executor?.ExecuteLoiTram();

        [ServerRpc]
        void RequestPhongBoServerRpc() => executor?.ExecutePhongBo();

        [ServerRpc]
        void RequestHoTheServerRpc() => executor?.ExecuteHoThe();
    }
}
