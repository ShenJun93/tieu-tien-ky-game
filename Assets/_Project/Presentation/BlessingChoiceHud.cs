using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Touchable Canvas/uGUI (Task A5) Cơ Duyên selection shell. Owns no run
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

        static readonly Color BackdropColor = new Color(0f, 0f, 0f, 0.75f);
        static readonly Color ButtonColor = new Color(0.9f, 0.9f, 0.92f, 1f);

        System.Action<BlessingId> onChosen;
        BlessingId? confirmingId;

        GameObject canvasRoot;
        GameObject choicePanel;
        GameObject confirmPanel;
        Text confirmTitleText;
        Text confirmFlavorText;

        public bool IsVisible { get; private set; }

        void Awake()
        {
            Build();
        }

        public void Show(System.Action<BlessingId> callback)
        {
            onChosen = callback;
            confirmingId = null;
            IsVisible = true;
            canvasRoot.SetActive(true);
            choicePanel.SetActive(true);
            confirmPanel.SetActive(false);
        }

        public void Hide()
        {
            IsVisible = false;
            onChosen = null;
            confirmingId = null;
            StopAllCoroutines();
            if (canvasRoot != null)
            {
                canvasRoot.SetActive(false);
            }
        }

        void Build()
        {
            Canvas canvas = UiBuilder.CreateCanvas("BlessingChoiceCanvas", sortOrder: 10);
            canvasRoot = canvas.gameObject;

            UiBuilder.CreatePanel(canvas.transform, "Backdrop", BackdropColor, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            UiBuilder.CreateText(canvas.transform, "Title", "CHỌN CƠ DUYÊN", 56, TextAnchor.MiddleCenter, Color.white,
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -140f), new Vector2(0f, 100f));

            choicePanel = new GameObject("ChoicePanel", typeof(RectTransform));
            RectTransform choiceRect = (RectTransform)choicePanel.transform;
            choiceRect.SetParent(canvas.transform, false);
            choiceRect.anchorMin = new Vector2(0.5f, 0.5f);
            choiceRect.anchorMax = new Vector2(0.5f, 0.5f);
            choiceRect.anchoredPosition = Vector2.zero;
            choiceRect.sizeDelta = new Vector2(1500f, 560f);

            BuildChoiceButton(choicePanel.transform, -520f, "LÔI KIẾM\n\nStronger Water x Lightning launch", () => Choose(BlessingId.ThunderSword));
            BuildChoiceButton(choicePanel.transform, 0f, "PHONG HÀNH\n\nFaster movement, shorter recovery", () => Choose(BlessingId.WindStride));
            BuildChoiceButton(choicePanel.transform, 520f, "HỘ THỂ\n\nMore max health", () => Choose(BlessingId.BodyWard));

            confirmPanel = new GameObject("ConfirmPanel", typeof(RectTransform));
            RectTransform confirmRect = (RectTransform)confirmPanel.transform;
            confirmRect.SetParent(canvas.transform, false);
            confirmRect.anchorMin = new Vector2(0.5f, 0.5f);
            confirmRect.anchorMax = new Vector2(0.5f, 0.5f);
            confirmRect.anchoredPosition = Vector2.zero;
            confirmRect.sizeDelta = new Vector2(1200f, 300f);

            confirmTitleText = UiBuilder.CreateText(confirmPanel.transform, "ConfirmTitle", string.Empty, 88, TextAnchor.MiddleCenter, new Color(1f, 0.85f, 0.3f),
                new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0f, -80f), new Vector2(0f, 140f));

            confirmFlavorText = UiBuilder.CreateText(confirmPanel.transform, "ConfirmFlavor", string.Empty, 40, TextAnchor.MiddleCenter, Color.white,
                new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0f, 90f), new Vector2(0f, 80f));

            canvasRoot.SetActive(false);
        }

        void BuildChoiceButton(Transform parent, float x, string label, UnityEngine.Events.UnityAction onClick)
        {
            Button button = UiBuilder.CreateButton(parent, "ChoiceButton", label, 32, ButtonColor,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(x, 0f), new Vector2(440f, 560f), out Text labelText);
            labelText.color = Color.black;
            button.onClick.AddListener(onClick);
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

            (string title, string flavor) = ConfirmationText[(int)id];
            confirmTitleText.text = title;
            confirmFlavorText.text = flavor;
            choicePanel.SetActive(false);
            confirmPanel.SetActive(true);

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
