using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class BlessingPresentationMathTests
    {
        [Test]
        public void SwordAccentScale_EscalatesMonotonically_AcrossStacks()
        {
            float s0 = BlessingPresentationMath.SwordAccentScale(0);
            float s1 = BlessingPresentationMath.SwordAccentScale(1);
            float s2 = BlessingPresentationMath.SwordAccentScale(2);
            float s3 = BlessingPresentationMath.SwordAccentScale(3);

            Assert.AreEqual(1f, s0, 1e-4f);
            Assert.Greater(s1, s0);
            Assert.Greater(s2, s1);
            Assert.Greater(s3, s2);
        }

        [Test]
        public void LightningFlash_PresentEvenAtZeroStacks_ForBaselineAttackReadability()
        {
            Assert.Greater(BlessingPresentationMath.LightningFlashPeakRadius(0), 0f);
            Assert.Greater(BlessingPresentationMath.LightningFlashLifetimeSeconds(0), 0f);
        }

        [Test]
        public void LightningFlash_EscalatesMonotonically_AcrossStacks()
        {
            float r0 = BlessingPresentationMath.LightningFlashPeakRadius(0);
            float r1 = BlessingPresentationMath.LightningFlashPeakRadius(1);
            float r2 = BlessingPresentationMath.LightningFlashPeakRadius(2);
            float r3 = BlessingPresentationMath.LightningFlashPeakRadius(3);

            Assert.Greater(r1, r0);
            Assert.Greater(r2, r1);
            Assert.Greater(r3, r2);
        }

        [Test]
        public void WindRing_HiddenAtZeroStacks()
        {
            Assert.IsFalse(BlessingPresentationMath.WindRingVisible(0));
            Assert.AreEqual(0f, BlessingPresentationMath.WindRingScale(0), 1e-4f);
        }

        [Test]
        public void WindRing_VisibleAndEscalating_OnceTaken()
        {
            Assert.IsTrue(BlessingPresentationMath.WindRingVisible(1));

            float s1 = BlessingPresentationMath.WindRingScale(1);
            float s2 = BlessingPresentationMath.WindRingScale(2);
            float s3 = BlessingPresentationMath.WindRingScale(3);

            Assert.Greater(s1, 0f);
            Assert.Greater(s2, s1);
            Assert.Greater(s3, s2);
        }

        [Test]
        public void WardAura_HiddenAtZeroStacks()
        {
            Assert.IsFalse(BlessingPresentationMath.WardAuraVisible(0));
            Assert.AreEqual(0f, BlessingPresentationMath.WardAuraScale(0), 1e-4f);
        }

        [Test]
        public void WardAura_VisibleAndEscalating_OnceTaken()
        {
            Assert.IsTrue(BlessingPresentationMath.WardAuraVisible(1));

            float s1 = BlessingPresentationMath.WardAuraScale(1);
            float s2 = BlessingPresentationMath.WardAuraScale(2);
            float s3 = BlessingPresentationMath.WardAuraScale(3);

            Assert.Greater(s1, 0f);
            Assert.Greater(s2, s1);
            Assert.Greater(s3, s2);
        }
    }
}
