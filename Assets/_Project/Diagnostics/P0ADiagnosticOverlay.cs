#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Text;
using TieuTienKy.Gameplay;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace TieuTienKy.Diagnostics
{
    /// <summary>
    /// Development-only IMGUI overlay for the P0A greybox arena. Surfaces
    /// player/arena/input/perf state, a top-down mini-map, and a RESET TEST
    /// button so the Human/Game Director can produce self-explanatory
    /// evidence of the known physical-device containment failure without any
    /// change to gameplay behavior. Compiled only in the Editor or
    /// DEVELOPMENT_BUILD; stripped entirely from release builds.
    /// </summary>
    public sealed class P0ADiagnosticOverlay : MonoBehaviour
    {
        const float FpsSmoothing = 0.9f;
        const float PanelWidth = 320f;
        const float PanelHeight = 760f;
        const float MiniMapWidth = 200f;
        const float MiniMapHeight = 120f;

        GameObject ground;
        GameObject player;
        GameObject dummy;
        Collider groundCollider;
        CharacterController playerController;
        TouchInputReader inputReader;
        Combatant dummyCombatant;

        int outOfBoundsEventCount;
        BoundarySide lastSide = BoundarySide.None;
        float smoothedFps;
        bool onGuiFirstFrameLogged;
        readonly StringBuilder textBuilder = new StringBuilder(512);

        void Awake()
        {
            Debug.Log("[P0A_DIAG] AWAKE");
        }

        public void Initialize(GameObject groundGO, GameObject playerGO, GameObject dummyGO)
        {
            ground = groundGO;
            player = playerGO;
            dummy = dummyGO;
            groundCollider = ground != null ? ground.GetComponent<Collider>() : null;
            playerController = player != null ? player.GetComponent<CharacterController>() : null;
            inputReader = player != null ? player.GetComponent<TouchInputReader>() : null;
            dummyCombatant = dummy != null ? dummy.GetComponent<Combatant>() : null;
        }

        void Update()
        {
            float deltaTime = Time.unscaledDeltaTime;
            float instantFps = deltaTime > 0f ? 1f / deltaTime : 0f;
            smoothedFps = smoothedFps <= 0f ? instantFps : Mathf.Lerp(instantFps, smoothedFps, FpsSmoothing);

            if (groundCollider == null || playerController == null)
            {
                return;
            }

            BoundarySide side = BoundaryClassifier.Classify(groundCollider.bounds, playerController.bounds);
            bool wasInBounds = lastSide == BoundarySide.None;
            bool isInBounds = side == BoundarySide.None;

            if (wasInBounds && !isInBounds)
            {
                outOfBoundsEventCount++;
                Vector3 pos = player.transform.position;
                Debug.Log($"[P0A_DIAG] OUT_OF_BOUNDS side={side} x={pos.x:F2} z={pos.z:F2}");
            }

            lastSide = side;
        }

        void OnGUI()
        {
            if (!onGuiFirstFrameLogged)
            {
                Debug.Log("[P0A_DIAG] ONGUI_FIRST_FRAME");
                onGuiFirstFrameLogged = true;
            }

            if (groundCollider == null || playerController == null)
            {
                return;
            }

            Bounds arenaBounds = groundCollider.bounds;
            bool isInBounds = lastSide == BoundarySide.None;
            int activeTouches = Touch.activeTouches.Count;
            Vector2 moveInput = inputReader != null ? inputReader.MoveInput : Vector2.zero;
            bool attackThisFrame = inputReader != null && inputReader.AttackTriggeredThisFrame;
            float frameMs = smoothedFps > 0f ? 1000f / smoothedFps : 0f;

            textBuilder.Clear();
            textBuilder.AppendLine("P0A DIAGNOSTIC");
            textBuilder.AppendLine();
            textBuilder.AppendLine("PLAYER");
            textBuilder.AppendLine($"X: {player.transform.position.x:F2}");
            textBuilder.AppendLine($"Z: {player.transform.position.z:F2}");
            textBuilder.AppendLine($"Grounded: {(playerController.isGrounded ? "YES" : "NO")}");
            textBuilder.AppendLine();
            textBuilder.AppendLine("ARENA");
            textBuilder.AppendLine($"minX: {arenaBounds.min.x:F2}");
            textBuilder.AppendLine($"maxX: {arenaBounds.max.x:F2}");
            textBuilder.AppendLine($"minZ: {arenaBounds.min.z:F2}");
            textBuilder.AppendLine($"maxZ: {arenaBounds.max.z:F2}");
            textBuilder.AppendLine($"State: {(isInBounds ? "IN_BOUNDS" : "OUT_OF_BOUNDS")}");
            textBuilder.AppendLine($"Side: {lastSide.ToString().ToUpperInvariant()}");
            textBuilder.AppendLine();
            textBuilder.AppendLine("INPUT");
            textBuilder.AppendLine($"Active touches: {activeTouches}");
            textBuilder.AppendLine($"Move X: {moveInput.x:F2}");
            textBuilder.AppendLine($"Move Y: {moveInput.y:F2}");
            textBuilder.AppendLine($"Attack this frame: {(attackThisFrame ? "YES" : "NO")}");
            textBuilder.AppendLine();
            textBuilder.AppendLine("PERFORMANCE");
            textBuilder.AppendLine($"FPS: {smoothedFps:F0}");
            textBuilder.AppendLine($"Frame ms: {frameMs:F1}");
            textBuilder.AppendLine();
            textBuilder.AppendLine("RUNTIME");
            textBuilder.AppendLine($"Out-of-bounds events: {outOfBoundsEventCount}");
            textBuilder.AppendLine();
            textBuilder.AppendLine("WATER DIAG V2");
            if (dummyCombatant != null)
            {
                textBuilder.AppendLine($"DummyInWater: {(dummyCombatant.IsInWaterZone ? "YES" : "NO")}");
                textBuilder.AppendLine($"LastElement: {(dummyCombatant.LastHitElement.HasValue ? dummyCombatant.LastHitElement.Value.ToString() : "NONE")}");
                textBuilder.AppendLine($"ReactionTriggered: {(dummyCombatant.LastReactionTriggered ? "YES" : "NO")}");
                textBuilder.AppendLine($"BurstSpawnCount: {dummyCombatant.BurstSpawnCount}");
            }

            GUI.Box(new Rect(10, 10, PanelWidth, PanelHeight), string.Empty);
            GUI.Label(new Rect(20, 15, PanelWidth - 20, PanelHeight - 10), textBuilder.ToString());

            var miniMapArea = new Rect(10, PanelHeight + 20, MiniMapWidth, MiniMapHeight);
            DrawMiniMap(arenaBounds, miniMapArea);

            var resetButtonArea = new Rect(10, miniMapArea.yMax + 10, 120, 30);
            if (GUI.Button(resetButtonArea, "RESET TEST"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        void DrawMiniMap(Bounds arenaBounds, Rect area)
        {
            GUI.Box(area, string.Empty);

            DrawMiniMapPoint(area, arenaBounds, player.transform.position, Color.yellow, "P");

            if (dummy != null)
            {
                DrawMiniMapPoint(area, arenaBounds, dummy.transform.position, Color.red, "D");
            }
        }

        static void DrawMiniMapPoint(Rect area, Bounds arenaBounds, Vector3 worldPosition, Color color, string label)
        {
            // Deliberately unclamped: a Player past the arena edge must be
            // visibly plottable outside the mini-map rectangle.
            float normalizedX = (worldPosition.x - arenaBounds.min.x) / arenaBounds.size.x;
            float normalizedZ = (worldPosition.z - arenaBounds.min.z) / arenaBounds.size.z;

            float pointX = area.x + normalizedX * area.width;
            float pointY = area.y + (1f - normalizedZ) * area.height;

            Color previousColor = GUI.color;
            GUI.color = color;
            GUI.Label(new Rect(pointX - 6, pointY - 8, 20, 16), label);
            GUI.color = previousColor;
        }
    }
}
#endif
