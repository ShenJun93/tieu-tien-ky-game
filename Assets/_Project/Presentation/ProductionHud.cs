using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Production mobile HUD for Arena_VerticalSlice_01: left movement
    /// affordance, right skill buttons (Lôi Trảm/Phong Bộ/Hộ Thể) with
    /// cooldown fill, HP/objective/blessing/kill/time readout, boss HP bar,
    /// pause (Resume/Restart/Menu), and a richer Victory/Defeat result panel
    /// with a build summary and Retry/Menu. Canvas/uGUI (Task A5) - no
    /// longer depends on debug-style OnGUI. UI never owns cooldown/combat
    /// state - every button here only calls into
    /// PlayerSkillController/ArenaRunDirector.
    /// </summary>
    public sealed class ProductionHud : MonoBehaviour, IBossArrivalCueDisplay
    {
        [SerializeField] string mainMenuSceneName = "MainMenu";

        const float BossArrivalCueSeconds = 1.6f;

        static readonly ArenaRunStage[] CombatStagesWithEnemyCount =
        {
            ArenaRunStage.Wave1, ArenaRunStage.Wave2, ArenaRunStage.EliteWave
        };

        static readonly Color PanelColor = new Color(0.05f, 0.05f, 0.07f, 0.85f);
        static readonly Color ButtonColor = new Color(0.9f, 0.9f, 0.92f, 1f);
        static readonly Color ButtonDisabledColor = new Color(0.4f, 0.4f, 0.42f, 0.8f);
        static readonly Color CooldownOverlayColor = new Color(0f, 0f, 0f, 0.55f);

        ArenaRunDirector director;
        Combatant playerCombatant;
        PlayerSkillController skillController;
        TieuTienKy.Input.TouchInputReader inputReader;

        float bossArrivalCueTimer;
        bool paused;

        // Top readout.
        Text hpText;
        Text stageText;
        Text blessingText;
        Text killsText;
        Text timeText;

        // Movement affordance.
        RectTransform moveBase;
        RectTransform moveKnob;

        // Skill buttons: index 0=Lôi Trảm, 1=Phong Bộ, 2=Hộ Thể.
        readonly Button[] skillButtons = new Button[3];
        readonly GameObject[] skillCooldownOverlays = new GameObject[3];

        // Pause.
        Button pauseButton;
        GameObject pausePanel;

        // Boss.
        GameObject bossPanel;
        Image bossHpFill;
        Text bossHpText;
        Text bossArrivalText;

        // Result.
        GameObject resultPanel;
        Text resultTitleText;
        Text resultSummaryText;

        public void Initialize(ArenaRunDirector runDirector, Combatant player, PlayerSkillController skills, TieuTienKy.Input.TouchInputReader touchReader)
        {
            director = runDirector;
            playerCombatant = player;
            skillController = skills;
            inputReader = touchReader;
        }

        public void ShowBossArrivalCue() => bossArrivalCueTimer = BossArrivalCueSeconds;

        void Awake()
        {
            Build();
        }

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

        /// <summary>Visual-only left-side circle tracking TouchInputReader.MoveInput - addresses the recorded P0A gap of no on-screen joystick affordance.</summary>
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
            var parts = new List<string>(3);
            AppendIfAcquired(parts, "Lôi Kiếm", director.Blessings.StackCount(BlessingId.ThunderSword));
            AppendIfAcquired(parts, "Phong Hành", director.Blessings.StackCount(BlessingId.WindStride));
            AppendIfAcquired(parts, "Hộ Thể", director.Blessings.StackCount(BlessingId.BodyWard));
            return string.Join("   ", parts);
        }

        static void AppendIfAcquired(List<string> parts, string displayName, int stacks)
        {
            if (stacks <= 0)
            {
                return;
            }

            parts.Add($"{displayName} {RomanNumeral(stacks)}");
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

        // --------------------------------------------------------------
        // Construction.
        // --------------------------------------------------------------

        void Build()
        {
            Canvas canvas = UiBuilder.CreateCanvas("ProductionHudCanvas", sortOrder: 5);
            Transform root = canvas.transform;

            BuildTopReadout(root);
            BuildMovementAffordance(root);
            BuildBasicAttackHint(root);
            BuildSkillButtons(root);
            BuildPause(root);
            BuildBoss(root);
            BuildResult(root);
        }

        void BuildTopReadout(Transform root)
        {
            hpText = UiBuilder.CreateText(root, "HpText", string.Empty, 34, TextAnchor.UpperLeft, Color.white,
                new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(30f, -30f), new Vector2(500f, 46f));

            stageText = UiBuilder.CreateText(root, "StageText", string.Empty, 34, TextAnchor.UpperLeft, Color.white,
                new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(30f, -80f), new Vector2(600f, 46f));

            blessingText = UiBuilder.CreateText(root, "BlessingText", string.Empty, 26, TextAnchor.UpperLeft, new Color(0.85f, 0.85f, 0.9f),
                new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(30f, -130f), new Vector2(700f, 40f));

            killsText = UiBuilder.CreateText(root, "KillsText", string.Empty, 34, TextAnchor.UpperRight, Color.white,
                new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-30f, -30f), new Vector2(400f, 46f));

            timeText = UiBuilder.CreateText(root, "TimeText", string.Empty, 34, TextAnchor.UpperRight, Color.white,
                new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-30f, -80f), new Vector2(400f, 46f));
        }

        void BuildMovementAffordance(Transform root)
        {
            moveBase = UiBuilder.CreatePanel(root, "MoveBase", new Color(1f, 1f, 1f, 0.25f),
                new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(220f, 220f), new Vector2(220f, 220f)).rectTransform;

            moveKnob = UiBuilder.CreatePanel(moveBase, "MoveKnob", new Color(1f, 1f, 1f, 0.55f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(80f, 80f)).rectTransform;
        }

        /// <summary>Basic Attack has no dedicated button (tap-anywhere-right-half already triggers it via TouchInputReader); this satisfies the HUD's required "Basic" readout without adding a second, conflicting touch target.</summary>
        void BuildBasicAttackHint(Transform root)
        {
            UiBuilder.CreateText(root, "BasicHint", "BASIC: TAP TO ATTACK", 22, TextAnchor.LowerRight, new Color(1f, 1f, 1f, 0.6f),
                new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(-30f, 30f), new Vector2(420f, 34f));
        }

        void BuildSkillButtons(Transform root)
        {
            string[] labels = { "LÔI\nTRẢM", "PHONG\nBỘ", "HỘ\nTHỂ" };
            float size = 190f;
            float gap = 30f;
            float startX = -(size * 3f + gap * 2f) - 40f;

            for (int i = 0; i < 3; i++)
            {
                float x = startX + i * (size + gap) + size * 0.5f;
                Button button = UiBuilder.CreateButton(root, $"SkillButton_{i}", labels[i], 28, ButtonColor,
                    new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(x, size * 0.5f + 60f), new Vector2(size, size), out _);

                int capturedIndex = i;
                button.onClick.AddListener(() => ActivateSkill(capturedIndex));
                skillButtons[i] = button;

                GameObject overlay = UiBuilder.CreatePanel(button.transform, "CooldownOverlay", CooldownOverlayColor,
                    Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero).gameObject;
                ((RectTransform)overlay.transform).offsetMin = Vector2.zero;
                ((RectTransform)overlay.transform).offsetMax = Vector2.zero;
                overlay.SetActive(false);
                skillCooldownOverlays[i] = overlay;
            }
        }

        void ActivateSkill(int index)
        {
            if (skillController == null)
            {
                return;
            }

            switch (index)
            {
                case 0:
                    skillController.TryActivateLoiTram();
                    break;
                case 1:
                    skillController.TryActivatePhongBo();
                    break;
                case 2:
                    skillController.TryActivateHoThe();
                    break;
            }
        }

        void BuildPause(Transform root)
        {
            pauseButton = UiBuilder.CreateButton(root, "PauseButton", "II", 32, ButtonColor,
                new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-100f, -30f), new Vector2(90f, 90f), out _);
            pauseButton.onClick.AddListener(() => SetPaused(true));

            pausePanel = new GameObject("PausePanel", typeof(RectTransform));
            RectTransform panelRect = (RectTransform)pausePanel.transform;
            panelRect.SetParent(root, false);
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(700f, 600f);

            UiBuilder.CreatePanel(pausePanel.transform, "Background", PanelColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            UiBuilder.CreateText(pausePanel.transform, "Title", "PAUSED", 48, TextAnchor.MiddleCenter, Color.white,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -70f), new Vector2(0f, 80f));

            Button resume = UiBuilder.CreateButton(pausePanel.transform, "ResumeButton", "RESUME", 32, ButtonColor,
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -230f), new Vector2(460f, 100f), out _);
            resume.onClick.AddListener(() => SetPaused(false));

            Button restart = UiBuilder.CreateButton(pausePanel.transform, "RestartButton", "RESTART", 32, ButtonColor,
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -350f), new Vector2(460f, 100f), out _);
            restart.onClick.AddListener(() =>
            {
                SetPaused(false);
                director.RestartRun();
            });

            Button exit = UiBuilder.CreateButton(pausePanel.transform, "ExitButton", "EXIT TO MENU", 32, ButtonColor,
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -470f), new Vector2(460f, 100f), out _);
            exit.onClick.AddListener(() =>
            {
                SetPaused(false);
                LoadMainMenu();
            });

            pausePanel.SetActive(false);
        }

        void BuildBoss(Transform root)
        {
            bossPanel = new GameObject("BossPanel", typeof(RectTransform));
            RectTransform bossRect = (RectTransform)bossPanel.transform;
            bossRect.SetParent(root, false);
            bossRect.anchorMin = new Vector2(0.5f, 1f);
            bossRect.anchorMax = new Vector2(0.5f, 1f);
            bossRect.anchoredPosition = new Vector2(0f, -60f);
            bossRect.sizeDelta = new Vector2(900f, 100f);

            UiBuilder.CreateText(bossPanel.transform, "Label", "MINI BOSS", 30, TextAnchor.LowerCenter, Color.white,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, 30f), new Vector2(0f, 40f));

            UiBuilder.CreateFillBar(bossPanel.transform, "HpBar", new Color(0.15f, 0.15f, 0.15f, 0.9f), new Color(0.9f, 0.75f, 0.15f),
                new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero, out bossHpFill);

            bossHpText = UiBuilder.CreateText(bossPanel.transform, "HpText", string.Empty, 26, TextAnchor.MiddleCenter, Color.white,
                Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            bossArrivalText = UiBuilder.CreateText(root, "BossArrivalText", "MINI BOSS", 90, TextAnchor.MiddleCenter, new Color(1f, 0.85f, 0.2f),
                new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, 100f), new Vector2(0f, 140f));

            bossPanel.SetActive(false);
            bossArrivalText.gameObject.SetActive(false);
        }

        void BuildResult(Transform root)
        {
            resultPanel = new GameObject("ResultPanel", typeof(RectTransform));
            RectTransform panelRect = (RectTransform)resultPanel.transform;
            panelRect.SetParent(root, false);
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.anchoredPosition = Vector2.zero;
            panelRect.sizeDelta = new Vector2(1000f, 650f);

            UiBuilder.CreatePanel(resultPanel.transform, "Background", PanelColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            resultTitleText = UiBuilder.CreateText(resultPanel.transform, "Title", string.Empty, 72, TextAnchor.MiddleCenter, Color.yellow,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -110f), new Vector2(0f, 110f));

            resultSummaryText = UiBuilder.CreateText(resultPanel.transform, "Summary", string.Empty, 32, TextAnchor.MiddleCenter, Color.white,
                new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, 40f), new Vector2(0f, 140f));

            Button retry = UiBuilder.CreateButton(resultPanel.transform, "RetryButton", "RETRY", 34, ButtonColor,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(-220f, 110f), new Vector2(380f, 110f), out _);
            retry.onClick.AddListener(() => director.RestartRun());

            Button menu = UiBuilder.CreateButton(resultPanel.transform, "MenuButton", "MENU", 34, ButtonColor,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(220f, 110f), new Vector2(380f, 110f), out _);
            menu.onClick.AddListener(LoadMainMenu);

            resultPanel.SetActive(false);
        }
    }
}
