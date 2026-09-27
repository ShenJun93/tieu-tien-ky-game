using System.Collections;
using NUnit.Framework;
using TieuTienKy.Brawl;
using UnityEngine;
using TieuTienKy.Combat;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace TieuTienKy.Brawl.Tests
{
    /// <summary>CORE-1a end to end: local command → CombatSim → FighterView in the real brawl scene.</summary>
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
