using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Production mobile HUD for Arena_VerticalSlice_01: left movement
    /// affordance, right skill buttons (Lôi Trảm/Phong Bộ/Hộ Thể) with
    /// cooldown fill, HP/objective/blessing/kill/time readout, boss HP bar,
    /// pause (Resume/Restart/Menu), and a richer Victory/Defeat result panel
    /// with a build summary and Retry/Menu. IMGUI-based, same rendering
    /// approach as RunHud/BlessingChoiceHud (which remain the P0A_Greybox
    /// sandbox's HUD) - extended rather than replaced with a new UI
    /// subsystem, since OnGUI already reaches real touch input on-device.
    /// UI never owns cooldown/combat state - every button here only calls
    /// into PlayerSkillController/ArenaRunDirector.
    /// </summary>
    public sealed class ProductionHud : MonoBehaviour, IBossArrivalCueDisplay
    {
        [SerializeField] string mainMenuSceneName = "MainMenu";

        const float BossArrivalCueSeconds = 1.6f;

        static readonly ArenaRunStage[] CombatStagesWithEnemyCount =
        {
            ArenaRunStage.Wave1, ArenaRunStage.Wave2, ArenaRunStage.EliteWave
        };

        ArenaRunDirector director;
        Combatant playerCombatant;
        PlayerSkillController skillController;
        TieuTienKy.Input.TouchInputReader inputReader;

        float bossArrivalCueTimer;
        bool paused;

        public void Initialize(ArenaRunDirector runDirector, Combatant player, PlayerSkillController skills, TieuTienKy.Input.TouchInputReader touchReader)
        {
            director = runDirector;
            playerCombatant = player;
            skillController = skills;
            inputReader = touchReader;
        }

        public void ShowBossArrivalCue() => bossArrivalCueTimer = BossArrivalCueSeconds;

        void Update()
        {
            if (bossArrivalCueTimer > 0f)
            {
                bossArrivalCueTimer = Mathf.Max(0f, bossArrivalCueTimer - Time.unscaledDeltaTime);
            }
        }

        void OnGUI()
        {
            if (director == null || playerCombatant == null)
            {
                return;
            }

            bool runEnded = director.Stage == ArenaRunStage.Victory || director.Stage == ArenaRunStage.Defeat;

            DrawTopReadout();
            DrawMovementAffordance();

            if (!runEnded)
            {
                DrawSkillButtons();
            }

            if (director.Stage == ArenaRunStage.Boss && director.CurrentBoss != null && !director.CurrentBoss.IsDefeated)
            {
                DrawBossHealthBar(director.CurrentBoss);
            }

            if (bossArrivalCueTimer > 0f)
            {
                DrawBossArrivalCue();
            }

            if (!runEnded)
            {
                DrawPauseButton();
            }

            if (paused && !runEnded)
            {
                DrawPausePanel();
            }

            if (runEnded)
            {
                DrawResultPanel(director.Stage == ArenaRunStage.Victory);
            }
        }

        int FontSize => Mathf.RoundToInt(Screen.height * 0.04f);

        void DrawTopReadout()
        {
            int fontSize = FontSize;
            var style = new GUIStyle(GUI.skin.label) { fontSize = fontSize, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };

            string hpLine = $"HP {playerCombatant.CurrentHealth}/{playerCombatant.MaxHealth}";
            string stageLine = ObjectiveLine();
            string killLine = $"Kills: {director.KillCount}";
            string timeLine = FormatElapsed(director.ElapsedSeconds);

            GUI.Label(new Rect(20f, 20f, Screen.width * 0.5f, fontSize * 1.4f), hpLine, style);
            GUI.Label(new Rect(20f, 20f + fontSize * 1.4f, Screen.width * 0.5f, fontSize * 1.4f), stageLine, style);

            string blessingLine = BuildBlessingLine();
            if (!string.IsNullOrEmpty(blessingLine))
            {
                var blessingStyle = new GUIStyle(style) { fontSize = Mathf.RoundToInt(fontSize * 0.75f) };
                GUI.Label(new Rect(20f, 20f + fontSize * 2.8f, Screen.width * 0.6f, fontSize), blessingLine, blessingStyle);
            }

            var rightStyle = new GUIStyle(style) { alignment = TextAnchor.UpperRight };
            float rightWidth = Screen.width * 0.3f;
            GUI.Label(new Rect(Screen.width - rightWidth - 90f, 20f, rightWidth, fontSize * 1.4f), killLine, rightStyle);
            GUI.Label(new Rect(Screen.width - rightWidth - 90f, 20f + fontSize * 1.4f, rightWidth, fontSize * 1.4f), timeLine, rightStyle);
        }

        /// <summary>Visual-only left-side circle tracking TouchInputReader.MoveInput - addresses the recorded P0A gap of no on-screen joystick affordance.</summary>
        void DrawMovementAffordance()
        {
            if (inputReader == null)
            {
                return;
            }

            float baseRadius = Screen.height * 0.09f;
            Vector2 center = new Vector2(Screen.width * 0.16f, Screen.height * 0.78f);

            Color previous = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.25f);
            GUI.DrawTexture(new Rect(center.x - baseRadius, center.y - baseRadius, baseRadius * 2f, baseRadius * 2f), Texture2D.whiteTexture);

            Vector2 knobOffset = inputReader.MoveInput * baseRadius * 0.6f;
            float knobRadius = baseRadius * 0.35f;
            GUI.color = new Color(1f, 1f, 1f, 0.55f);
            GUI.DrawTexture(new Rect(center.x + knobOffset.x - knobRadius, center.y - knobOffset.y - knobRadius, knobRadius * 2f, knobRadius * 2f), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        void DrawSkillButtons()
        {
            if (skillController == null)
            {
                return;
            }

            float size = Screen.height * 0.14f;
            float gap = Screen.width * 0.02f;
            float y = Screen.height - size - Screen.height * 0.05f;
            float startX = Screen.width - (size * 3f + gap * 2f) - Screen.width * 0.04f;

            DrawSkillButton(new Rect(startX, y, size, size), "LÔI\nTRẢM", skillController.LoiTram.IsReady(Time.time), CooldownFraction(skillController.LoiTram.CooldownDuration, skillController.LoiTram.IsReady), () => skillController.TryActivateLoiTram());
            DrawSkillButton(new Rect(startX + size + gap, y, size, size), "PHONG\nBỘ", skillController.PhongBo.IsReady(Time.time), CooldownFraction(skillController.PhongBo.CooldownDuration, skillController.PhongBo.IsReady), () => skillController.TryActivatePhongBo());
            DrawSkillButton(new Rect(startX + (size + gap) * 2f, y, size, size), "HỘ\nTHỂ", skillController.HoThe.IsReady(Time.time), CooldownFraction(skillController.HoThe.CooldownDuration, skillController.HoThe.IsReady), () => skillController.TryActivateHoThe());
        }

        static float CooldownFraction(float duration, System.Func<float, bool> isReady)
        {
            if (duration <= 0f || isReady(Time.time))
            {
                return 0f;
            }

            return 1f;
        }

        void DrawSkillButton(Rect rect, string label, bool ready, float cooldownFraction01, System.Action activate)
        {
            Color previous = GUI.color;
            GUI.color = ready ? Color.white : new Color(0.6f, 0.6f, 0.6f, 0.7f);

            var style = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(rect.height * 0.18f), fontStyle = FontStyle.Bold, wordWrap = true, alignment = TextAnchor.MiddleCenter };
            if (GUI.Button(rect, label, style) && ready)
            {
                activate?.Invoke();
            }

            GUI.color = previous;

            if (!ready && cooldownFraction01 > 0f)
            {
                var overlayStyle = new GUIStyle(GUI.skin.box);
                GUI.color = new Color(0f, 0f, 0f, 0.35f);
                GUI.Box(new Rect(rect.x, rect.y, rect.width, rect.height * cooldownFraction01), string.Empty, overlayStyle);
                GUI.color = previous;
            }
        }

        void DrawPauseButton()
        {
            float size = Screen.height * 0.07f;
            var rect = new Rect(Screen.width - size - 20f, 20f, size, size);
            var style = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(size * 0.4f), fontStyle = FontStyle.Bold };
            if (GUI.Button(rect, "II", style))
            {
                SetPaused(true);
            }
        }

        void DrawPausePanel()
        {
            float panelWidth = Screen.width * 0.45f;
            float panelHeight = Screen.height * 0.45f;
            var panelRect = new Rect((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.5f, panelWidth, panelHeight);

            GUI.Box(panelRect, string.Empty);

            var titleStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(panelHeight * 0.14f), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            GUI.Label(new Rect(panelRect.x, panelRect.y + panelHeight * 0.08f, panelWidth, panelHeight * 0.18f), "PAUSED", titleStyle);

            var buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(panelHeight * 0.09f), fontStyle = FontStyle.Bold };
            float buttonHeight = panelHeight * 0.18f;
            float buttonWidth = panelWidth * 0.7f;
            float buttonX = panelRect.x + (panelWidth - buttonWidth) * 0.5f;

            if (GUI.Button(new Rect(buttonX, panelRect.y + panelHeight * 0.32f, buttonWidth, buttonHeight), "RESUME", buttonStyle))
            {
                SetPaused(false);
            }

            if (GUI.Button(new Rect(buttonX, panelRect.y + panelHeight * 0.54f, buttonWidth, buttonHeight), "RESTART", buttonStyle))
            {
                SetPaused(false);
                director.RestartRun();
            }

            if (GUI.Button(new Rect(buttonX, panelRect.y + panelHeight * 0.76f, buttonWidth, buttonHeight), "EXIT TO MENU", buttonStyle))
            {
                SetPaused(false);
                LoadMainMenu();
            }
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

        void DrawBossHealthBar(Combatant boss)
        {
            float barWidth = Screen.width * 0.42f;
            float barHeight = Screen.height * 0.055f;
            float x = (Screen.width - barWidth) * 0.5f;
            float y = Screen.height * 0.05f;

            var labelStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(Screen.height * 0.035f), fontStyle = FontStyle.Bold, alignment = TextAnchor.LowerCenter, normal = { textColor = Color.white } };
            GUI.Label(new Rect(x, y - Screen.height * 0.045f, barWidth, Screen.height * 0.04f), "MINI BOSS", labelStyle);

            GUI.Box(new Rect(x, y, barWidth, barHeight), string.Empty);

            float fraction = Mathf.Clamp01(boss.HealthNormalized);
            Color previousColor = GUI.color;
            GUI.color = Color.Lerp(new Color(0.55f, 0.1f, 0.1f), new Color(0.9f, 0.75f, 0.15f), fraction);
            GUI.Box(new Rect(x + 3f, y + 3f, (barWidth - 6f) * fraction, barHeight - 6f), string.Empty);
            GUI.color = previousColor;

            var hpStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(Screen.height * 0.03f), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            GUI.Label(new Rect(x, y, barWidth, barHeight), $"{boss.CurrentHealth} / {boss.MaxHealth}", hpStyle);
        }

        void DrawBossArrivalCue()
        {
            float alpha = Mathf.Clamp01(bossArrivalCueTimer / BossArrivalCueSeconds);
            var style = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(Screen.height * 0.1f), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = new Color(1f, 0.85f, 0.2f, alpha) } };
            GUI.Label(new Rect(0f, Screen.height * 0.3f, Screen.width, Screen.height * 0.15f), "MINI BOSS", style);
        }

        void DrawResultPanel(bool victory)
        {
            float panelWidth = Screen.width * 0.55f;
            float panelHeight = Screen.height * 0.55f;
            var panelRect = new Rect((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.5f, panelWidth, panelHeight);

            GUI.Box(panelRect, string.Empty);

            var messageStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(panelHeight * 0.14f), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = victory ? Color.yellow : Color.red } };
            GUI.Label(new Rect(panelRect.x, panelRect.y + panelHeight * 0.06f, panelWidth, panelHeight * 0.2f), victory ? "VICTORY" : "DEFEAT", messageStyle);

            var summaryStyle = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(panelHeight * 0.06f), alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            string summary = $"Time {FormatElapsed(director.ElapsedSeconds)}   Kills {director.KillCount}\n{BuildSummaryLine()}";
            GUI.Label(new Rect(panelRect.x, panelRect.y + panelHeight * 0.3f, panelWidth, panelHeight * 0.2f), summary, summaryStyle);

            var buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = Mathf.RoundToInt(panelHeight * 0.07f), fontStyle = FontStyle.Bold };
            float buttonWidth = panelWidth * 0.38f;
            float buttonHeight = panelHeight * 0.16f;
            float buttonY = panelRect.y + panelHeight * 0.68f;

            if (GUI.Button(new Rect(panelRect.x + panelWidth * 0.08f, buttonY, buttonWidth, buttonHeight), "RETRY", buttonStyle))
            {
                director.RestartRun();
            }

            if (GUI.Button(new Rect(panelRect.x + panelWidth * 0.54f, buttonY, buttonWidth, buttonHeight), "MENU", buttonStyle))
            {
                LoadMainMenu();
            }
        }

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
