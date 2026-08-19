using System.Collections.Generic;
using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    [RequireComponent(typeof(Combatant))]
    public sealed class LoiTramSkill : MonoBehaviour
    {
        [SerializeField] float cooldownSeconds = 3.5f;
        [SerializeField] float rangeMeters = 2.6f;
        [SerializeField] float radiusMeters = 1.1f;
        [SerializeField] int damage = 2;
        [SerializeField] float knockbackImpulseMagnitude = 9f;
        [SerializeField] float conductiveKnockbackMultiplier = 2.5f;
        [SerializeField] LayerMask hittableLayers = ~0;
        [SerializeField] float hitStopSeconds = 0.07f;
        [SerializeField] float hitStopTimeScale = 0.05f;

        static readonly Color ImpactFlashColor = new Color(0.75f, 0.55f, 1f, 1f);
        static readonly Color StormPulseColor = new Color(0.35f, 0.8f, 1f, 1f);

        Combatant selfCombatant;
        Cooldown cooldown;
        ProductProofRunStyle runStyle;

        public event System.Action Activated;
        public event System.Action HitLanded;
        public event System.Action RunTuningChanged;

        void Awake()
        {
            selfCombatant = GetComponent<Combatant>();
            cooldown = new Cooldown(cooldownSeconds);
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);
        public bool ThunderInvestmentActive => conductiveKnockbackMultiplier > RunBlessingState.BaseConductiveMultiplier + 0.001f;

        public void SetConductiveMultiplier(float multiplier)
        {
            conductiveKnockbackMultiplier = multiplier;
            RunTuningChanged?.Invoke();
        }

        public void SetStormControl(ProductProofRunStyle style) => runStyle = style;

        public bool TryActivate(float currentTime)
        {
            if (cooldown == null || !cooldown.TryUse(currentTime))
            {
                return false;
            }

            CombatAudio.Play("LoiTramCast", transform.position);

            Vector3 origin = transform.position + transform.forward * (rangeMeters * 0.5f);
            Collider[] hits = Physics.OverlapSphere(origin, radiusMeters, hittableLayers);
            bool landedAnyHit = false;
            bool stormTriggered = false;
            Vector3 stormOrigin = Vector3.zero;
            var directTargets = new HashSet<Combatant>();

            foreach (Collider hitCollider in hits)
            {
                Combatant target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target == selfCombatant || target.IsDefeated || !directTargets.Add(target))
                {
                    continue;
                }

                Vector3 knockDirection = target.transform.position - transform.position;
                knockDirection.y = 0f;
                knockDirection = knockDirection.sqrMagnitude > 0.0001f ? knockDirection.normalized : transform.forward;

                target.TakeHit(new HitInfo(damage, DamageElement.Lightning, knockDirection * knockbackImpulseMagnitude, conductiveKnockbackMultiplier));
                landedAnyHit = true;

                if (!stormTriggered && runStyle.StormControlActive && target.LastReactionTriggered)
                {
                    stormTriggered = true;
                    stormOrigin = target.transform.position;
                }
            }

            if (stormTriggered)
            {
                ApplyStormControlPulse(stormOrigin, directTargets);
            }

            if (landedAnyHit)
            {
                StartCoroutine(HitStop.Routine(hitStopSeconds, hitStopTimeScale));
                PrimitiveBurstVFX.SpawnAt(origin, radiusMeters, 0.3f, ImpactFlashColor);
                CombatAudio.Play("LoiTramImpact", origin);
                HitLanded?.Invoke();
            }

            Activated?.Invoke();
            return true;
        }

        void ApplyStormControlPulse(Vector3 origin, HashSet<Combatant> directTargets)
        {
            Collider[] hits = Physics.OverlapSphere(origin, runStyle.StormPulseRadius, hittableLayers);
            var affected = new HashSet<Combatant>();

            foreach (Collider hitCollider in hits)
            {
                Combatant target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target == selfCombatant || target.IsDefeated || directTargets.Contains(target) || !affected.Add(target))
                {
                    continue;
                }

                Vector3 direction = target.transform.position - origin;
                direction.y = 0f;
                direction = direction.sqrMagnitude > 0.0001f ? direction.normalized : transform.forward;
                target.TakeHit(new HitInfo(0, DamageElement.Physical, direction * runStyle.StormPulseImpulse));
            }

            PrimitiveBurstVFX.SpawnAt(origin, runStyle.StormPulseRadius, 0.32f, StormPulseColor);
        }
    }
}
