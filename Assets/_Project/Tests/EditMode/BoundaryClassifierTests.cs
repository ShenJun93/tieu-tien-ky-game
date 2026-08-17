using NUnit.Framework;
using TieuTienKy.Diagnostics;
using UnityEngine;

namespace TieuTienKy.Diagnostics.Tests
{
    /// <summary>
    /// Deterministic coverage for the P0A diagnostic boundary classifier.
    /// Exercises the pure function directly with hand-built Bounds - no
    /// scene, no GameObjects - so it also pins that classification reads the
    /// supplied runtime Bounds rather than any hardcoded arena size.
    /// </summary>
    public class BoundaryClassifierTests
    {
        static readonly Bounds ArenaBounds = new Bounds(Vector3.zero, new Vector3(20f, 1f, 20f));

        static Bounds PlayerBoundsAt(float x, float z)
        {
            return new Bounds(new Vector3(x, 1f, z), new Vector3(1f, 2f, 1f));
        }

        [Test]
        public void Classify_FullyInsideArena_ReturnsNone()
        {
            Assert.AreEqual(BoundarySide.None, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(0f, 0f)));
        }

        [Test]
        public void Classify_PastNorthEdge_ReturnsNorth()
        {
            Assert.AreEqual(BoundarySide.North, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(0f, 11f)));
        }

        [Test]
        public void Classify_PastSouthEdge_ReturnsSouth()
        {
            Assert.AreEqual(BoundarySide.South, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(0f, -11f)));
        }

        [Test]
        public void Classify_PastEastEdge_ReturnsEast()
        {
            Assert.AreEqual(BoundarySide.East, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(11f, 0f)));
        }

        [Test]
        public void Classify_PastWestEdge_ReturnsWest()
        {
            Assert.AreEqual(BoundarySide.West, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(-11f, 0f)));
        }

        [Test]
        public void Classify_PastNorthAndEastEdges_ReturnsCorner()
        {
            Assert.AreEqual(BoundarySide.Corner, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(11f, 11f)));
        }

        [Test]
        public void Classify_PastSouthAndWestEdges_ReturnsCorner()
        {
            Assert.AreEqual(BoundarySide.Corner, BoundaryClassifier.Classify(ArenaBounds, PlayerBoundsAt(-11f, -11f)));
        }

        [Test]
        public void Classify_UsesProvidedRuntimeBounds_NotHardcodedArenaSize()
        {
            var smallArena = new Bounds(Vector3.zero, new Vector3(4f, 1f, 4f));
            var largeArena = new Bounds(Vector3.zero, new Vector3(200f, 1f, 200f));
            Bounds player = PlayerBoundsAt(3f, 0f);

            // Same absolute position: OUT_OF_BOUNDS against a small arena,
            // IN_BOUNDS against a large one - proves the classifier reads
            // the supplied Bounds instead of a fixed arena size constant.
            Assert.AreEqual(BoundarySide.East, BoundaryClassifier.Classify(smallArena, player));
            Assert.AreEqual(BoundarySide.None, BoundaryClassifier.Classify(largeArena, player));
        }
    }
}
