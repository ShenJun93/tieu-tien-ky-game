using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class ProductProofRunStyleTests
    {
        [Test]
        public void ThunderInvestment_ActivatesStormControlWithSpatialPulse()
        {
            ProductProofRunStyle style = ProductProofRunStyle.FromStacks(thunderStacks: 1, windStacks: 0, wardStacks: 0);

            Assert.IsTrue(style.StormControlActive);
            Assert.Greater(style.StormPulseRadius, 0f);
            Assert.Greater(style.StormPulseImpulse, 0f);
            Assert.IsFalse(style.WindWardActive);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        public void WindWard_RequiresBothWindAndWardInvestment(int windStacks, int wardStacks)
        {
            ProductProofRunStyle style = ProductProofRunStyle.FromStacks(thunderStacks: 0, windStacks: windStacks, wardStacks: wardStacks);

            Assert.IsFalse(style.WindWardActive);
        }

        [Test]
        public void WindWard_BothInvestments_EnableBehaviorChangingCounter()
        {
            ProductProofRunStyle style = ProductProofRunStyle.FromStacks(thunderStacks: 0, windStacks: 1, wardStacks: 1);

            Assert.IsTrue(style.WindWardActive);
            Assert.Greater(style.GaleCounter.DashDistanceMultiplier, 1f);
            Assert.Greater(style.GaleCounter.PushRadius, 0f);
            Assert.Greater(style.GaleCounter.PushImpulse, 0f);
        }

        [Test]
        public void FromInvestments_MatchesBinaryRuntimeActivationRules()
        {
            ProductProofRunStyle style = ProductProofRunStyle.FromInvestments(
                thunderInvested: true,
                windInvested: true,
                wardInvested: true);

            Assert.IsTrue(style.StormControlActive);
            Assert.IsTrue(style.WindWardActive);
        }

        [Test]
        public void WindWardCombo_BlockPrimesExactlyOneCounter()
        {
            ProductProofRunStyle style = ProductProofRunStyle.FromStacks(thunderStacks: 0, windStacks: 1, wardStacks: 1);
            var combo = new WindWardComboState();
            combo.Configure(style);

            Assert.IsTrue(combo.RecordWardBlock());
            Assert.IsTrue(combo.TryConsume(out GaleCounterSpec first));
            Assert.Greater(first.DashDistanceMultiplier, 1f);
            Assert.IsFalse(combo.TryConsume(out _));
        }

        [Test]
        public void WindWardCombo_DisablingStyleClearsPrimedCounter()
        {
            var combo = new WindWardComboState();
            combo.Configure(ProductProofRunStyle.FromStacks(thunderStacks: 0, windStacks: 1, wardStacks: 1));
            Assert.IsTrue(combo.RecordWardBlock());

            combo.Configure(ProductProofRunStyle.FromStacks(thunderStacks: 1, windStacks: 0, wardStacks: 0));

            Assert.IsFalse(combo.TryConsume(out _));
        }

        [Test]
        public void HudLayout_PrimaryLoiButtonIsLargerAndThumbClusterDoesNotOverlap()
        {
            ProductProofHudButtonSpec loi = ProductProofHudLayout.LoiTram;
            ProductProofHudButtonSpec phong = ProductProofHudLayout.PhongBo;
            ProductProofHudButtonSpec ward = ProductProofHudLayout.HoThe;

            Assert.Greater(loi.Size, phong.Size);
            Assert.AreEqual(phong.Size, ward.Size, 0.001f);
            Assert.IsFalse(loi.Overlaps(phong));
            Assert.IsFalse(loi.Overlaps(ward));
            Assert.IsFalse(phong.Overlaps(ward));
        }
    }
}
