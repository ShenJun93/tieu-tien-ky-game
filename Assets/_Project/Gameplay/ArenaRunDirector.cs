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

        const int PursuerMaxHealth = 2;
        const int LancerMaxHealth = 3;
        const int EliteMaxHealth = 4;
        const int BossPlaceholderMaxHealth = 6;

        Transform playerRoot;
        Combatant playerCombatant;
        PlayerController playerController;
        BasicAttack playerAttack;
        Vector3 playerSpawnPosition;
        int playerBaseMaxHealth;

        BlessingChoiceHud blessingHud;
        RunHud runHud;

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

        public void Initialize(
            Transform playerRootTransform,
            Combatant playerCombatantRef,
            PlayerController playerControllerRef,
            BasicAttack playerAttackRef,
            int baseMaxHealth,
            BlessingChoiceHud blessingChoiceHud,
            RunHud hud)
        {
            playerRoot = playerRootTransform;
            playerCombatant = playerCombatantRef;
            playerController = playerControllerRef;
            playerAttack = playerAttackRef;
            playerSpawnPosition = playerRootTransform.position;
            playerBaseMaxHealth = baseMaxHealth;
            blessingHud = blessingChoiceHud;
            runHud = hud;

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

            playerCombatant.ResetCombatant(playerSpawnPosition);
            playerCombatant.ConfigureMaxHealth(playerBaseMaxHealth);
            playerController.SetRunMoveSpeedMultiplier(1f);
            playerAttack.SetRunModifiers(1f, RunBlessingState.BaseConductiveMultiplier);

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

            yield return WaitForEnemiesCleared();
            if (defeated)
            {
                yield break;
            }

            yield return SettleAndAdvanceAfterCombat();
        }

        /// <summary>
        /// Placeholder boss stage kept minimal and reusable-foundation-only
        /// (same Combatant/enemy spawn path) so the run remains completable
        /// end to end. Replaced by a real MiniBossController in the mini-boss
        /// task.
        /// </summary>
        IEnumerator RunBossStage()
        {
            SpawnEnemy(EnemyCombatProfile.Pursuer(chaseSpeedMultiplier: 0.9f), BossColor, BossPlaceholderMaxHealth, SpawnOffset(0f, 6f));

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
        }

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
            progression.MarkDefeat();
        }

        Vector3 SpawnOffset(float x, float z) => new Vector3(playerSpawnPosition.x + x, playerSpawnPosition.y, playerSpawnPosition.z + z);

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

        void HandleEnemyDefeated(Combatant combatant, GameObject enemyObject)
        {
            activeEnemies.Remove(combatant);
            activeEnemyObjects.Remove(enemyObject);
            killCount++;

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
        }
    }
}
