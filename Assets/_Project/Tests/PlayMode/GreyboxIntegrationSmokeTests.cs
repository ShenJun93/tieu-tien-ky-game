using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// One bounded Play Mode smoke test exercising the real bootstrap ->
    /// ArenaRunDirector wiring end to end (Awake/coroutines/physics actually
    /// run here, unlike EditMode). Confirms the object graph the P0A+ slice
    /// built is genuinely connected - it is not a fun/balance gate. The
    /// authoritative fun/feel judgment remains the Human physical playtest.
    /// </summary>
    public class GreyboxIntegrationSmokeTests
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
        public IEnumerator Bootstrap_BuildsFullyWiredRunReadyForWave1()
        {
            bootstrapRoot = new GameObject("P0A_Bootstrap");
            bootstrapRoot.AddComponent<GreyboxSceneBootstrapper>();

            yield return null;
            yield return null;

            GameObject player = GameObject.Find("Player");
            Assert.IsNotNull(player, "Player must exist after bootstrap.");
            Assert.IsNotNull(player.GetComponent<CharacterController>());
            Assert.IsNotNull(player.GetComponent<Combatant>());
            Assert.IsNotNull(player.GetComponent<PlayerController>());
            Assert.IsNotNull(player.GetComponent<BasicAttack>());
            Assert.IsNotNull(player.GetComponent<SwordAttackView>());
            Assert.IsNotNull(player.GetComponent<PlayerBlessingPresentation>(), "Player must carry the blessing presentation controller.");
            Assert.IsNotNull(player.transform.Find("CharacterView/WeaponSocket/Sword"), "Player must be a full-body armed sword cultivator.");
            Assert.AreEqual(5, player.GetComponent<Combatant>().MaxHealth, "Player base max health must match bootstrap configuration.");

            GameObject directorObject = GameObject.Find("P0A_ArenaRunDirector");
            Assert.IsNotNull(directorObject, "ArenaRunDirector must exist after bootstrap.");
            var runDirector = directorObject.GetComponent<ArenaRunDirector>();
            Assert.IsNotNull(runDirector);
            Assert.AreEqual(ArenaRunStage.Wave1, runDirector.Stage, "Run must start at Wave1.");

            GameObject pursuer = GameObject.Find("Pursuer");
            Assert.IsNotNull(pursuer, "Wave 1 must have spawned at least one Pursuer.");
            Assert.IsNotNull(pursuer.GetComponent<Combatant>());
            Assert.IsNotNull(pursuer.GetComponent<EnemyCombatController>());
            Assert.IsNotNull(pursuer.GetComponent<PrimitiveTelegraphVFX>());

            Assert.IsNotNull(GameObject.Find("P0A_RunHud"), "Temporary run HUD must be attached.");
            Assert.IsNotNull(GameObject.Find("P0A_BlessingChoiceHud"), "Temporary blessing choice HUD must be attached.");
            Assert.IsNotNull(GameObject.Find("P0A_OnboardingHud"), "Temporary onboarding HUD must be attached.");
            Assert.IsNotNull(GameObject.Find("P0A_ArenaEventDirector"), "Arena event director must be attached.");
            Assert.IsNotNull(GameObject.Find("WaterZone"), "Water Zone must exist for Water Shift to relocate.");
            Assert.IsNotNull(GameObject.Find("HazardObstacle"), "Hazard obstacle must exist for knockback play.");

            foreach (string wallName in new[] { "ArenaWall_North", "ArenaWall_South", "ArenaWall_East", "ArenaWall_West" })
            {
                Assert.IsNotNull(GameObject.Find(wallName), $"Arena boundary '{wallName}' must exist to contain the run.");
            }
        }
    }
}
