using NUnit.Framework;
using TieuTienKy.Core;

namespace TieuTienKy.Gameplay.Tests
{
    public class AttackCooldownTests
    {
        [Test]
        public void FirstUse_AtTimeZero_Succeeds()
        {
            var cooldown = new Cooldown(0.6f);

            bool used = cooldown.TryUse(0f);

            Assert.IsTrue(used);
        }

        [Test]
        public void SecondUse_BeforeDurationElapsed_Fails()
        {
            var cooldown = new Cooldown(0.6f);
            cooldown.TryUse(0f);

            bool used = cooldown.TryUse(0.59f);

            Assert.IsFalse(used);
        }

        [Test]
        public void SecondUse_ExactlyAtDuration_Succeeds()
        {
            var cooldown = new Cooldown(0.6f);
            cooldown.TryUse(0f);

            bool used = cooldown.TryUse(0.6f);

            Assert.IsTrue(used);
        }

        [Test]
        public void SecondUse_AfterDurationElapsed_Succeeds()
        {
            var cooldown = new Cooldown(0.6f);
            cooldown.TryUse(0f);

            bool used = cooldown.TryUse(1.2f);

            Assert.IsTrue(used);
        }

        [Test]
        public void FailedUse_DoesNotResetOrExtendTheWindow()
        {
            var cooldown = new Cooldown(0.6f);
            cooldown.TryUse(0f);

            cooldown.TryUse(0.1f);
            bool usedAtOriginalWindow = cooldown.TryUse(0.6f);

            Assert.IsTrue(usedAtOriginalWindow);
        }
    }
}
