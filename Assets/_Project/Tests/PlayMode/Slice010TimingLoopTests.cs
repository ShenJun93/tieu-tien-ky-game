using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    public class Slice010TimingLoopTests
    {
        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            foreach (var combatant in Object.FindObjectsByType<Combatant>(FindObjectsSortMode.None))
            {
                Object.DestroyImmediate(combatant.gameObject);
            }
        }

        [Test]
        public void PerfectTiming_IsFirstPointTwelveSecondsOnly()
        {
            const float start = 10f;
            Assert.IsTrue(HoTheSkill.IsPerfectTiming(start, 0.12f, start));
            Assert.IsTrue(HoTheSkill.IsPerfectTiming(start, 0.12f, start + 0.119f));
            Assert.IsFalse(HoTheSkill.IsPerfectTiming(start, 0.12f, start + 0.12f));
        }

        [UnityTest]
        public IEnumerator NormalHoTheBlock_BlocksDamageWithoutPhanChan()
        {
            GameObject player = CreateActor("Player", Vector3.zero);
            HoTheSkill hoThe = player.AddComponent<HoTheSkill>();
            Combatant combatant = player.GetComponent<Combatant>();
            int healthBefore = combatant.CurrentHealth;
            int phanChanCount = 0;
            hoThe.PhanChanTriggered += () => phanChanCount++;

            yield return null;
            Assert.IsTrue(hoThe.TryActivate(Time.time));
            yield return new WaitForSecondsRealtime(0.16f);

            combatant.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));

            Assert.AreEqual(healthBefore, combatant.CurrentHealth,
                "A hit inside the 0.45s ward window must still be fully blocked.");
            Assert.AreEqual(0, phanChanCount,
                "A late ward hit must not receive the perfect-timing offensive payoff.");
        }

        [UnityTest]
        public IEnumerator PerfectHoThe_TriggersPhanChanAndInterruptsLancer()
        {
            GameObject player = CreateActor("Player", Vector3.zero);
            HoTheSkill hoThe = player.AddComponent<HoTheSkill>();
            Combatant playerCombatant = player.GetComponent<Combatant>();

            GameObject lancer = CreateActor("Lancer", new Vector3(0f, 0f, 1.5f));
            Combatant lancerCombatant = lancer.GetComponent<Combatant>();
            KnockbackReceiver lancerKnockback = lancer.GetComponent<KnockbackReceiver>();
            EnemyCombatController enemy = lancer.AddComponent<EnemyCombatController>();
            enemy.Initialize(player.transform, playerCombatant, EnemyCombatProfile.Lancer());

            int phanChanCount = 0;
            hoThe.PhanChanTriggered += () => phanChanCount++;
            yield return null;
            Assert.AreEqual(EnemyAttackPhase.Telegraph, enemy.Phase);

            int healthBefore = playerCombatant.CurrentHealth;
            Assert.IsTrue(hoThe.TryActivate(Time.time));
            playerCombatant.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));

            Assert.AreEqual(healthBefore, playerCombatant.CurrentHealth,
                "Perfect timing is still a full block, not a damage trade.");
            Assert.AreEqual(1, phanChanCount);
            Assert.IsTrue(lancerKnockback.IsBeingKnockedBack,
                "Phan Chan must create the offensive opening through zero-damage stagger.");

            yield return new WaitForSecondsRealtime(1.0f);

            Assert.AreEqual(healthBefore, playerCombatant.CurrentHealth,
                "The committed Lancer telegraph must not land after the perfect defensive counter.");
            Assert.IsFalse(lancerCombatant.IsDefeated,
                "Phan Chan is an interrupt/stagger payoff, not free lethal damage.");
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
            Combatant combatant = actor.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(5);
            return actor;
        }
    }
}
