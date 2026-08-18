using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class BossAttackCycleTests
    {
        [Test]
        public void NewCycle_StartsAtArcStrike()
        {
            var cycle = new BossAttackCycle();

            Assert.AreEqual(BossPattern.ArcStrike, cycle.CurrentPattern);
        }

        [Test]
        public void AdvancePattern_CyclesInExactDeterministicOrder()
        {
            var cycle = new BossAttackCycle();

            Assert.AreEqual(BossPattern.Charge, cycle.AdvancePattern());
            Assert.AreEqual(BossPattern.RadialPulse, cycle.AdvancePattern());
            Assert.AreEqual(BossPattern.ArcStrike, cycle.AdvancePattern());
            Assert.AreEqual(BossPattern.Charge, cycle.AdvancePattern());
        }

        [Test]
        public void Reset_ReturnsToArcStrike()
        {
            var cycle = new BossAttackCycle();
            cycle.AdvancePattern();
            cycle.AdvancePattern();

            cycle.Reset();

            Assert.AreEqual(BossPattern.ArcStrike, cycle.CurrentPattern);
        }
    }
}
