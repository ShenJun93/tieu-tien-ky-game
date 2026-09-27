using System;
using System.Collections.Generic;
using TieuTienKy.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Runs one offline match: steps <see cref="CombatSim"/> at its fixed tick, feeds it the local
    /// player's command from the Input System (the HUD's OnScreenStick drives &lt;Gamepad&gt;/leftStick;
    /// WASD in the Editor), and hands interpolated state to the fighter views. Bots come in CORE-1c.
    /// </summary>
    public sealed class BrawlMatch : MonoBehaviour
    {
        [Serializable]
        public struct Entry
        {
            public FighterView View;
            public Team Team;
        }

        [SerializeField] Entry[] fighters = Array.Empty<Entry>();
        [SerializeField] int localPlayer;
        [SerializeField] Rect arenaBounds = new Rect(-22f, -14f, 44f, 28f);
        [SerializeField] CombatConfig config = new CombatConfig();

        CombatSim sim;
        InputAction move;
        readonly List<FighterCommand> commands = new List<FighterCommand>();
        Vector2[] previous = Array.Empty<Vector2>();
        float accumulator;

        public CombatSim Sim => sim;

        /// <summary>
        /// When set, replaces the local player's stick input. Tests use it today; bots and the network
        /// layer feed commands through the same seam later.
        /// </summary>
        public Func<FighterCommand> LocalCommandOverride { get; set; }

        /// <summary>Editor/scene-building entry: sets the roster, the local player and the arena rectangle.</summary>
        public void Configure(Entry[] roster, int local, Rect bounds)
        {
            fighters = roster;
            localPlayer = local;
            arenaBounds = bounds;
        }
        public FighterView LocalView => fighters.Length > localPlayer ? fighters[localPlayer].View : null;

        void Awake()
        {
            sim = new CombatSim(config, arenaBounds);
            foreach (var e in fighters)
            {
                var p = e.View.transform.position;
                sim.AddFighter(e.Team, new Vector2(p.x, p.z));
            }
            previous = new Vector2[fighters.Length];
            for (int i = 0; i < fighters.Length; i++)
            {
                previous[i] = sim.Fighters[i].Position;
                commands.Add(FighterCommand.Idle);
            }

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
                for (int i = 0; i < fighters.Length; i++) previous[i] = sim.Fighters[i].Position;
                commands[localPlayer] = LocalCommandOverride != null
                    ? LocalCommandOverride()
                    : new FighterCommand { Move = move.ReadValue<Vector2>() };
                sim.Step(commands);
                accumulator -= tick;
            }

            float alpha = accumulator / tick;
            for (int i = 0; i < fighters.Length; i++)
            {
                var s = sim.Fighters[i];
                fighters[i].View.Present(Vector2.Lerp(previous[i], s.Position, alpha), s.Facing, s.Velocity.magnitude / config.MoveSpeed);
            }
        }
    }
}
