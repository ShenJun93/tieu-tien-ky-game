using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Temporary production-direction mobile affordance for a brand-new
    /// player: a subtle left-side move hint, a clearly-visible right-side
    /// "[⚔] LÔI KIẾM" attack hint, and a few seconds of instructional text
    /// that fades out. Purely visual - it never reads or changes touch
    /// input; TouchInputReader's left-half/right-half zones are unaffected.
    /// Not a tutorial framework: time-based only, shown once per run/restart.
    /// </summary>
    public sealed class OnboardingHud : MonoBehaviour
    {
        const float VisibleSeconds = 3.5f;
        const float FadeSeconds = 1f;
        const float TotalSeconds = VisibleSeconds + FadeSeconds;

        float timer;

        /// <summary>Restarts the onboarding display. Called by ArenaRunDirector at the start of every run (including restart).</summary>
        public void Show()
        {
            timer = TotalSeconds;
        }

        void Update()
        {
            if (timer > 0f)
            {
                timer = Mathf.Max(0f, timer - Time.deltaTime);
            }
        }

        void OnGUI()
        {
            if (timer <= 0f)
            {
                return;
            }

            float alpha = timer > FadeSeconds ? 1f : timer / FadeSeconds;

            DrawMoveHint(alpha);
            DrawAttackHint(alpha);
            DrawInstructionText(alpha);
        }

        static void DrawMoveHint(float alpha)
        {
            float size = Screen.height * 0.16f;
            var rect = new Rect(Screen.width * 0.08f, Screen.height * 0.62f, size, size);

            Color previous = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.35f * alpha);
            GUI.Box(rect, string.Empty);
            GUI.color = new Color(1f, 1f, 1f, 0.6f * alpha);
            GUI.Box(new Rect(rect.x + size * 0.32f, rect.y + size * 0.32f, size * 0.36f, size * 0.36f), string.Empty);
            GUI.color = previous;
        }

        static void DrawAttackHint(float alpha)
        {
            float width = Screen.width * 0.18f;
            float height = Screen.height * 0.13f;
            var rect = new Rect(Screen.width * 0.86f - width, Screen.height * 0.6f, width, height);

            var style = new GUIStyle(GUI.skin.box)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.035f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 0.9f, 0.4f, alpha) }
            };

            Color previous = GUI.color;
            GUI.color = new Color(1f, 0.9f, 0.4f, 0.5f * alpha);
            GUI.Box(rect, string.Empty);
            GUI.color = previous;
            GUI.Label(rect, "[⚔]\nLÔI KIẾM", style);
        }

        static void DrawInstructionText(float alpha)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.045f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 1f, 1f, alpha) }
            };

            GUI.Label(new Rect(0f, Screen.height * 0.8f, Screen.width, Screen.height * 0.06f), "Kéo bên trái để di chuyển", style);
            GUI.Label(new Rect(0f, Screen.height * 0.86f, Screen.width, Screen.height * 0.06f), "Chạm Lôi Kiếm để tấn công", style);
        }
    }
}
