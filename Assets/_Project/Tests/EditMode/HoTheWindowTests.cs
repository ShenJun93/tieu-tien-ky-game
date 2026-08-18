using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class HoTheWindowTests
    {
        [Test]
        public void IsActive_AtActivationTime_IsTrue()
        {
            var window = new HoTheWindow(10f, 0.5f);
            Assert.IsTrue(window.IsActive(10f));
        }

        [Test]
        public void IsActive_BeforeActivationTime_IsFalse()
        {
            var window = new HoTheWindow(10f, 0.5f);
            Assert.IsFalse(window.IsActive(9.99f));
        }

        [Test]
        public void IsActive_JustBeforeWindowEnds_IsTrue()
        {
            var window = new HoTheWindow(10f, 0.5f);
            Assert.IsTrue(window.IsActive(10.49f));
        }

        [Test]
        public void IsActive_AtOrAfterWindowEnds_IsFalse()
        {
            var window = new HoTheWindow(10f, 0.5f);
            Assert.IsFalse(window.IsActive(10.5f));
            Assert.IsFalse(window.IsActive(11f));
        }

        [Test]
        public void NegativeDuration_ClampsToZero_NeverActive()
        {
            var window = new HoTheWindow(10f, -1f);
            Assert.IsFalse(window.IsActive(10f));
        }
    }
}
