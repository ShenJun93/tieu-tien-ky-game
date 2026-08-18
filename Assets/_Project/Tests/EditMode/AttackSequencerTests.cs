using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class AttackSequencerTests
    {
        [Test]
        public void Idle_TryBeginAttack_Succeeds()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);

            bool began = sequencer.TryBeginAttack(0f);

            Assert.IsTrue(began);
            Assert.AreEqual(AttackPhase.Anticipation, sequencer.Phase);
        }

        [Test]
        public void DuringAnticipation_TryBeginAttack_Fails()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);

            bool began = sequencer.TryBeginAttack(0.05f);

            Assert.IsFalse(began);
        }

        [Test]
        public void DuringRecovery_TryBeginAttack_Fails()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);
            sequencer.Tick(0.1f, out _);

            bool began = sequencer.TryBeginAttack(0.15f);

            Assert.IsFalse(began);
            Assert.AreEqual(AttackPhase.Recovery, sequencer.Phase);
        }

        [Test]
        public void Tick_BeforeAnticipationElapses_DoesNotFireImpact()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);

            bool impact = sequencer.Tick(0.05f, out _);

            Assert.IsFalse(impact);
            Assert.AreEqual(AttackPhase.Anticipation, sequencer.Phase);
        }

        [Test]
        public void Tick_ExactlyAtAnticipationEnd_FiresImpactOnceAndEntersRecovery()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);

            bool impact = sequencer.Tick(0.1f, out _);

            Assert.IsTrue(impact);
            Assert.AreEqual(AttackPhase.Recovery, sequencer.Phase);
        }

        [Test]
        public void Tick_ImmediatelyAfterImpact_DoesNotFireImpactAgain()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);
            sequencer.Tick(0.1f, out _);

            bool impact = sequencer.Tick(0.15f, out _);

            Assert.IsFalse(impact);
        }

        [Test]
        public void Tick_AfterRecoveryElapses_ReturnsToIdle()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);
            sequencer.Tick(0.1f, out _);

            sequencer.Tick(0.3f, out bool recoveryEnded);

            Assert.IsTrue(recoveryEnded);
            Assert.AreEqual(AttackPhase.Idle, sequencer.Phase);
        }

        [Test]
        public void AfterRecoveryEnds_CanBeginAttackAgain()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);
            sequencer.TryBeginAttack(0f);
            sequencer.Tick(0.1f, out _);
            sequencer.Tick(0.3f, out _);

            bool began = sequencer.TryBeginAttack(0.3f);

            Assert.IsTrue(began);
        }

        [Test]
        public void Idle_Tick_NeverFiresImpact()
        {
            var sequencer = new AttackSequencer(anticipationSeconds: 0.1f, recoverySeconds: 0.2f);

            bool impact = sequencer.Tick(5f, out bool recoveryEnded);

            Assert.IsFalse(impact);
            Assert.IsFalse(recoveryEnded);
        }
    }
}
