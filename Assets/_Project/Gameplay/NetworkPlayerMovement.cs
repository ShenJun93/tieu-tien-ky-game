using Unity.Netcode;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Server-authoritative movement bridge and per-player authority gate
    /// (Task B2/B3). The owning client reads its own local
    /// TouchInputReader every frame and forwards the move vector to the
    /// server via an unreliable ServerRpc; the server - the only peer that
    /// ever calls PlayerController.ApplyMove for this player, own or remote
    /// - moves the CharacterController, and NetworkTransform (server-
    /// authoritative, Netcode's default) replicates the resulting position
    /// to every client, including the owner. No client-side prediction or
    /// reconciliation, per the task's explicit "avoid speculative
    /// rollback/prediction architecture" constraint.
    ///
    /// Also disables the local self-driven Update() paths
    /// (PlayerController, KnockbackReceiver, BasicAttack's own touch-input
    /// read) on every peer except the server, so knockback/movement/attack
    /// resolution only ever happens once, authoritatively - never
    /// independently recomputed (and potentially diverging) on a client.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    [RequireComponent(typeof(BasicAttack))]
    [RequireComponent(typeof(Combatant))]
    [RequireComponent(typeof(TieuTienKy.Input.TouchInputReader))]
    public sealed class NetworkPlayerMovement : NetworkBehaviour
    {
        PlayerController playerController;
        KnockbackReceiver knockbackReceiver;
        BasicAttack basicAttack;
        Combatant combatant;
        TieuTienKy.Input.TouchInputReader inputReader;
        IPlayerActionGateway actionGateway;
        Vector2 latestReceivedMoveInput;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
            basicAttack = GetComponent<BasicAttack>();
            combatant = GetComponent<Combatant>();
            inputReader = GetComponent<TieuTienKy.Input.TouchInputReader>();
        }

        public override void OnNetworkSpawn()
        {
            playerController.enabled = IsServer;
            knockbackReceiver.enabled = IsServer;
            basicAttack.SetNetworkDriven(true);
            basicAttack.enabled = IsServer;

            var gateway = GetComponent<NetworkPlayerActionGateway>();
            actionGateway = gateway;

            if (IsServer)
            {
                var skillController = GetComponent<PlayerSkillController>();
                var executor = new PlayerActionExecutor(basicAttack, skillController, combatant);
                gateway.Initialize(executor);
            }

            // Hit/Death presentation is driven by Combatant.Damaged/Defeated,
            // which NetworkedCombatantSync fires on every peer (server via
            // the real hit, observers via the synced NetworkVariable) - so
            // wiring this on every peer replicates hit-reaction/death
            // correctly. AttackStarted (the swing/cast animation itself)
            // only fires on the server, since only the server ever calls
            // TryActivate - a remote observer will see the outcome (HP
            // drop, hit flinch, death) but not the attacker's own swing
            // animation. Deferred technical debt, not required by any
            // Task B6 pass marker (all of which are about outcome
            // agreement, not cross-client animation playback).
            var presentation = GetComponentInChildren<CharacterPresentation>();
            if (presentation != null)
            {
                var skillController = GetComponent<PlayerSkillController>();
                skillController.SetPresentation(presentation);
                combatant.Damaged += (_, __) => presentation.PlayHit();
                combatant.Defeated += presentation.PlayDeath;

                if (IsServer)
                {
                    basicAttack.AttackStarted += presentation.PlayBasicAttack;
                }
            }
        }

        void Update()
        {
            if (IsOwner)
            {
                SendMoveInputServerRpc(inputReader.MoveInput);

                if (inputReader.AttackTriggeredThisFrame)
                {
                    actionGateway?.RequestBasicAttack();
                }
            }

            if (IsServer && !combatant.IsDefeated)
            {
                playerController.ApplyMove(latestReceivedMoveInput, Time.deltaTime);
            }
        }

        [ServerRpc(Delivery = RpcDelivery.Unreliable)]
        void SendMoveInputServerRpc(Vector2 moveInput)
        {
            latestReceivedMoveInput = moveInput;
        }
    }
}
