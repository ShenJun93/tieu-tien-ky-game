using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Real-object-graph coverage for the Boss stage lifecycle (section 5 of
    /// the P0A+ remediation task): damage must actually reduce boss HP,
    /// zero HP must register defeat, defeat must clear the boss from
    /// ArenaRunDirector's live tracking (CurrentBoss/activeEnemies) exactly
    /// like any other enemy, and RestartRun must fully reset boss/blessing
    /// state. This exercises the exact SpawnBoss/HandleEnemyDefeated path
    /// ArenaRunDirector uses in a real run (via reflection to invoke the
    /// same private spawn method, since driving three full waves + three
    /// blessing gates just to reach the Boss stage is impractical for a
    /// focused regression test) - the Wave1->...->Boss->Victory stage
    /// sequencing itself is already fully covered by the pure
    /// ArenaRunProgressionTests.
    /// </summary>
    public class BossLifecycleIntegrationTests
    {
        static readonly string[] TrackedNames =
        {
            "Ground", "Player", "WaterZone", "HazardObstacle",
            "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West",
            "P0A_RunHud", "P0A_BlessingChoiceHud", "P0A_OnboardingHud", "P0A_ArenaEventDirector", "P0A_ArenaRunDirector",
            "P0A_DiagnosticOverlay", "Pursuer", "Lancer", "MiniBoss"
        };

        GameObject bootstrapRoot;

        [TearDown]
        public void TearDown()
        {
            foreach (string name in TrackedNames)
            {
                var go = GameObject.Find(name);
                if (go != null)
                {
                    Object.Destroy(go);
                }
            }

            if (bootstrapRoot != null)
            {
                Object.Destroy(bootstrapRoot);
            }
        }

        [UnityTest]
        public IEnumerator BossDefeat_ClearsFromActiveTrackingAndCurrentBoss()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();
            yield return null;
            yield return null;

            var director = GameObject.Find("P0A_ArenaRunDirector").GetComponent<ArenaRunDirector>();
            int enemyCountBeforeBoss = director.ActiveEnemyCount;

            InvokeSpawnBoss(director, new Vector3(0f, 1f, 3f));
            yield return null;

            GameObject bossObject = GameObject.Find("MiniBoss");
            Assert.IsNotNull(bossObject, "SpawnBoss must create a 'MiniBoss' GameObject.");
            var bossCombatant = bossObject.GetComponent<Combatant>();
            Assert.IsNotNull(bossCombatant);
            Assert.IsNotNull(director.CurrentBoss, "CurrentBoss must be set once the boss is spawned.");
            Assert.AreEqual(enemyCountBeforeBoss + 1, director.ActiveEnemyCount);

            bossCombatant.TakeHit(new HitInfo(bossCombatant.MaxHealth, DamageElement.Physical, Vector3.zero));

            Assert.IsTrue(bossCombatant.IsDefeated, "Lethal damage must register boss defeat.");
            Assert.IsNull(director.CurrentBoss, "CurrentBoss must clear the instant the boss is defeated, so the boss HP bar disappears.");
            Assert.AreEqual(enemyCountBeforeBoss, director.ActiveEnemyCount, "The defeated boss must be removed from active enemy tracking exactly like any other enemy.");

            yield return new WaitForSeconds(0.6f);
            Assert.IsNull(GameObject.Find("MiniBoss"), "The defeated boss's GameObject must actually be destroyed, not just marked defeated.");
        }

        [UnityTest]
        public IEnumerator BossDamage_ReducesHealthWithoutDefeat_BeforeLethal()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();
            yield return null;
            yield return null;

            var director = GameObject.Find("P0A_ArenaRunDirector").GetComponent<ArenaRunDirector>();
            InvokeSpawnBoss(director, new Vector3(0f, 1f, 3f));
            yield return null;

            Combatant boss = director.CurrentBoss;
            int startingHealth = boss.CurrentHealth;
            Assert.Greater(startingHealth, 1, "Boss must start with more than 1 HP for this test to be meaningful.");

            boss.TakeHit(new HitInfo(1, DamageElement.Physical, Vector3.zero));

            Assert.AreEqual(startingHealth - 1, boss.CurrentHealth, "A non-lethal hit must reduce boss HP by exactly its damage.");
            Assert.IsFalse(boss.IsDefeated);
            Assert.IsNotNull(director.CurrentBoss, "The boss must remain tracked while still alive.");
        }

        [UnityTest]
        public IEnumerator RestartRun_ClearsCurrentBossAndResetsBlessingStacks()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();
            yield return null;
            yield return null;

            var director = GameObject.Find("P0A_ArenaRunDirector").GetComponent<ArenaRunDirector>();
            InvokeSpawnBoss(director, new Vector3(0f, 1f, 3f));
            yield return null;
            Assert.IsNotNull(director.CurrentBoss);

            InvokeApplyBlessing(director, BlessingId.ThunderSword);
            InvokeApplyBlessing(director, BlessingId.WindStride);
            InvokeApplyBlessing(director, BlessingId.BodyWard);
            Assert.Greater(director.Blessings.StackCount(BlessingId.ThunderSword), 0);

            director.RestartRun();
            yield return null;

            Assert.IsNull(director.CurrentBoss, "Restart must clear any live boss reference.");
            Assert.AreEqual(0, director.Blessings.StackCount(BlessingId.ThunderSword), "Restart must reset Lôi Kiếm stacks to 0.");
            Assert.AreEqual(0, director.Blessings.StackCount(BlessingId.WindStride), "Restart must reset Phong Hành stacks to 0.");
            Assert.AreEqual(0, director.Blessings.StackCount(BlessingId.BodyWard), "Restart must reset Hộ Thể stacks to 0.");
            Assert.AreEqual(ArenaRunStage.Wave1, director.Stage, "Restart must return the run to Wave1.");
        }

        static void InvokeSpawnBoss(ArenaRunDirector director, Vector3 position)
        {
            MethodInfo method = typeof(ArenaRunDirector).GetMethod("SpawnBoss", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, "Expected private instance method 'SpawnBoss' on ArenaRunDirector.");
            method.Invoke(director, new object[] { position });
        }

        static void InvokeApplyBlessing(ArenaRunDirector director, BlessingId id)
        {
            MethodInfo method = typeof(ArenaRunDirector).GetMethod("ApplyBlessing", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(method, "Expected private instance method 'ApplyBlessing' on ArenaRunDirector.");
            method.Invoke(director, new object[] { id });
        }
    }
}
