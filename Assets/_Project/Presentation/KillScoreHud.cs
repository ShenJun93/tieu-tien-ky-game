using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Minimal always-on kill counter for the Human playtest - the only
    /// player-facing readability this slice adds. Not a production HUD: one
    /// label, no settings/inventory/progression surface.
    /// </summary>
    public sealed class KillScoreHud : MonoBehaviour
    {
        public int KillCount { get; private set; }

        public void RegisterKill()
        {
            KillCount++;
        }

        void OnGUI()
        {
            int fontSize = Mathf.RoundToInt(Screen.height * 0.045f);
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = fontSize,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };

            float width = Screen.width * 0.4f;
            style.alignment = TextAnchor.UpperRight;
            Rect rect = new Rect(Screen.width - width - 20f, 20f, width, fontSize * 1.5f);
            GUI.Label(rect, $"Kills: {KillCount}", style);
        }
    }
}
