using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Regression coverage for the physical-device bug where a Player driven
    /// purely by CharacterController.Move could walk past the finite Ground
    /// plane (no boundary colliders existed) and fall out of the world under
    /// gravity. GreyboxSceneBootstrapper now builds four invisible,
    /// collider-only perimeter walls around the Ground. EditMode cannot run
    /// gravity/physics over time, so these tests pin the deterministic parts:
    /// the walls exist as plain BoxColliders (no renderer), their inner faces
    /// sit exactly on the Ground's perimeter, and they fully cover the
    /// perimeter (including corners) without intruding into - and thereby
    /// shrinking - the playable Ground area.
    /// </summary>
    public class GreyboxArenaBoundaryTests
    {
        const float FloatTolerance = 1e-4f;

        static readonly string[] WallNames =
        {
            "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West"
        };

        [TearDown]
        public void TearDown()
        {
            foreach (string name in new[] { "Ground", "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West" })
            {
                var go = GameObject.Find(name);
                if (go != null)
                {
                    Object.DestroyImmediate(go);
                }
            }
        }

        [Test]
        public void BuildArenaBoundaries_CreatesFourColliderOnlyWalls_SolidAndInvisible()
        {
            GameObject ground = BuildGroundAndBoundaries();

            foreach (string wallName in WallNames)
            {
                var wall = GameObject.Find(wallName);
                Assert.IsNotNull(wall, $"Expected a boundary wall GameObject named '{wallName}'.");

                var collider = wall.GetComponent<BoxCollider>();
                Assert.IsNotNull(collider, $"'{wallName}' must have a BoxCollider to block CharacterController.Move.");
                Assert.IsFalse(collider.isTrigger, $"'{wallName}' must be solid (non-trigger) to contain movement.");

                Assert.IsNull(wall.GetComponent<Renderer>(), $"'{wallName}' must have no renderer (invisible boundary).");
            }
        }

        [Test]
        public void BuildArenaBoundaries_InnerFacesAlignExactlyWithGroundPerimeter()
        {
            GameObject ground = BuildGroundAndBoundaries();
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;

            Bounds north = GameObject.Find("ArenaWall_North").GetComponent<BoxCollider>().bounds;
            Bounds south = GameObject.Find("ArenaWall_South").GetComponent<BoxCollider>().bounds;
            Bounds east = GameObject.Find("ArenaWall_East").GetComponent<BoxCollider>().bounds;
            Bounds west = GameObject.Find("ArenaWall_West").GetComponent<BoxCollider>().bounds;

            Assert.AreEqual(groundBounds.max.z, north.min.z, FloatTolerance, "North wall's inner face must sit flush with the Ground's +Z edge.");
            Assert.AreEqual(groundBounds.min.z, south.max.z, FloatTolerance, "South wall's inner face must sit flush with the Ground's -Z edge.");
            Assert.AreEqual(groundBounds.max.x, east.min.x, FloatTolerance, "East wall's inner face must sit flush with the Ground's +X edge.");
            Assert.AreEqual(groundBounds.min.x, west.max.x, FloatTolerance, "West wall's inner face must sit flush with the Ground's -X edge.");
        }

        [Test]
        public void BuildArenaBoundaries_FullyCoverPerimeterCorners_WithoutIntrudingOnPlayableGround()
        {
            GameObject ground = BuildGroundAndBoundaries();
            Bounds groundBounds = ground.GetComponent<Collider>().bounds;

            Bounds north = GameObject.Find("ArenaWall_North").GetComponent<BoxCollider>().bounds;
            Bounds south = GameObject.Find("ArenaWall_South").GetComponent<BoxCollider>().bounds;
            Bounds east = GameObject.Find("ArenaWall_East").GetComponent<BoxCollider>().bounds;
            Bounds west = GameObject.Find("ArenaWall_West").GetComponent<BoxCollider>().bounds;

            // North/South must span the full X width, including both corners,
            // so a diagonal move can't slip past where East/West would meet them.
            Assert.LessOrEqual(north.min.x, groundBounds.min.x + FloatTolerance);
            Assert.GreaterOrEqual(north.max.x, groundBounds.max.x - FloatTolerance);
            Assert.LessOrEqual(south.min.x, groundBounds.min.x + FloatTolerance);
            Assert.GreaterOrEqual(south.max.x, groundBounds.max.x - FloatTolerance);

            // East/West only need to span the Ground's own Z extent; the
            // corners are already covered by the extended North/South walls.
            Assert.LessOrEqual(east.min.z, groundBounds.min.z + FloatTolerance);
            Assert.GreaterOrEqual(east.max.z, groundBounds.max.z - FloatTolerance);
            Assert.LessOrEqual(west.min.z, groundBounds.min.z + FloatTolerance);
            Assert.GreaterOrEqual(west.max.z, groundBounds.max.z - FloatTolerance);

            // No wall may intrude past the Ground's own edge into the
            // playable interior - that would shrink the arena.
            Assert.GreaterOrEqual(north.min.z, groundBounds.max.z - FloatTolerance);
            Assert.LessOrEqual(south.max.z, groundBounds.min.z + FloatTolerance);
            Assert.GreaterOrEqual(east.min.x, groundBounds.max.x - FloatTolerance);
            Assert.LessOrEqual(west.max.x, groundBounds.min.x + FloatTolerance);
        }

        static GameObject BuildGroundAndBoundaries()
        {
            var ground = (GameObject)InvokeBuilder("BuildGround");
            InvokeBuilder("BuildArenaBoundaries", ground);
            return ground;
        }

        static object InvokeBuilder(string methodName, params object[] args)
        {
            MethodInfo method = typeof(GreyboxSceneBootstrapper).GetMethod(
                methodName, BindingFlags.NonPublic | BindingFlags.Static);

            Assert.IsNotNull(method, $"Expected private static method '{methodName}' on GreyboxSceneBootstrapper.");
            return method.Invoke(null, args);
        }
    }
}
