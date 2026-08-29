using TieuTienKy.Input;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Composition root for the authored Arena_VerticalSlice_01 scene.
    /// Unlike GreyboxSceneBootstrapper (kept as the regression sandbox),
    /// static arena geometry - Ground, boundary walls, WaterZone, hazard,
    /// spawn-zone markers - is authored scene content wired via serialized
    /// references here, not built at runtime from primitives. This Awake
    /// only builds the Player (CultivatorProxy presentation + full skill
    /// kit) and the runtime actors ArenaRunDirector owns (HUD, blessing
    /// gate, arena events).
    /// </summary>
    public sealed class ArenaVerticalSliceBootstrapper : MonoBehaviour
    {
        const float ArenaUsableMargin = 0.75f;
        const int PlayerMaxHealth = 5;

        [SerializeField] Transform groundTransform;
        [SerializeField] Transform playerSpawn;
        [SerializeField] WaterZone waterZone;
        [SerializeField] Transform[] waterZonePositionMarkers;
        [SerializeField] GameObject cultivatorProxyPrefab;
        [SerializeField] GameObject pursuerPrefab;
        [SerializeField] GameObject lancerPrefab;
        [SerializeField] GameObject bossPrefab;
        [SerializeField] GameObject combatHudPrefab;

        /// <summary>Editor-time wiring of authored scene content and prefab references. Called once by the scene-authoring tool that builds Arena_VerticalSlice_01.unity, never at runtime.</summary>
        public void ConfigureAuthoring(
            Transform ground,
            Transform spawn,
            WaterZone zone,
            Transform[] zoneMarkers,
            GameObject cultivatorProxy,
            GameObject pursuer,
            GameObject lancer,
            GameObject boss,
            // Optional so the older whole-scene builder (Assets/Editor/
            // VerticalSliceContentBuilder.cs, outside this task's scope)
            // still compiles; ProductProofCombatHudAuthoring.BuildAndWire is
            // what assigns the authored HUD prefab the runtime requires.
            GameObject combatHud = null)
        {
            groundTransform = ground;
            playerSpawn = spawn;
            waterZone = zone;
            waterZonePositionMarkers = zoneMarkers;
            cultivatorProxyPrefab = cultivatorProxy;
            pursuerPrefab = pursuer;
            lancerPrefab = lancer;
            bossPrefab = boss;
            combatHudPrefab = combatHud;
        }

        void Awake()
        {
            GameObject player = BuildPlayer(playerSpawn != null ? playerSpawn.position : Vector3.zero);

            Bounds groundBounds = groundTransform.GetComponent<Collider>().bounds;
            ArenaBounds arenaBounds = ArenaBounds.FromGroundBounds(groundBounds, ArenaUsableMargin);

            PlayerFollowCamera follow = AttachPlayerFollowCamera(player);
            WireCameraImpulse(player, follow);
            AttachRunDirector(player, arenaBounds);
        }

        /// <summary>Bounded one-shot camera dip on meaningful player-side impact (Task A4): a landed Basic/Lôi Trảm hit gives a small dip, the player taking damage gives a larger one. Never wired to a whiff or to enemy-on-enemy events.</summary>
        static void WireCameraImpulse(GameObject player, PlayerFollowCamera follow)
        {
            if (follow == null)
            {
                return;
            }

            var basicAttack = player.GetComponent<BasicAttack>();
            var loiTram = player.GetComponent<LoiTramSkill>();
            var combatant = player.GetComponent<Combatant>();

            if (basicAttack != null)
            {
                basicAttack.HitLanded += () => follow.ApplyImpulse(0.08f);
            }

            if (loiTram != null)
            {
                loiTram.HitLanded += () => follow.ApplyImpulse(0.12f);
            }

            if (combatant != null)
            {
                combatant.Damaged += (_, __) => follow.ApplyImpulse(0.18f);
            }
        }

        GameObject BuildPlayer(Vector3 position)
        {
            var player = new GameObject("Player");
            player.transform.position = position;

            var controller = player.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            player.AddComponent<KnockbackReceiver>();
            player.AddComponent<Combatant>();
            player.AddComponent<TouchInputReader>();
            var playerController = player.AddComponent<PlayerController>();
            var basicAttack = player.AddComponent<BasicAttack>();
            player.AddComponent<LoiTramSkill>();
            player.AddComponent<PhongBoSkill>();
            player.AddComponent<HoTheSkill>();
            var skillController = player.AddComponent<PlayerSkillController>();
            player.AddComponent<PlayerBlessingPresentation>();

            var executor = new PlayerActionExecutor(basicAttack, skillController, player.GetComponent<Combatant>());
            player.AddComponent<LocalPlayerActionGateway>().Initialize(executor);

            CharacterPresentation presentation = null;
            if (cultivatorProxyPrefab != null)
            {
                GameObject presentationGO = Instantiate(cultivatorProxyPrefab, player.transform);
                presentationGO.transform.localPosition = Vector3.zero;
                presentationGO.transform.localRotation = Quaternion.identity;
                presentation = presentationGO.GetComponent<CharacterPresentation>();
            }

            if (presentation != null)
            {
                skillController.SetPresentation(presentation);
                basicAttack.AttackStarted += presentation.PlayBasicAttack;

                PlayerController capturedController = playerController;
                presentation.gameObject.AddComponent<PresentationMovementDriver>().Initialize(capturedController, presentation);

                if (presentation.WeaponSocket != null)
                {
                    player.AddComponent<SwordAttackView>().Initialize(basicAttack, presentation.WeaponSocket);
                }
            }

            return player;
        }

        void AttachRunDirector(GameObject player, ArenaBounds arenaBounds)
        {
            if (combatHudPrefab == null)
            {
                throw new System.InvalidOperationException(
                    "ArenaVerticalSliceBootstrapper requires the authored ProductProofCombatHud prefab. Run ProductProofCombatHudAuthoring.BuildAndWire to author and assign it.");
            }

            // The whole player-facing HUD - combat readout, controls and the
            // Cơ Duyên surface - is one authored prefab with one Canvas. The
            // presenters bind to its serialized references; nothing here
            // builds UI at runtime, and the prototype-era OnboardingHud
            // overlay is no longer part of the production arena.
            GameObject hudRoot = Instantiate(combatHudPrefab);
            var view = hudRoot.GetComponent<ProductionCombatHudView>();
            var hud = hudRoot.GetComponent<ProductionHud>();
            var blessingHud = hudRoot.GetComponent<BlessingChoiceHud>();

            // Basic is a dedicated on-screen button in this composition, so
            // the greybox world-tap trigger must not also fire underneath it.
            player.GetComponent<BasicAttack>().SetLocalWorldTapEnabled(false);

            var arenaEvents = new GameObject("ArenaEventDirector").AddComponent<ArenaEventDirector>();
            Vector3[] waterPositions = BuildWaterPositions();
            if (waterZone != null)
            {
                arenaEvents.Initialize(waterZone, waterZone.transform, waterPositions);
            }

            var director = new GameObject("ArenaRunDirector").AddComponent<ArenaRunDirector>();
            director.SetEnemyPrefabs(pursuerPrefab, lancerPrefab, bossPrefab);

            var presentation = player.GetComponentInChildren<CharacterPresentation>();
            var skillController = player.GetComponent<PlayerSkillController>();

            // Both presenters are bound to the same authored view before the
            // director starts the run, because ArenaRunDirector.Initialize
            // immediately calls StartRun(), which hides the Cơ Duyên surface.
            blessingHud.Initialize(view);
            hud.Initialize(director, player.GetComponent<Combatant>(), skillController, player.GetComponent<TouchInputReader>(), view);
            hud.SetActionGateway(player.GetComponent<LocalPlayerActionGateway>());

            director.Initialize(
                player.transform,
                player.GetComponent<Combatant>(),
                player.GetComponent<PlayerController>(),
                player.GetComponent<BasicAttack>(),
                player.GetComponent<PlayerBlessingPresentation>(),
                PlayerMaxHealth,
                blessingHud,
                hud,
                arenaEvents,
                arenaBounds,
                onboardingHudRef: null,
                skillController,
                presentation);
        }

        Vector3[] BuildWaterPositions()
        {
            if (waterZonePositionMarkers == null || waterZonePositionMarkers.Length == 0)
            {
                return waterZone != null ? new[] { waterZone.transform.position } : System.Array.Empty<Vector3>();
            }

            var positions = new Vector3[waterZonePositionMarkers.Length];
            for (int i = 0; i < waterZonePositionMarkers.Length; i++)
            {
                positions[i] = waterZonePositionMarkers[i].position;
            }

            return positions;
        }

        static PlayerFollowCamera AttachPlayerFollowCamera(GameObject player)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return null;
            }

            var follow = mainCamera.gameObject.AddComponent<PlayerFollowCamera>();
            follow.Initialize(player.transform);
            return follow;
        }
    }
}
