using System;
using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Combat
{
    /// <summary>
    /// What a fighter wants to do this tick. Players and bots produce the same commands, which is
    /// what lets the match run offline, host bots, or later travel over the network (ADR 007).
    /// </summary>
    public struct FighterCommand
    {
        /// <summary>Desired move direction on the arena plane (x = world X, y = world Z), length 0..1.</summary>
        public Vector2 Move;

        public static FighterCommand Idle => default;
    }

    public enum Team { Jade, Crimson, Azure }

    /// <summary>Simulation state of one fighter. Plain data: views read it, only <see cref="CombatSim"/> writes it.</summary>
    public sealed class FighterState
    {
        public int Id;
        public Team Team;
        public Vector2 Position;
        public Vector2 Velocity;
        /// <summary>Unit facing on the arena plane; keeps the last non-zero move direction.</summary>
        public Vector2 Facing = new Vector2(0f, -1f);
        public float Radius = 0.9f;
    }

    /// <summary>A static round blocker on the arena plane (gate pillar, incense burner, rubble).</summary>
    [Serializable]
    public struct CircleObstacle
    {
        public Vector2 Center;
        public float Radius;

        public CircleObstacle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }
    }

    /// <summary>Tuning in arena units (KayKit fighters are about 2.5 units tall).</summary>
    [Serializable]
    public sealed class CombatConfig
    {
        /// <summary>Fixed simulation rate. 30 Hz is a common authoritative rate for mobile brawlers; views interpolate.</summary>
        public int TickRate = 30;
        public float MoveSpeed = 7f;
        public float Acceleration = 70f;
        public float Deceleration = 90f;
        public float DeadZone = 0.12f;

        public float TickSeconds => 1f / TickRate;
    }

    /// <summary>
    /// The authoritative match simulation. It is pure C#: no MonoBehaviour, no scene, no Time.*,
    /// and a fixed tick. It covers movement, facing, fighter separation, static obstacles and arena bounds.
    /// </summary>
    public sealed class CombatSim
    {
        public readonly CombatConfig Config;
        public readonly Rect Bounds;
        public readonly List<FighterState> Fighters = new List<FighterState>();
        public readonly List<CircleObstacle> Obstacles = new List<CircleObstacle>();
        public int Tick { get; private set; }

        public CombatSim(CombatConfig config, Rect bounds)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Bounds = bounds;
        }

        public FighterState AddFighter(Team team, Vector2 position, float radius = 0.9f)
        {
            var fighter = new FighterState { Id = Fighters.Count, Team = team, Radius = radius, Position = ClampToBounds(position, radius) };
            Fighters.Add(fighter);
            return fighter;
        }

        /// <summary>Advances one fixed tick. <paramref name="commands"/> is indexed by fighter id; missing entries mean idle.</summary>
        public void Step(IReadOnlyList<FighterCommand> commands)
        {
            float dt = Config.TickSeconds;
            for (int i = 0; i < Fighters.Count; i++)
            {
                var f = Fighters[i];
                var cmd = commands != null && i < commands.Count ? commands[i] : FighterCommand.Idle;
                Vector2 move = cmd.Move.magnitude > 1f ? cmd.Move.normalized : cmd.Move;
                if (move.magnitude < Config.DeadZone) move = Vector2.zero;

                Vector2 target = move * Config.MoveSpeed;
                float rate = move == Vector2.zero ? Config.Deceleration : Config.Acceleration;
                f.Velocity = Vector2.MoveTowards(f.Velocity, target, rate * dt);
                f.Position += f.Velocity * dt;
                if (move != Vector2.zero) f.Facing = move.normalized;
            }

            Separate();
            ResolveObstacles();
            foreach (var f in Fighters)
                f.Position = ClampToBounds(f.Position, f.Radius);
            Tick++;
        }

        /// <summary>Pushes overlapping fighters apart symmetrically so bodies never stack.</summary>
        void Separate()
        {
            for (int i = 0; i < Fighters.Count; i++)
            for (int j = i + 1; j < Fighters.Count; j++)
            {
                var a = Fighters[i];
                var b = Fighters[j];
                Vector2 delta = b.Position - a.Position;
                float minDist = a.Radius + b.Radius;
                float dist = delta.magnitude;
                if (dist >= minDist) continue;
                Vector2 dir = dist > 1e-4f ? delta / dist : new Vector2(1f, 0f);
                Vector2 push = dir * ((minDist - dist) * 0.5f);
                a.Position -= push;
                b.Position += push;
            }
        }

        /// <summary>
        /// Pushes fighters out of obstacles and removes the velocity pointing into them, so a fighter
        /// running into a pillar slides around it instead of sticking.
        /// </summary>
        void ResolveObstacles()
        {
            foreach (var f in Fighters)
            foreach (var o in Obstacles)
            {
                Vector2 delta = f.Position - o.Center;
                float minDist = f.Radius + o.Radius;
                float dist = delta.magnitude;
                if (dist >= minDist) continue;
                Vector2 normal = dist > 1e-4f ? delta / dist : new Vector2(0f, -1f);
                f.Position = o.Center + normal * minDist;
                float into = Vector2.Dot(f.Velocity, normal);
                if (into < 0f) f.Velocity -= normal * into;
            }
        }

        Vector2 ClampToBounds(Vector2 p, float radius) => new Vector2(
            Mathf.Clamp(p.x, Bounds.xMin + radius, Bounds.xMax - radius),
            Mathf.Clamp(p.y, Bounds.yMin + radius, Bounds.yMax - radius));
    }
}
