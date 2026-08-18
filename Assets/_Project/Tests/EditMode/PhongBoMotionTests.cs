using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    public class PhongBoMotionTests
    {
        static readonly ArenaBounds WideBounds = new ArenaBounds(-100f, 100f, -100f, 100f);

        [Test]
        public void ComputeDestination_MovesForwardByDistance_WhenWellInsideBounds()
        {
            Vector3 destination = PhongBoMotion.ComputeDestination(Vector3.zero, Vector3.forward, 3f, WideBounds);
            Assert.AreEqual(new Vector3(0f, 0f, 3f), destination);
        }

        [Test]
        public void ComputeDestination_NeverLeavesArenaBounds()
        {
            var tightBounds = new ArenaBounds(-2f, 2f, -2f, 2f);
            Vector3 destination = PhongBoMotion.ComputeDestination(Vector3.zero, Vector3.forward, 10f, tightBounds);

            Assert.IsTrue(tightBounds.Contains(destination), "Phong Bộ must never place the player outside the authoritative arena bounds.");
            Assert.AreEqual(2f, destination.z, 0.001f);
        }

        [Test]
        public void ComputeDestination_IgnoresVerticalForwardComponent()
        {
            Vector3 tiltedForward = new Vector3(0f, 0.9f, 1f);
            Vector3 destination = PhongBoMotion.ComputeDestination(Vector3.zero, tiltedForward, 2f, WideBounds);

            Assert.AreEqual(0f, destination.x, 0.001f);
            Assert.AreEqual(2f, destination.z, 0.001f);
        }

        [Test]
        public void ComputeDestination_ZeroForward_FallsBackToWorldForward()
        {
            Vector3 destination = PhongBoMotion.ComputeDestination(Vector3.zero, Vector3.zero, 2f, WideBounds);
            Assert.AreEqual(new Vector3(0f, 0f, 2f), destination);
        }

        [Test]
        public void ComputeDestination_NegativeDistance_ClampsToZeroDistance()
        {
            Vector3 destination = PhongBoMotion.ComputeDestination(new Vector3(1f, 0f, 1f), Vector3.forward, -5f, WideBounds);
            Assert.AreEqual(new Vector3(1f, 0f, 1f), destination);
        }
    }
}
