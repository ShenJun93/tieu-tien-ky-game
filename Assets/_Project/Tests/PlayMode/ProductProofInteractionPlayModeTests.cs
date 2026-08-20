using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    public class ProductProofInteractionPlayModeTests
    {
        [TearDown]
        public void TearDown()
        {
            // Defensive: a real-time HitStop coroutine (LoiTram/PhongBo/HoThe)
            // can still be mid-flight when a test ends or its actor gets
            // destroyed, which aborts it before it restores Time.timeScale
            // and would otherwise leak the frozen scale into later tests.
            Time.timeScale = 1f;
        }

        [UnityTest]
        public IEnumerator StormControl_WetPrimaryTargetPushesNearbyBystander()
        {
            GameObject player = CreateActor("Player", Vector3.zero);
            var loiTram = player.AddComponent<LoiTramSkill>();
            player.transform.forward = Vector3.forward;

            GameObject wetTarget = CreateActor("WetTarget", new Vector3(0f, 0f, 1.8f));
            wetTarget.GetComponent<Combatant>().SetInWaterZone(true);

            GameObject bystander = CreateActor("Bystander", new Vector3(1.5f, 0f, 1.8f));
            Combatant bystanderCombatant = bystander.GetComponent<Combatant>();

            yield return null;

            loiTram.SetStormControl(ProductProofRunStyle.FromStacks(thunderStacks: 1, windStacks: 0, wardStacks: 0));

            Assert.IsTrue(loiTram.TryActivate(Time.time));
            yield return null;

            Assert.AreEqual(DamageElement.Physical, bystanderCombatant.LastHitElement,
                "Storm Control should convert a deliberate wet Lôi hit into a spatial pulse that reaches a nearby enemy outside the base Lôi hit sphere.");

            Object.DestroyImmediate(player);
            Object.DestroyImmediate(wetTarget);
            Object.DestroyImmediate(bystander);
        }

        [UnityTest]
        public IEnumerator WindWard_BlockThenPhongBoProducesSingleGaleCounterPulse()
        {
            GameObject player = CreateActor("Player", Vector3.zero);
            player.AddComponent<LoiTramSkill>();
            player.AddComponent<PhongBoSkill>();
            player.AddComponent<HoTheSkill>();
            var skills = player.AddComponent<PlayerSkillController>();
            player.transform.forward = Vector3.forward;

            GameObject bystander = CreateActor("Bystander", new Vector3(1.4f, 0f, 3f));
            Combatant bystanderCombatant = bystander.GetComponent<Combatant>();

            yield return null;

            skills.ConfigureRunStyle(ProductProofRunStyle.FromStacks(thunderStacks: 0, windStacks: 1, wardStacks: 1));
            Assert.IsTrue(skills.TryActivateHoThe());

            Combatant playerCombatant = player.GetComponent<Combatant>();
            int healthBeforeBlock = playerCombatant.CurrentHealth;
            playerCombatant.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));
            Assert.AreEqual(healthBeforeBlock, playerCombatant.CurrentHealth, "Hộ Thể must actually block the priming hit.");

            Assert.IsTrue(skills.TryActivatePhongBo());
            yield return new WaitForSeconds(0.35f);

            Assert.AreEqual(DamageElement.Physical, bystanderCombatant.LastHitElement,
                "The primed Phong Bộ should end with a bounded Gale Counter push pulse.");

            Object.DestroyImmediate(player);
            Object.DestroyImmediate(bystander);
        }

        [UnityTest]
        public IEnumerator PhanChan_PerfectHoThe_KnocksBackAndInterruptsNearbyLancerTelegraph()
        {
            GameObject player = CreateActor("Player", Vector3.zero);
            player.AddComponent<LoiTramSkill>();
            player.AddComponent<PhongBoSkill>();
            var hoThe = player.AddComponent<HoTheSkill>();
            player.AddComponent<PlayerSkillController>();
            Combatant playerCombatant = player.GetComponent<Combatant>();

            GameObject lancerObject = CreateActor("Lancer", new Vector3(0f, 0f, 1.5f));
            Combatant lancerCombatant = lancerObject.GetComponent<Combatant>();
            KnockbackReceiver lancerKnockback = lancerObject.GetComponent<KnockbackReceiver>();
            var lancerController = lancerObject.AddComponent<EnemyCombatController>();
            lancerController.Initialize(player.transform, playerCombatant, EnemyCombatProfile.Lancer());

            yield return null;

            Assert.AreEqual(EnemyAttackPhase.Telegraph, lancerController.Phase,
                "The Lancer must be mid-telegraph (within AttackRange, chase already resolved) before Phan Chan can prove an interrupt.");

            int playerHealthBeforePerfectBlock = playerCombatant.CurrentHealth;
            Assert.IsTrue(hoThe.TryActivate(Time.time));

            // Perfect-timed hit: well inside HoTheSkill's default perfect sub-window (first ~0.12s of a 0.45s block window).
            playerCombatant.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));

            Assert.AreEqual(playerHealthBeforePerfectBlock, playerCombatant.CurrentHealth, "Ho The must still fully block the priming hit even when it is also a perfect-timed Phan Chan trigger.");
            Assert.IsTrue(lancerKnockback.IsBeingKnockedBack, "A perfect Ho The must reflect a radial stagger that knocks back a nearby enemy.");

            // Wait past the Lancer's full original telegraph deadline (0.65s) plus margin: the knockback must have shoved it out of attack range, so its eventually-resolved attack signal lands on nobody.
            yield return new WaitForSeconds(1.0f);

            Assert.AreEqual(playerHealthBeforePerfectBlock, playerCombatant.CurrentHealth,
                "The Lancer's telegraphed attack must never connect - Phan Chan's knockback interrupted it 'for free' via the existing knockback pipeline, with no enemy-AI file changes.");
            Assert.IsFalse(lancerCombatant.IsDefeated);

            Object.DestroyImmediate(player);
            Object.DestroyImmediate(lancerObject);
        }

        static GameObject CreateActor(string name, Vector3 position)
        {
            var actor = new GameObject(name);
            actor.transform.position = position;

            var controller = actor.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            actor.AddComponent<KnockbackReceiver>();
            var combatant = actor.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(5);
            return actor;
        }
    }
}
