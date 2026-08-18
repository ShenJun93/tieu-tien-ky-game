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
        readonly Combatant combatant;

        public PlayerActionExecutor(BasicAttack basicAttack, PlayerSkillController skillController, Combatant combatant = null)
        {
            this.basicAttack = basicAttack;
            this.skillController = skillController;
            this.combatant = combatant;
        }

        /// <summary>Task B5: a defeated player (network death lifecycle, awaiting respawn) can request nothing until full gameplay control is restored. Solo play is unaffected - a defeated solo run has already ended.</summary>
        bool IsDefeated => combatant != null && combatant.IsDefeated;

        public bool ExecuteBasicAttack() => !IsDefeated && basicAttack != null && basicAttack.TryActivate(Time.time);
        public bool ExecuteLoiTram() => !IsDefeated && skillController != null && skillController.TryActivateLoiTram();
        public bool ExecutePhongBo() => !IsDefeated && skillController != null && skillController.TryActivatePhongBo();
        public bool ExecuteHoThe() => !IsDefeated && skillController != null && skillController.TryActivateHoThe();
    }
}
