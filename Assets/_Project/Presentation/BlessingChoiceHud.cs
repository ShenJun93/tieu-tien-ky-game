using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Temporary touchable IMGUI Cơ Duyên selection shell. Owns no run
    /// state - it only reports the chosen BlessingId back through the
    /// callback passed to Show. Not a production HUD framework.
    /// </summary>
    public sealed class BlessingChoiceHud : MonoBehaviour
    {
        System.Action<BlessingId> onChosen;

        public bool IsVisible { get; private set; }

        public void Show(System.Action<BlessingId> callback)
        {
            onChosen = callback;
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
            onChosen = null;
        }

        void OnGUI()
        {
            if (!IsVisible)
            {
                return;
            }

            GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty);

            float width = Screen.width * 0.24f;
            float height = Screen.height * 0.55f;
            float gap = Screen.width * 0.03f;
            float totalWidth = width * 3f + gap * 2f;
            float startX = (Screen.width - totalWidth) * 0.5f;
            float y = (Screen.height - height) * 0.5f;

            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.06f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperCenter,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(0f, y - Screen.height * 0.09f, Screen.width, Screen.height * 0.08f), "CHỌN CƠ DUYÊN", titleStyle);

            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.032f),
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };

            if (GUI.Button(new Rect(startX, y, width, height), "LÔI KIẾM\n\nStronger Water x Lightning launch", buttonStyle))
            {
                Choose(BlessingId.ThunderSword);
            }

            if (GUI.Button(new Rect(startX + width + gap, y, width, height), "PHONG HÀNH\n\nFaster movement, shorter recovery", buttonStyle))
            {
                Choose(BlessingId.WindStride);
            }

            if (GUI.Button(new Rect(startX + (width + gap) * 2f, y, width, height), "HỘ THỂ\n\nMore max health", buttonStyle))
            {
                Choose(BlessingId.BodyWard);
            }
        }

        void Choose(BlessingId id)
        {
            System.Action<BlessingId> callback = onChosen;
            Hide();
            callback?.Invoke(id);
        }
    }
}
