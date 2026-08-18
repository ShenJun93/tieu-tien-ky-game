using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Server-only bounded death/respawn lifecycle for the 2-player network
    /// duel arena (Task B5): alive -> authoritative death -> visible death
    /// state (Combatant.Defeated already drives presentation.PlayDeath,
    /// replicated via NetworkedCombatantSync) -> short respawn delay ->
    /// legal respawn position -> full gameplay control restored
    /// (Combatant.ResetCombatant clears IsDefeated, which
    /// PlayerActionExecutor/NetworkPlayerMovement were gating on). No
    /// revive, reconnect or persistence - reuses the exact same
    /// Combatant.Defeated/ResetCombatant primitives solo play already uses,
    /// just triggered per-connection instead of ending a run.
    /// </summary>
    public sealed class NetworkArenaSessionDirector : NetworkBehaviour
    {
        [SerializeField] float respawnDelaySeconds = 2f;

        Transform[] spawnPoints;
        int nextSpawnIndex;

        public void Initialize(Transform[] spawns)
        {
            spawnPoints = spawns;
        }

        /// <summary>Called once per spawned player, server-side only, so this director owns exactly one respawn subscription per Combatant.</summary>
        public void RegisterPlayer(Combatant combatant)
        {
            if (!IsServer)
            {
                return;
            }

            combatant.Defeated += () => StartCoroutine(RespawnAfterDelay(combatant));
        }

        IEnumerator RespawnAfterDelay(Combatant combatant)
        {
            yield return new WaitForSeconds(respawnDelaySeconds);

            combatant.ResetCombatant(NextSpawnPosition());
        }

        Vector3 NextSpawnPosition()
        {
            if (spawnPoints == null || spawnPoints.Length == 0)
            {
                return Vector3.zero;
            }

            Vector3 position = spawnPoints[nextSpawnIndex % spawnPoints.Length].position;
            nextSpawnIndex++;
            return position;
        }
    }
}
