using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    public sealed class ProductionHud : MonoBehaviour, IBossArrivalCueDisplay
    {
        [SerializeField] string mainMenuSceneName = "MainMenu";

        const float BossArrivalCueSeconds = 1.6f;

        static readonly ArenaRunStage[] CombatStagesWithEnemyCount =
        {
            ArenaRunStage.Wave1, ArenaRunStage.Wave2, ArenaRunStage.EliteWave
        };

        static readonly Color ButtonColor = new Color(0.9f, 0.9f, 0.92f, 1f);
        static readonly Color ButtonDisabledColor = new Color(0.4f, 0.4f, 0.42f, 0.8f);

        ArenaRunDirector director;
        Combatant playerCombatant;
        PlayerSkillController skillController;
        TieuTienKy.Input.TouchInputReader inputReader;
        IPlayerActionGateway actionGateway;

        float bossArrivalCueTimer;
        bool paused;
        bool initialized;

        Text hpText;
        Text stageText;
        Text blessingText;
        Text killsText;
        Text timeText;

        RectTransform moveBase;
        RectTransform moveKnob;

        readonly Button[] skillButtons = new Button[3];
        readonly Text[] skillLabels = new Text[3];
        readonly GameObject[] skillCooldownOverlays = new GameObject[3];

        Button pauseButton;
        GameObject pausePanel;

        GameObject bossPanel;
        Image bossHpFill;
        Text bossHpText;
        Text bossArrivalText;

        GameObject resultPanel;
        Text resultTitleText;
        Text resultSummaryText;

        public void Initialize(ArenaRunDirector runDirector, Combatant player, PlayerSkillController skills, TieuTienKy.Input.TouchInputReader touchReader, ProductionCombatHudView view = null)
        {
            if (view == null || !view.IsComplete)
            {
                throw new InvalidOperationException("ProductionHud.Initialize requires a fully wired ProductionCombatHudView.");
            }

            if (initialized)
            {
                throw new InvalidOperationException("ProductionHud.Initialize must not be called more than once.");
            }

            initialized = true;

            director = runDirector;
            playerCombatant = player;
            skillController = skills;
            inputReader = touchReader;

            BindView(view);
        }

        void BindView(ProductionCombatHudView view)
        {
            hpText = view.HpText;
            stageText = view.StageText;
            blessingText = view.BlessingText;
            killsText = view.KillsText;
            timeText = view.TimeText;
            moveBase = view.MoveBase;
            moveKnob = view.MoveKnob;
            pauseButton = view.PauseButton;
            pausePanel = view.PausePanel;
            bossPanel = view.BossPanel;
            bossHpFill = view.BossHpFill;
            bossHpText = view.BossHpText;
            bossArrivalText = view.BossArrivalText;
            resultPanel = view.ResultPanel;
            resultTitleText = view.ResultTitleText;
            resultSummaryText = view.ResultSummaryText;

            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i] = view.SkillButtons[i];
                skillLabels[i] = view.SkillLabels[i];
                skillCooldownOverlays[i] = view.SkillCooldownOverlays[i];
            }

            view.BasicButton.onClick.AddListener(() => actionGateway?.RequestBasicAttack());
            view.SkillButtons[0].onClick.AddListener(() => actionGateway?.RequestLoiTram());
            view.SkillButtons[1].onClick.AddListener(() => actionGateway?.RequestPhongBo());
            view.SkillButtons[2].onClick.AddListener(() => actionGateway?.RequestHoThe());

            view.PauseButton.onClick.AddListener(() => SetPaused(true));
            view.ResumeButton.onClick.AddListener(() => SetPaused(false));
            view.RestartButton.onClick.AddListener(() =>
            {
                SetPaused(false);
                director.RestartRun();
            });
            view.ExitButton.onClick.AddListener(() =>
            {
                SetPaused(false);
                LoadMainMenu();
            });

            view.RetryButton.onClick.AddListener(() => director.RestartRun());
            view.MenuButton.onClick.AddListener(LoadMainMenu);

            bossArrivalText.gameObject.SetActive(false);
            bossPanel.SetActive(false);
            pausePanel.SetActive(false);
            resultPanel.SetActive(false);
        }

        public void ShowBossArrivalCue() => bossArrivalCueTimer = BossArrivalCueSeconds;

        public void SetActionGateway(IPlayerActionGateway gateway) => actionGateway = gateway;

        void Update()
        {
            if (bossArrivalCueTimer > 0f)
            {
                bossArrivalCueTimer = Mathf.Max(0f, bossArrivalCueTimer - Time.unscaledDeltaTime);
            }

            Refresh();
        }

        void Refresh()
        {
            if (director == null || playerCombatant == null)
            {
                return;
            }

            bool runEnded = director.Stage == ArenaRunStage.Victory || director.Stage == ArenaRunStage.Defeat;

            RefreshTopReadout();
            RefreshMovementAffordance();
            RefreshSkillButtons(!runEnded);

            bool bossActive = director.Stage == ArenaRunStage.Boss && director.CurrentBoss != null && !director.CurrentBoss.IsDefeated;
            bossPanel.SetActive(bossActive);
            if (bossActive)
            {
                RefreshBossHealthBar(director.CurrentBoss);
            }

            bossArrivalText.gameObject.SetActive(bossArrivalCueTimer > 0f);
            if (bossArrivalCueTimer > 0f)
            {
                float alpha = Mathf.Clamp01(bossArrivalCueTimer / BossArrivalCueSeconds);
                bossArrivalText.color = new Color(1f, 0.85f, 0.2f, alpha);
            }

            pauseButton.gameObject.SetActive(!runEnded);
            pausePanel.SetActive(paused && !runEnded);
            resultPanel.SetActive(runEnded);
            if (runEnded)
            {
                RefreshResultPanel(director.Stage == ArenaRunStage.Victory);
            }
        }

        void RefreshTopReadout()
        {
            hpText.text = $"HP {playerCombatant.CurrentHealth}/{playerCombatant.MaxHealth}";
            stageText.text = ObjectiveLine();
            killsText.text = $"Kills: {director.KillCount}";
            timeText.text = FormatElapsed(director.ElapsedSeconds);

            string blessingLine = BuildBlessingLine();
            blessingText.gameObject.SetActive(!string.IsNullOrEmpty(blessingLine));
            blessingText.text = blessingLine;
        }

        void RefreshMovementAffordance()
        {
            if (inputReader == null)
            {
                return;
            }

            float baseRadius = moveBase.sizeDelta.x * 0.5f;
            Vector2 knobOffset = inputReader.MoveInput * baseRadius * 0.6f;
            moveKnob.anchoredPosition = knobOffset;
        }

        void RefreshSkillButtons(bool visible)
        {
            for (int i = 0; i < skillButtons.Length; i++)
            {
                skillButtons[i].gameObject.SetActive(visible);
            }

            if (!visible || skillController == null)
            {
                return;
            }

            skillLabels[0].text = "LÔI\nTRẢM";
            skillLabels[1].text = skillController.GaleCounterPrimed ? "PHONG BỘ\nPHẢN KÍCH" : "PHONG\nBỘ";
            skillLabels[2].text = "HỘ\nTHỂ";

            RefreshSkillButton(0, skillController.LoiTram.IsReady(Time.time), skillController.LoiTram.CooldownDuration);
            RefreshSkillButton(1, skillController.PhongBo.IsReady(Time.time), skillController.PhongBo.CooldownDuration);
            RefreshSkillButton(2, skillController.HoThe.IsReady(Time.time), skillController.HoThe.CooldownDuration);
        }

        void RefreshSkillButton(int index, bool ready, float cooldownDuration)
        {
            skillButtons[index].interactable = ready;
            skillButtons[index].targetGraphic.color = ready ? ButtonColor : ButtonDisabledColor;
            skillCooldownOverlays[index].SetActive(!ready && cooldownDuration > 0f);
        }

        void RefreshBossHealthBar(Combatant boss)
        {
            float fraction = Mathf.Clamp01(boss.HealthNormalized);
            bossHpFill.fillAmount = fraction;
            bossHpFill.color = Color.Lerp(new Color(0.55f, 0.1f, 0.1f), new Color(0.9f, 0.75f, 0.15f), fraction);
            bossHpText.text = $"{boss.CurrentHealth} / {boss.MaxHealth}";
        }

        void RefreshResultPanel(bool victory)
        {
            resultTitleText.text = victory ? "VICTORY" : "DEFEAT";
            resultTitleText.color = victory ? Color.yellow : Color.red;
            resultSummaryText.text = $"Time {FormatElapsed(director.ElapsedSeconds)}   Kills {director.KillCount}\n{BuildSummaryLine()}";
        }

        void SetPaused(bool value)
        {
            paused = value;
            Time.timeScale = value ? 0f : 1f;
        }

        void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }

        string ObjectiveLine()
        {
            string label = StageLabel(director.Stage);
            foreach (ArenaRunStage combatStage in CombatStagesWithEnemyCount)
            {
                if (director.Stage == combatStage)
                {
                    return $"{label}   Enemies: {director.ActiveEnemyCount}";
                }
            }

            return label;
        }

        string BuildBlessingLine()
        {
            var parts = new List<string>(5);
            if (skillController != null)
            {
                ProductProofRunStyle style = skillController.CurrentRunStyle;
                if (style.StormControlActive)
                {
                    parts.Add("STORM CONTROL");
                }
                if (style.WindWardActive)
                {
                    parts.Add(skillController.GaleCounterPrimed ? "WIND WARD: COUNTER READY" : "WIND WARD");
                }
            }

            AppendIfAcquired(parts, "Lôi Kiếm", director.Blessings.StackCount(BlessingId.ThunderSword));
            AppendIfAcquired(parts, "Phong Hành", director.Blessings.StackCount(BlessingId.WindStride));
            AppendIfAcquired(parts, "Hộ Thể", director.Blessings.StackCount(BlessingId.BodyWard));
            return string.Join("   ", parts);
        }

        static void AppendIfAcquired(List<string> parts, string displayName, int stacks)
        {
            if (stacks > 0)
            {
                parts.Add($"{displayName} {RomanNumeral(stacks)}");
            }
        }

        static string RomanNumeral(int stacks) => stacks switch
        {
            1 => "I",
            2 => "II",
            3 => "III",
            _ => stacks.ToString()
        };

        string BuildSummaryLine()
        {
            string line = BuildBlessingLine();
            return string.IsNullOrEmpty(line) ? "No Cơ Duyên acquired" : line;
        }

        static string StageLabel(ArenaRunStage stage) => stage switch
        {
            ArenaRunStage.Wave1 => "Wave 1",
            ArenaRunStage.Blessing1 => "Cơ Duyên",
            ArenaRunStage.Wave2 => "Wave 2",
            ArenaRunStage.Blessing2 => "Cơ Duyên",
            ArenaRunStage.EliteWave => "ELITE",
            ArenaRunStage.Blessing3 => "Cơ Duyên",
            ArenaRunStage.Boss => "MINI BOSS",
            ArenaRunStage.Victory => "Victory",
            ArenaRunStage.Defeat => "Defeat",
            _ => stage.ToString()
        };

        static string FormatElapsed(float seconds)
        {
            int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(seconds));
            int minutes = totalSeconds / 60;
            int secs = totalSeconds % 60;
            return $"{minutes:00}:{secs:00}";
        }

    }
}
