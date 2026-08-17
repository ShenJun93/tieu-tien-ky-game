using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class EnemyAttackCycleTests
    {
        [Test]
        public void Chase_TryBeginTelegraph_Succeeds()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);

            bool began = cycle.TryBeginTelegraph(0f);

            Assert.IsTrue(began);
            Assert.AreEqual(EnemyAttackPhase.Telegraph, cycle.Phase);
        }

        [Test]
        public void DuringTelegraph_TryBeginTelegraph_Fails()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);

            bool began = cycle.TryBeginTelegraph(0.1f);

            Assert.IsFalse(began);
        }

        [Test]
        public void DuringRecovery_TryBeginTelegraph_Fails()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);
            cycle.Tick(0.4f, out _);

            bool began = cycle.TryBeginTelegraph(0.5f);

            Assert.IsFalse(began);
            Assert.AreEqual(EnemyAttackPhase.Recovery, cycle.Phase);
        }

        [Test]
        public void Tick_BeforeTelegraphElapses_DoesNotFireAttack()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);

            bool attack = cycle.Tick(0.39f, out _);

            Assert.IsFalse(attack);
            Assert.AreEqual(EnemyAttackPhase.Telegraph, cycle.Phase);
        }

        [Test]
        public void Tick_ExactlyAtTelegraphEnd_FiresAttackOnceAndEntersRecovery()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);

            bool attack = cycle.Tick(0.4f, out _);

            Assert.IsTrue(attack);
            Assert.AreEqual(EnemyAttackPhase.Recovery, cycle.Phase);
        }

        [Test]
        public void Tick_ImmediatelyAfterAttack_DoesNotFireAttackAgain()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);
            cycle.Tick(0.4f, out _);

            bool attack = cycle.Tick(0.45f, out _);

            Assert.IsFalse(attack);
        }

        [Test]
        public void Tick_AfterRecoveryElapses_ReturnsToChase()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);
            cycle.Tick(0.4f, out _);

            cycle.Tick(1.0f, out bool recoveryEnded);

            Assert.IsTrue(recoveryEnded);
            Assert.AreEqual(EnemyAttackPhase.Chase, cycle.Phase);
        }

        [Test]
        public void AfterRecoveryEnds_CanBeginTelegraphAgain()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);
            cycle.Tick(0.4f, out _);
            cycle.Tick(1.0f, out _);

            bool began = cycle.TryBeginTelegraph(1.0f);

            Assert.IsTrue(began);
        }

        [Test]
        public void Chase_Tick_NeverFiresAttack()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);

            bool attack = cycle.Tick(5f, out bool recoveryEnded);

            Assert.IsFalse(attack);
            Assert.IsFalse(recoveryEnded);
        }

        [Test]
        public void Reset_FromAnyPhase_ReturnsToChase()
        {
            var cycle = new EnemyAttackCycle(telegraphSeconds: 0.4f, recoverySeconds: 0.6f);
            cycle.TryBeginTelegraph(0f);
            cycle.Tick(0.4f, out _);

            cycle.Reset();

            Assert.AreEqual(EnemyAttackPhase.Chase, cycle.Phase);
            Assert.IsTrue(cycle.TryBeginTelegraph(0f));
        }
    }
}
