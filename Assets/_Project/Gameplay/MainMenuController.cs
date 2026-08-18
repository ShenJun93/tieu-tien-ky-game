using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.Gameplay
{
    /// <summary>Main Menu: one Start affordance into the production arena. No settings/account framework. Canvas/uGUI (Task A5) - production play no longer depends on debug-style IMGUI.</summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] string arenaSceneName = "Arena_VerticalSlice_01";
        [SerializeField] string title = "TIỂU TIÊN KÝ";

        static readonly Color BackdropColor = new Color(0.08f, 0.09f, 0.11f, 1f);
        static readonly Color ButtonColor = new Color(0.85f, 0.74f, 0.35f, 1f);

        void Awake()
        {
            Canvas canvas = UiBuilder.CreateCanvas("MainMenuCanvas", sortOrder: 0);

            UiBuilder.CreatePanel(canvas.transform, "Backdrop", BackdropColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            UiBuilder.CreateText(canvas.transform, "Title", title, 96, TextAnchor.MiddleCenter, Color.white,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -260f), new Vector2(0f, 160f));

            var startButton = UiBuilder.CreateButton(canvas.transform, "StartButton", "START", 48, ButtonColor,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(420f, 150f), out _);
            startButton.onClick.AddListener(() => SceneManager.LoadScene(arenaSceneName));
        }
    }
}
