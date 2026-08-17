using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Owns only: stage/wave progression, blessing gates, elite/boss
    /// progression, victory/defeat, restart. Live enemy references, run
    /// timer and kills live here too since they are direct consequences of
    /// those responsibilities - this is not a general scene/game manager.
    /// Arena events (Water Shift/Spirit Wind) are a separate
    /// ArenaEventDirector; presentation (RunHud/BlessingChoiceHud) only
    /// displays or collects a choice.
    /// </summary>
    public sealed class ArenaRunDirector : MonoBehaviour
    {
        [SerializeField] float postCombatSettleSeconds = 0.15f;
        [SerializeField] float waveStaggerSeconds = 1.0f;

        static readonly Color PursuerColor = new Color(0.8f, 0.3f, 0.3f);
        static readonly Color LancerColor = new Color(0.55f, 0.25f, 0.65f);
        static readonly Color BossColor = new Color(0.85f, 0.65f, 0.15f);
        static readonly Color BossAccentColor = new Color(1f, 0.9f, 0.4f);

        const int PursuerMaxHealth = 2;
        const int LancerMaxHealth = 3;
        const int EliteMaxHealth = 4;
        const int BossMaxHealth = 18;
        const float BossVisualScale = 1.35f;

        Transform playerRoot;
        Combatant playerCombatant;
        PlayerController playerController;
        BasicAttack playerAttack;
        PlayerBlessingPresentation playerBlessingPresentation;
        Vector3 playerSpawnPosition;
        int playerBaseMaxHealth;
        ArenaBounds arenaBounds;

        BlessingChoiceHud blessingHud;
        RunHud runHud;
        ArenaEventDirector arenaEvents;
        OnboardingHud onboardingHud;

        static readonly Vector3 SpiritWindImpulse = new Vector3(7f, 0f, 0f);

        readonly RunBlessingState blessings = new RunBlessingState();
        readonly ArenaRunProgression progression = new ArenaRunProgression();
        readonly List<Combatant> activeEnemies = new List<Combatant>();
        readonly List<GameObject> activeEnemyObjects = new List<GameObject>();

        int killCount;
        float elapsedSeconds;
        bool defeated;
        bool running;

        public ArenaRunStage Stage => progression.Stage;
        public int KillCount => killCount;
        public float ElapsedSeconds => elapsedSeconds;

        /// <summary>Read-only current Cơ Duyên stacks, for RunHud's compact build readout. HUD reads this; it never mutates it.</summary>
        public RunBlessingState Blessings => blessings;

        /// <summary>Live boss Combatant while Stage == Boss, for RunHud's boss HP bar. Null outside the Boss stage or after the boss is defeated/reset.</summary>
        public Combatant CurrentBoss { get; private set; }

        public int ActiveEnemyCount => activeEnemies.Count;

        public void Initialize(
            Transform playerRootTransform,
            Combatant playerCombatantRef,
            PlayerController playerControllerRef,
            BasicAttack playerAttackRef,
            PlayerBlessingPresentation playerBlessingPresentationRef,
            int baseMaxHealth,
            BlessingChoiceHud blessingChoiceHud,
            RunHud hud,
            ArenaEventDirector arenaEventDirector,
            ArenaBounds arenaUsableBounds,
            OnboardingHud onboardingHudRef)
        {
            playerRoot = playerRootTransform;
            playerCombatant = playerCombatantRef;
            playerController = playerControllerRef;
            playerAttack = playerAttackRef;
            playerBlessingPresentation = playerBlessingPresentationRef;
            playerSpawnPosition = playerRootTransform.position;
            playerBaseMaxHealth = baseMaxHealth;
            blessingHud = blessingChoiceHud;
            runHud = hud;
            arenaEvents = arenaEventDirector;
            arenaBounds = arenaUsableBounds;
            onboardingHud = onboardingHudRef;

            playerCombatant.Defeated += HandlePlayerDefeated;

            StartRun();
        }

        public void RestartRun() => StartRun();

        void StartRun()
        {
            progression.Reset();
            blessings.Reset();
            killCount = 0;
            elapsedSeconds = 0f;
            defeated = false;
            Time.timeScale = 1f;

            ClearActiveEnemies();
            blessingHud.Hide();
            arenaEvents?.ResetEvents();

            playerCombatant.ResetCombatant(playerSpawnPosition);
            playerCombatant.ConfigureMaxHealth(playerBaseMaxHealth);
            playerController.SetRunMoveSpeedMultiplier(1f);
            playerAttack.SetRunModifiers(1f, RunBlessingState.BaseConductiveMultiplier);
            playerBlessingPresentation?.ApplyStacks(0, 0, 0);
            onboardingHud?.Show();

            running = true;
            StopAllCoroutines();
            StartCoroutine(RunLoop());
        }

        void Update()
        {
            if (running && !defeated)
            {
                elapsedSeconds += Time.deltaTime;
            }
        }

        IEnumerator RunLoop()
        {
            while (progression.Stage != ArenaRunStage.Victory && progression.Stage != ArenaRunStage.Defeat)
            {
                switch (progression.Stage)
                {
                    case ArenaRunStage.Wave1:
                        yield return RunWave1();
                        break;
                    case ArenaRunStage.Wave2:
                        yield return RunWave2();
                        break;
                    case ArenaRunStage.EliteWave:
                        yield return RunEliteWave();
                        break;
                    case ArenaRunStage.Boss:
                        yield return RunBossStage();
                        break;
                    default:
                        yield return RunBlessingGate();
                        break;
                }

                if (defeated)
                {
                    yield break;
                }
            }
        }

        IEnumerator RunWave1()
        {
            SpawnEnemy(EnemyCombatProfile.Pursuer(), PursuerColor, PursuerMaxHealth, SpawnOffset(4f, 0f));

            yield return new WaitForSeconds(waveStaggerSeconds);
            if (defeated)
            {
                yield break;
            }

            SpawnEnemy(EnemyCombatProfile.Pursuer(), PursuerColor, PursuerMaxHealth, SpawnOffset(-4f, 1f));

            yield return WaitForEnemiesCleared();
            if (defeated)
            {
                yield break;
            }

            yield return SettleAndAdvanceAfterCombat();
        }

        IEnumerator RunWave2()
        {
            const int startCount = 2;
            SpawnEnemy(EnemyCombatProfile.Pursuer(), PursuerColor, PursuerMaxHealth, SpawnOffset(4f, 2f));
            SpawnEnemy(EnemyCombatProfile.Lancer(), LancerColor, LancerMaxHealth, SpawnOffset(-4f, -2f));

            StartCoroutine(DelayedWaterShift(3.5f));

            while (activeEnemies.Count >= startCount && !defeated)
            {
                yield return null;
            }
            if (defeated)
            {
                yield break;
            }

            SpawnEnemy(EnemyCombatProfile.Pursuer(), PursuerColor, PursuerMaxHealth, SpawnOffset(0f, 5f));

            yield return WaitForEnemiesCleared();
            if (defeated)
            {
                yield break;
            }

            yield return SettleAndAdvanceAfterCombat();
        }

        IEnumerator RunEliteWave()
        {
            SpawnEnemy(EnemyCombatProfile.Pursuer(chaseSpeedMultiplier: 1.15f), PursuerColor, EliteMaxHealth, SpawnOffset(5f, 0f));
            SpawnEnemy(EnemyCombatProfile.Lancer(chaseSpeedMultiplier: 1.1f), LancerColor, EliteMaxHealth, SpawnOffset(-5f, 0f));

            StartCoroutine(DelayedSpiritWind(2f));
            StartCoroutine(DelayedWaterShiftIfWaveStillActive(6f));

            yield return WaitForEnemiesCleared();
            if (defeated)
            {
                yield break;
            }

            yield return SettleAndAdvanceAfterCombat();
        }

        IEnumerator RunBossStage()
        {
            SpawnBoss(SpawnOffset(0f, 6f));
            runHud.ShowBossArrivalCue();

            yield return WaitForEnemiesCleared();
            if (defeated)
            {
                yield break;
            }

            yield return SettleAndAdvanceAfterCombat();
        }

        IEnumerator RunBlessingGate()
        {
            yield return new WaitForSecondsRealtime(postCombatSettleSeconds);
            if (defeated)
            {
                yield break;
            }

            Time.timeScale = 0f;

            BlessingId? chosen = null;
            blessingHud.Show(id => chosen = id);

            while (chosen == null && !defeated)
            {
                yield return null;
            }

            Time.timeScale = 1f;

            if (defeated)
            {
                blessingHud.Hide();
                yield break;
            }

            ApplyBlessing(chosen.Value);
            progression.AdvanceAfterBlessingChoice();
        }

        void ApplyBlessing(BlessingId id)
        {
            blessings.Apply(id);
            RunCombatModifiers modifiers = blessings.CurrentModifiers;

            playerController.SetRunMoveSpeedMultiplier(modifiers.MoveSpeedMultiplier);
            playerAttack.SetRunModifiers(modifiers.AttackRecoveryMultiplier, modifiers.ConductiveMultiplier);
            playerCombatant.SetMaxHealthAndRestore(playerBaseMaxHealth + modifiers.MaxHealthBonus);

            playerBlessingPresentation?.ApplyStacks(
                blessings.StackCount(BlessingId.ThunderSword),
                blessings.StackCount(BlessingId.WindStride),
                blessings.StackCount(BlessingId.BodyWard));
        }

        IEnumerator DelayedWaterShift(float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            if (defeated || arenaEvents == null)
            {
                yield break;
            }

            yield return arenaEvents.PlayWaterShift();
        }

        IEnumerator DelayedWaterShiftIfWaveStillActive(float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            if (defeated || arenaEvents == null || activeEnemies.Count == 0)
            {
                yield break;
            }

            yield return arenaEvents.PlayWaterShift();
        }

        IEnumerator DelayedSpiritWind(float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            if (defeated || arenaEvents == null)
            {
                yield break;
            }

            yield return arenaEvents.PlaySpiritWind(CollectActors(), SpiritWindLane(), SpiritWindImpulse);
        }

        List<Combatant> CollectActors()
        {
            var actors = new List<Combatant>(activeEnemies);
            if (playerCombatant != null)
            {
                actors.Add(playerCombatant);
            }

            return actors;
        }

        Bounds SpiritWindLane() => new Bounds(playerSpawnPosition, new Vector3(3f, 4f, 20f));

        IEnumerator WaitForEnemiesCleared()
        {
            while (activeEnemies.Count > 0 && !defeated)
            {
                yield return null;
            }
        }

        IEnumerator SettleAndAdvanceAfterCombat()
        {
            yield return new WaitForSecondsRealtime(postCombatSettleSeconds);
            if (!defeated)
            {
                progression.AdvanceAfterCombatClear();
            }
        }

        void HandlePlayerDefeated()
        {
            defeated = true;
            Time.timeScale = 1f;
            blessingHud.Hide();
            ClearActiveEnemies();
            arenaEvents?.ResetEvents();
            progression.MarkDefeat();
        }

        /// <summary>
        /// Root-cause fix for the physical-device Boss-stage blocker: offsets
        /// from the player's CURRENT position (not the frozen run-start
        /// anchor), clamped into the arena's usable interior, so a spawn -
        /// most importantly the boss - is always within a bounded, reachable
        /// distance of wherever the player actually is by that point in the
        /// run.
        /// </summary>
        Vector3 SpawnOffset(float x, float z) => ArenaSpawnPlanner.ComputeSpawnPosition(playerRoot.position, x, z, arenaBounds);

        void SpawnEnemy(EnemyCombatProfile profile, Color tint, int maxHealth, Vector3 position)
        {
            var enemy = new GameObject(profile.Archetype == EnemyArchetype.Lancer ? "Lancer" : "Pursuer");
            enemy.transform.position = position;

            var controller = enemy.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f;
            controller.radius = 0.5f;

            enemy.AddComponent<KnockbackReceiver>();
            var combatant = enemy.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(maxHealth);

            var view = enemy.AddComponent<PrimitiveCharacterView>();
            view.Build(tint, tint, armed: false, visualScale: 1f);

            enemy.AddComponent<PrimitiveTelegraphVFX>();

            var combatController = enemy.AddComponent<EnemyCombatController>();
            combatController.Initialize(playerRoot, playerCombatant, profile);

            activeEnemies.Add(combatant);
            activeEnemyObjects.Add(enemy);
            combatant.Defeated += () => HandleEnemyDefeated(combatant, enemy);
        }

        /// <summary>
        /// Same reusable Combatant/root+view spawn path as SpawnEnemy, with
        /// a MiniBossController instead of EnemyCombatController and a
        /// visibly larger/contrasting shell. On defeat this participates in
        /// wave-clear tracking exactly like any other enemy.
        /// </summary>
        void SpawnBoss(Vector3 position)
        {
            var boss = new GameObject("MiniBoss");
            boss.transform.position = position;

            var controller = boss.AddComponent<CharacterController>();
            controller.center = Vector3.zero;
            controller.height = 2f * BossVisualScale;
            controller.radius = 0.5f * BossVisualScale;

            boss.AddComponent<KnockbackReceiver>();
            var combatant = boss.AddComponent<Combatant>();
            combatant.ConfigureMaxHealth(BossMaxHealth);

            var view = boss.AddComponent<PrimitiveCharacterView>();
            view.Build(BossColor, BossAccentColor, armed: true, visualScale: BossVisualScale);

            boss.AddComponent<PrimitiveTelegraphVFX>();

            var bossController = boss.AddComponent<MiniBossController>();
            bossController.Initialize(playerRoot, playerCombatant, arenaBounds);

            activeEnemies.Add(combatant);
            activeEnemyObjects.Add(boss);
            CurrentBoss = combatant;
            combatant.Defeated += () => HandleEnemyDefeated(combatant, boss);
        }

        void HandleEnemyDefeated(Combatant combatant, GameObject enemyObject)
        {
            activeEnemies.Remove(combatant);
            activeEnemyObjects.Remove(enemyObject);
            killCount++;

            if (combatant == CurrentBoss)
            {
                CurrentBoss = null;
            }

            if (enemyObject != null)
            {
                Destroy(enemyObject, 0.5f);
            }
        }

        void ClearActiveEnemies()
        {
            foreach (GameObject enemyObject in activeEnemyObjects)
            {
                if (enemyObject != null)
                {
                    Destroy(enemyObject);
                }
            }

            activeEnemies.Clear();
            activeEnemyObjects.Clear();
            CurrentBoss = null;
        }
    }
}
