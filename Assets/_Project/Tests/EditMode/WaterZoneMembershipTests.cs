using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Covers the full WaterZone membership lifecycle - not just "starts
    /// true" - against WaterZone.ApplyMembership directly, the same method
    /// FixedUpdate feeds from a live Physics.OverlapBox result in
    /// production. This project's EditMode batch harness cannot fire PhysX
    /// trigger callbacks at all (confirmed with a throwaway Rigidbody
    /// control test during investigation - even a plain kinematic Rigidbody
    /// moved through a trigger via Physics.Simulate() never received
    /// OnTriggerEnter under -batchmode -nographics on this Unity version),
    /// so membership here is driven by directly supplying the overlapping
    /// Collider set ApplyMembership would have received, rather than
    /// depending on that live simulation.
    /// </summary>
    public class WaterZoneMembershipTests
    {
        GameObject zoneObject;
        GameObject dummyRoot;
        GameObject dummyColliderObject;

        [TearDown]
        public void TearDown()
        {
            if (dummyColliderObject != null)
            {
                Object.DestroyImmediate(dummyColliderObject);
            }

            if (dummyRoot != null)
            {
                Object.DestroyImmediate(dummyRoot);
            }

            if (zoneObject != null)
            {
                Object.DestroyImmediate(zoneObject);
            }
        }

        [Test]
        public void InitialInside_MoveOutside_MoveBackInside_TracksMembershipCorrectly()
        {
            zoneObject = new GameObject("Zone");
            WaterZone zone = zoneObject.AddComponent<WaterZone>();

            dummyRoot = new GameObject("Dummy");
            var stub = dummyRoot.AddComponent<StubWaterZoneAware>();
            dummyColliderObject = new GameObject("DummyCollider");
            dummyColliderObject.transform.SetParent(dummyRoot.transform);
            BoxCollider dummyCollider = dummyColliderObject.AddComponent<BoxCollider>();

            Assert.IsFalse(stub.IsInWaterZone, "sanity: starts false, exactly like DummyTarget's default");

            // INITIAL INSIDE -> true (the case OnTriggerEnter misses: spawned already overlapping)
            zone.ApplyMembership(new Collider[] { dummyCollider });
            Assert.IsTrue(stub.IsInWaterZone);

            // MOVE OUTSIDE -> false (must not stay stuck true after leaving)
            zone.ApplyMembership(System.Array.Empty<Collider>());
            Assert.IsFalse(stub.IsInWaterZone);

            // MOVE BACK INSIDE -> true
            zone.ApplyMembership(new Collider[] { dummyCollider });
            Assert.IsTrue(stub.IsInWaterZone);
        }

        [Test]
        public void ApplyMembership_IgnoresCollidersWithoutIWaterZoneAwareParent()
        {
            zoneObject = new GameObject("Zone");
            WaterZone zone = zoneObject.AddComponent<WaterZone>();

            GameObject plain = GameObject.CreatePrimitive(PrimitiveType.Cube);
            try
            {
                Assert.DoesNotThrow(() => zone.ApplyMembership(new[] { plain.GetComponent<Collider>() }));
            }
            finally
            {
                Object.DestroyImmediate(plain);
            }
        }

        sealed class StubWaterZoneAware : MonoBehaviour, IWaterZoneAware
        {
            public bool IsInWaterZone { get; private set; }
            public void SetInWaterZone(bool inWaterZone) => IsInWaterZone = inWaterZone;
        }
    }
}
