using System.Collections;
using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Skill 2 - Phong Bộ: a short controlled reposition, bounded by the
    /// authoritative arena via PhongBoMotion/ArenaBounds so it can never
    /// leave the arena. Usable against telegraphs (counterplay window).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public sealed class PhongBoSkill : MonoBehaviour
    {
        [SerializeField] float cooldownSeconds = 2.5f;
        [SerializeField] float dashDistanceMeters = 3f;
        [SerializeField] float dashDurationSeconds = 0.15f;

        CharacterController controller;
        Cooldown cooldown;
        ArenaBounds bounds;
        bool boundsConfigured;

        /// <summary>Fires on every successful activation - presentation binds PlayMobility here.</summary>
        public event System.Action Activated;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            cooldown = new Cooldown(cooldownSeconds);
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);

        public void SetArenaBounds(ArenaBounds arenaBounds)
        {
            bounds = arenaBounds;
            boundsConfigured = true;
        }

        /// <summary>Run-blessing-driven cooldown reduction (Phong Hành). Only takes effect immediately if currently ready, so an in-flight cooldown is never disrupted.</summary>
        public void SetCooldownDuration(float seconds, float currentTime)
        {
            float clamped = Mathf.Max(0.1f, seconds);
            if (Mathf.Approximately(clamped, cooldownSeconds))
            {
                return;
            }

            cooldownSeconds = clamped;
            if (cooldown == null || cooldown.IsReady(currentTime))
            {
                cooldown = new Cooldown(cooldownSeconds);
            }
        }

        public bool TryActivate(float currentTime)
        {
            if (cooldown == null || !cooldown.TryUse(currentTime))
            {
                return false;
            }

            Vector3 destination = boundsConfigured
                ? PhongBoMotion.ComputeDestination(transform.position, transform.forward, dashDistanceMeters, bounds)
                : transform.position + transform.forward * dashDistanceMeters;

            StartCoroutine(DashRoutine(destination));
            Activated?.Invoke();
            return true;
        }

        IEnumerator DashRoutine(Vector3 destination)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;
            float duration = Mathf.Max(0.01f, dashDurationSeconds);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 next = Vector3.Lerp(start, destination, Mathf.Clamp01(elapsed / duration));
                Vector3 delta = next - transform.position;
                controller.Move(delta);
                yield return null;
            }
        }
    }
}
