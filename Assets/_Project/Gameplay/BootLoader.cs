using UnityEngine;
using UnityEngine.SceneManagement;

namespace TieuTienKy.Gameplay
{
    /// <summary>Boot scene entry point: loads Main Menu next frame. No splash/loading-screen framework - this is the smallest real Boot step the product flow needs.</summary>
    public sealed class BootLoader : MonoBehaviour
    {
        [SerializeField] string mainMenuSceneName = "MainMenu";

        void Start()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
