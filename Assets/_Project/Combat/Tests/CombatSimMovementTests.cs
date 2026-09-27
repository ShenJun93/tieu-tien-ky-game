using System.Collections.Generic;
using NUnit.Framework;
using TieuTienKy.Combat;
using UnityEngine;

namespace TieuTienKy.Combat.Tests
{
    public class CombatSimMovementTests
    {
        static CombatSim NewSim() => new CombatSim(new CombatConfig(), new Rect(-20f, -12f, 40f, 24f));

        static List<FighterCommand> Commands(params Vector2[] moves)
        {
            var list = new List<FighterCommand>();
            foreach (var m in moves) list.Add(new FighterCommand { Move = m });
            return list;
        }

        static void Run(CombatSim sim, List<FighterCommand> commands, int ticks)
        {
            for (int i = 0; i < ticks; i++) sim.Step(commands);
        }

        [Test]
        public void FighterReachesMoveSpeedAndMovesInInputDirection()
        {
            var sim = NewSim();
            var f = sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, Commands(new Vector2(1f, 0f)), sim.Config.TickRate); // one second

            Assert.AreEqual(sim.Config.MoveSpeed, f.Velocity.x, 1e-3f);
            Assert.Greater(f.Position.x, sim.Config.MoveSpeed * 0.8f);
            Assert.AreEqual(0f, f.Position.y, 1e-4f);
        }

        [Test]
        public void DiagonalInputIsNotFasterThanStraightInput()
        {
            var sim = NewSim();
            var f = sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, Commands(new Vector2(1f, 1f)), sim.Config.TickRate);
            Assert.LessOrEqual(f.Velocity.magnitude, sim.Config.MoveSpeed + 1e-3f);
        }

        [Test]
        public void SmallStickNoiseInsideDeadZoneDoesNotMove()
        {
            var sim = NewSim();
            var f = sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, Commands(new Vector2(0.05f, 0.05f)), 30);
            Assert.AreEqual(Vector2.zero, f.Position);
        }

        [Test]
        public void ReleasingTheStickStopsQuicklyAndKeepsFacing()
        {
            var sim = NewSim();
            var f = sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, Commands(new Vector2(0f, 1f)), 20);
            Run(sim, Commands(Vector2.zero), 5);
            Assert.AreEqual(Vector2.zero, f.Velocity);
            Assert.AreEqual(new Vector2(0f, 1f), f.Facing);
        }

        [Test]
        public void FightersStayInsideArenaBounds()
        {
            var sim = NewSim();
            var f = sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, Commands(new Vector2(-1f, -1f)), 300);
            Assert.AreEqual(sim.Bounds.xMin + f.Radius, f.Position.x, 1e-4f);
            Assert.AreEqual(sim.Bounds.yMin + f.Radius, f.Position.y, 1e-4f);
        }

        [Test]
        public void OverlappingFightersArePushedApart()
        {
            var sim = NewSim();
            var a = sim.AddFighter(Team.Jade, Vector2.zero);
            var b = sim.AddFighter(Team.Crimson, new Vector2(0.3f, 0f));
            sim.Step(null);
            Assert.GreaterOrEqual((b.Position - a.Position).magnitude, a.Radius + b.Radius - 1e-4f);
        }

        [Test]
        public void SameCommandsGiveSameResult()
        {
            Vector2 Final()
            {
                var sim = NewSim();
                var f = sim.AddFighter(Team.Jade, new Vector2(1f, 2f));
                sim.AddFighter(Team.Crimson, new Vector2(3f, 2f));
                for (int i = 0; i < 90; i++)
                    sim.Step(Commands(new Vector2(Mathf.Sin(i * 0.1f), Mathf.Cos(i * 0.07f)), Vector2.zero));
                return f.Position;
            }
            Assert.AreEqual(Final(), Final());
        }

        [Test]
        public void TickCounterAdvancesOncePerStep()
        {
            var sim = NewSim();
            sim.AddFighter(Team.Jade, Vector2.zero);
            Run(sim, null, 7);
            Assert.AreEqual(7, sim.Tick);
        }
    }
}
