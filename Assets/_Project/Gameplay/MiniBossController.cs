using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Bounded three-pattern mini-boss built on the same reusable combat
    /// foundation as Pursuer/Lancer: BossAttackCycle picks the deterministic
    /// pattern, EnemyAttackCycle (reused, reconfigured per pattern) drives
    /// that pattern's telegraph/attack/recovery timing. No boss framework,
    /// no phase graph, no randomness.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    [RequireComponent(typeof(Combatant))]
    public sealed class MiniBossController : MonoBehaviour
    {
        [SerializeField] float gravity = -9.81f;
        [SerializeField] float chaseSpeed = 1.6f;
        [SerializeField] float chaseStoppingDistance = 2.5f;

        struct PatternTiming
        {
            public readonly float TelegraphSeconds;
            public readonly float RecoverySeconds;
            public readonly int Damage;
            public readonly float KnockbackMagnitude;
            public readonly float HitRadius;
            public readonly float LungeDistance;

            public PatternTiming(float telegraphSeconds, float recoverySeconds, int damage, float knockbackMagnitude, float hitRadius, float lungeDistance)
            {
                TelegraphSeconds = telegraphSeconds;
                RecoverySeconds = recoverySeconds;
                Damage = damage;
                KnockbackMagnitude = knockbackMagnitude;
                HitRadius = hitRadius;
                LungeDistance = lungeDistance;
            }
        }

        static readonly PatternTiming ArcStrikeTiming = new PatternTiming(0.55f, 0.8f, damage: 1, knockbackMagnitude: 4f, hitRadius: 1.1f, lungeDistance: 0f);
        static readonly PatternTiming ChargeTiming = new PatternTiming(0.8f, 1.1f, damage: 1, knockbackMagnitude: 6.5f, hitRadius: 0.8f, lungeDistance: 3.2f);
        static readonly PatternTiming RadialPulseTiming = new PatternTiming(0.9f, 1.2f, damage: 1, knockbackMagnitude: 5f, hitRadius: 1.6f, lungeDistance: 0f);

        CharacterController controller;
        KnockbackReceiver knockbackReceiver;
        Combatant selfCombatant;
        PrimitiveTelegraphVFX telegraphVfx;

        Transform player;
        Combatant playerCombatant;
        readonly BossAttackCycle patternCycle = new BossAttackCycle();
        EnemyAttackCycle timingCycle;
        Vector3 lockedForward = Vector3.forward;
        float verticalVelocity;

        public BossPattern CurrentPattern => patternCycle.CurrentPattern;
        public EnemyAttackPhase Phase => timingCycle?.Phase ?? EnemyAttackPhase.Chase;

        public void Initialize(Transform playerTransform, Combatant playerTargetCombatant)
        {
            player = playerTransform;
            playerCombatant = playerTargetCombatant;
            patternCycle.Reset();
            timingCycle = new EnemyAttackCycle(CurrentTiming().TelegraphSeconds, CurrentTiming().RecoverySeconds);
        }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
            selfCombatant = GetComponent<Combatant>();
            telegraphVfx = GetComponent<PrimitiveTelegraphVFX>();
        }

        void Update()
        {
            ApplyGravity();

            if (timingCycle == null || player == null || selfCombatant.IsDefeated || knockbackReceiver.IsBeingKnockedBack)
            {
                return;
            }

            switch (timingCycle.Phase)
            {
                case EnemyAttackPhase.Chase:
                    TickChase();
                    break;

                case EnemyAttackPhase.Telegraph:
                    if (timingCycle.Tick(Time.time, out _))
                    {
                        ResolveAttack();
                    }
                    break;

                case EnemyAttackPhase.Recovery:
                    timingCycle.Tick(Time.time, out bool recoveryEnded);
                    if (recoveryEnded)
                    {
                        AdvanceToNextPattern();
                    }
                    break;
            }
        }

        PatternTiming CurrentTiming() => patternCycle.CurrentPattern switch
        {
            BossPattern.ArcStrike => ArcStrikeTiming,
            BossPattern.Charge => ChargeTiming,
            _ => RadialPulseTiming
        };

        void TickChase()
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;

            PatternTiming timing = CurrentTiming();
            float attackRange = chaseStoppingDistance + timing.LungeDistance;

            if (distance <= attackRange)
            {
                lockedForward = distance > 0.0001f ? toPlayer.normalized : transform.forward;
                transform.rotation = Quaternion.LookRotation(lockedForward, Vector3.up);
                timingCycle.TryBeginTelegraph(Time.time);
                telegraphVfx?.Show(patternCycle.CurrentPattern == BossPattern.Charge ? EnemyArchetype.Lancer : EnemyArchetype.Pursuer, transform, lockedForward);
                return;
            }

            if (distance <= chaseStoppingDistance)
            {
                return;
            }

            Vector3 direction = toPlayer.normalized;
            controller.Move(direction * chaseSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        void ResolveAttack()
        {
            telegraphVfx?.Hide();
            PatternTiming timing = CurrentTiming();

            if (timing.LungeDistance > 0f)
            {
                controller.Move(lockedForward * timing.LungeDistance);
            }

            bool radial = patternCycle.CurrentPattern == BossPattern.RadialPulse;
            Vector3 hitOrigin = radial ? transform.position : transform.position + lockedForward * (timing.HitRadius * 0.5f);

            Collider[] hits = Physics.OverlapSphere(hitOrigin, timing.HitRadius);
            foreach (Collider hitCollider in hits)
            {
                var target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target != playerCombatant || target.IsDefeated)
                {
                    continue;
                }

                Vector3 knockDirection = radial ? SafeDirection(target.transform.position - transform.position) : lockedForward;
                target.TakeHit(new HitInfo(timing.Damage, DamageElement.Physical, knockDirection * timing.KnockbackMagnitude));
                break;
            }
        }

        static Vector3 SafeDirection(Vector3 direction)
        {
            direction.y = 0f;
            return direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.forward;
        }

        void AdvanceToNextPattern()
        {
            patternCycle.AdvancePattern();
            PatternTiming timing = CurrentTiming();
            timingCycle = new EnemyAttackCycle(timing.TelegraphSeconds, timing.RecoverySeconds);
        }

        void ApplyGravity()
        {
            if (controller.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -0.5f;
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            controller.Move(new Vector3(0f, verticalVelocity * Time.deltaTime, 0f));
        }

        /// <summary>Returns the boss to its first pattern in Chase; used by run reset.</summary>
        public void ResetCombatState()
        {
            patternCycle.Reset();
            PatternTiming timing = CurrentTiming();
            timingCycle = new EnemyAttackCycle(timing.TelegraphSeconds, timing.RecoverySeconds);
            telegraphVfx?.Hide();
        }
    }
}
