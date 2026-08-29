using TieuTienKy.Input;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// The single P0A attack: a short-range lightning palm strike. Its element
    /// is always Lightning so the same action can, in context, produce a plain
    /// hit, a knockback-into-hazard, or (inside a WaterZone) a Conductive Burst.
    /// Sequenced as anticipation -> impact -> recovery via AttackSequencer so
    /// it reads as a fast arcade swing instead of an instantaneous debug
    /// trigger. No combo tree/progression.
    /// </summary>
    [RequireComponent(typeof(TouchInputReader))]
    public sealed class BasicAttack : MonoBehaviour
    {
        [SerializeField] float anticipationSeconds = 0.12f;
        [SerializeField] float recoverySeconds = 0.28f;
        [SerializeField] float rangeMeters = 1.75f;
        [SerializeField] float radiusMeters = 0.9f;
        [SerializeField] int damage = 1;
        [SerializeField] float knockbackImpulseMagnitude = 6f;
        [SerializeField] LayerMask hittableLayers = ~0;
        [SerializeField] float hitStopSeconds = 0.05f;
        [SerializeField] float hitStopTimeScale = 0.05f;
        [SerializeField] float conductiveKnockbackMultiplier = 2.5f;

        /// <summary>Distinct from Conductive Burst's cyan: this is the sword/attack's own Lôi Kiếm impact flash, present on every landed hit (even at 0 stacks) and never gated on Water.</summary>
        static readonly Color LightningImpactFlashColor = new Color(1f, 0.95f, 0.4f, 1f);

        TouchInputReader inputReader;
        Combatant selfCombatant;
        AttackSequencer sequencer;
        float recoveryMultiplier = 1f;
        int thunderStacks;
        bool networkDriven;

        [SerializeField] bool localWorldTapEnabled = true;

        public event System.Action AttackStarted;
        public event System.Action AttackImpacted;
        public event System.Action AttackRecovered;

        /// <summary>Fires only when a hit actually landed (unlike AttackImpacted, which fires on every swing resolution) - presentation binds bounded camera impulse here, never on a whiff.</summary>
        public event System.Action HitLanded;

        void Awake()
        {
            inputReader = GetComponent<TouchInputReader>();
            selfCombatant = GetComponent<Combatant>();
            sequencer = new AttackSequencer(anticipationSeconds, recoverySeconds);
        }

        /// <summary>
        /// Task B2/B3: in network mode, only the server ever resolves an
        /// attack, and triggering always comes from the shared
        /// PlayerActionExecutor (via NetworkPlayerActionGateway's ServerRpc),
        /// never from a peer's own local TouchInputReader - so a remote
        /// observer's copy of this player can never grant itself damage.
        /// Solo play never calls this (networkDriven stays false).
        /// </summary>
        public void SetNetworkDriven(bool value) => networkDriven = value;

        /// <summary>Whether Update()'s direct local-input branch is allowed to trigger an attack. Defaults to true for greybox/backward compatibility; production composition can disable it once the shared PlayerActionGateway path is wired, without affecting TryActivate or network semantics.</summary>
        public bool LocalWorldTapEnabled => localWorldTapEnabled;

        public void SetLocalWorldTapEnabled(bool enabled) => localWorldTapEnabled = enabled;

        void Update()
        {
            if (!networkDriven && localWorldTapEnabled && inputReader.AttackTriggeredThisFrame)
            {
                TryActivate(Time.time);
            }

            if (sequencer.Tick(Time.time, out bool recoveryEnded))
            {
                PerformAttack();
                AttackImpacted?.Invoke();
            }

            if (recoveryEnded)
            {
                AttackRecovered?.Invoke();
            }
        }

        /// <summary>
        /// Explicit activation entry point (Task B1's shared action-gateway
        /// seam): Update() calls this from local touch input exactly as
        /// before; PlayerActionExecutor calls the same method from either
        /// LocalPlayerActionGateway or (server-side, on the authoritative
        /// copy) NetworkPlayerActionGateway, so local and network play
        /// resolve through this one method - never a second attack path.
        /// </summary>
        public bool TryActivate(float currentTime)
        {
            if (!sequencer.TryBeginAttack(currentTime))
            {
                return false;
            }

            CombatAudio.Play("BasicSwing", transform.position);
            AttackStarted?.Invoke();
            return true;
        }

        /// <summary>Sets the run-blessing-driven attack recovery/Conductive strength. Recreates the sequencer only while Idle, so an in-flight swing is never disrupted.</summary>
        public void SetRunModifiers(float newRecoveryMultiplier, float newConductiveMultiplier)
        {
            recoveryMultiplier = newRecoveryMultiplier;
            conductiveKnockbackMultiplier = newConductiveMultiplier;

            if (sequencer.Phase == AttackPhase.Idle)
            {
                sequencer = new AttackSequencer(anticipationSeconds, recoverySeconds * recoveryMultiplier);
            }
        }

        /// <summary>Lôi Kiếm stack level driving this attack's own impact-flash size/duration (BlessingPresentationMath). Never affects whether Conductive Burst fires - that stays Water-gated in Combatant/ElementalReaction.</summary>
        public void SetLightningStacks(int newThunderStacks)
        {
            thunderStacks = Mathf.Max(0, newThunderStacks);
        }

        void PerformAttack()
        {
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

                Vector3 knockDirection = (target.transform.position - transform.position);
                knockDirection.y = 0f;
                knockDirection = knockDirection.sqrMagnitude > 0.0001f ? knockDirection.normalized : transform.forward;

                var hit = new HitInfo(damage, DamageElement.Lightning, knockDirection * knockbackImpulseMagnitude, conductiveKnockbackMultiplier);
                target.TakeHit(hit);
                landedAnyHit = true;
            }

            if (landedAnyHit)
            {
                StartCoroutine(HitStop.Routine(hitStopSeconds, hitStopTimeScale));
                PrimitiveBurstVFX.SpawnAt(
                    origin,
                    BlessingPresentationMath.LightningFlashPeakRadius(thunderStacks),
                    BlessingPresentationMath.LightningFlashLifetimeSeconds(thunderStacks),
                    LightningImpactFlashColor);
                CombatAudio.Play("BasicHit", origin);
                HitLanded?.Invoke();
            }
        }
    }
}
