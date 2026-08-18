using System.Collections;
using NUnit.Framework;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Task B1 regression: LocalPlayerActionGateway must resolve every
    /// request through the same PlayerActionExecutor - and therefore the
    /// same BasicAttack/PlayerSkillController state (cooldowns, events,
    /// damage) - a caller would see going directly through those components.
    /// NetworkPlayerActionGateway's server-side ServerRpc handler calls the
    /// identical PlayerActionExecutor type; full client/server routing is
    /// proven end to end by the Task B6 two-process smoke test, not here.
    /// </summary>
    public class PlayerActionGatewayIntegrationTests
    {
        GameObject playerObject;
        GameObject targetObject;

        [TearDown]
        public void TearDown()
        {
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

        (LocalPlayerActionGateway gateway, PlayerSkillController skillController, BasicAttack basicAttack) BuildPlayerWithGateway()
        {
            playerObject = BuildActor("Player", Vector3.zero);
            playerObject.AddComponent<TouchInputReader>();
            var basicAttack = playerObject.AddComponent<BasicAttack>();
            playerObject.AddComponent<LoiTramSkill>();
            playerObject.AddComponent<PhongBoSkill>();
            playerObject.AddComponent<HoTheSkill>();
            var skillController = playerObject.AddComponent<PlayerSkillController>();

            var executor = new PlayerActionExecutor(basicAttack, skillController);
            var gateway = playerObject.AddComponent<LocalPlayerActionGateway>();
            gateway.Initialize(executor);

            return (gateway, skillController, basicAttack);
        }

        [UnityTest]
        public IEnumerator RequestLoiTram_RoutesThroughExecutor_DamagesTargetAndRespectsCooldown()
        {
            (LocalPlayerActionGateway gateway, _, _) = BuildPlayerWithGateway();
            targetObject = BuildActor("Target", new Vector3(0f, 0f, 1.2f));
            var targetCombatant = targetObject.GetComponent<Combatant>();
            yield return null;

            int healthBefore = targetCombatant.CurrentHealth;

            bool firstRequest = gateway.RequestLoiTram();
            Assert.IsTrue(firstRequest, "Gateway must report the request resolved.");
            Assert.Less(targetCombatant.CurrentHealth, healthBefore, "The gateway request must reach the same LoiTramSkill that deals real damage.");

            int healthAfterFirst = targetCombatant.CurrentHealth;
            bool secondRequestImmediately = gateway.RequestLoiTram();
            Assert.IsFalse(secondRequestImmediately, "A second gateway request during cooldown must be rejected, same as calling the skill directly.");
            Assert.AreEqual(healthAfterFirst, targetCombatant.CurrentHealth);
        }

        [UnityTest]
        public IEnumerator RequestBasicAttack_RoutesThroughExecutor_TriggersRealAttackSequence()
        {
            (LocalPlayerActionGateway gateway, _, BasicAttack basicAttack) = BuildPlayerWithGateway();
            yield return null;

            int startedCount = 0;
            basicAttack.AttackStarted += () => startedCount++;

            bool requested = gateway.RequestBasicAttack();

            Assert.IsTrue(requested, "Gateway must report the Basic Attack request resolved.");
            Assert.AreEqual(1, startedCount, "The gateway request must reach the same BasicAttack instance Update() would have triggered from local input.");
        }

        [UnityTest]
        public IEnumerator RequestPhongBoAndHoThe_RouteThroughExecutor_FireTheSameActivatedEvents()
        {
            (LocalPlayerActionGateway gateway, PlayerSkillController skillController, _) = BuildPlayerWithGateway();
            yield return null;

            int phongBoFired = 0;
            int hoTheFired = 0;
            skillController.PhongBo.Activated += () => phongBoFired++;
            skillController.HoThe.Activated += () => hoTheFired++;

            Assert.IsTrue(gateway.RequestPhongBo());
            Assert.IsTrue(gateway.RequestHoThe());

            Assert.AreEqual(1, phongBoFired);
            Assert.AreEqual(1, hoTheFired);
        }
    }
}
