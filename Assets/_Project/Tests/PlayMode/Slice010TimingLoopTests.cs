using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
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


        [UnityTest]
        public IEnumerator RepresentativeSkilledRunProxy_UsesSixtyToNinetyGameSeconds()
        {
            yield return SceneManager.LoadSceneAsync("Arena_VerticalSlice_01");
            yield return null;
            yield return null;
            TraceActiveAudioListeners("START");

            ArenaRunDirector director = Object.FindFirstObjectByType<ArenaRunDirector>();
            GameObject playerObject = GameObject.Find("Player");
            Assert.IsNotNull(director);
            Assert.IsNotNull(playerObject);

            Combatant player = playerObject.GetComponent<Combatant>();
            PlayerController playerController = playerObject.GetComponent<PlayerController>();
            Assert.IsNotNull(player);
            Assert.IsNotNull(playerController);
            player.SetDamageMitigation(0f);

            ArenaRunStage lastStage = director.Stage;

            const float TestTimeScale = 10f;
            System.Reflection.FieldInfo settleField = typeof(ArenaRunDirector).GetField(
                "postCombatSettleSeconds",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.IsNotNull(settleField);
            float authoredRealtimeSettleSeconds = (float)settleField.GetValue(director);
            settleField.SetValue(director, authoredRealtimeSettleSeconds / TestTimeScale);

            const float NetLandedHitCadenceSeconds = 1.5f;
            float nextHitAt = director.ElapsedSeconds + NetLandedHitCadenceSeconds;
            float realDeadline = Time.realtimeSinceStartup + 20f;
            int blessingPickIndex = 0;
            bool choiceIssuedForCurrentGate = false;

            while (director.Stage != ArenaRunStage.Victory &&
                   director.Stage != ArenaRunStage.Defeat &&
                   Time.realtimeSinceStartup < realDeadline)
            {
                BlessingChoiceHud blessingHud = Object.FindFirstObjectByType<BlessingChoiceHud>();
                if (blessingHud != null && blessingHud.IsVisible)
                {
                    if (!choiceIssuedForCurrentGate)
                    {
                        BlessingId[] order = { BlessingId.WindStride, BlessingId.ThunderSword, BlessingId.BodyWard };
                        BlessingId choice = order[Mathf.Min(blessingPickIndex, order.Length - 1)];
                        typeof(BlessingChoiceHud)
                            .GetMethod("Choose", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.Invoke(blessingHud, new object[] { choice });
                        blessingPickIndex++;
                        choiceIssuedForCurrentGate = true;
                    }
                }
                else
                {
                    choiceIssuedForCurrentGate = false;
                    Time.timeScale = TestTimeScale;
                }

                if (DriveRepresentativeSkilledCombat(player, playerController, director.ElapsedSeconds, nextHitAt))
                {
                    nextHitAt = director.ElapsedSeconds + NetLandedHitCadenceSeconds;
                }

                if (director.Stage != lastStage)
                {
                    TestContext.WriteLine($"SLICE010_PROXY_STAGE={lastStage}->{director.Stage} AT={director.ElapsedSeconds:F2}");
                    TraceActiveAudioListeners($"STAGE_{director.Stage}");
                    lastStage = director.Stage;
                }

                yield return null;
            }

            string liveActors = string.Join("; ",
                Object.FindObjectsByType<Combatant>(FindObjectsSortMode.None)
                    .Where(c => c != player && !c.IsDefeated)
                    .Select(c => $"{c.name}@{c.transform.position}"));
            TestContext.WriteLine(
                $"SLICE010_PROXY_FINAL stage={director.Stage} elapsed={director.ElapsedSeconds:F2} player={player.transform.position} live={liveActors}");
            Assert.AreEqual(ArenaRunStage.Victory, director.Stage,
                "The representative skilled-run proxy must complete the existing encounter rather than time out or lose.");
            TestContext.WriteLine($"SLICE010_SKILLED_PROXY_SECONDS={director.ElapsedSeconds:F2}");
            Assert.That(director.ElapsedSeconds, Is.InRange(60f, 90f),
                $"Representative encounter should plausibly fit the 60-90s product target; proxy measured {director.ElapsedSeconds:F2}s of game time.");
        }

        [UnityTearDown]
        public IEnumerator CleanupArenaScene()
        {
            Time.timeScale = 1f;
            if (SceneManager.GetActiveScene().name == "Arena_VerticalSlice_01")
            {
                yield return SceneManager.LoadSceneAsync("Boot");
                yield return null;
            }
        }

        static void TraceActiveAudioListeners(string marker)
        {
            AudioListener[] listeners = Resources.FindObjectsOfTypeAll<AudioListener>()
                .Where(listener => listener != null && listener.enabled && listener.gameObject.activeInHierarchy)
                .ToArray();
            string details = string.Join("; ", listeners.Select(listener =>
                $"{listener.gameObject.name}[scene={listener.gameObject.scene.name},hide={listener.gameObject.hideFlags}]"));
            TestContext.WriteLine($"SLICE010_AUDIO_LISTENERS marker={marker} count={listeners.Length} active={details}");
        }

        static bool DriveRepresentativeSkilledCombat(
            Combatant player,
            PlayerController playerController,
            float elapsedSeconds,
            float nextAttackAt)
        {
            Combatant nearest = null;
            float nearestDistance = float.PositiveInfinity;
            foreach (Combatant combatant in Object.FindObjectsByType<Combatant>(FindObjectsSortMode.None))
            {
                if (combatant == player || combatant.IsDefeated)
                {
                    continue;
                }

                float distance = Vector3.Distance(player.transform.position, combatant.transform.position);
                if (distance < nearestDistance)
                {
                    nearest = combatant;
                    nearestDistance = distance;
                }
            }

            if (nearest == null)
            {
                return false;
            }

            Vector3 delta = nearest.transform.position - player.transform.position;
            Vector2 move = new Vector2(delta.x, delta.z);
            if (move.sqrMagnitude > 0.0001f)
            {
                move.Normalize();
            }

            if (nearestDistance > 1.6f)
            {
                playerController.ApplyMove(move, Time.deltaTime);
                return false;
            }

            playerController.ApplyMove(move * 0.01f, Time.deltaTime);
            if (elapsedSeconds < nextAttackAt)
            {
                return false;
            }

            int healthBefore = nearest.CurrentHealth;
            nearest.TakeHit(new HitInfo(1, DamageElement.Lightning, Vector3.zero));
            TestContext.WriteLine(
                $"SLICE010_PROXY_HIT t={elapsedSeconds:F2} target={nearest.name} hp={healthBefore}->{nearest.CurrentHealth} dist={nearestDistance:F2} next={nextAttackAt:F2}");
            return true;
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
