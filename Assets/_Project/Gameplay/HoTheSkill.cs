using System.Collections.Generic;
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

        [Header("Phan Chan (Perfect Ho The)")]
        [SerializeField] float perfectWindowSeconds = 0.12f;
        [SerializeField] float reflectStaggerRadius = 2.4f;
        [SerializeField] float reflectStaggerImpulseMagnitude = 15f;
        [SerializeField] LayerMask reflectStaggerHittableLayers = ~0;
        [SerializeField] float phanChanHitStopSeconds = 0.11f;
        [SerializeField] float phanChanHitStopTimeScale = 0.03f;
        [SerializeField] float phanChanBurstLifetimeSeconds = 0.4f;
        [SerializeField] Color phanChanBurstColor = new Color(1f, 0.9f, 0.4f, 1f);
        [SerializeField] float phanChanCameraImpulse = 0.32f;

        Combatant selfCombatant;
        Cooldown cooldown;
        HoTheWindow activeWindow;
        bool windowOpen;
        float baseWindowDurationSeconds;

        public event System.Action Activated;
        public event System.Action WindowClosed;
        public event System.Action BlockedHit;
        public event System.Action RunTuningChanged;
        public event System.Action PhanChanTriggered;

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
            if (!windowOpen || !activeWindow.IsActive(Time.time))
            {
                return;
            }

            float hitTime = Time.time;
            BlockedHit?.Invoke();

            if (IsPerfectTiming(activeWindow.ActivatedAtTime, perfectWindowSeconds, hitTime))
            {
                TriggerPhanChan();
            }
        }

        const float PerfectTimingBoundaryEpsilonSeconds = 0.0001f;

        /// <summary>Pure boundary check for Phan Chan's perfect-timing sub-window: the first `perfectWindowSeconds` of the active Ho The block window. Same inclusive-start/exclusive-end style as HoTheWindow.IsActive. The elapsed-time epsilon guards the exclusive edge against float rounding noise (e.g. extended-precision intermediates) that can otherwise make a hit landing exactly at the boundary compare as still inside the window.</summary>
        public static bool IsPerfectTiming(float windowActivatedAtTime, float perfectWindowSeconds, float hitTime)
        {
            float clampedPerfectWindow = Mathf.Max(0f, perfectWindowSeconds);
            float elapsed = hitTime - windowActivatedAtTime;
            return elapsed >= 0f && elapsed < clampedPerfectWindow - PerfectTimingBoundaryEpsilonSeconds;
        }

        /// <summary>Perfect-timed block reflects a radial stagger around the player, reusing the exact OverlapSphere + zero-damage-knockback pattern already proven by Loi Tram's Storm Control pulse. Reuses the existing knockback pipeline only - EnemyCombatController already pauses its attack cycle while knocked back, so this interrupts a locked telegraph without touching any enemy AI file.</summary>
        void TriggerPhanChan()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, reflectStaggerRadius, reflectStaggerHittableLayers);
            var affected = new HashSet<Combatant>();

            foreach (Collider hitCollider in hits)
            {
                Combatant target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target == selfCombatant || target.IsDefeated || !affected.Add(target))
                {
                    continue;
                }

                Vector3 direction = target.transform.position - transform.position;
                direction.y = 0f;
                direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.forward;
                target.TakeHit(new HitInfo(0, DamageElement.Physical, direction * reflectStaggerImpulseMagnitude));
            }

            StartCoroutine(HitStop.Routine(phanChanHitStopSeconds, phanChanHitStopTimeScale));
            PrimitiveBurstVFX.SpawnAt(transform.position, reflectStaggerRadius, phanChanBurstLifetimeSeconds, phanChanBurstColor);
            CombatAudio.Play("HoTheActivate", transform.position, volume: 1.1f, pitch: 0.8f);
            Camera.main?.GetComponent<PlayerFollowCamera>()?.ApplyImpulse(phanChanCameraImpulse);

            PhanChanTriggered?.Invoke();
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
