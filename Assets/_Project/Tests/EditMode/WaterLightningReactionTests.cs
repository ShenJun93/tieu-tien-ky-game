using NUnit.Framework;

namespace TieuTienKy.Gameplay.Tests
{
    public class WaterLightningReactionTests
    {
        [Test]
        public void LightningHit_InsideWaterZone_TriggersConductiveBurst()
        {
            bool triggered = ElementalReaction.TryTriggerConductiveBurst(
                targetInWaterZone: true,
                incomingElement: DamageElement.Lightning);

            Assert.IsTrue(triggered);
        }

        [Test]
        public void LightningHit_OutsideWaterZone_DoesNotTrigger()
        {
            bool triggered = ElementalReaction.TryTriggerConductiveBurst(
                targetInWaterZone: false,
                incomingElement: DamageElement.Lightning);

            Assert.IsFalse(triggered);
        }

        [Test]
        public void PhysicalHit_InsideWaterZone_DoesNotTrigger()
        {
            bool triggered = ElementalReaction.TryTriggerConductiveBurst(
                targetInWaterZone: true,
                incomingElement: DamageElement.Physical);

            Assert.IsFalse(triggered);
        }

        [Test]
        public void PhysicalHit_OutsideWaterZone_DoesNotTrigger()
        {
            bool triggered = ElementalReaction.TryTriggerConductiveBurst(
                targetInWaterZone: false,
                incomingElement: DamageElement.Physical);

            Assert.IsFalse(triggered);
        }
    }
}
