using Unity.Netcode;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Composition root for the authored 2-player network duel scene (Task
    /// B2). Places each newly-connected client's auto-spawned player at an
    /// alternating spawn point and registers it with
    /// NetworkArenaSessionDirector for the death/respawn lifecycle (Task
    /// B5). Not a generic session/lobby framework - exactly two seats, no
    /// matchmaking, no reconnect.
    /// </summary>
    public sealed class NetworkArenaSceneBootstrap : MonoBehaviour
    {
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] NetworkArenaSessionDirector sessionDirector;

        void Start()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            if (NetworkManager.Singleton.IsServer)
            {
                sessionDirector.Initialize(spawnPoints);
            }
        }

        void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            }
        }

        void OnClientConnected(ulong clientId)
        {
            if (!NetworkManager.Singleton.IsServer || spawnPoints == null || spawnPoints.Length == 0)
            {
                return;
            }

            if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out NetworkClient client) || client.PlayerObject == null)
            {
                return;
            }

            int index = (int)(clientId % (ulong)spawnPoints.Length);
            var combatant = client.PlayerObject.GetComponent<Combatant>();
            combatant.ResetCombatant(spawnPoints[index].position);

            sessionDirector.RegisterPlayer(combatant);
        }
    }
}
