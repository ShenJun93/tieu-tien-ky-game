using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class DamageMitigationTests
    {
        [Test]
        public void Apply_FullMultiplier_ReturnsRawDamage()
        {
            Assert.AreEqual(3, DamageMitigation.Apply(3, 1f));
        }

        [Test]
        public void Apply_ZeroMultiplier_FullyBlocksDamage()
        {
            Assert.AreEqual(0, DamageMitigation.Apply(5, 0f));
        }

        [Test]
        public void Apply_MultiplierAboveOne_ClampsToRawDamage()
        {
            Assert.AreEqual(2, DamageMitigation.Apply(2, 5f));
        }

        [Test]
        public void Apply_NegativeMultiplier_ClampsToZero()
        {
            Assert.AreEqual(0, DamageMitigation.Apply(2, -1f));
        }

        [Test]
        public void Apply_NeverReturnsNegativeDamage()
        {
            Assert.AreEqual(0, DamageMitigation.Apply(0, 1f));
        }
    }
}
