using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Temporary always-on run HUD: player HP, wave/stage, kills, elapsed
    /// time, and a Victory/Defeat + RESTART panel when the run ends. Reads
    /// run state only - the only mutation it ever performs is calling
    /// ArenaRunDirector.RestartRun() from the RESTART button. Not a
    /// production HUD framework.
    /// </summary>
    public sealed class RunHud : MonoBehaviour
    {
        ArenaRunDirector director;
        Combatant playerCombatant;

        public void Initialize(ArenaRunDirector runDirector, Combatant player)
        {
            director = runDirector;
            playerCombatant = player;
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
            string stageLine = $"Stage: {StageLabel(director.Stage)}";
            string killLine = $"Kills: {director.KillCount}";
            string timeLine = FormatElapsed(director.ElapsedSeconds);

            GUI.Label(new Rect(20f, 20f, Screen.width * 0.5f, fontSize * 1.4f), hpLine, style);
            GUI.Label(new Rect(20f, 20f + fontSize * 1.4f, Screen.width * 0.5f, fontSize * 1.4f), stageLine, style);

            var rightStyle = new GUIStyle(style) { alignment = TextAnchor.UpperRight };
            float rightWidth = Screen.width * 0.4f;
            GUI.Label(new Rect(Screen.width - rightWidth - 20f, 20f, rightWidth, fontSize * 1.4f), killLine, rightStyle);
            GUI.Label(new Rect(Screen.width - rightWidth - 20f, 20f + fontSize * 1.4f, rightWidth, fontSize * 1.4f), timeLine, rightStyle);

            if (director.Stage == ArenaRunStage.Victory || director.Stage == ArenaRunStage.Defeat)
            {
                DrawEndPanel(director.Stage == ArenaRunStage.Victory);
            }
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
            ArenaRunStage.EliteWave => "Elite Wave",
            ArenaRunStage.Blessing3 => "Cơ Duyên",
            ArenaRunStage.Boss => "Boss",
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
