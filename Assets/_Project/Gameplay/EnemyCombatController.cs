using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// CharacterController movement + telegraphed attacks for one enemy,
    /// driven by EnemyAttackCycle and a concrete EnemyCombatProfile. No
    /// behavior tree, nav framework or generic planner: Chase closes
    /// distance, Telegraph locks a facing/warning, the attack signal
    /// resolves a single hit-volume check against the player, Recovery
    /// leaves the enemy exposed to counterattack. A Lancer never retargets
    /// once its telegraph begins - the committed direction is what makes
    /// lateral movement meaningful counterplay.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    [RequireComponent(typeof(Combatant))]
    public sealed class EnemyCombatController : MonoBehaviour
    {
        [SerializeField] float gravity = -9.81f;

        CharacterController controller;
        KnockbackReceiver knockbackReceiver;
        Combatant selfCombatant;
        PrimitiveTelegraphVFX telegraphVfx;

        Transform player;
        Combatant playerCombatant;
        EnemyCombatProfile profile;
        EnemyAttackCycle cycle;
        Vector3 lockedForward = Vector3.forward;
        float verticalVelocity;

        public EnemyCombatProfile Profile => profile;
        public EnemyAttackPhase Phase => cycle?.Phase ?? EnemyAttackPhase.Chase;

        public void Initialize(Transform playerTransform, Combatant playerTargetCombatant, EnemyCombatProfile enemyProfile)
        {
            player = playerTransform;
            playerCombatant = playerTargetCombatant;
            profile = enemyProfile;
            cycle = new EnemyAttackCycle(profile.TelegraphSeconds, profile.RecoverySeconds);
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

            if (cycle == null || player == null || selfCombatant.IsDefeated || knockbackReceiver.IsBeingKnockedBack)
            {
                return;
            }

            switch (cycle.Phase)
            {
                case EnemyAttackPhase.Chase:
                    TickChase();
                    break;

                case EnemyAttackPhase.Telegraph:
                    if (cycle.Tick(Time.time, out _))
                    {
                        ResolveAttack();
                    }
                    break;

                case EnemyAttackPhase.Recovery:
                    cycle.Tick(Time.time, out _);
                    break;
            }
        }

        void TickChase()
        {
            Vector3 toPlayer = player.position - transform.position;
            toPlayer.y = 0f;
            float distance = toPlayer.magnitude;

            if (distance <= profile.AttackRange)
            {
                lockedForward = distance > 0.0001f ? toPlayer.normalized : transform.forward;
                transform.rotation = Quaternion.LookRotation(lockedForward, Vector3.up);
                cycle.TryBeginTelegraph(Time.time);
                telegraphVfx?.Show(profile.Archetype, transform, lockedForward);
                return;
            }

            if (distance <= profile.StoppingDistance)
            {
                return;
            }

            Vector3 direction = toPlayer.normalized;
            controller.Move(direction * profile.ChaseSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        void ResolveAttack()
        {
            telegraphVfx?.Hide();

            if (profile.LungeDistance > 0f)
            {
                controller.Move(lockedForward * profile.LungeDistance);
            }

            Vector3 hitOrigin = transform.position + lockedForward * (profile.AttackRange * 0.5f);
            float hitRadius = profile.Archetype == EnemyArchetype.Lancer ? 0.6f : 0.9f;
            Collider[] hits = Physics.OverlapSphere(hitOrigin, hitRadius);

            foreach (Collider hitCollider in hits)
            {
                var target = hitCollider.GetComponentInParent<Combatant>();
                if (target == null || target != playerCombatant || target.IsDefeated)
                {
                    continue;
                }

                target.TakeHit(new HitInfo(profile.Damage, DamageElement.Physical, lockedForward * profile.KnockbackMagnitude));
                break;
            }
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

        /// <summary>Returns the cycle to Chase and clears any active telegraph warning; used by run reset/wave setup.</summary>
        public void ResetCombatState()
        {
            cycle?.Reset();
            telegraphVfx?.Hide();
        }
    }
}
