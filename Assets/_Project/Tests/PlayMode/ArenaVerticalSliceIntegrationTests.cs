using System.Collections;
using NUnit.Framework;
using TieuTienKy.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Controlled integration proof for the production game-flow scenes
    /// (Work Package 1/5/7): Boot actually advances to Main Menu, and the
    /// authored Arena_VerticalSlice_01 scene boots into a fully wired
    /// Wave 1 with a real prefab-spawned Pursuer, the production HUD, skill
    /// kit, and CultivatorProxy presentation - all without waiting the real
    /// 4-6 minute run length. This is wiring proof, not a fun/balance
    /// judgment (that remains the Human physical playtest).
    /// </summary>
    public class ArenaVerticalSliceIntegrationTests
    {
        [UnityTest]
        public IEnumerator Boot_LoadsMainMenu()
        {
            yield return SceneManager.LoadSceneAsync("Boot");
            yield return null;
            yield return null;

            Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
            Assert.IsNotNull(Object.FindFirstObjectByType<MainMenuController>());
        }

        [UnityTest]
        public IEnumerator ArenaScene_BootstrapsFullyWiredRun_ReadyForWave1()
        {
            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;

            GameObject player = GameObject.Find("Player");
            Assert.IsNotNull(player, "Player must exist after Arena_VerticalSlice_01 boots.");
            Assert.IsNotNull(player.GetComponent<Combatant>());
            Assert.IsNotNull(player.GetComponent<BasicAttack>());
            Assert.IsNotNull(player.GetComponent<PlayerSkillController>());
            Assert.IsNotNull(player.GetComponent<LoiTramSkill>());
            Assert.IsNotNull(player.GetComponent<PhongBoSkill>());
            Assert.IsNotNull(player.GetComponent<HoTheSkill>());
            Assert.IsNotNull(player.GetComponent<TouchInputReader>());

            var presentation = player.GetComponentInChildren<CharacterPresentation>();
            Assert.IsNotNull(presentation, "Player must carry the animated CultivatorProxy presentation.");
            Assert.IsTrue(presentation.IsConfigured);

            var director = Object.FindFirstObjectByType<ArenaRunDirector>();
            Assert.IsNotNull(director);
            Assert.AreEqual(ArenaRunStage.Wave1, director.Stage);
            Assert.AreEqual(1, director.ActiveEnemyCount, "Wave 1 spawns its first enemy immediately.");

            GameObject pursuer = GameObject.Find("Pursuer");
            Assert.IsNotNull(pursuer, "The live Wave 1 enemy must be the authored Pursuer prefab, not an inline-built stand-in.");
            Assert.IsNotNull(pursuer.GetComponent<EnemyCombatController>());

            var pursuerPresentation = pursuer.GetComponentInChildren<CharacterPresentation>();
            Assert.IsNotNull(pursuerPresentation, "Pursuer must carry an animated CharacterPresentation (Task A3), not the primitive placeholder view.");
            Assert.IsTrue(pursuerPresentation.IsConfigured);

            Assert.IsNotNull(Object.FindFirstObjectByType<ArenaEventDirector>());
            Assert.IsNotNull(GameObject.Find("Boundaries"));
            Assert.IsNotNull(GameObject.Find("GameplaySurface"));

            // Slice 009 Task 3: the production arena must instantiate the
            // authored ProductProofCombatHud prefab and bind both presenters
            // to that single authored view, instead of building Canvas/uGUI
            // hierarchies at runtime on empty GameObjects.
            var hudView = Object.FindFirstObjectByType<ProductionCombatHudView>();
            Assert.IsNotNull(hudView, "The arena must instantiate the authored ProductProofCombatHud prefab.");
            Assert.IsTrue(hudView.IsComplete, "Every serialized reference on the authored ProductionCombatHudView must be assigned.");

            var hud = Object.FindFirstObjectByType<ProductionHud>();
            var blessingHud = Object.FindFirstObjectByType<BlessingChoiceHud>();
            Assert.IsNotNull(hud);
            Assert.IsNotNull(blessingHud);
            Assert.AreSame(hudView.gameObject, hud.gameObject, "ProductionHud must live on the authored HUD prefab root, not a runtime-built empty GameObject.");
            Assert.AreSame(hudView.gameObject, blessingHud.gameObject, "BlessingChoiceHud must live on the same authored HUD prefab root.");
            Assert.IsTrue(IsPresenterInitialized(hud), "ProductionHud must be initialized from the authored view.");
            Assert.IsTrue(IsPresenterInitialized(blessingHud), "BlessingChoiceHud must be initialized from the same authored view.");

            Assert.IsNull(Object.FindFirstObjectByType<OnboardingHud>(), "The prototype-era OnboardingHud surface must be absent from the production arena path.");

            Assert.IsNotNull(GameObject.Find("BasicButton"), "The authored HUD must expose a visible, always-reachable Basic attack button.");
            Assert.IsNotNull(GameObject.Find("BlessingChoicePanel"), "The authored HUD must carry the Cơ Duyên choice surface.");

            Canvas[] canvases = Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Assert.AreEqual(1, canvases.Length, "Exactly one production Canvas must exist - combat HUD and Cơ Duyên share the one authored HUD Canvas.");
            Assert.AreSame(hudView.RootCanvas, canvases[0], "That single Canvas must be the authored view's own root Canvas.");

            Assert.IsFalse(player.GetComponent<BasicAttack>().LocalWorldTapEnabled, "Production composition must route Basic through the HUD button, never through a raw world tap.");

            Assert.IsNotNull(hudView.SkillButtons[0], "Skill buttons must exist as real authored Canvas UI elements.");
            Assert.IsNotNull(Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>(), "A Canvas UI needs a live EventSystem to receive touch/click input.");
        }

        /// <summary>Reads the presenter's private one-shot initialization guard, so "the bootstrapper actually bound this presenter to the authored view" is proven rather than assumed from mere presence.</summary>
        static bool IsPresenterInitialized(MonoBehaviour presenter)
        {
            System.Reflection.FieldInfo field = presenter.GetType().GetField(
                "initialized", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(field, $"Expected private field 'initialized' on {presenter.GetType().Name}.");
            return (bool)field.GetValue(presenter);
        }

        [UnityTest]
        public IEnumerator ArenaScene_BoundaryWalls_AreFlushWithVisibleGameplaySurface()
        {
            // Regression for the Human-reported "visible arena larger than
            // reachable arena" blocker: ArenaWall_* must physically contain
            // exactly the visible GameplaySurface extent, no smaller. Mirrors
            // GreyboxArenaBoundaryTests' flush-wall invariant, but against the
            // authored production scene instead of the runtime-built Greybox.
            const float FloatTolerance = 1e-3f;

            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;

            Bounds surfaceBounds = GameObject.Find("GameplaySurface").GetComponent<Collider>().bounds;
            Bounds north = GameObject.Find("ArenaWall_North").GetComponent<BoxCollider>().bounds;
            Bounds south = GameObject.Find("ArenaWall_South").GetComponent<BoxCollider>().bounds;
            Bounds east = GameObject.Find("ArenaWall_East").GetComponent<BoxCollider>().bounds;
            Bounds west = GameObject.Find("ArenaWall_West").GetComponent<BoxCollider>().bounds;

            Assert.AreEqual(surfaceBounds.max.z, north.min.z, FloatTolerance, "North wall's inner face must sit flush with the visible GameplaySurface's +Z edge, not clamp movement to a smaller area.");
            Assert.AreEqual(surfaceBounds.min.z, south.max.z, FloatTolerance, "South wall's inner face must sit flush with the visible GameplaySurface's -Z edge.");
            Assert.AreEqual(surfaceBounds.max.x, east.min.x, FloatTolerance, "East wall's inner face must sit flush with the visible GameplaySurface's +X edge.");
            Assert.AreEqual(surfaceBounds.min.x, west.max.x, FloatTolerance, "West wall's inner face must sit flush with the visible GameplaySurface's -X edge.");
        }

        [UnityTest]
        public IEnumerator ArenaScene_LancerAndMiniBoss_CarryConfiguredCharacterPresentations()
        {
            // Task A3 regression: Lancer (Wave 2) and MiniBoss must also
            // carry the animated CharacterPresentation pipeline, not just
            // the player and Pursuer.
            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;

            var director = Object.FindFirstObjectByType<ArenaRunDirector>();
            Assert.IsNotNull(director);

            float timeout = Time.realtimeSinceStartup + 20f;
            while (director.Stage != ArenaRunStage.Wave2 && Time.realtimeSinceStartup < timeout)
            {
                DefeatAllActiveEnemies();
                AutoPickBlessingIfOffered();
                yield return null;
            }

            Assert.AreEqual(ArenaRunStage.Wave2, director.Stage, "Must reach Wave 2, where the Lancer spawns.");
            yield return null;

            GameObject lancer = GameObject.Find("Lancer");
            Assert.IsNotNull(lancer, "Wave 2 must spawn the authored Lancer prefab.");
            var lancerPresentation = lancer.GetComponentInChildren<CharacterPresentation>();
            Assert.IsNotNull(lancerPresentation, "Lancer must carry an animated CharacterPresentation (Task A3).");
            Assert.IsTrue(lancerPresentation.IsConfigured);

            timeout = Time.realtimeSinceStartup + 20f;
            while (director.Stage != ArenaRunStage.Boss && director.Stage != ArenaRunStage.Victory && Time.realtimeSinceStartup < timeout)
            {
                DefeatAllActiveEnemies();
                AutoPickBlessingIfOffered();
                yield return null;
            }

            Assert.AreEqual(ArenaRunStage.Boss, director.Stage, "Must reach the Boss stage.");
            yield return null;

            GameObject boss = GameObject.Find("MiniBoss");
            Assert.IsNotNull(boss, "The Boss stage must spawn the authored MiniBoss prefab.");
            var bossPresentation = boss.GetComponentInChildren<CharacterPresentation>();
            Assert.IsNotNull(bossPresentation, "MiniBoss must carry an animated CharacterPresentation (Task A3).");
            Assert.IsTrue(bossPresentation.IsConfigured);
        }

        [UnityTest]
        public IEnumerator ArenaScene_FullRunToVictory_ReachesResultAndRetryResetsRun()
        {
            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;

            var director = Object.FindFirstObjectByType<ArenaRunDirector>();
            Assert.IsNotNull(director);

            // Controlled test hook: defeat every enemy and auto-pick the first
            // blessing offered at each gate, driving the run to Victory without
            // waiting the real ~4-6 minute length.
            float timeout = Time.realtimeSinceStartup + 20f;
            while (director.Stage != ArenaRunStage.Victory && director.Stage != ArenaRunStage.Defeat && Time.realtimeSinceStartup < timeout)
            {
                DefeatAllActiveEnemies();
                AutoPickBlessingIfOffered();
                yield return null;
            }

            Assert.AreEqual(ArenaRunStage.Victory, director.Stage, "A controlled full run (auto-defeat + auto-blessing-pick) must reach Victory.");
            Assert.Greater(director.KillCount, 0);

            director.RestartRun();
            yield return null;

            Assert.AreEqual(ArenaRunStage.Wave1, director.Stage, "Retry must reset the run back to Wave 1.");
            Assert.AreEqual(0, director.Blessings.StackCount(BlessingId.ThunderSword), "Retry must reset blessing stacks.");
            Assert.AreEqual(0, director.KillCount, "Retry must reset the kill count.");
        }

        static void DefeatAllActiveEnemies()
        {
            foreach (Combatant combatant in Object.FindObjectsByType<Combatant>(FindObjectsSortMode.None))
            {
                if (combatant.gameObject.name == "Player" || combatant.IsDefeated)
                {
                    continue;
                }

                combatant.TakeHit(new HitInfo(999, DamageElement.Lightning, Vector3.zero));
            }
        }

        static void AutoPickBlessingIfOffered()
        {
            var blessingHud = Object.FindFirstObjectByType<BlessingChoiceHud>();
            if (blessingHud == null || !blessingHud.IsVisible)
            {
                return;
            }

            typeof(BlessingChoiceHud)
                .GetMethod("Choose", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(blessingHud, new object[] { BlessingId.ThunderSword });
        }
    }
}
