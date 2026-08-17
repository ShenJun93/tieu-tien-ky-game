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

        TouchInputReader inputReader;
        AttackSequencer sequencer;

        void Awake()
        {
            inputReader = GetComponent<TouchInputReader>();
            sequencer = new AttackSequencer(anticipationSeconds, recoverySeconds);
        }

        void Update()
        {
            if (inputReader.AttackTriggeredThisFrame)
            {
                sequencer.TryBeginAttack(Time.time);
            }

            if (sequencer.Tick(Time.time, out _))
            {
                PerformAttack();
            }
        }

        void PerformAttack()
        {
            Vector3 origin = transform.position + transform.forward * (rangeMeters * 0.5f);
            Collider[] hits = Physics.OverlapSphere(origin, radiusMeters, hittableLayers);

            bool landedAnyHit = false;

            foreach (Collider hitCollider in hits)
            {
                var target = hitCollider.GetComponentInParent<DummyTarget>();
                if (target == null || target.IsDefeated)
                {
                    continue;
                }

                Vector3 knockDirection = (target.transform.position - transform.position);
                knockDirection.y = 0f;
                knockDirection = knockDirection.sqrMagnitude > 0.0001f ? knockDirection.normalized : transform.forward;

                var hit = new HitInfo(damage, DamageElement.Lightning, knockDirection * knockbackImpulseMagnitude);
                target.TakeHit(hit);
                landedAnyHit = true;
            }

            if (landedAnyHit)
            {
                StartCoroutine(HitStop.Routine(hitStopSeconds, hitStopTimeScale));
            }
        }
    }
}
