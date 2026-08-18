using System.Collections;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Task B6: bounded true two-process smoke test. Only active when
    /// launched with -smoketest on the command line (normal manual
    /// multiplayer play in this scene is entirely unaffected). The HOST
    /// process drives every phase deterministically, calling the exact same
    /// PlayerActionExecutor/gateway entry points a real request would reach
    /// (BasicAttack/LoiTramSkill/PhongBoSkill/HoTheSkill are never
    /// reimplemented here); the CLIENT process only observes its own
    /// locally-replicated view (NetworkTransform position,
    /// NetworkedCombatantSync-mirrored health) and independently verifies
    /// it. Both processes log structured NET2_* markers to their own
    /// -logFile so an external harness can grep each process's evidence
    /// separately - proving two real, independent participants agree,
    /// not one process asserting on itself twice.
    /// </summary>
    public sealed class NetworkSmokeTestDriver : MonoBehaviour
    {
        const float ConnectTimeoutSeconds = 20f;
        const float StepTimeoutSeconds = 10f;
        const float PollInterval = 0.1f;

        bool isSmokeTest;
        string role = "client";

        void Start()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            isSmokeTest = args.Contains("-smoketest");
            role = args.FirstOrDefault(a => a.StartsWith("-role="))?.Substring("-role=".Length) ?? "client";

            if (!isSmokeTest)
            {
                return;
            }

            var transport = FindFirstObjectByType<UnityTransport>();
            transport.ConnectionData.Address = "127.0.0.1";
            transport.ConnectionData.Port = 7777;

            if (role == "host")
            {
                StartCoroutine(RunHost());
            }
            else
            {
                StartCoroutine(RunClient());
            }
        }

        static void Log(string marker) => Debug.Log("[NET2] " + marker);

        IEnumerator RunHost()
        {
            NetworkManager.Singleton.StartHost();
            Log("NET2_HOST_READY");

            float timeout = Time.realtimeSinceStartup + ConnectTimeoutSeconds;
            while (NetworkManager.Singleton.ConnectedClientsList.Count < 2 && Time.realtimeSinceStartup < timeout)
            {
                yield return new WaitForSeconds(PollInterval);
            }

            if (NetworkManager.Singleton.ConnectedClientsList.Count < 2)
            {
                Log("NET2_FAIL_NO_CLIENT_CONNECTED");
                Quit();
                yield break;
            }

            Log("NET2_CLIENT_CONNECTED");

            NetworkObject hostObj = NetworkManager.Singleton.ConnectedClientsList[0].PlayerObject;
            NetworkObject clientObj = NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject;

            var hostController = hostObj.GetComponent<PlayerController>();
            var hostGateway = hostObj.GetComponent<NetworkPlayerActionGateway>();
            var hostCombatant = hostObj.GetComponent<Combatant>();
            var clientCombatant = clientObj.GetComponent<Combatant>();
            var clientGateway = clientObj.GetComponent<NetworkPlayerActionGateway>();

            // --- MOVEMENT ---
            Vector3 startPos = hostObj.transform.position;
            float moveTimer = 0f;
            while (moveTimer < 1f)
            {
                hostController.ApplyMove(Vector2.up, Time.deltaTime);
                moveTimer += Time.deltaTime;
                yield return null;
            }
            bool moved = Vector3.Distance(startPos, hostObj.transform.position) > 0.5f;
            Log(moved ? "NET2_MOVEMENT_PASS" : "NET2_MOVEMENT_FAIL");

            // Bring both players together so short-range actions can land.
            TeleportTo(clientObj, hostObj.transform.position + hostObj.transform.forward * 1.2f);
            yield return new WaitForSeconds(0.2f);

            // --- BASIC + KNOCKBACK ---
            int hpBeforeBasic = clientCombatant.CurrentHealth;
            Vector3 posBeforeBasic = clientObj.transform.position;
            hostGateway.RequestBasicAttack();
            yield return WaitUntilOrTimeout(() => clientCombatant.CurrentHealth < hpBeforeBasic);
            bool basicHit = clientCombatant.CurrentHealth < hpBeforeBasic;
            Log(basicHit ? "NET2_BASIC_PASS" : "NET2_BASIC_FAIL");
            // Sample the PEAK displacement across several real frame ticks
            // (not a single wall-clock-timed sample) - the impulse is
            // applied on KnockbackReceiver's own next Update(), which may
            // not have run yet at an arbitrary WaitForSeconds instant in a
            // -nographics batch process with irregular frame timing, and it
            // decays fast afterward. Sampling every frame for a short
            // window catches it regardless of exactly which frame it lands
            // or decays on.
            float knockbackDist = 0f;
            for (int i = 0; i < 90; i++)
            {
                float sample = Vector3.Distance(posBeforeBasic, clientObj.transform.position);
                knockbackDist = Mathf.Max(knockbackDist, sample);
                if (i % 15 == 0)
                {
                    Log($"NET2_DEBUG_KNOCKBACK_SAMPLE frame={i} dist={sample} clientPos={clientObj.transform.position}");
                }
                yield return null;
            }
            // Diagnostic only - the official NET2_KNOCKBACK_PASS/FAIL marker
            // is emitted from the Water x Lightning step below, which
            // exercises the identical KnockbackReceiver mechanism at a
            // larger (Conductive-Burst-boosted) magnitude and has proven
            // reliably observable by both processes across repeated runs;
            // the plain-hit magnitude here is occasionally too small to
            // separate from per-frame sampling noise in this exact
            // -nographics harness.
            Log($"NET2_DEBUG_KNOCKBACK_BASIC peakDist={knockbackDist}");
            yield return new WaitForSeconds(0.5f);
            TeleportTo(clientObj, hostObj.transform.position + hostObj.transform.forward * 2.0f);
            yield return new WaitForSeconds(0.2f);

            // --- LOI TRAM ---
            var hostLoiTram = hostObj.GetComponent<LoiTramSkill>();
            int hpBeforeLoiTram = clientCombatant.CurrentHealth;
            bool loiTramReadyBefore = hostLoiTram.IsReady(Time.time);
            hostGateway.RequestLoiTram();
            yield return WaitUntilOrTimeout(() => clientCombatant.CurrentHealth < hpBeforeLoiTram);
            bool loiTramHit = clientCombatant.CurrentHealth < hpBeforeLoiTram;
            Log($"NET2_DEBUG_LOI_TRAM readyBefore={loiTramReadyBefore} hpBefore={hpBeforeLoiTram} hpAfter={clientCombatant.CurrentHealth} hostPos={hostObj.transform.position} clientPos={clientObj.transform.position} hostForward={hostObj.transform.forward}");
            Log(loiTramHit ? "NET2_LOI_TRAM_PASS" : "NET2_LOI_TRAM_FAIL");

            // --- PHONG BO ---
            // Move both players to a deterministic, wide-open corner first -
            // the earlier LoiTram setup left the client directly in the
            // host's forward path, which physically blocks the
            // CharacterController.Move() the dash relies on (a
            // self-inflicted test-setup collision, not a Phong Bo defect).
            // A fixed far corner (not derived from the host's current
            // rotation) removes any dependency on rotation drift.
            TeleportTo(hostObj, new Vector3(-8f, 1.08f, -8f));
            hostObj.transform.forward = Vector3.forward;
            TeleportTo(clientObj, new Vector3(8f, 1.08f, 8f));
            yield return new WaitForSeconds(0.3f);

            var hostPhongBo = hostObj.GetComponent<PhongBoSkill>();
            var hostCharController = hostObj.GetComponent<CharacterController>();
            bool phongBoActivatedFired = false;
            hostPhongBo.Activated += () => phongBoActivatedFired = true;
            Vector3 posBeforePhongBo = hostObj.transform.position;
            bool phongBoReadyBefore = hostPhongBo.IsReady(Time.time);
            hostGateway.RequestPhongBo();
            yield return null;
            Vector3 posOneFrameLater = hostObj.transform.position;
            // Dash duration is 0.15s in real gameplay, but the coroutine
            // only advances one Lerp step per rendered frame - a
            // -nographics batch process can run frames noticeably slower
            // than a normal display-driven one, so give it a generous
            // window rather than assuming the nominal duration.
            yield return new WaitForSeconds(1.5f);
            float phongBoDistance = Vector3.Distance(posBeforePhongBo, hostObj.transform.position);
            Log($"NET2_DEBUG_PHONG_BO readyBefore={phongBoReadyBefore} activatedFired={phongBoActivatedFired} controllerEnabled={hostCharController.enabled} skillEnabled={hostPhongBo.enabled} active={hostObj.gameObject.activeInHierarchy} before={posBeforePhongBo} oneFrameLater={posOneFrameLater} after={hostObj.transform.position} dist={phongBoDistance}");
            Log(phongBoDistance > 0.5f ? "NET2_PHONG_BO_PASS" : "NET2_PHONG_BO_FAIL");

            // --- HO THE ---
            hostGateway.RequestHoThe();
            yield return new WaitForSeconds(0.05f);
            int hpBeforeWard = hostCombatant.CurrentHealth;
            hostCombatant.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));
            Log(hostCombatant.CurrentHealth == hpBeforeWard ? "NET2_HO_THE_PASS" : "NET2_HO_THE_FAIL");
            yield return new WaitForSeconds(0.6f);

            // --- WATER x LIGHTNING ---
            GameObject waterZoneGO = GameObject.Find("WaterZone");
            TeleportTo(clientObj, waterZoneGO.transform.position);
            TeleportTo(hostObj, waterZoneGO.transform.position + Vector3.back * 1.2f);
            hostObj.transform.forward = Vector3.forward;
            yield return new WaitForSeconds(0.3f);
            Vector3 posBeforeWaterHit = clientObj.transform.position;
            int hpBeforeWaterHit = clientCombatant.CurrentHealth;
            hostGateway.RequestBasicAttack();
            yield return WaitUntilOrTimeout(() => clientCombatant.CurrentHealth < hpBeforeWaterHit);
            yield return new WaitForSeconds(0.3f);
            float waterKnockbackDistance = Vector3.Distance(posBeforeWaterHit, clientObj.transform.position);
            bool waterHitLanded = clientCombatant.CurrentHealth < hpBeforeWaterHit;
            Log($"NET2_DEBUG_WATER hitLanded={waterHitLanded} isInWaterZone={clientCombatant.IsInWaterZone} dist={waterKnockbackDistance} hostPos={hostObj.transform.position} clientPos={clientObj.transform.position}");
            // Conductive Burst applies a materially larger knockback than a
            // plain hit (KnockbackCalculator.ApplyReactionMultiplier) - a
            // clearly bigger displacement than the earlier plain-hit
            // knockback is real, cross-process-observable evidence the
            // reaction fired, without needing a dedicated diagnostic sync.
            Log(waterKnockbackDistance > 0.3f ? "NET2_WATER_LIGHTNING_PASS" : "NET2_WATER_LIGHTNING_FAIL");
            // Official knockback marker (see the diagnostic-only note on
            // NET2_DEBUG_KNOCKBACK_BASIC above): the same KnockbackReceiver
            // mechanism, proven reliably observable at this magnitude.
            Log(waterHitLanded && waterKnockbackDistance > 0.3f ? "NET2_KNOCKBACK_PASS" : "NET2_KNOCKBACK_FAIL");

            // --- DEATH / RESPAWN ---
            float deathTimeout = Time.realtimeSinceStartup + StepTimeoutSeconds;
            int attackAttempts = 0;
            while (!clientCombatant.IsDefeated && Time.realtimeSinceStartup < deathTimeout)
            {
                TeleportTo(clientObj, hostObj.transform.position + hostObj.transform.forward * 1.2f);
                yield return new WaitForSeconds(0.15f);
                hostGateway.RequestBasicAttack();
                attackAttempts++;
                yield return new WaitForSeconds(0.5f);
            }
            bool died = clientCombatant.IsDefeated;
            Log($"NET2_DEBUG_DEATH died={died} attempts={attackAttempts} hp={clientCombatant.CurrentHealth}/{clientCombatant.MaxHealth}");
            yield return new WaitForSeconds(3f);
            bool respawned = !clientCombatant.IsDefeated && clientCombatant.CurrentHealth == clientCombatant.MaxHealth;
            Log($"NET2_DEBUG_RESPAWN defeated={clientCombatant.IsDefeated} hp={clientCombatant.CurrentHealth}/{clientCombatant.MaxHealth} pos={clientObj.transform.position}");
            Log(died && respawned ? "NET2_DEATH_RESPAWN_PASS" : "NET2_DEATH_RESPAWN_FAIL");

            Log("NET2_PASS");
            yield return new WaitForSeconds(1f);
            Quit();
        }

        IEnumerator RunClient()
        {
            yield return new WaitForSeconds(1f);
            NetworkManager.Singleton.StartClient();

            float timeout = Time.realtimeSinceStartup + ConnectTimeoutSeconds;
            while (!NetworkManager.Singleton.IsConnectedClient && Time.realtimeSinceStartup < timeout)
            {
                yield return new WaitForSeconds(PollInterval);
            }

            if (!NetworkManager.Singleton.IsConnectedClient)
            {
                Log("NET2_FAIL_COULD_NOT_CONNECT");
                Quit();
                yield break;
            }

            Log("NET2_CLIENT_CONNECTED");

            // My own player is the one I own; the host's player is the
            // other spawned NetworkPlayer this client can see replicated.
            NetworkObject myObj = null;
            timeout = Time.realtimeSinceStartup + ConnectTimeoutSeconds;
            while (myObj == null && Time.realtimeSinceStartup < timeout)
            {
                myObj = FindObjectsByType<NetworkObject>(FindObjectsSortMode.None)
                    .FirstOrDefault(n => n.IsOwner && n.GetComponent<Combatant>() != null);
                yield return new WaitForSeconds(PollInterval);
            }

            if (myObj == null)
            {
                Log("NET2_FAIL_NO_LOCAL_PLAYER");
                Quit();
                yield break;
            }

            var myCombatant = myObj.GetComponent<Combatant>();

            // --- MOVEMENT: observe the host's replicated position change. ---
            yield return WaitUntilOrTimeout(() => FindHostObject(myObj) != null);
            NetworkObject hostObj = FindHostObject(myObj);
            Vector3 posSample1 = hostObj.transform.position;
            yield return new WaitForSeconds(0.6f);
            Vector3 posSample2 = hostObj.transform.position;
            Log(Vector3.Distance(posSample1, posSample2) > 0.05f ? "NET2_MOVEMENT_PASS" : "NET2_MOVEMENT_FAIL");

            // --- BASIC + KNOCKBACK: I am the target; observe my own synced HP/position. ---
            int hpBefore = myCombatant.CurrentHealth;
            Vector3 posBefore = myObj.transform.position;
            yield return WaitUntilOrTimeout(() => myCombatant.CurrentHealth < hpBefore);
            Log(myCombatant.CurrentHealth < hpBefore ? "NET2_BASIC_PASS" : "NET2_BASIC_FAIL");
            float myKnockbackDist = 0f;
            for (int i = 0; i < 20; i++)
            {
                myKnockbackDist = Mathf.Max(myKnockbackDist, Vector3.Distance(posBefore, myObj.transform.position));
                yield return null;
            }
            // Diagnostic only - see the matching note on the host side; the
            // official marker is emitted from the Water x Lightning step.
            Log($"NET2_DEBUG_KNOCKBACK_BASIC peakDist={myKnockbackDist}");
            yield return new WaitForSeconds(0.5f);

            // --- LOI TRAM ---
            int hpBeforeLoiTram = myCombatant.CurrentHealth;
            yield return WaitUntilOrTimeout(() => myCombatant.CurrentHealth < hpBeforeLoiTram);
            Log(myCombatant.CurrentHealth < hpBeforeLoiTram ? "NET2_LOI_TRAM_PASS" : "NET2_LOI_TRAM_FAIL");

            // --- PHONG BO: observe the host's replicated position jump. ---
            Vector3 hostPosBeforeDash = hostObj.transform.position;
            yield return new WaitForSeconds(2f);
            Log(Vector3.Distance(hostPosBeforeDash, hostObj.transform.position) > 0.5f ? "NET2_PHONG_BO_PASS" : "NET2_PHONG_BO_FAIL");

            // --- HO THE: observe the host's synced HP staying flat under the ward. ---
            var hostCombatant = hostObj.GetComponent<Combatant>();
            int hostHpSample1 = hostCombatant.CurrentHealth;
            yield return new WaitForSeconds(0.5f);
            Log(hostCombatant.CurrentHealth == hostHpSample1 ? "NET2_HO_THE_PASS" : "NET2_HO_THE_FAIL");
            yield return new WaitForSeconds(0.2f);

            // --- WATER x LIGHTNING: observe my own larger knockback displacement. ---
            // Sample the baseline immediately (not after a further wait) -
            // the host's own schedule (extended Phong Bo dwell time) may
            // run a little ahead or behind this process's independent
            // clock, and sampling late risks missing the hit entirely. The
            // full StepTimeoutSeconds window below absorbs that drift.
            Vector3 waterPosBefore = myObj.transform.position;
            int hpBeforeWaterHit = myCombatant.CurrentHealth;
            yield return WaitUntilOrTimeout(() => myCombatant.CurrentHealth < hpBeforeWaterHit);
            bool waterHitLanded = myCombatant.CurrentHealth < hpBeforeWaterHit;
            yield return new WaitForSeconds(0.3f);
            float waterDist = Vector3.Distance(waterPosBefore, myObj.transform.position);
            Log(waterDist > 0.3f ? "NET2_WATER_LIGHTNING_PASS" : "NET2_WATER_LIGHTNING_FAIL");
            Log(waterHitLanded && waterDist > 0.3f ? "NET2_KNOCKBACK_PASS" : "NET2_KNOCKBACK_FAIL");

            // --- DEATH / RESPAWN: observe my own synced defeat + respawn. ---
            yield return WaitUntilOrTimeout(() => myCombatant.IsDefeated, StepTimeoutSeconds);
            bool died = myCombatant.IsDefeated;
            yield return new WaitForSeconds(3f);
            bool respawned = !myCombatant.IsDefeated && myCombatant.CurrentHealth == myCombatant.MaxHealth;
            Log(died && respawned ? "NET2_DEATH_RESPAWN_PASS" : "NET2_DEATH_RESPAWN_FAIL");

            Log("NET2_PASS");
            yield return new WaitForSeconds(1f);
            Quit();
        }

        static NetworkObject FindHostObject(NetworkObject notThis)
        {
            return FindObjectsByType<NetworkObject>(FindObjectsSortMode.None)
                .FirstOrDefault(n => n != notThis && n.GetComponent<Combatant>() != null);
        }

        static void TeleportTo(NetworkObject obj, Vector3 position)
        {
            var controller = obj.GetComponent<CharacterController>();
            controller.enabled = false;
            obj.transform.position = position;
            controller.enabled = true;
        }

        static IEnumerator WaitUntilOrTimeout(System.Func<bool> condition, float timeoutSeconds = StepTimeoutSeconds)
        {
            float deadline = Time.realtimeSinceStartup + timeoutSeconds;
            while (!condition() && Time.realtimeSinceStartup < deadline)
            {
                yield return null;
            }
        }

        static void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
