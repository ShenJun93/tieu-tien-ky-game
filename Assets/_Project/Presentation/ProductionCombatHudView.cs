using UnityEngine;
using UnityEngine.UI;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Serialized reference container for the authored production combat HUD
    /// prefab. Owns no gameplay rules, damage math, or run-state advancement -
    /// it only exposes the authored Canvas/uGUI surfaces so ProductionHud and
    /// BlessingChoiceHud can bind to authored content instead of building UI
    /// at runtime.
    /// </summary>
    public sealed class ProductionCombatHudView : MonoBehaviour
    {
        [SerializeField] Canvas rootCanvas;
        [SerializeField] Text hpText;
        [SerializeField] Text stageText;
        [SerializeField] Text blessingText;
        [SerializeField] Text killsText;
        [SerializeField] Text timeText;
        [SerializeField] RectTransform moveBase;
        [SerializeField] RectTransform moveKnob;
        [SerializeField] Button basicButton;
        [SerializeField] Button[] skillButtons = new Button[3];
        [SerializeField] Text[] skillLabels = new Text[3];
        [SerializeField] GameObject[] skillCooldownOverlays = new GameObject[3];
        [SerializeField] Button pauseButton;
        [SerializeField] Button resumeButton;
        [SerializeField] Button restartButton;
        [SerializeField] Button exitButton;
        [SerializeField] GameObject pausePanel;
        [SerializeField] GameObject bossPanel;
        [SerializeField] GameObject resultPanel;
        [SerializeField] Image bossHpFill;
        [SerializeField] Text bossHpText;
        [SerializeField] Text bossArrivalText;
        [SerializeField] Text resultTitleText;
        [SerializeField] Text resultSummaryText;
        [SerializeField] Button retryButton;
        [SerializeField] Button menuButton;
        [SerializeField] GameObject blessingRoot;
        [SerializeField] GameObject blessingChoicePanel;
        [SerializeField] GameObject blessingConfirmPanel;
        [SerializeField] Button[] blessingButtons = new Button[3];
        [SerializeField] Text blessingConfirmTitleText;
        [SerializeField] Text blessingConfirmFlavorText;

        public Canvas RootCanvas => rootCanvas;
        public Text HpText => hpText;
        public Text StageText => stageText;
        public Text BlessingText => blessingText;
        public Text KillsText => killsText;
        public Text TimeText => timeText;
        public RectTransform MoveBase => moveBase;
        public RectTransform MoveKnob => moveKnob;
        public Button BasicButton => basicButton;
        public Button[] SkillButtons => skillButtons;
        public Text[] SkillLabels => skillLabels;
        public GameObject[] SkillCooldownOverlays => skillCooldownOverlays;
        public Button PauseButton => pauseButton;
        public Button ResumeButton => resumeButton;
        public Button RestartButton => restartButton;
        public Button ExitButton => exitButton;
        public GameObject PausePanel => pausePanel;
        public GameObject BossPanel => bossPanel;
        public GameObject ResultPanel => resultPanel;
        public Image BossHpFill => bossHpFill;
        public Text BossHpText => bossHpText;
        public Text BossArrivalText => bossArrivalText;
        public Text ResultTitleText => resultTitleText;
        public Text ResultSummaryText => resultSummaryText;
        public Button RetryButton => retryButton;
        public Button MenuButton => menuButton;
        public GameObject BlessingRoot => blessingRoot;
        public GameObject BlessingChoicePanel => blessingChoicePanel;
        public GameObject BlessingConfirmPanel => blessingConfirmPanel;
        public Button[] BlessingButtons => blessingButtons;
        public Text BlessingConfirmTitleText => blessingConfirmTitleText;
        public Text BlessingConfirmFlavorText => blessingConfirmFlavorText;

        public bool IsComplete =>
            rootCanvas != null
            && hpText != null
            && stageText != null
            && blessingText != null
            && killsText != null
            && timeText != null
            && moveBase != null
            && moveKnob != null
            && basicButton != null
            && HasExactlyThreeAssigned(skillButtons)
            && HasExactlyThreeAssigned(skillLabels)
            && HasExactlyThreeAssigned(skillCooldownOverlays)
            && pauseButton != null
            && resumeButton != null
            && restartButton != null
            && exitButton != null
            && pausePanel != null
            && bossPanel != null
            && resultPanel != null
            && bossHpFill != null
            && bossHpText != null
            && bossArrivalText != null
            && resultTitleText != null
            && resultSummaryText != null
            && retryButton != null
            && menuButton != null
            && blessingRoot != null
            && blessingChoicePanel != null
            && blessingConfirmPanel != null
            && HasExactlyThreeAssigned(blessingButtons)
            && blessingConfirmTitleText != null
            && blessingConfirmFlavorText != null;

        static bool HasExactlyThreeAssigned<T>(T[] items) where T : Object
        {
            if (items == null || items.Length != 3)
            {
                return false;
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
