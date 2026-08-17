using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Turns the passive DummyTarget into one extremely simple pressure
    /// enemy: approach the Player while alive and not being knocked back,
    /// stop at melee range. No behavior tree, no navigation/pathfinding -
    /// just distance-gated chase toward a directly assigned target
    /// (assigned by GreyboxSceneBootstrapper, no GameObject.Find).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(DummyTarget))]
    [RequireComponent(typeof(KnockbackReceiver))]
    public sealed class EnemyPressure : MonoBehaviour
    {
        [SerializeField] float chaseSpeed = 2.5f;
        [SerializeField] float stoppingDistance = 1.4f;
        [SerializeField] float gravity = -9.81f;

        CharacterController controller;
        DummyTarget dummyTarget;
        KnockbackReceiver knockbackReceiver;
        Transform target;
        float verticalVelocity;

        public void Initialize(Transform playerTarget)
        {
            target = playerTarget;
        }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            dummyTarget = GetComponent<DummyTarget>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
        }

        void Update()
        {
            if (target != null && !dummyTarget.IsDefeated && !knockbackReceiver.IsBeingKnockedBack)
            {
                Chase();
            }

            ApplyGravity();
        }

        void Chase()
        {
            Vector3 toTarget = target.position - transform.position;
            toTarget.y = 0f;
            float distance = toTarget.magnitude;

            if (distance <= stoppingDistance)
            {
                return;
            }

            Vector3 direction = toTarget / distance;
            controller.Move(direction * chaseSpeed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
    }
}
