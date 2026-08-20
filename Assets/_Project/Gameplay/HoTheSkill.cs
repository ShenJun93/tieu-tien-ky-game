using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    [RequireComponent(typeof(Combatant))]
    public sealed class HoTheSkill : MonoBehaviour
    {
        [SerializeField] float cooldownSeconds = 6f;
        [SerializeField] float windowDurationSeconds = 0.45f;
        [SerializeField] Color wardColor = new Color(0.6f, 1f, 0.7f, 1f);
        [SerializeField] float wardPeakRadius = 1.1f;

        Combatant selfCombatant;
        Cooldown cooldown;
        HoTheWindow activeWindow;
        bool windowOpen;
        float baseWindowDurationSeconds;

        public event System.Action Activated;
        public event System.Action WindowClosed;
        public event System.Action BlockedHit;
        public event System.Action RunTuningChanged;

        void Awake()
        {
            selfCombatant = GetComponent<Combatant>();
            baseWindowDurationSeconds = windowDurationSeconds;
            cooldown = new Cooldown(cooldownSeconds);
            selfCombatant.Damaged += HandleCombatantDamaged;
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public float BaseWindowDurationSeconds => baseWindowDurationSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);
        public bool IsWindowActive(float currentTime) => windowOpen && activeWindow.IsActive(currentTime);
        public bool WardInvestmentActive => windowDurationSeconds > baseWindowDurationSeconds + 0.001f;

        public void SetWindowDuration(float seconds)
        {
            float clamped = Mathf.Max(0.05f, seconds);
            if (Mathf.Approximately(clamped, windowDurationSeconds))
            {
                return;
            }

            windowDurationSeconds = clamped;
            RunTuningChanged?.Invoke();
        }

        public bool TryActivate(float currentTime)
        {
            if (cooldown == null || !cooldown.TryUse(currentTime))
            {
                return false;
            }

            activeWindow = new HoTheWindow(currentTime, windowDurationSeconds);
            windowOpen = true;
            selfCombatant.SetDamageMitigation(0f);
            PrimitiveBurstVFX.SpawnAt(transform.position, wardPeakRadius, windowDurationSeconds, wardColor);
            CombatAudio.Play("HoTheActivate", transform.position);

            Activated?.Invoke();
            return true;
        }

        void HandleCombatantDamaged(int currentHealth, int maxHealth)
        {
            if (windowOpen && activeWindow.IsActive(Time.time))
            {
                BlockedHit?.Invoke();
            }
        }

        void Update()
        {
            if (windowOpen && !activeWindow.IsActive(Time.time))
            {
                windowOpen = false;
                selfCombatant.SetDamageMitigation(1f);
                WindowClosed?.Invoke();
            }
        }
    }
}
