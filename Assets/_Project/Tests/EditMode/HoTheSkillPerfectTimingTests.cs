using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class HoTheSkillPerfectTimingTests
    {
        // 0.125 (1/8) is exactly representable in binary floating point, like
        // HoTheWindowTests' own 0.5s window - unlike 0.12, so the boundary
        // literal below is guaranteed bit-identical to the runtime sum
        // HoTheSkill.IsPerfectTiming computes, with no rounding-path ambiguity.
        const float ActivatedAtTime = 10f;
        const float PerfectWindowSeconds = 0.125f;

        [Test]
        public void IsPerfectTiming_AtWindowActivationTime_IsTrue()
        {
            Assert.IsTrue(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 10f));
        }

        [Test]
        public void IsPerfectTiming_BeforeWindowActivationTime_IsFalse()
        {
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 9.99f));
        }

        [Test]
        public void IsPerfectTiming_JustBeforePerfectWindowEnds_IsTrue()
        {
            Assert.IsTrue(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 10.124f));
        }

        [Test]
        public void IsPerfectTiming_AtOrAfterPerfectWindowEnds_IsFalse()
        {
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 10.125f));
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 10.3f));
        }

        [Test]
        public void IsPerfectTiming_StillWithinBroaderBlockWindowButPastPerfectSubWindow_IsFalse()
        {
            // Mirrors the real usage: a 0.45s Ho The block window with a small
            // perfect sub-window - a hit well after the perfect sub-window but
            // still inside the broader block window is a normal (non-perfect)
            // block, not a Phan Chan trigger.
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(ActivatedAtTime, PerfectWindowSeconds, 10.2f));
        }

        [Test]
        public void IsPerfectTiming_NegativePerfectWindow_ClampsToZero_NeverPerfect()
        {
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(ActivatedAtTime, -1f, 10f));
        }
    }
}
