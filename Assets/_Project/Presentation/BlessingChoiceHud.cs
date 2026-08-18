using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Temporary touchable IMGUI Cơ Duyên selection shell. Owns no run
    /// state - it only reports the chosen BlessingId back through the
    /// callback passed to Show, after a brief on-screen confirmation so the
    /// Human sees "I just became stronger" without needing source code or
    /// numeric knowledge. Not a production HUD framework.
    /// </summary>
    public sealed class BlessingChoiceHud : MonoBehaviour
    {
        [SerializeField] float confirmationSeconds = 1.0f;

        static readonly (string Title, string Flavor)[] ConfirmationText =
        {
            ("LÔI KIẾM", "Kiếm lôi được cường hóa"),
            ("PHONG HÀNH", "Thân pháp nhanh nhẹn hơn"),
            ("HỘ THỂ", "Hộ thể vững chắc hơn")
        };

        System.Action<BlessingId> onChosen;
        BlessingId? confirmingId;

        public bool IsVisible { get; private set; }

        public void Show(System.Action<BlessingId> callback)
        {
            onChosen = callback;
            confirmingId = null;
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
            onChosen = null;
            confirmingId = null;
            StopAllCoroutines();
        }

        void OnGUI()
        {
            if (!IsVisible)
            {
                return;
            }

            GUI.Box(new Rect(0f, 0f, Screen.width, Screen.height), string.Empty);

            if (confirmingId.HasValue)
            {
                DrawConfirmation(confirmingId.Value);
                return;
            }

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

        void DrawConfirmation(BlessingId id)
        {
            (string title, string flavor) = ConfirmationText[(int)id];

            var titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.09f),
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = new Color(1f, 0.85f, 0.3f) }
            };
            GUI.Label(new Rect(0f, Screen.height * 0.38f, Screen.width, Screen.height * 0.14f), title, titleStyle);

            var flavorStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = Mathf.RoundToInt(Screen.height * 0.045f),
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
            GUI.Label(new Rect(0f, Screen.height * 0.52f, Screen.width, Screen.height * 0.08f), flavor, flavorStyle);
        }

        /// <summary>
        /// Shows a brief confirmation (title + flavor line) on the chosen
        /// blessing, using unscaled real time since ArenaRunDirector pauses
        /// Time.timeScale for the whole blessing gate, then hides and
        /// invokes the callback so the run resumes.
        /// </summary>
        void Choose(BlessingId id)
        {
            confirmingId = id;
            CombatAudio.Play("UIConfirm", transform.position);
            StartCoroutine(ConfirmThenInvoke(id));
        }

        IEnumerator ConfirmThenInvoke(BlessingId id)
        {
            yield return new WaitForSecondsRealtime(confirmationSeconds);

            System.Action<BlessingId> callback = onChosen;
            Hide();
            callback?.Invoke(id);
        }
    }
}
