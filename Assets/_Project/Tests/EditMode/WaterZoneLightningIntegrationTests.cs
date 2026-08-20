using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Exercises the actual failed integration boundary end to end: WaterZone
    /// membership -> Combatant.IsInWaterZone -> a Lightning hit via
    /// Combatant.TakeHit -> whether a Conductive Burst primitive is
    /// actually spawned. A test that only calls
    /// ElementalReaction.TryTriggerConductiveBurst(true, Lightning) does not
    /// cover this: it can't catch a membership flag that was never set, or
    /// one that got stuck true after the actor left the zone. Membership is
    /// driven via WaterZone.ApplyMembership directly (this project's
    /// EditMode batch harness cannot run live PhysX trigger simulation - see
    /// WaterZoneMembershipTests), but TakeHit and PrimitiveBurstVFX run for
    /// real, so a genuine burst GameObject is asserted to exist or not.
    ///
    /// Combatant.Awake() is invoked explicitly via reflection after
    /// AddComponent: outside Play Mode, a plain MonoBehaviour (no
    /// [ExecuteAlways]) never gets Awake called automatically by this
    /// project's EditMode batch harness, which left health uninitialized and
    /// made every TakeHit() call silently no-op through its
    /// "if (IsDefeated) return;" guard - a test-harness gap that produced
    /// two false-positive "no burst" passes before a DIAGNOSTIC assertion
    /// caught it (originally against DummyTarget). Play Mode / the real
    /// device always calls Awake normally.
    /// </summary>
    public class WaterZoneLightningIntegrationTests
    {
        const string BurstName = "ConductiveBurstVFX_Primitive";
        const float ConductiveKnockbackMultiplier = 2.5f;

        GameObject zoneObject;
        GameObject dummyObject;

        [TearDown]
        public void TearDown()
        {
            GameObject burst = GameObject.Find(BurstName);
            if (burst != null)
            {
                Object.DestroyImmediate(burst);
            }

            if (dummyObject != null)
            {
                Object.DestroyImmediate(dummyObject);
            }

            if (zoneObject != null)
            {
                Object.DestroyImmediate(zoneObject);
            }
        }

        static Combatant BuildDummy()
        {
            GameObject dummyObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Object.DestroyImmediate(dummyObj.GetComponent<CapsuleCollider>());
            var controller = dummyObj.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;
            dummyObj.AddComponent<KnockbackReceiver>();
            Combatant dummy = dummyObj.AddComponent<Combatant>();

            InvokeAwake(dummy);
            Assert.IsFalse(dummy.IsDefeated, "sanity: a freshly built, Awake-initialized combatant must not start defeated");

            return dummy;
        }

        static void InvokeAwake(MonoBehaviour component)
        {
            MethodInfo awake = component.GetType().GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(awake, $"Expected a private Awake() on {component.GetType().Name}");
            awake.Invoke(component, null);
        }

        const string MaterialLeakWarning = "Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.";

        /// <summary>
        /// TakeHit's existing HitFeedbackFlash starts a coroutine that reads
        /// Renderer.material, which only logs an [Error] because the Editor
        /// (not Play Mode / the real device) considers that an unintended
        /// material leak. Expected and harmless here - not a defect in the
        /// untouched hit-flash feedback - so it's suppressed the same way
        /// Unity's own docs recommend for EditMode tests that exercise real
        /// MonoBehaviour code paths.
        /// </summary>
        static void TakeHitExpectingEditorMaterialWarning(Combatant dummy, HitInfo hit)
        {
            LogAssert.Expect(LogType.Error, MaterialLeakWarning);
            dummy.TakeHit(hit);
        }

        [Test]
        public void LightningHit_InsideWaterZone_SpawnsConductiveBurst()
        {
            zoneObject = new GameObject("Zone");
            WaterZone zone = zoneObject.AddComponent<WaterZone>();

            Combatant dummy = BuildDummy();
            dummyObject = dummy.gameObject;

            zone.ApplyMembership(new[] { dummyObject.GetComponent<Collider>() });
            Assert.IsTrue(dummy.IsInWaterZone, "sanity: membership set");

            // This path additionally reaches PrimitiveBurstVFX.SpawnAt
            // (tints via sharedMaterial + MaterialPropertyBlock, like
            // GreyboxSceneBootstrapper.Tint - no renderer.material leak). The
            // particle burst has no Collider, so unlike the old cube-based
            // implementation there is nothing to Object.Destroy synchronously
            // - only the flash's material-leak warning is expected here.
            LogAssert.Expect(LogType.Error, MaterialLeakWarning);
            dummy.TakeHit(new HitInfo(1, DamageElement.Lightning, Vector3.zero, ConductiveKnockbackMultiplier));

            Assert.IsNotNull(GameObject.Find(BurstName), "Lightning hit inside the WaterZone must spawn a Conductive Burst");
        }

        [Test]
        public void LightningHit_OutsideWaterZone_DoesNotSpawnConductiveBurst()
        {
            zoneObject = new GameObject("Zone");
            WaterZone zone = zoneObject.AddComponent<WaterZone>();

            Combatant dummy = BuildDummy();
            dummyObject = dummy.gameObject;

            zone.ApplyMembership(System.Array.Empty<Collider>());
            Assert.IsFalse(dummy.IsInWaterZone, "sanity: never entered");

            TakeHitExpectingEditorMaterialWarning(dummy, new HitInfo(1, DamageElement.Lightning, Vector3.zero, ConductiveKnockbackMultiplier));

            Assert.IsNull(GameObject.Find(BurstName), "Lightning hit outside the WaterZone must NOT spawn a Conductive Burst");
        }

        [Test]
        public void LightningHit_AfterLeavingWaterZone_DoesNotSpawnConductiveBurst()
        {
            zoneObject = new GameObject("Zone");
            WaterZone zone = zoneObject.AddComponent<WaterZone>();

            Combatant dummy = BuildDummy();
            dummyObject = dummy.gameObject;
            Collider dummyCollider = dummyObject.GetComponent<Collider>();

            zone.ApplyMembership(new[] { dummyCollider });
            Assert.IsTrue(dummy.IsInWaterZone, "sanity: entered zone");

            zone.ApplyMembership(System.Array.Empty<Collider>());
            Assert.IsFalse(dummy.IsInWaterZone, "sanity: exited zone");

            TakeHitExpectingEditorMaterialWarning(dummy, new HitInfo(1, DamageElement.Lightning, Vector3.zero, ConductiveKnockbackMultiplier));

            Assert.IsNull(
                GameObject.Find(BurstName),
                "Regression guard: an actor that left the WaterZone must not spawn a Conductive Burst - this is exactly the false-positive risk of a one-time 'set true, never clear' overlap check");
        }
    }
}
