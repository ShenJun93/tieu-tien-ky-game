using System.Collections.Generic;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Temporary always-on run HUD: player HP, objective (stage + remaining
    /// enemies), current Cơ Duyên build, kills, elapsed time, a boss HP bar +
    /// arrival cue during the Boss stage, and a Victory/Defeat + RESTART
    /// panel when the run ends. Reads run state only - the only mutation it
    /// ever performs is calling ArenaRunDirector.RestartRun() from the
    /// RESTART button. Not a production HUD framework.
    /// </summary>
    public sealed class RunHud : MonoBehaviour
    {
        const float BossArrivalCueSeconds = 1.6f;

        static readonly ArenaRunStage[] CombatStagesWithEnemyCount =
        {
            ArenaRunStage.Wave1, ArenaRunStage.Wave2, ArenaRunStage.EliteWave
        };

        ArenaRunDirector director;
        Combatant playerCombatant;
        float bossArrivalCueTimer;

        public void Initialize(ArenaRunDirector runDirector, Combatant player)
        {
            director = runDirector;
            playerCombatant = player;
        }

        /// <summary>Triggers the brief boss arrival flash. Called by ArenaRunDirector at the exact moment the boss spawns.</summary>
        public void ShowBossArrivalCue()
        {
            bossArrivalCueTimer = BossArrivalCueSeconds;
        }

        void Update()
        {
            if (bossArrivalCueTimer > 0f)
            {
                bossArrivalCueTimer = Mathf.Max(0f, bossArrivalCueTimer - Time.deltaTime);
            }
        }

        void OnGUI()
        {
            if (director == null || playerCombatant == null)
            {
                return;
            }

            int fontSize = Mathf.RoundToInt(Screen.height * 0.04f);
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };

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
            float rightWidth = Screen.width * 0.4f;
            GUI.Label(new Rect(Screen.width - rightWidth - 20f, 20f, rightWidth, fontSize * 1.4f), killLine, rightStyle);
            GUI.Label(new Rect(Screen.width - rightWidth - 20f, 20f + fontSize * 1.4f, rightWidth, fontSize * 1.4f), timeLine, rightStyle);

            if (director.Stage == ArenaRunStage.Boss && director.CurrentBoss != null && !director.CurrentBoss.IsDefeated)
            {
                DrawBossHealthBar(director.CurrentBoss);
            }

            if (bossArrivalCueTimer > 0f)
            {
                DrawBossArrivalCue();
            }

            if (director.Stage == ArenaRunStage.Victory || director.Stage == ArenaRunStage.Defeat)
            {
                DrawEndPanel(director.Stage == ArenaRunStage.Victory);
            }
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

            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.035f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerCenter,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(x, y - Screen.height * 0.045f, barWidth, Screen.height * 0.04f), "MINI BOSS", labelStyle);

            GUI.Box(new Rect(x, y, barWidth, barHeight), string.Empty);

            float fraction = Mathf.Clamp01(boss.HealthNormalized);
            Color previousColor = GUI.color;
            GUI.color = Color.Lerp(new Color(0.55f, 0.1f, 0.1f), new Color(0.9f, 0.75f, 0.15f), fraction);
            GUI.Box(new Rect(x + 3f, y + 3f, (barWidth - 6f) * fraction, barHeight - 6f), string.Empty);
            GUI.color = previousColor;

            var hpStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.03f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(x, y, barWidth, barHeight), $"{boss.CurrentHealth} / {boss.MaxHealth}", hpStyle);
        }

        void DrawBossArrivalCue()
        {
            float alpha = Mathf.Clamp01(bossArrivalCueTimer / BossArrivalCueSeconds);
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.1f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 0.85f, 0.2f, alpha) }
            };
            GUI.Label(new Rect(0f, Screen.height * 0.3f, Screen.width, Screen.height * 0.15f), "MINI BOSS", style);
        }

        void DrawEndPanel(bool victory)
        {
            float panelWidth = Screen.width * 0.5f;
            float panelHeight = Screen.height * 0.4f;
            var panelRect = new Rect((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.5f, panelWidth, panelHeight);

            GUI.Box(panelRect, string.Empty);

            var messageStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.08f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = victory ? Color.yellow : Color.red }
            };
            GUI.Label(new Rect(panelRect.x, panelRect.y + panelHeight * 0.15f, panelWidth, panelHeight * 0.35f), victory ? "VICTORY" : "DEFEAT", messageStyle);

            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.05f),
                fontStyle = FontStyle.Bold
            };
            var buttonRect = new Rect(panelRect.x + panelWidth * 0.25f, panelRect.y + panelHeight * 0.6f, panelWidth * 0.5f, panelHeight * 0.25f);
            if (GUI.Button(buttonRect, "RESTART", buttonStyle))
            {
                director.RestartRun();
            }
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
