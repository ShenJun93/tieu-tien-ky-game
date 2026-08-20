using System.Collections;
using System.Collections.Generic;
using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PhongBoSkill : MonoBehaviour
    {
        [SerializeField] float cooldownSeconds = 2.5f;
        [SerializeField] float dashDistanceMeters = 3f;
        [SerializeField] float dashDurationSeconds = 0.15f;
        [SerializeField] Color windTrailColor = new Color(0.55f, 0.9f, 0.75f, 1f);
        [SerializeField] Color galeCounterBurstColor = new Color(0.75f, 1f, 0.85f, 1f);
        [SerializeField] LayerMask galeCounterHittableLayers = ~0;
        [SerializeField] float galeCounterHitStopSeconds = 0.1f;
        [SerializeField] float galeCounterHitStopTimeScale = 0.04f;
        [SerializeField] float galeCounterBurstLifetimeSeconds = 0.4f;
        [SerializeField] float galeCounterCameraImpulse = 0.26f;

        CharacterController controller;
        Combatant selfCombatant;
        Cooldown cooldown;
        ArenaBounds bounds;
        bool boundsConfigured;
        float baseCooldownSeconds;
        bool galeCounterPrimed;
        GaleCounterSpec galeCounterSpec;

        public event System.Action Activated;
        public event System.Action RunTuningChanged;
        public event System.Action GaleCounterTriggered;

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            selfCombatant = GetComponent<Combatant>();
            baseCooldownSeconds = cooldownSeconds;
            cooldown = new Cooldown(cooldownSeconds);
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public float BaseCooldownSeconds => baseCooldownSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);
        public bool WindInvestmentActive => cooldownSeconds < baseCooldownSeconds - 0.001f;

        public void SetArenaBounds(ArenaBounds arenaBounds)
        {
            bounds = arenaBounds;
            boundsConfigured = true;
        }

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

            RunTuningChanged?.Invoke();
        }

        public void PrimeGaleCounter(GaleCounterSpec spec)
        {
            galeCounterSpec = spec;
            galeCounterPrimed = true;
        }

        public bool TryActivate(float currentTime)
        {
            if (cooldown == null || !cooldown.TryUse(currentTime))
            {
                return false;
            }

            bool useGaleCounter = galeCounterPrimed;
            GaleCounterSpec counter = galeCounterSpec;
            galeCounterPrimed = false;

            float distance = dashDistanceMeters * (useGaleCounter ? counter.DashDistanceMultiplier : 1f);
            Vector3 destination = boundsConfigured
                ? PhongBoMotion.ComputeDestination(transform.position, transform.forward, distance, bounds)
                : transform.position + transform.forward * distance;

            PrimitiveBurstVFX.SpawnAt(transform.position, 0.7f, 0.25f, windTrailColor);
            CombatAudio.Play("PhongBoMove", transform.position);

            StartCoroutine(DashRoutine(destination, useGaleCounter, counter));
            Activated?.Invoke();
            return true;
        }

        IEnumerator DashRoutine(Vector3 destination, bool useGaleCounter, GaleCounterSpec counter)
        {
            Vector3 start = transform.position;
            float elapsed = 0f;
            float duration = Mathf.Max(0.01f, dashDurationSeconds);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 next = Vector3.Lerp(start, destination, Mathf.Clamp01(elapsed / duration));
                controller.Move(next - transform.position);
                yield return null;
            }

            if (useGaleCounter)
            {
                ApplyGaleCounterPulse(counter);
            }
        }

        void ApplyGaleCounterPulse(GaleCounterSpec counter)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, counter.PushRadius, galeCounterHittableLayers);
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
                target.TakeHit(new HitInfo(0, DamageElement.Physical, direction * counter.PushImpulse));
            }

            StartCoroutine(HitStop.Routine(galeCounterHitStopSeconds, galeCounterHitStopTimeScale));
            PrimitiveBurstVFX.SpawnAt(transform.position, counter.PushRadius, galeCounterBurstLifetimeSeconds, galeCounterBurstColor);
            CombatAudio.Play("PhongBoMove", transform.position, volume: 1.1f, pitch: 1.25f);
            Camera.main?.GetComponent<PlayerFollowCamera>()?.ApplyImpulse(galeCounterCameraImpulse);

            GaleCounterTriggered?.Invoke();
        }
    }
}
