using TieuTienKy.Input;
using UnityEngine;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
using TieuTienKy.Diagnostics;
#endif

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Builds the entire P0A greybox arena procedurally at runtime from Unity
    /// primitives (player, enemies, water zone, hazard obstacle). This
    /// keeps the .unity scene file itself trivial (camera, light, and this
    /// single bootstrap object) instead of hand-authoring a large serialized
    /// scene graph outside the Editor.
    /// </summary>
    public sealed class GreyboxSceneBootstrapper : MonoBehaviour
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");

        const float ArenaWallHeight = 4f;
        const float ArenaWallThickness = 1f;
        const float ArenaWallBelowGroundMargin = 1f;

        static readonly Color GroundColor = new Color(0.55f, 0.55f, 0.55f);
        static readonly Color PlayerColor = new Color(0.9f, 0.75f, 0.2f);
        static readonly Color PlayerAccentColor = new Color(0.65f, 0.9f, 1f);
        static readonly Color WaterZoneColor = new Color(0.2f, 0.5f, 0.9f, 0.6f);
        static readonly Color HazardColor = new Color(0.3f, 0.3f, 0.3f);

        const int PlayerMaxHealth = 5;

        static Material s_PrimitiveMaterial;

        static readonly Vector3[] WaterZonePositions =
        {
            new Vector3(3f, 0.5f, 0f),
            new Vector3(-3f, 0.5f, 3f),
            new Vector3(3f, 0.5f, -4f)
        };

        void Awake()
        {
            GameObject ground = BuildGround();
            GameObject player = BuildPlayer(new Vector3(0f, 1f, 0f));

            WaterZone waterZone = BuildWaterZone(WaterZonePositions[0], new Vector3(3f, 1f, 3f));
            BuildHazardObstacle(new Vector3(5.5f, 1f, 0f));
            BuildArenaBoundaries(ground);

            AttachPlayerFollowCamera(player);
            AttachRunDirector(player, waterZone);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            AttachDiagnosticOverlay(ground, player);
#endif
        }

        /// <summary>
        /// Wires the reusable run/wave/blessing/arena-event owner and its
        /// two temporary presentation shells (RunHud, BlessingChoiceHud).
        /// ArenaRunDirector spawns and owns all enemies and events for the
        /// run from here on; this bootstrap builds only the static arena +
        /// player once.
        /// </summary>
        static void AttachRunDirector(GameObject player, WaterZone waterZone)
        {
            var runHud = new GameObject("P0A_RunHud").AddComponent<RunHud>();
            var blessingHud = new GameObject("P0A_BlessingChoiceHud").AddComponent<BlessingChoiceHud>();

            var arenaEvents = new GameObject("P0A_ArenaEventDirector").AddComponent<ArenaEventDirector>();
            arenaEvents.Initialize(waterZone, waterZone.transform, WaterZonePositions);

            var director = new GameObject("P0A_ArenaRunDirector").AddComponent<ArenaRunDirector>();
            director.Initialize(
                player.transform,
                player.GetComponent<Combatant>(),
                player.GetComponent<PlayerController>(),
                player.GetComponent<BasicAttack>(),
                PlayerMaxHealth,
                blessingHud,
                runHud,
                arenaEvents);

            runHud.Initialize(director, player.GetComponent<Combatant>());
        }

        /// <summary>
        /// Physical-playtest visibility fix: the Main Camera is a fixed scene
        /// object, so a Player moving across the arena walks out of its
        /// viewport. Attached here - using the Player reference this
        /// bootstrap just built, not GameObject.Find - so the camera tracks
        /// the exact instance without any change to Player/Camera behavior
        /// otherwise.
        /// </summary>
        static void AttachPlayerFollowCamera(GameObject player)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            var follow = mainCamera.gameObject.AddComponent<PlayerFollowCamera>();
            follow.Initialize(player.transform);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Dev/Editor-only P0A diagnostic overlay (player/arena/input/perf
        /// readout, mini-map, RESET TEST). Attached here - rather than via
        /// Find-based resolution inside the overlay itself - so it receives
        /// the exact Ground/Player instances this bootstrap just built, with
        /// no change to their behavior. Enemies are now dynamically
        /// spawned/despawned per wave by ArenaRunDirector, so there is no
        /// single persistent instance left to track for the Water diagnostic
        /// section; it stays null-safe and simply shows nothing there.
        /// </summary>
        static void AttachDiagnosticOverlay(GameObject ground, GameObject player)
        {
            Debug.Log("[P0A_DIAG] ATTACH");
            var overlay = new GameObject("P0A_DiagnosticOverlay").AddComponent<P0ADiagnosticOverlay>();
            overlay.Initialize(ground, player, null);
        }
#endif

        static GameObject BuildGround()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2f, 1f, 2f);
            Tint(ground, GroundColor);
            return ground;
        }

        /// <summary>
        /// Empty gameplay root (no root Renderer/collider primitive) with a
        /// replaceable PrimitiveCharacterView child. Gameplay components
        /// live only on this root; a future imported character model can
        /// replace CharacterView without touching combat/run logic.
        /// </summary>
        static GameObject BuildPlayer(Vector3 position)
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
            player.AddComponent<PlayerController>();
            var basicAttack = player.AddComponent<BasicAttack>();

            var view = player.AddComponent<PrimitiveCharacterView>();
            view.Build(PlayerColor, PlayerAccentColor, armed: true, visualScale: 1f);

            var swordView = player.AddComponent<SwordAttackView>();
            swordView.Initialize(basicAttack, view.WeaponSocket);

            return player;
        }

        static WaterZone BuildWaterZone(Vector3 position, Vector3 size)
        {
            GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            zone.name = "WaterZone";
            zone.transform.position = position;
            zone.transform.localScale = size;
            Tint(zone, WaterZoneColor);

            var boxCollider = zone.GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            return zone.AddComponent<WaterZone>();
        }

        static void BuildHazardObstacle(Vector3 position)
        {
            GameObject hazard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            hazard.name = "HazardObstacle";
            hazard.transform.position = position;
            hazard.transform.localScale = new Vector3(1f, 2f, 3f);
            Tint(hazard, HazardColor);

            hazard.AddComponent<HazardObstacle>();
        }

        /// <summary>
        /// Invisible, collider-only perimeter walls sized off the Ground's
        /// actual bounds (not a hardcoded plane-size assumption) so a player
        /// or dummy driven purely by CharacterController.Move can no longer
        /// walk/get-knocked-back past the finite Ground and fall out of the
        /// world. Wall inner faces sit flush with the Ground edge, and the
        /// North/South walls are extended by one thickness on each side so
        /// the four walls fully overlap at the corners with no gap.
        /// </summary>
        static void BuildArenaBoundaries(GameObject ground)
        {
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;
            float halfThickness = ArenaWallThickness * 0.5f;
            float wallBottomY = ground.transform.position.y - ArenaWallBelowGroundMargin;
            float centerY = wallBottomY + ArenaWallHeight * 0.5f;

            BuildArenaWall(
                "ArenaWall_North",
                new Vector3(groundBounds.center.x, centerY, groundBounds.max.z + halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildArenaWall(
                "ArenaWall_South",
                new Vector3(groundBounds.center.x, centerY, groundBounds.min.z - halfThickness),
                new Vector3(groundBounds.size.x + ArenaWallThickness * 2f, ArenaWallHeight, ArenaWallThickness));

            BuildArenaWall(
                "ArenaWall_East",
                new Vector3(groundBounds.max.x + halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));

            BuildArenaWall(
                "ArenaWall_West",
                new Vector3(groundBounds.min.x - halfThickness, centerY, groundBounds.center.z),
                new Vector3(ArenaWallThickness, ArenaWallHeight, groundBounds.size.z));
        }

        static void BuildArenaWall(string name, Vector3 center, Vector3 size)
        {
            GameObject wall = new GameObject(name);
            wall.transform.position = center;

            var collider = wall.AddComponent<BoxCollider>();
            collider.size = size;
        }

        static void Tint(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = PrimitiveMaterial();

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(TintColorPropertyId, color);
                renderer.SetPropertyBlock(block);
            }
        }

        static Material PrimitiveMaterial()
        {
            if (s_PrimitiveMaterial == null)
            {
                s_PrimitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_PrimitiveMaterial;
        }
    }
}
