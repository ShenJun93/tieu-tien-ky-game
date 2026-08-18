using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// The one place local and network play both resolve a player action
    /// against real gameplay components (Task B1). LocalPlayerActionGateway
    /// calls these methods directly on the local player's executor;
    /// NetworkPlayerActionGateway's server-side ServerRpc handler calls the
    /// identical methods on the authoritative player's executor. This class
    /// never reimplements BasicAttack/skill logic - it is a thin
    /// coordination boundary, so there is exactly one gameplay execution
    /// path regardless of mode.
    /// </summary>
    public sealed class PlayerActionExecutor
    {
        readonly BasicAttack basicAttack;
        readonly PlayerSkillController skillController;

        public PlayerActionExecutor(BasicAttack basicAttack, PlayerSkillController skillController)
        {
            this.basicAttack = basicAttack;
            this.skillController = skillController;
        }

        public bool ExecuteBasicAttack() => basicAttack != null && basicAttack.TryActivate(Time.time);
        public bool ExecuteLoiTram() => skillController != null && skillController.TryActivateLoiTram();
        public bool ExecutePhongBo() => skillController != null && skillController.TryActivatePhongBo();
        public bool ExecuteHoThe() => skillController != null && skillController.TryActivateHoThe();
    }
}
