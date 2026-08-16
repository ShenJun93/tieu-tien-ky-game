using TieuTienKy.Core;
using TieuTienKy.Input;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// The single P0A attack: a short-range lightning palm strike. Its element
    /// is always Lightning so the same action can, in context, produce a plain
    /// hit, a knockback-into-hazard, or (inside a WaterZone) a Conductive Burst.
    /// No combo tree/progression.
    /// </summary>
    [RequireComponent(typeof(TouchInputReader))]
    public sealed class BasicAttack : MonoBehaviour
    {
        [SerializeField] float cooldownSeconds = 0.6f;
        [SerializeField] float rangeMeters = 1.75f;
        [SerializeField] float radiusMeters = 0.9f;
        [SerializeField] int damage = 1;
        [SerializeField] float knockbackImpulseMagnitude = 6f;
        [SerializeField] LayerMask hittableLayers = ~0;

        TouchInputReader inputReader;
        Cooldown cooldown;

        void Awake()
        {
            inputReader = GetComponent<TouchInputReader>();
            cooldown = new Cooldown(cooldownSeconds);
        }

        void Update()
        {
            if (!inputReader.AttackTriggeredThisFrame)
            {
                return;
            }

            if (!cooldown.TryUse(Time.time))
            {
                return;
            }

            PerformAttack();
        }

        void PerformAttack()
        {
            Vector3 origin = transform.position + transform.forward * (rangeMeters * 0.5f);
            Collider[] hits = Physics.OverlapSphere(origin, radiusMeters, hittableLayers);

            foreach (Collider hitCollider in hits)
            {
                var target = hitCollider.GetComponentInParent<DummyTarget>();
                if (target == null)
                {
                    continue;
                }

                Vector3 knockDirection = (target.transform.position - transform.position);
                knockDirection.y = 0f;
                knockDirection = knockDirection.sqrMagnitude > 0.0001f ? knockDirection.normalized : transform.forward;

                var hit = new HitInfo(damage, DamageElement.Lightning, knockDirection * knockbackImpulseMagnitude);
                target.TakeHit(hit);
            }
        }
    }
}
