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
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        }

        void OnDestroy()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
                NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            }
        }

        /// <summary>
        /// Fires exactly when hosting/serving actually begins - unlike a
        /// synchronous IsServer check in Start(), this is not racing
        /// whichever other component's Start() actually calls StartHost().
        /// NetworkArenaSessionDirector is itself a NetworkBehaviour and must
        /// be spawned (it has no in-scene NetworkObject auto-spawn wiring)
        /// before RegisterPlayer's IsServer check can ever be true.
        /// </summary>
        void OnServerStarted()
        {
            sessionDirector.GetComponent<NetworkObject>().Spawn();
            sessionDirector.Initialize(spawnPoints);
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
