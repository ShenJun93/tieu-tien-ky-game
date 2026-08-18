using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Skill 3 - Hộ Thể: an active bounded defensive window that fully
    /// blocks incoming damage (not knockback) while open, with a visible
    /// ward burst as its distinct active-hit feedback. A timing decision,
    /// not a passive damage-reduction stat.
    /// </summary>
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

        /// <summary>Fires on every successful activation - presentation binds PlayCast/SetBlessingVisual-style ward feedback here.</summary>
        public event System.Action Activated;
        public event System.Action WindowClosed;

        void Awake()
        {
            selfCombatant = GetComponent<Combatant>();
            baseWindowDurationSeconds = windowDurationSeconds;
            cooldown = new Cooldown(cooldownSeconds);
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public float BaseWindowDurationSeconds => baseWindowDurationSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);
        public bool IsWindowActive(float currentTime) => windowOpen && activeWindow.IsActive(currentTime);

        /// <summary>Run-blessing-driven window extension (Hộ Thể stacks). Callers compute the target duration from BaseWindowDurationSeconds so repeated blessing picks never compound. Takes effect on the next activation.</summary>
        public void SetWindowDuration(float seconds) => windowDurationSeconds = Mathf.Max(0.05f, seconds);

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
