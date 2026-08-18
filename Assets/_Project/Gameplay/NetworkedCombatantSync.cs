using Unity.Netcode;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Mirrors this player's server-authoritative Combatant health/defeat
    /// state to every non-authoritative observer (Task B3/B4): the server
    /// is the only peer whose Combatant.TakeHit ever actually resolves
    /// damage (reached only through PlayerActionExecutor inside a
    /// ServerRpc handler), and this component republishes the result via
    /// NetworkVariables so remote clients' own local Combatant - used for
    /// their own HUD/presentation reads - reflects the same truth. Never
    /// recomputes damage/mitigation/knockback itself.
    /// </summary>
    [RequireComponent(typeof(Combatant))]
    public sealed class NetworkedCombatantSync : NetworkBehaviour
    {
        readonly NetworkVariable<int> syncedCurrentHealth = new NetworkVariable<int>(
            writePerm: NetworkVariableWritePermission.Server);
        readonly NetworkVariable<int> syncedMaxHealth = new NetworkVariable<int>(
            writePerm: NetworkVariableWritePermission.Server);

        Combatant combatant;

        void Awake()
        {
            combatant = GetComponent<Combatant>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                syncedCurrentHealth.Value = combatant.CurrentHealth;
                syncedMaxHealth.Value = combatant.MaxHealth;
                combatant.Damaged += OnLocalDamaged;
            }
            else
            {
                syncedCurrentHealth.OnValueChanged += OnSyncedHealthChanged;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
            {
                combatant.Damaged -= OnLocalDamaged;
            }
            else
            {
                syncedCurrentHealth.OnValueChanged -= OnSyncedHealthChanged;
            }
        }

        void OnLocalDamaged(int current, int max)
        {
            syncedCurrentHealth.Value = current;
            syncedMaxHealth.Value = max;
        }

        void OnSyncedHealthChanged(int previous, int current)
        {
            combatant.ApplySyncedHealth(current, syncedMaxHealth.Value);
        }
    }
}
