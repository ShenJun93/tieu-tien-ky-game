using System.Collections.Generic;
using NUnit.Framework;
using TieuTienKy.Combat;
using UnityEngine;

namespace TieuTienKy.Combat.Tests
{
    public class CombatSimAttackTests
    {
        static CombatSim NewSim() => new CombatSim(new CombatConfig(), new Rect(-20f, -12f, 40f, 24f));

        static readonly FighterCommand Attack = new FighterCommand { Attack = true };

        /// <summary>Attacker at the origin facing +x, plus one fighter at <paramref name="at"/>.</summary>
        static (CombatSim sim, FighterState attacker, FighterState target) Duel(Vector2 at, Team targetTeam = Team.Crimson)
        {
            var sim = NewSim();
            var attacker = sim.AddFighter(Team.Jade, Vector2.zero);
            attacker.Facing = new Vector2(1f, 0f);
            var target = sim.AddFighter(targetTeam, at);
            return (sim, attacker, target);
        }

        static List<HitEvent> Run(CombatSim sim, int ticks, FighterCommand attackerCommand)
        {
            var hits = new List<HitEvent>();
            var commands = new List<FighterCommand> { attackerCommand, FighterCommand.Idle };
            for (int i = 0; i < ticks; i++)
            {
                sim.Step(commands);
                hits.AddRange(sim.Hits);
            }
            return hits;
        }

        [Test]
        public void OneTapHitsTheEnemyInFrontOnceForTheFirstStepDamage()
        {
            var (sim, attacker, target) = Duel(new Vector2(2.2f, 0f));
            var hits = Run(sim, 1, Attack);
            hits.AddRange(Run(sim, 30, FighterCommand.Idle));

            Assert.AreEqual(1, hits.Count, "a single tap is a single swing that hits once");
            Assert.AreEqual(sim.Config.Combo[0].Damage, hits[0].Damage);
            Assert.AreEqual(target.MaxHp - sim.Config.Combo[0].Damage, target.Hp);
            Assert.AreEqual(1, target.HitsTaken);
            Assert.AreEqual(0, attacker.ComboStep, "the swing ends after its recovery");
        }

        [Test]
        public void SwingsMissBehindOutOfRangeAndAllies()
        {
            var behind = Duel(new Vector2(-2.2f, 0f));
            Assert.IsEmpty(Run(behind.sim, 20, Attack), "target behind the attacker");

            var far = Duel(new Vector2(6f, 0f));
            Assert.IsEmpty(Run(far.sim, 20, Attack), "target out of reach");

            var ally = Duel(new Vector2(2.2f, 0f), Team.Jade);
            Assert.IsEmpty(Run(ally.sim, 20, Attack), "same sect never takes damage");
            Assert.AreEqual(ally.target.MaxHp, ally.target.Hp);
        }

        [Test]
        public void HoldingAttackChainsTheThreeStepsWithAHeavierFinisher()
        {
            var (sim, attacker, target) = Duel(new Vector2(2.4f, 0f));
            var steps = new List<int>();
            var hits = new List<HitEvent>();
            var commands = new List<FighterCommand> { Attack, FighterCommand.Idle };
            for (int i = 0; i < 90 && hits.Count < 3; i++)
            {
                // Keep the dummy in reach: the chain is what is under test, not the knockback.
                target.Position = attacker.Position + new Vector2(2.4f, 0f);
                sim.Step(commands);
                hits.AddRange(sim.Hits);
                if (attacker.ComboStep > 0 && (steps.Count == 0 || steps[steps.Count - 1] != attacker.ComboStep))
                    steps.Add(attacker.ComboStep);
            }

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, steps.GetRange(0, 3));
            Assert.AreEqual(3, hits.Count);
            Assert.IsFalse(hits[1].Finisher);
            Assert.IsTrue(hits[2].Finisher);
            Assert.Greater(sim.Config.Combo[2].Damage, sim.Config.Combo[0].Damage);
            Assert.Greater(target.Impulse.magnitude, sim.Config.Combo[0].Knockback, "the finisher knocks back harder");
        }

        [Test]
        public void TheComboResetsWhenTheButtonIsNotPressedAgain()
        {
            var (sim, attacker, _) = Duel(new Vector2(8f, 0f));
            Run(sim, 1, Attack);
            Run(sim, sim.Config.Combo[0].Total + 2, FighterCommand.Idle);
            Assert.AreEqual(0, attacker.ComboStep);

            Run(sim, 1, Attack);
            Assert.AreEqual(1, attacker.ComboStep, "a late press starts again from the first step");
        }

        [Test]
        public void HitstopFreezesAttackerAndVictimForTheSameTicks()
        {
            var (sim, attacker, target) = Duel(new Vector2(2.2f, 0f));
            var commands = new List<FighterCommand> { Attack, FighterCommand.Idle };
            int guard = 0;
            while (sim.Hits.Count == 0 && guard++ < 30) sim.Step(commands);
            Assert.AreEqual(1, sim.Hits.Count);

            int stop = sim.Config.Combo[0].Hitstop;
            Assert.AreEqual(stop, attacker.Hitstop);
            Assert.AreEqual(stop, target.Hitstop);
            var frozenAttacker = attacker.Position;
            var frozenTarget = target.Position;
            int frozenTick = attacker.StepTick;

            Run(sim, stop - 1, FighterCommand.Idle);
            Assert.AreEqual(frozenAttacker, attacker.Position);
            Assert.AreEqual(frozenTarget, target.Position);
            Assert.AreEqual(frozenTick, attacker.StepTick, "the swing does not advance during hitstop");

            Run(sim, 3, FighterCommand.Idle);
            Assert.AreNotEqual(frozenTarget, target.Position, "knockback plays out after the freeze");
        }

        [Test]
        public void ADefeatedFighterRespawnsAtItsSpawnWithFullHp()
        {
            var (sim, attacker, target) = Duel(new Vector2(2.2f, 0f));
            Vector2 spawn = target.Spawn;
            target.Hp = 10;
            var hits = Run(sim, 20, Attack);

            Assert.IsTrue(hits[0].Defeated);
            Assert.IsTrue(target.Defeated);
            Assert.AreEqual(0, target.Hp);

            attacker.Position = new Vector2(-10f, 0f); // clear the spawn so separation does not nudge the respawn
            Run(sim, sim.Config.RespawnTicks + 1, FighterCommand.Idle);
            Assert.IsFalse(target.Defeated);
            Assert.AreEqual(target.MaxHp, target.Hp);
            Assert.AreEqual(spawn, target.Position);
        }

        [Test]
        public void SameCommandsWithAttacksGiveTheSameResult()
        {
            (int hp, Vector2 pos) Play()
            {
                var (sim, _, target) = Duel(new Vector2(2.2f, 0.4f));
                var commands = new List<FighterCommand> { default, FighterCommand.Idle };
                for (int i = 0; i < 120; i++)
                {
                    commands[0] = new FighterCommand { Move = new Vector2(Mathf.Sin(i * 0.1f), 0.2f), Attack = i % 7 < 3 };
                    sim.Step(commands);
                }
                return (target.Hp, target.Position);
            }

            var first = Play();
            var second = Play();
            Assert.AreEqual(first.hp, second.hp);
            Assert.AreEqual(first.pos, second.pos);
            Assert.Less(first.hp, 1000, "the scripted fight should land hits");
        }
    }
}
