using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// The single intent -> gameplay boundary for the three player skills
    /// (Lôi Trảm / Phong Bộ / Hộ Thể). UI (HUD skill buttons) call
    /// TryActivateXxx and never touch cooldown/combat state directly; Basic
    /// Attack's own input path (TouchInputReader -> BasicAttack) is
    /// unchanged by this controller.
    /// </summary>
    [RequireComponent(typeof(LoiTramSkill))]
    [RequireComponent(typeof(PhongBoSkill))]
    [RequireComponent(typeof(HoTheSkill))]
    public sealed class PlayerSkillController : MonoBehaviour
    {
        CharacterPresentation presentation;

        public LoiTramSkill LoiTram { get; private set; }
        public PhongBoSkill PhongBo { get; private set; }
        public HoTheSkill HoThe { get; private set; }

        void Awake()
        {
            LoiTram = GetComponent<LoiTramSkill>();
            PhongBo = GetComponent<PhongBoSkill>();
            HoThe = GetComponent<HoTheSkill>();

            LoiTram.Activated += () => presentation?.PlayCast();
            PhongBo.Activated += () => presentation?.PlayMobility();
            HoThe.Activated += () => presentation?.PlayCast();
        }

        public void SetPresentation(CharacterPresentation characterPresentation) => presentation = characterPresentation;

        public bool TryActivateLoiTram() => LoiTram.TryActivate(Time.time);
        public bool TryActivatePhongBo() => PhongBo.TryActivate(Time.time);
        public bool TryActivateHoThe() => HoThe.TryActivate(Time.time);
    }
}
