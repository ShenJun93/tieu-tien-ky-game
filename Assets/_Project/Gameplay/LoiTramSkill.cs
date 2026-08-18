using TieuTienKy.Core;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Skill 1 - Lôi Trảm: a directional, stronger Lightning burst with its
    /// own cooldown, distinct from Basic (Lôi Kiếm). Interacts with Water
    /// exactly like Basic (Conductive Burst is Water-gated in Combatant/
    /// ElementalReaction, not duplicated here).
    /// </summary>
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

        Combatant selfCombatant;
        Cooldown cooldown;

        /// <summary>Fires on every successful activation (cooldown consumed), whether or not a hit landed - presentation binds PlayCast here.</summary>
        public event System.Action Activated;

        /// <summary>Fires only when a hit actually landed - presentation binds bounded camera impulse here, never on a whiff.</summary>
        public event System.Action HitLanded;

        void Awake()
        {
            selfCombatant = GetComponent<Combatant>();
            cooldown = new Cooldown(cooldownSeconds);
        }

        public float CooldownDuration => cooldown?.Duration ?? cooldownSeconds;
        public bool IsReady(float currentTime) => cooldown != null && cooldown.IsReady(currentTime);

        /// <summary>Run-blessing-driven Conductive strength, same knob BasicAttack.SetRunModifiers exposes for Lôi Kiếm.</summary>
        public void SetConductiveMultiplier(float multiplier) => conductiveKnockbackMultiplier = multiplier;

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

            foreach (Collider hitCollider in hits)
            {
                var target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target == selfCombatant || target.IsDefeated)
                {
                    continue;
                }

                Vector3 knockDirection = target.transform.position - transform.position;
                knockDirection.y = 0f;
                knockDirection = knockDirection.sqrMagnitude > 0.0001f ? knockDirection.normalized : transform.forward;

                var hit = new HitInfo(damage, DamageElement.Lightning, knockDirection * knockbackImpulseMagnitude, conductiveKnockbackMultiplier);
                target.TakeHit(hit);
                landedAnyHit = true;
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
    }
}
