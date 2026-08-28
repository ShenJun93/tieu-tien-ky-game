using System.Collections;
using System.Reflection;
using NUnit.Framework;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Slice 009 Task 1 regression: production composition must be able to
    /// disable the greybox local world-tap attack trigger (Update()'s direct
    /// read of TouchInputReader.AttackTriggeredThisFrame) without disturbing
    /// the shared TryActivate() path every other caller (gateway, network)
    /// resolves through.
    /// </summary>
    public class RepresentativeCombatSpinePlayModeTests
    {
        static readonly FieldInfo AttackTriggeredThisFrameField =
            typeof(TouchInputReader).GetField("<AttackTriggeredThisFrame>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

        GameObject playerObject;

        [TearDown]
        public void TearDown()
        {
            if (playerObject != null) Object.Destroy(playerObject);
        }

        (TouchInputReader inputReader, BasicAttack basicAttack) BuildPlayer()
        {
            playerObject = new GameObject("Player");
            playerObject.AddComponent<CharacterController>();
            playerObject.AddComponent<KnockbackReceiver>();
            playerObject.AddComponent<Combatant>();
            var inputReader = playerObject.AddComponent<TouchInputReader>();
            var basicAttack = playerObject.AddComponent<BasicAttack>();

            // Test-only seam: disable the reader's own Update() so the
            // reflection-set trigger below is not reset before BasicAttack's
            // Update() runs in the same frame.
            inputReader.enabled = false;

            return (inputReader, basicAttack);
        }

        static void SetAttackTriggeredThisFrame(TouchInputReader inputReader, bool value)
        {
            AttackTriggeredThisFrameField.SetValue(inputReader, value);
        }

        [UnityTest]
        public IEnumerator DefaultLocalWorldTapEnabled_UpdateBranch_TriggersRealAttack()
        {
            (TouchInputReader inputReader, BasicAttack basicAttack) = BuildPlayer();
            yield return null;

            Assert.IsTrue(basicAttack.LocalWorldTapEnabled, "Default must remain enabled for greybox/backward compatibility.");

            int startedCount = 0;
            basicAttack.AttackStarted += () => startedCount++;

            SetAttackTriggeredThisFrame(inputReader, true);
            yield return null;

            Assert.AreEqual(1, startedCount, "With the switch left at its default (enabled), Update()'s local-input branch must still trigger a real attack.");
        }

        [UnityTest]
        public IEnumerator DisablingLocalWorldTap_GatesUpdateBranch_ButDirectTryActivateStillWorks()
        {
            (TouchInputReader inputReader, BasicAttack basicAttack) = BuildPlayer();
            yield return null;

            Assert.IsTrue(basicAttack.LocalWorldTapEnabled);
            basicAttack.SetLocalWorldTapEnabled(false);
            Assert.IsFalse(basicAttack.LocalWorldTapEnabled);

            int startedCount = 0;
            basicAttack.AttackStarted += () => startedCount++;

            SetAttackTriggeredThisFrame(inputReader, true);
            yield return null;

            Assert.AreEqual(0, startedCount, "Update()'s local-input branch must be gated once the switch is disabled, even though TouchInputReader still reports a triggered tap.");

            Assert.IsTrue(basicAttack.TryActivate(Time.time));
            Assert.AreEqual(1, startedCount, "Direct TryActivate() must still work with local world tap disabled - the shared gateway/network path is never gated by this switch.");
        }

        [UnityTest]
        public IEnumerator ArenaScene_WaterZoneMaterial_IsTranslucentAndDoesNotHardOcclude()
        {
            // Slice 009 Task 4 regression: the authored WaterZone rendered as
            // an opaque raised blue cuboid, hard-occluding any actor
            // overlapping it (REPRODUCED / CONFOUNDING against the
            // representative Human Product Gate artifact). Water must use a
            // dedicated Water-only transparent shader with a real alpha hole,
            // never the shared opaque P0A_Unlit/Standard material.
            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;

            var water = GameObject.Find("WaterZone").GetComponent<MeshRenderer>().sharedMaterial;

            Assert.AreEqual("TieuTienKy/P0A_WaterUnlitTransparent", water.shader.name, "WaterZone must use the dedicated Water-only transparent shader, not the shared opaque shader.");
            Assert.GreaterOrEqual(water.renderQueue, 3000, "WaterZone must render in the Transparent queue (>= 3000), not the Opaque queue.");
            Assert.Greater(water.color.a, 0f, "WaterZone alpha must be greater than 0 (not fully invisible).");
            Assert.Less(water.color.a, 1f, "WaterZone alpha must be less than 1 (translucent, not opaque/hard-occluding).");
        }
    }
}
