using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Play Mode integration coverage for the Work Package 3 four-action
    /// player kit's three new skills. EditMode cannot run coroutines
    /// (Phong Bộ's dash, Hộ Thể's window close on Update) the way a live
    /// run does, so this proves real cooldown gating, arena-bounded
    /// reposition, and damage-mitigation wiring end to end.
    /// </summary>
    public class PlayerSkillKitIntegrationTests
    {
        GameObject playerObject;
        GameObject targetObject;

        [TearDown]
        public void TearDown()
        {
            // Defensive: a test here can end (or its object can be
            // destroyed) while a skill's real-time HitStop coroutine is
            // still mid-flight, which aborts it before it restores
            // Time.timeScale - silently leaving every later test in the
            // batch running at the frozen scale. Reset unconditionally so
            // this fixture never leaks that global state forward.
            Time.timeScale = 1f;

            if (playerObject != null) Object.Destroy(playerObject);
            if (targetObject != null) Object.Destroy(targetObject);
        }

        static GameObject BuildActor(string name, Vector3 position)
        {
            var go = new GameObject(name);
            var controller = go.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.4f;
            go.AddComponent<KnockbackReceiver>();
            go.AddComponent<Combatant>();
            go.transform.position = position;
            return go;
        }

        GameObject BuildPlayerWithSkillKit()
        {
            playerObject = BuildActor("Player", Vector3.zero);
            playerObject.AddComponent<LoiTramSkill>();
            playerObject.AddComponent<PhongBoSkill>();
            playerObject.AddComponent<HoTheSkill>();
            playerObject.AddComponent<PlayerSkillController>();
            return playerObject;
        }

        [UnityTest]
        public IEnumerator LoiTram_DamagesTargetInRange_AndRespectsCooldown()
        {
            BuildPlayerWithSkillKit();
            targetObject = BuildActor("Target", new Vector3(0f, 0f, 1.2f));
            var targetCombatant = targetObject.GetComponent<Combatant>();
            yield return null;

            var controller = playerObject.GetComponent<PlayerSkillController>();
            int healthBeforeFirst = targetCombatant.CurrentHealth;

            bool firstActivation = controller.TryActivateLoiTram();
            Assert.IsTrue(firstActivation);
            Assert.Less(targetCombatant.CurrentHealth, healthBeforeFirst, "Lôi Trảm should have damaged the in-range target.");

            int healthAfterFirst = targetCombatant.CurrentHealth;
            bool secondActivationImmediately = controller.TryActivateLoiTram();
            Assert.IsFalse(secondActivationImmediately, "Lôi Trảm must be on cooldown immediately after use.");
            Assert.AreEqual(healthAfterFirst, targetCombatant.CurrentHealth, "A rejected activation must not deal any damage.");
        }

        [UnityTest]
        public IEnumerator PhongBo_Dash_NeverLeavesArenaBounds()
        {
            BuildPlayerWithSkillKit();
            playerObject.transform.position = new Vector3(1.5f, 0f, 1.5f);
            playerObject.transform.forward = Vector3.forward;
            yield return null;

            var controller = playerObject.GetComponent<PlayerSkillController>();
            var tightBounds = new ArenaBounds(-2f, 2f, -2f, 2f);
            controller.PhongBo.SetArenaBounds(tightBounds);

            bool activated = controller.TryActivatePhongBo();
            Assert.IsTrue(activated);

            yield return new WaitForSeconds(0.3f);

            Assert.IsTrue(tightBounds.Contains(playerObject.transform.position), "Phong Bộ must never leave the authoritative arena bounds, even dashing toward the edge.");
        }

        [UnityTest]
        public IEnumerator HoThe_BlocksDamageDuringWindow_ThenRestoresAfter()
        {
            BuildPlayerWithSkillKit();
            var combatant = playerObject.GetComponent<Combatant>();
            var controller = playerObject.GetComponent<PlayerSkillController>();
            controller.HoThe.SetWindowDuration(0.05f);
            yield return null;

            int healthBeforeWindow = combatant.CurrentHealth;
            bool activated = controller.TryActivateHoThe();
            Assert.IsTrue(activated);

            combatant.TakeHit(new HitInfo(1, DamageElement.Lightning, Vector3.zero));
            Assert.AreEqual(healthBeforeWindow, combatant.CurrentHealth, "Hộ Thể's active window must fully block incoming damage.");

            yield return new WaitForSeconds(0.15f);

            combatant.TakeHit(new HitInfo(1, DamageElement.Lightning, Vector3.zero));
            Assert.Less(combatant.CurrentHealth, healthBeforeWindow, "Damage must apply normally again once the Hộ Thể window has closed.");
        }

        [UnityTest]
        public IEnumerator PlayerSkillController_ActivatedEvents_FireOnSuccessfulActivation()
        {
            BuildPlayerWithSkillKit();
            var controller = playerObject.GetComponent<PlayerSkillController>();
            yield return null;

            int loiTramFired = 0;
            int phongBoFired = 0;
            int hoTheFired = 0;
            controller.LoiTram.Activated += () => loiTramFired++;
            controller.PhongBo.Activated += () => phongBoFired++;
            controller.HoThe.Activated += () => hoTheFired++;

            Assert.IsTrue(controller.TryActivateLoiTram());
            Assert.IsTrue(controller.TryActivatePhongBo());
            Assert.IsTrue(controller.TryActivateHoThe());

            Assert.AreEqual(1, loiTramFired);
            Assert.AreEqual(1, phongBoFired);
            Assert.AreEqual(1, hoTheFired);
        }
    }
}
