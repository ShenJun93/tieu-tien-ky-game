using System;
using System.Collections.Generic;
using TieuTienKy.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Runs one offline match. It spawns the roster from <see cref="CharacterDefinition"/>s, steps
    /// <see cref="CombatSim"/> at its fixed tick, feeds it the local player's command from the Input
    /// System (the HUD's OnScreenStick drives &lt;Gamepad&gt;/leftStick; WASD in the Editor), and hands
    /// interpolated state to the fighter views. Bots come in CORE-1c.
    /// </summary>
    public sealed class BrawlMatch : MonoBehaviour
    {
        [Serializable]
        public struct Entry
        {
            public CharacterDefinition Character;
            public Team Team;
            public Vector2 Spawn;
        }

        [SerializeField] Entry[] roster = Array.Empty<Entry>();
        [SerializeField] int localPlayer;
        [SerializeField] Rect arenaBounds = new Rect(-22f, -14f, 44f, 28f);
        [SerializeField] CombatConfig config = new CombatConfig();
        [Tooltip("Static blockers baked from the arena props (gate pillars, incense burner, rubble).")]
        [SerializeField] CircleObstacle[] obstacles = Array.Empty<CircleObstacle>();
        [Tooltip("Follows the local fighter every frame; the gameplay camera tracks it, not the model.")]
        [SerializeField] Transform cameraTarget;
        [Tooltip("Ground marker placed under the local fighter (the hero ring).")]
        [SerializeField] GameObject localMarker;

        CombatSim sim;
        InputAction move;
        FighterView[] views = Array.Empty<FighterView>();
        readonly List<FighterCommand> commands = new List<FighterCommand>();
        Vector2[] previous = Array.Empty<Vector2>();
        float accumulator;

        public CombatSim Sim => sim;
        public IReadOnlyList<FighterView> Views => views;
        public FighterView LocalView => views.Length > localPlayer ? views[localPlayer] : null;
        public Transform CameraTarget => cameraTarget;

        /// <summary>
        /// When set, replaces the local player's stick input. Tests use it today; bots and the network
        /// layer feed commands through the same seam later.
        /// </summary>
        public Func<FighterCommand> LocalCommandOverride { get; set; }

        /// <summary>Scene-building entry: roster, local player, arena rectangle, obstacles and presentation hooks.</summary>
        public void Configure(Entry[] entries, int local, Rect bounds, CircleObstacle[] blockers, Transform target, GameObject marker)
        {
            roster = entries;
            obstacles = blockers;
            localPlayer = local;
            arenaBounds = bounds;
            cameraTarget = target;
            localMarker = marker;
        }

        void Awake()
        {
            sim = new CombatSim(config, arenaBounds);
            sim.Obstacles.AddRange(obstacles);
            var parent = new GameObject("Fighters").transform;
            views = new FighterView[roster.Length];
            previous = new Vector2[roster.Length];
            for (int i = 0; i < roster.Length; i++)
            {
                var e = roster[i];
                var state = sim.AddFighter(e.Team, e.Spawn, e.Character.BodyRadius);
                views[i] = FighterSpawner.Spawn(e.Character, e.Team, state.Position, parent);
                previous[i] = state.Position;
                commands.Add(FighterCommand.Idle);
            }

            var local = LocalView;
            if (local != null && localMarker != null)
            {
                var ring = Instantiate(localMarker, local.transform);
                ring.transform.localPosition = Vector3.zero;
                ring.transform.localScale = Vector3.one * (roster[localPlayer].Character.BodyRadius * 2.9f) / local.transform.lossyScale.x;
            }
            FollowLocal();

            move = new InputAction("Move", InputActionType.Value);
            move.AddBinding("<Gamepad>/leftStick");
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
        }

        void OnEnable() => move?.Enable();
        void OnDisable() => move?.Disable();
        void OnDestroy() => move?.Dispose();

        void Update()
        {
            accumulator += Time.deltaTime;
            float tick = config.TickSeconds;
            // Clamp the backlog after a hitch so the sim never spirals.
            if (accumulator > tick * 5f) accumulator = tick * 5f;
            while (accumulator >= tick)
            {
                for (int i = 0; i < views.Length; i++) previous[i] = sim.Fighters[i].Position;
                commands[localPlayer] = LocalCommandOverride != null
                    ? LocalCommandOverride()
                    : new FighterCommand { Move = move.ReadValue<Vector2>() };
                sim.Step(commands);
                accumulator -= tick;
            }

            float alpha = accumulator / tick;
            for (int i = 0; i < views.Length; i++)
            {
                var s = sim.Fighters[i];
                views[i].Present(Vector2.Lerp(previous[i], s.Position, alpha), s.Facing, s.Velocity.magnitude / config.MoveSpeed);
            }
            FollowLocal();
        }

        void FollowLocal()
        {
            if (cameraTarget != null && LocalView != null)
                cameraTarget.position = LocalView.transform.position;
        }
    }
}
