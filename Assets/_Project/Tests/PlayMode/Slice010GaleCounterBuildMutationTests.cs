using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    public class Slice010GaleCounterBuildMutationTests
    {
        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            foreach (PhongBoSkill skill in Object.FindObjectsByType<PhongBoSkill>(FindObjectsSortMode.None))
            {
                Object.DestroyImmediate(skill.gameObject);
            }
            foreach (Combatant combatant in Object.FindObjectsByType<Combatant>(FindObjectsSortMode.None))
            {
                if (combatant != null)
                {
                    Object.DestroyImmediate(combatant.gameObject);
                }
            }
        }

        [UnityTest]
        public IEnumerator DefaultPhong_RemainsMobilityWithoutGalePulse()
        {
            PhongBoSkill phong = CreatePlayer(Vector3.zero);
            int pulseCount = 0;
            phong.GaleCounterTriggered += () => pulseCount++;
            phong.SetCooldownDuration(0.1f, Time.time);

            Assert.IsFalse(phong.GaleCounterBuildMutationActive);
            Assert.IsTrue(phong.TryActivate(Time.time));
            yield return new WaitForSecondsRealtime(0.35f);

            Assert.AreEqual(0, pulseCount,
                "Default Phong must stay a mobility/repositioning tool before the build mutation is selected.");
        }

        [UnityTest]
        public IEnumerator WindMutation_MakesEveryPhongLandingProduceZeroDamageDisplacement()
        {
            PhongBoSkill phong = CreatePlayer(Vector3.zero);
            Combatant firstTarget = CreateTarget("FirstTarget", new Vector3(0f, 0f, 3.7f));
            int firstHealth = firstTarget.CurrentHealth;
            Vector3 firstStart = firstTarget.transform.position;
            int pulseCount = 0;
            phong.GaleCounterTriggered += () => pulseCount++;
            phong.SetCooldownDuration(0.1f, Time.time);

            phong.SetGaleCounterBuildMutationActive(true);
            Assert.IsTrue(phong.GaleCounterBuildMutationActive);
            Assert.IsTrue(phong.TryActivate(Time.time));
            yield return new WaitForSecondsRealtime(0.45f);

            Assert.AreEqual(1, pulseCount);
            Assert.AreEqual(firstHealth, firstTarget.CurrentHealth,
                "Gale Counter is spatial control, not a second damage skill.");
            Assert.Greater(Vector3.Distance(firstStart, firstTarget.transform.position), 0.05f,
                "The landing pulse must visibly displace a nearby target.");

            Object.DestroyImmediate(firstTarget.gameObject);
            yield return new WaitForSecondsRealtime(0.15f);
            Vector3 secondTargetPosition = phong.transform.position + phong.transform.forward * 3.7f;
            Combatant secondTarget = CreateTarget("SecondTarget", secondTargetPosition);
            int secondHealth = secondTarget.CurrentHealth;
            Vector3 secondStart = secondTarget.transform.position;

            Assert.IsTrue(phong.TryActivate(Time.time),
                "The mutation must persist for later Phong uses; it is not a one-use ward token.");
            yield return new WaitForSecondsRealtime(0.45f);

            Assert.AreEqual(2, pulseCount);
            Assert.AreEqual(secondHealth, secondTarget.CurrentHealth);
            Assert.Greater(Vector3.Distance(secondStart, secondTarget.transform.position), 0.05f);
        }

        static PhongBoSkill CreatePlayer(Vector3 position)
        {
            GameObject player = new GameObject("Player");
            player.transform.position = position;
            player.transform.forward = Vector3.forward;
            AddCombatBody(player);
            return player.AddComponent<PhongBoSkill>();
        }

        static Combatant CreateTarget(string name, Vector3 position)
        {
            GameObject target = new GameObject(name);
            target.transform.position = position;
            return AddCombatBody(target);
        }

        static Combatant AddCombatBody(GameObject actor)
        {
            var controller = actor.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;
            actor.AddComponent<KnockbackReceiver>();
            var combatant = actor.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(5);
            return combatant;
        }
    }
}
