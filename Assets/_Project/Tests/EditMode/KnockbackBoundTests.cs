using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    public class KnockbackBoundTests
    {
        [Test]
        public void ImpulseBelowBound_IsUnchanged()
        {
            Vector3 raw = new Vector3(2f, 0f, 0f);

            Vector3 clamped = KnockbackCalculator.ClampToBound(raw, 12f);

            Assert.AreEqual(raw, clamped);
        }

        [Test]
        public void ImpulseAboveBound_IsClampedToMaxMagnitude()
        {
            Vector3 raw = new Vector3(50f, 0f, 0f);

            Vector3 clamped = KnockbackCalculator.ClampToBound(raw, 12f);

            Assert.AreEqual(12f, clamped.magnitude, 0.0001f);
        }

        [Test]
        public void ImpulseAboveBound_PreservesDirection()
        {
            Vector3 raw = new Vector3(0f, 0f, 30f);

            Vector3 clamped = KnockbackCalculator.ClampToBound(raw, 12f);

            Assert.AreEqual(raw.normalized, clamped.normalized);
        }

        [Test]
        public void ImpulseExactlyAtBound_IsUnchanged()
        {
            Vector3 raw = new Vector3(12f, 0f, 0f);

            Vector3 clamped = KnockbackCalculator.ClampToBound(raw, 12f);

            Assert.AreEqual(raw.magnitude, clamped.magnitude, 0.0001f);
        }

        [Test]
        public void ZeroImpulse_StaysZero()
        {
            Vector3 clamped = KnockbackCalculator.ClampToBound(Vector3.zero, 12f);

            Assert.AreEqual(Vector3.zero, clamped);
        }

        [Test]
        public void NegativeMaxMagnitude_IsTreatedAsZeroBound()
        {
            Vector3 raw = new Vector3(5f, 0f, 0f);

            Vector3 clamped = KnockbackCalculator.ClampToBound(raw, -3f);

            Assert.AreEqual(0f, clamped.magnitude, 0.0001f);
        }
    }
}
