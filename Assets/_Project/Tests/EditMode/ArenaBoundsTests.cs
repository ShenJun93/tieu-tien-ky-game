using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    public class ArenaBoundsTests
    {
        [Test]
        public void FromGroundBounds_InsetsEverySideByMargin()
        {
            var ground = new Bounds(Vector3.zero, new Vector3(20f, 1f, 20f));

            ArenaBounds bounds = ArenaBounds.FromGroundBounds(ground, margin: 0.5f);

            Assert.AreEqual(-9.5f, bounds.MinX, 1e-4f);
            Assert.AreEqual(9.5f, bounds.MaxX, 1e-4f);
            Assert.AreEqual(-9.5f, bounds.MinZ, 1e-4f);
            Assert.AreEqual(9.5f, bounds.MaxZ, 1e-4f);
        }

        [Test]
        public void Clamp_PositionInsideBounds_IsUnchanged()
        {
            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

            Vector3 result = bounds.Clamp(new Vector3(3f, 1f, -4f));

            Assert.AreEqual(new Vector3(3f, 1f, -4f), result);
        }

        [Test]
        public void Clamp_PositionPastEdges_IsClampedPerAxis()
        {
            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

            Vector3 result = bounds.Clamp(new Vector3(20f, 1f, -20f));

            Assert.AreEqual(9f, result.x, 1e-4f);
            Assert.AreEqual(1f, result.y, 1e-4f);
            Assert.AreEqual(-9f, result.z, 1e-4f);
        }

        [Test]
        public void Clamp_NeverTouchesY()
        {
            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

            Vector3 result = bounds.Clamp(new Vector3(0f, 42f, 0f));

            Assert.AreEqual(42f, result.y, 1e-4f);
        }

        [Test]
        public void Contains_InsidePosition_ReturnsTrue()
        {
            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

            Assert.IsTrue(bounds.Contains(new Vector3(9f, 1f, -9f)));
        }

        [Test]
        public void Contains_OutsidePosition_ReturnsFalse()
        {
            var bounds = new ArenaBounds(-9f, 9f, -9f, 9f);

            Assert.IsFalse(bounds.Contains(new Vector3(9.1f, 1f, 0f)));
        }
    }
}
