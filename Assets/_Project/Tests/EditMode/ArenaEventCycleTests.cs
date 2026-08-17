using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class ArenaEventCycleTests
    {
        [Test]
        public void Inactive_Begin_Succeeds()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);

            bool began = cycle.Begin(0f);

            Assert.IsTrue(began);
            Assert.AreEqual(ArenaEventPhase.Warning, cycle.Phase);
        }

        [Test]
        public void DuringWarning_Begin_Fails()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);

            bool began = cycle.Begin(0.1f);

            Assert.IsFalse(began);
        }

        [Test]
        public void Tick_BeforeWarningElapses_StaysInWarning()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);

            cycle.Tick(0.79f, out bool becameActive, out bool finished);

            Assert.IsFalse(becameActive);
            Assert.IsFalse(finished);
            Assert.AreEqual(ArenaEventPhase.Warning, cycle.Phase);
        }

        [Test]
        public void Tick_ExactlyAtWarningEnd_BecomesActiveExactlyOnce()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);

            bool signaled = cycle.Tick(0.8f, out bool becameActive, out _);

            Assert.IsTrue(signaled);
            Assert.IsTrue(becameActive);
            Assert.AreEqual(ArenaEventPhase.Active, cycle.Phase);

            bool signaledAgain = cycle.Tick(0.81f, out bool becameActiveAgain, out _);
            Assert.IsFalse(signaledAgain);
            Assert.IsFalse(becameActiveAgain);
        }

        [Test]
        public void Tick_AfterActiveElapses_EntersCooldown()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);
            cycle.Tick(0.8f, out _, out _);

            cycle.Tick(1.0f, out bool becameActive, out bool finished);

            Assert.IsFalse(becameActive);
            Assert.IsFalse(finished);
            Assert.AreEqual(ArenaEventPhase.Cooldown, cycle.Phase);
        }

        [Test]
        public void Tick_AfterCooldownElapses_ReturnsToInactiveAndFinishesOnce()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);
            cycle.Tick(0.8f, out _, out _);
            cycle.Tick(1.0f, out _, out _);

            cycle.Tick(1.3f, out bool becameActive, out bool finished);

            Assert.IsFalse(becameActive);
            Assert.IsTrue(finished);
            Assert.AreEqual(ArenaEventPhase.Inactive, cycle.Phase);
        }

        [Test]
        public void AfterFullCycle_CanBeginAgain()
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);
            cycle.Tick(0.8f, out _, out _);
            cycle.Tick(1.0f, out _, out _);
            cycle.Tick(1.3f, out _, out _);

            bool began = cycle.Begin(1.3f);

            Assert.IsTrue(began);
        }

        [TestCase(ArenaEventPhase.Warning)]
        [TestCase(ArenaEventPhase.Active)]
        [TestCase(ArenaEventPhase.Cooldown)]
        public void Reset_FromAnyPhase_ReturnsToInactive(ArenaEventPhase reachedPhase)
        {
            var cycle = new ArenaEventCycle(warningSeconds: 0.8f, activeSeconds: 0.2f, cooldownSeconds: 0.3f);
            cycle.Begin(0f);
            if (reachedPhase == ArenaEventPhase.Active || reachedPhase == ArenaEventPhase.Cooldown)
            {
                cycle.Tick(0.8f, out _, out _);
            }
            if (reachedPhase == ArenaEventPhase.Cooldown)
            {
                cycle.Tick(1.0f, out _, out _);
            }

            cycle.Reset();

            Assert.AreEqual(ArenaEventPhase.Inactive, cycle.Phase);
            Assert.IsTrue(cycle.Begin(0f));
        }
    }
}
