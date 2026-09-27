using System.Collections;
using System.Linq;
using NUnit.Framework;
using TieuTienKy.Brawl;
using UnityEngine;
using TieuTienKy.Combat;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TieuTienKy.Brawl.Tests
{
    /// <summary>CORE-1 end to end in the real brawl scene: local command → CombatSim → views, obstacles, roof fade.</summary>
    public class BrawlArenaPlayModeTests
    {
        const string ScenePath = "Assets/_Project/Scenes/Brawl/Arena_Brawl_01.unity";

        IEnumerator LoadArena()
        {
#if UNITY_EDITOR
            // The brawl scene is not in EditorBuildSettings (it ships in its own APK), so load it by path.
            yield return UnityEditor.SceneManagement.EditorSceneManager.LoadSceneAsyncInPlayMode(ScenePath, new LoadSceneParameters(LoadSceneMode.Single));
#endif
            yield return null;
        }

        [UnityTest]
        public IEnumerator StickMovesTheLocalFighterAndItStaysInsideTheArena()
        {
            yield return LoadArena();
            var match = Object.FindFirstObjectByType<BrawlMatch>();
            Assert.IsNotNull(match, "BrawlMatch missing from the brawl scene");
            var hero = match.LocalView.transform;
            Vector3 start = hero.position;

            match.LocalCommandOverride = () => new FighterCommand { Move = new Vector2(1f, 0f) };
            yield return new WaitForSeconds(1f);

            Assert.Greater(hero.position.x - start.x, 3f, "hero should run right for about a second");
            Assert.AreEqual(start.z, hero.position.z, 0.5f);

            yield return new WaitForSeconds(5f);
            var sim = match.Sim;
            Assert.LessOrEqual(sim.Fighters[0].Position.x, sim.Bounds.xMax, "hero must be clamped inside the arena");
        }

        [UnityTest]
        public IEnumerator RosterSpawnsFromDefinitionsAndTheCameraAnchorFollowsTheHero()
        {
            yield return LoadArena();
            var match = Object.FindFirstObjectByType<BrawlMatch>();
            Assert.AreEqual(5, match.Views.Count);
            for (int i = 0; i < match.Views.Count; i++)
            {
                var animator = match.Views[i].GetComponentInChildren<Animator>();
                Assert.IsNotNull(animator.runtimeAnimatorController, $"fighter {i} has no animations");
                Assert.Greater(match.Sim.Fighters[i].Radius, 0f);
            }

            match.LocalCommandOverride = () => new FighterCommand { Move = new Vector2(0f, 1f) };
            yield return new WaitForSeconds(0.5f);
            Assert.Less(Vector3.Distance(match.CameraTarget.position, match.LocalView.transform.position), 0.01f,
                "the camera anchor must sit on the local fighter");
        }

        [UnityTest]
        public IEnumerator TheHeroCannotWalkThroughTheIncenseBurner()
        {
            yield return LoadArena();
            var match = Object.FindFirstObjectByType<BrawlMatch>();
            var hero = match.Sim.Fighters[0];
            Assert.AreEqual(0f, hero.Position.x, 0.01f, "test assumes the hero spawns on the burner's axis");
            Assert.IsNotEmpty(match.Sim.Obstacles, "arena props should bake into sim obstacles");

            match.LocalCommandOverride = () => new FighterCommand { Move = new Vector2(0f, 1f) };
            yield return new WaitForSeconds(3f);

            // The burner is the first obstacle straight ahead (a banner on the back wall shares its x).
            var burner = match.Sim.Obstacles.Where(o => Mathf.Abs(o.Center.x) < 0.2f && o.Center.y > 0f).OrderBy(o => o.Center.y).First();
            Assert.Greater(burner.Radius, 0f, "incense burner obstacle missing");
            Assert.AreEqual(burner.Center.y - burner.Radius - hero.Radius, hero.Position.y, 0.05f, "hero should rest against the burner");
        }

        [UnityTest]
        public IEnumerator TheGateRoofFadesWhenTheHeroWalksBehindIt()
        {
            yield return LoadArena();
            var match = Object.FindFirstObjectByType<BrawlMatch>();
            var roof = Object.FindFirstObjectByType<OccluderFade>();
            Assert.IsNotNull(roof, "the paifang needs an OccluderFade");
            Assert.AreEqual(1f, roof.Alpha, "roof starts opaque");

            // Step right past the burner, then walk up until just behind the gate and stop there.
            float behindGate = roof.transform.position.z + 1.5f;
            var hero = match.Sim.Fighters[0];
            match.LocalCommandOverride = () =>
                hero.Position.x < 2.5f ? new FighterCommand { Move = new Vector2(1f, 0f) }
                : hero.Position.y < behindGate ? new FighterCommand { Move = new Vector2(0f, 1f) }
                : FighterCommand.Idle;
            yield return new WaitForSeconds(4f);

            Assert.Greater(hero.Position.y, behindGate - 0.5f, "hero should have reached the gate");
            Assert.IsTrue(roof.IsOccluding, "roof should be between camera and hero");
            Assert.Less(roof.Alpha, 0.5f, "roof should have faded");

            match.LocalCommandOverride = () => new FighterCommand { Move = new Vector2(0f, -1f) };
            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(1f, roof.Alpha, 1e-3f, "roof returns to opaque once the hero is in front");
        }

#if UNITY_EDITOR
        [Test]
        public void EveryCharacterDefinitionIsUsable()
        {
            var guids = UnityEditor.AssetDatabase.FindAssets("t:" + nameof(CharacterDefinition));
            Assert.IsNotEmpty(guids, "no CharacterDefinition assets found");
            foreach (var guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var def = UnityEditor.AssetDatabase.LoadAssetAtPath<CharacterDefinition>(path);
                CollectionAssert.IsEmpty(def.Validate(), path);
            }
        }
#endif

        [UnityTest]
        public IEnumerator OtherFightersHoldStillWithoutCommands()
        {
            yield return LoadArena();
            var match = Object.FindFirstObjectByType<BrawlMatch>();
            var before = match.Sim.Fighters[2].Position;
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(before, match.Sim.Fighters[2].Position);
        }
    }
}
