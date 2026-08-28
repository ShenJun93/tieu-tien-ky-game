using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Touchable Canvas/uGUI Cơ Duyên selection shell bound to an authored
    /// ProductionCombatHudView. Owns no run state - it only reports the
    /// chosen BlessingId back through the callback passed to Show, after a
    /// brief on-screen confirmation so the Human sees "I just became
    /// stronger" without needing source code or numeric knowledge. Not a
    /// production HUD framework.
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

        GameObject canvasRoot;
        GameObject choicePanel;
        GameObject confirmPanel;
        Text confirmTitleText;
        Text confirmFlavorText;

        public bool IsVisible { get; private set; }

        public void Initialize(ProductionCombatHudView view)
        {
            if (view == null || !view.IsComplete)
            {
                throw new InvalidOperationException("BlessingChoiceHud.Initialize requires a fully wired ProductionCombatHudView.");
            }

            canvasRoot = view.BlessingRoot;
            choicePanel = view.BlessingChoicePanel;
            confirmPanel = view.BlessingConfirmPanel;
            confirmTitleText = view.BlessingConfirmTitleText;
            confirmFlavorText = view.BlessingConfirmFlavorText;

            view.BlessingButtons[0].onClick.AddListener(() => Choose(BlessingId.ThunderSword));
            view.BlessingButtons[1].onClick.AddListener(() => Choose(BlessingId.WindStride));
            view.BlessingButtons[2].onClick.AddListener(() => Choose(BlessingId.BodyWard));

            canvasRoot.SetActive(false);
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
