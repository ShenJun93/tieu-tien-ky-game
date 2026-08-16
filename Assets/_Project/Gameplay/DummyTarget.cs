using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Simple dummy target: idle, takes a hit, gets knocked back, tracks whether
    /// it is standing in a WaterZone, and respawns at its start position.
    /// No production AI.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    public sealed class DummyTarget : MonoBehaviour, IWaterZoneAware
    {
        [SerializeField] int maxHealth = 3;
        [SerializeField] float respawnDelaySeconds = 2f;
        [SerializeField] Color hitFlashColor = Color.white;
        [SerializeField] float hitFlashSeconds = 0.1f;
        [SerializeField] float conductiveBurstPeakRadius = 1.25f;
        [SerializeField] float conductiveBurstLifetimeSeconds = 0.35f;
        [SerializeField] Color conductiveBurstColor = new Color(0.4f, 0.8f, 1f, 1f);

        int currentHealth;
        Vector3 spawnPosition;
        Renderer cachedRenderer;
        KnockbackReceiver knockbackReceiver;
        CharacterController controller;

        public bool IsInWaterZone { get; private set; }
        public bool IsDefeated => currentHealth <= 0;

        void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
            controller = GetComponent<CharacterController>();
            spawnPosition = transform.position;
            currentHealth = maxHealth;
        }

        public void SetInWaterZone(bool inWaterZone)
        {
            IsInWaterZone = inWaterZone;
        }

        public void TakeHit(HitInfo hit)
        {
            if (IsDefeated)
            {
                return;
            }

            currentHealth -= hit.Damage;
            knockbackReceiver.ApplyKnockback(hit.KnockbackImpulse);

            if (cachedRenderer != null)
            {
                StartCoroutine(HitFeedbackFlash.FlashRoutine(cachedRenderer, hitFlashColor, hitFlashSeconds));
            }

            if (ElementalReaction.TryTriggerConductiveBurst(IsInWaterZone, hit.Element))
            {
                PrimitiveBurstVFX.SpawnAt(transform.position, conductiveBurstPeakRadius, conductiveBurstLifetimeSeconds, conductiveBurstColor);
            }

            if (IsDefeated)
            {
                Invoke(nameof(Respawn), respawnDelaySeconds);
            }
        }

        void Respawn()
        {
            currentHealth = maxHealth;
            controller.enabled = false;
            transform.position = spawnPosition;
            controller.enabled = true;
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!knockbackReceiver.IsBeingKnockedBack)
            {
                return;
            }

            var hazard = hit.collider.GetComponent<HazardObstacle>();
            hazard?.OnImpact(this, hit.point);
        }
    }
}
