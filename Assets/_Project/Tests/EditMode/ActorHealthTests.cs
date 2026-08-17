using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class ActorHealthTests
    {
        [Test]
        public void Damage_ReachesZero_ReportsDefeatOnce()
        {
            var health = new ActorHealth(3);
            Assert.IsFalse(health.ApplyDamage(1));
            Assert.IsFalse(health.ApplyDamage(1));
            Assert.IsTrue(health.ApplyDamage(1));
            Assert.IsTrue(health.IsDefeated);
            Assert.AreEqual(0, health.CurrentHealth);
            Assert.IsFalse(health.ApplyDamage(1));
        }

        [Test]
        public void RestoreToFull_ClearsDefeat()
        {
            var health = new ActorHealth(2);
            health.ApplyDamage(2);
            health.RestoreToFull();
            Assert.AreEqual(2, health.CurrentHealth);
            Assert.IsFalse(health.IsDefeated);
        }

        [Test]
        public void SetMaxHealthAndRestore_UpdatesMaximumAndCurrent()
        {
            var health = new ActorHealth(2);
            health.SetMaxHealthAndRestore(5);
            Assert.AreEqual(5, health.MaxHealth);
            Assert.AreEqual(5, health.CurrentHealth);
        }
    }
}
