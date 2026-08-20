using UnityEngine;

namespace TieuTienKy.Gameplay
{
    [RequireComponent(typeof(LoiTramSkill))]
    [RequireComponent(typeof(PhongBoSkill))]
    [RequireComponent(typeof(HoTheSkill))]
    public sealed class PlayerSkillController : MonoBehaviour
    {
        CharacterPresentation presentation;
        readonly WindWardComboState windWardCombo = new WindWardComboState();

        public LoiTramSkill LoiTram { get; private set; }
        public PhongBoSkill PhongBo { get; private set; }
        public HoTheSkill HoThe { get; private set; }
        public ProductProofRunStyle CurrentRunStyle { get; private set; }
        public bool GaleCounterPrimed => windWardCombo.IsPrimed;

        void Awake()
        {
            LoiTram = GetComponent<LoiTramSkill>();
            PhongBo = GetComponent<PhongBoSkill>();
            HoThe = GetComponent<HoTheSkill>();

            LoiTram.Activated += () => presentation?.PlayCast();
            PhongBo.Activated += () => presentation?.PlayMobility();
            HoThe.Activated += () => presentation?.PlayCast();
            HoThe.BlockedHit += HandleWardBlock;

            LoiTram.RunTuningChanged += RefreshRunStyleFromTuning;
            PhongBo.RunTuningChanged += RefreshRunStyleFromTuning;
            HoThe.RunTuningChanged += RefreshRunStyleFromTuning;
        }

        public void SetPresentation(CharacterPresentation characterPresentation) => presentation = characterPresentation;

        /// <summary>Explicit test/composition seam; normal solo runtime refreshes from the existing blessing-driven skill tuning setters.</summary>
        public void ConfigureRunStyle(ProductProofRunStyle style)
        {
            CurrentRunStyle = style;
            windWardCombo.Configure(style);
            LoiTram.SetStormControl(style);
        }

        public bool TryActivateLoiTram() => LoiTram.TryActivate(Time.time);

        public bool TryActivatePhongBo()
        {
            float currentTime = Time.time;
            if (!PhongBo.IsReady(currentTime))
            {
                return false;
            }

            if (windWardCombo.TryConsume(out GaleCounterSpec counter))
            {
                PhongBo.PrimeGaleCounter(counter);
            }

            return PhongBo.TryActivate(currentTime);
        }

        public bool TryActivateHoThe() => HoThe.TryActivate(Time.time);

        void RefreshRunStyleFromTuning()
        {
            ConfigureRunStyle(ProductProofRunStyle.FromInvestments(
                LoiTram.ThunderInvestmentActive,
                PhongBo.WindInvestmentActive,
                HoThe.WardInvestmentActive));
        }

        void HandleWardBlock()
        {
            windWardCombo.RecordWardBlock();
        }
    }
}
