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

        /// <summary>Attack button pressed or held this tick. The sim buffers it briefly and chains the combo.</summary>
        public bool Attack;

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
        /// <summary>Unit facing on the arena plane; keeps the last non-zero move direction and locks during a swing.</summary>
        public Vector2 Facing = new Vector2(0f, -1f);
        public float Radius = 0.9f;
        public Vector2 Spawn;

        public int Hp;
        public int MaxHp;

        /// <summary>0 when not attacking, otherwise the 1-based combo step being swung.</summary>
        public int ComboStep;
        /// <summary>Ticks since the current combo step started.</summary>
        public int StepTick;
        /// <summary>Ticks left on a buffered attack press.</summary>
        public int Buffer;
        /// <summary>Ticks left frozen by hitstop. Nothing about the fighter advances while it is above zero.</summary>
        public int Hitstop;
        /// <summary>Ticks left unable to move or attack after being hit.</summary>
        public int Hitstun;
        /// <summary>Knockback and lunge velocity, separate from steering so it decays at its own rate.</summary>
        public Vector2 Impulse;
        /// <summary>Counts hits received; views watch it to trigger flashes and hit reactions.</summary>
        public int HitsTaken;
        public bool Defeated;
        public int RespawnIn;

        /// <summary>Bit per fighter id already hit by the current combo step.</summary>
        internal ulong StepVictims;

        public bool IsAttacking => ComboStep > 0;
    }

    /// <summary>One hit resolved during a tick. Views use it for sparks, numbers, flashes and shake.</summary>
    public struct HitEvent
    {
        public int Attacker;
        public int Victim;
        public int Damage;
        public int ComboStep;
        public bool Finisher;
        public bool Defeated;
        /// <summary>Contact point on the arena plane, on the victim's edge facing the attacker.</summary>
        public Vector2 Point;
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

    /// <summary>
    /// One swing of the basic combo, in ticks at <see cref="CombatConfig.TickRate"/>. Startup is the
    /// wind-up, Active the ticks that can hit, Recovery the tail. The next step can cancel in after Active.
    /// </summary>
    [Serializable]
    public sealed class AttackStep
    {
        public int Startup = 5;
        public int Active = 2;
        public int Recovery = 8;
        public int Damage = 60;
        public float Range = 1.6f;
        public float ArcDegrees = 130f;
        public float Knockback = 5f;
        public float Lunge = 5.5f;
        public int Hitstop = 3;
        public int Hitstun = 9;

        public int Total => Startup + Active + Recovery;
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

        public int MaxHp = 1000;
        /// <summary>Steering speed multiplier while swinging, so an attack commits you without rooting you.</summary>
        public float AttackMoveFactor = 0.2f;
        /// <summary>How long an early attack press is remembered (about 0.2 s).</summary>
        public int BufferTicks = 6;
        /// <summary>How fast knockback and lunge velocity die out (units per second squared).</summary>
        public float ImpulseFriction = 40f;
        public int RespawnTicks = 90;

        /// <summary>Basic three-hit combo: two quick cuts and a heavier finisher with more hitstop and knockback.</summary>
        public AttackStep[] Combo =
        {
            // Distance travelled by an impulse v is v² / (2 × ImpulseFriction): lunge 5.5 ≈ 0.4 units, finisher knockback 12 ≈ 1.8 units.
            new AttackStep { Startup = 5, Active = 2, Recovery = 8, Damage = 60, Range = 1.6f, ArcDegrees = 130f, Knockback = 5f, Lunge = 5.5f, Hitstop = 3, Hitstun = 9 },
            new AttackStep { Startup = 4, Active = 2, Recovery = 8, Damage = 70, Range = 1.6f, ArcDegrees = 140f, Knockback = 5f, Lunge = 5.5f, Hitstop = 3, Hitstun = 9 },
            new AttackStep { Startup = 7, Active = 3, Recovery = 14, Damage = 120, Range = 1.9f, ArcDegrees = 170f, Knockback = 12f, Lunge = 7f, Hitstop = 6, Hitstun = 16 },
        };

        public float TickSeconds => 1f / TickRate;
    }

    /// <summary>
    /// The authoritative match simulation. It is pure C#: no MonoBehaviour, no scene, no Time.*,
    /// and a fixed tick. It covers movement, the basic combo (hits, hitstop, hitstun, knockback,
    /// defeat and respawn), fighter separation, static obstacles and arena bounds.
    /// </summary>
    public sealed class CombatSim
    {
        public readonly CombatConfig Config;
        public readonly Rect Bounds;
        public readonly List<FighterState> Fighters = new List<FighterState>();
        public readonly List<CircleObstacle> Obstacles = new List<CircleObstacle>();
        /// <summary>Hits resolved during the last <see cref="Step"/>.</summary>
        public readonly List<HitEvent> Hits = new List<HitEvent>();
        public int Tick { get; private set; }

        public CombatSim(CombatConfig config, Rect bounds)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Bounds = bounds;
        }

        public FighterState AddFighter(Team team, Vector2 position, float radius = 0.9f)
        {
            if (Fighters.Count >= 64) throw new InvalidOperationException("CombatSim supports at most 64 fighters");
            var p = ClampToBounds(position, radius);
            var fighter = new FighterState
            {
                Id = Fighters.Count, Team = team, Radius = radius, Position = p, Spawn = p,
                Hp = Config.MaxHp, MaxHp = Config.MaxHp,
            };
            Fighters.Add(fighter);
            return fighter;
        }

        /// <summary>Advances one fixed tick. <paramref name="commands"/> is indexed by fighter id; missing entries mean idle.</summary>
        public void Step(IReadOnlyList<FighterCommand> commands)
        {
            Hits.Clear();
            float dt = Config.TickSeconds;
            for (int i = 0; i < Fighters.Count; i++)
            {
                var cmd = commands != null && i < commands.Count ? commands[i] : FighterCommand.Idle;
                Advance(Fighters[i], cmd, dt);
            }

            // Hits resolve after everyone moved, so fighter order never decides who lands first.
            foreach (var f in Fighters) ResolveSwing(f);

            Separate();
            ResolveObstacles();
            foreach (var f in Fighters)
                f.Position = ClampToBounds(f.Position, f.Radius);
            Tick++;
        }

        void Advance(FighterState f, FighterCommand cmd, float dt)
        {
            if (f.Defeated)
            {
                if (--f.RespawnIn <= 0) Respawn(f);
                return;
            }
            if (f.Hitstop > 0)
            {
                f.Hitstop--;
                return;
            }

            if (cmd.Attack) f.Buffer = Config.BufferTicks;
            else if (f.Buffer > 0) f.Buffer--;

            Vector2 move = cmd.Move.magnitude > 1f ? cmd.Move.normalized : cmd.Move;
            if (move.magnitude < Config.DeadZone) move = Vector2.zero;

            if (f.Hitstun > 0)
            {
                f.Hitstun--;
                move = Vector2.zero;
            }
            else
            {
                AdvanceCombo(f, move);
            }

            float factor = f.IsAttacking ? Config.AttackMoveFactor : 1f;
            Vector2 target = move * Config.MoveSpeed * factor;
            float rate = move == Vector2.zero ? Config.Deceleration : Config.Acceleration;
            f.Velocity = Vector2.MoveTowards(f.Velocity, target, rate * dt);
            f.Position += (f.Velocity + f.Impulse) * dt;
            f.Impulse = Vector2.MoveTowards(f.Impulse, Vector2.zero, Config.ImpulseFriction * dt);
            if (move != Vector2.zero && !f.IsAttacking) f.Facing = move.normalized;
        }

        void AdvanceCombo(FighterState f, Vector2 move)
        {
            if (!f.IsAttacking)
            {
                if (f.Buffer > 0) StartStep(f, 1, move);
                return;
            }

            f.StepTick++;
            var step = Config.Combo[f.ComboStep - 1];
            bool canChain = f.StepTick >= step.Startup + step.Active && f.Buffer > 0;
            if (canChain && f.ComboStep < Config.Combo.Length)
            {
                StartStep(f, f.ComboStep + 1, move);
            }
            else if (f.StepTick >= step.Total)
            {
                f.ComboStep = 0;
                f.StepTick = 0;
                if (f.Buffer > 0) StartStep(f, 1, move); // holding attack loops the combo
            }
        }

        void StartStep(FighterState f, int stepIndex, Vector2 move)
        {
            // A new swing may re-aim toward the stick; the facing then locks until the swing ends.
            if (move != Vector2.zero) f.Facing = move.normalized;
            f.ComboStep = stepIndex;
            f.StepTick = 0;
            f.Buffer = 0;
            f.StepVictims = 0;
            f.Impulse += f.Facing * Config.Combo[stepIndex - 1].Lunge;
        }

        void ResolveSwing(FighterState a)
        {
            if (!a.IsAttacking || a.Defeated || a.Hitstop > 0) return;
            var step = Config.Combo[a.ComboStep - 1];
            if (a.StepTick < step.Startup || a.StepTick >= step.Startup + step.Active) return;

            float halfArc = step.ArcDegrees * 0.5f;
            foreach (var v in Fighters)
            {
                if (v == a || v.Team == a.Team || v.Defeated) continue;
                ulong bit = 1UL << v.Id;
                if ((a.StepVictims & bit) != 0) continue;

                Vector2 delta = v.Position - a.Position;
                float dist = delta.magnitude;
                if (dist > a.Radius + step.Range + v.Radius) continue;
                bool overlapping = dist < a.Radius + v.Radius * 0.5f;
                if (!overlapping && Vector2.Angle(a.Facing, delta) > halfArc) continue;

                a.StepVictims |= bit;
                Vector2 dir = dist > 1e-4f ? delta / dist : a.Facing;
                bool finisher = a.ComboStep == Config.Combo.Length;

                v.Hp = Mathf.Max(0, v.Hp - step.Damage);
                v.HitsTaken++;
                v.ComboStep = 0;
                v.StepTick = 0;
                v.Buffer = 0;
                v.Velocity = Vector2.zero;
                v.Impulse = dir * step.Knockback;
                v.Hitstun = step.Hitstun;
                v.Hitstop = step.Hitstop;
                a.Hitstop = step.Hitstop;
                if (v.Hp == 0)
                {
                    v.Defeated = true;
                    v.RespawnIn = Config.RespawnTicks;
                    v.Hitstun = 0;
                }

                Hits.Add(new HitEvent
                {
                    Attacker = a.Id, Victim = v.Id, Damage = step.Damage, ComboStep = a.ComboStep,
                    Finisher = finisher, Defeated = v.Defeated, Point = v.Position - dir * v.Radius,
                });
            }
        }

        void Respawn(FighterState f)
        {
            f.Defeated = false;
            f.Hp = f.MaxHp;
            f.Position = f.Spawn;
            f.Velocity = Vector2.zero;
            f.Impulse = Vector2.zero;
            f.ComboStep = 0;
            f.StepTick = 0;
            f.Hitstun = 0;
            f.Hitstop = 0;
        }

        /// <summary>Pushes overlapping fighters apart symmetrically so bodies never stack. Defeated fighters are walked over.</summary>
        void Separate()
        {
            for (int i = 0; i < Fighters.Count; i++)
            for (int j = i + 1; j < Fighters.Count; j++)
            {
                var a = Fighters[i];
                var b = Fighters[j];
                if (a.Defeated || b.Defeated) continue;
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
                float pushed = Vector2.Dot(f.Impulse, normal);
                if (pushed < 0f) f.Impulse -= normal * pushed;
            }
        }

        Vector2 ClampToBounds(Vector2 p, float radius) => new Vector2(
            Mathf.Clamp(p.x, Bounds.xMin + radius, Bounds.xMax - radius),
            Mathf.Clamp(p.y, Bounds.yMin + radius, Bounds.yMax - radius));
    }
}
