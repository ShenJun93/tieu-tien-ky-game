using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.Gameplay
{
    /// <summary>Main Menu: one Start affordance into the production arena. No settings/account framework.</summary>
    public sealed class MainMenuController : MonoBehaviour
    {
        [SerializeField] string arenaSceneName = "Arena_VerticalSlice_01";
        [SerializeField] string title = "TIỂU TIÊN KÝ";

        void OnGUI()
        {
            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.09f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(0f, Screen.height * 0.28f, Screen.width, Screen.height * 0.16f), title, titleStyle);

            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.07f),
                fontStyle = FontStyle.Bold
            };
            float buttonWidth = Screen.width * 0.32f;
            float buttonHeight = Screen.height * 0.14f;
            var buttonRect = new Rect((Screen.width - buttonWidth) * 0.5f, Screen.height * 0.56f, buttonWidth, buttonHeight);

            if (GUI.Button(buttonRect, "START", buttonStyle))
            {
                SceneManager.LoadScene(arenaSceneName);
            }
        }
    }
}
