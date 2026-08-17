using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Reusable Unity-facing combat foundation shared by the player and every
    /// enemy: health/defeat state, Water membership, the existing Water +
    /// Lightning reaction, and knockback. Does not self-respawn — run
    /// ownership (when/whether to reset a defeated actor) belongs to
    /// ArenaRunDirector, not this component.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(KnockbackReceiver))]
    public sealed class Combatant : MonoBehaviour, IWaterZoneAware
    {
        [SerializeField] int maxHealth = 3;
        [SerializeField] Color hitFlashColor = Color.white;
        [SerializeField] float hitFlashSeconds = 0.1f;
        [SerializeField] float conductiveBurstPeakRadius = 1.25f;
        [SerializeField] float conductiveBurstLifetimeSeconds = 0.35f;
        [SerializeField] Color conductiveBurstColor = new Color(0.4f, 0.8f, 1f, 1f);

        ActorHealth health;
        Renderer cachedRenderer;
        KnockbackReceiver knockbackReceiver;
        CharacterController controller;

        public event System.Action<int, int> Damaged; // current, max
        public event System.Action Defeated;

        public bool IsDefeated => health != null && health.IsDefeated;
        public bool IsInWaterZone { get; private set; }
        public float HealthNormalized => health == null ? 0f : (float)health.CurrentHealth / health.MaxHealth;
        public int CurrentHealth => health?.CurrentHealth ?? 0;
        public int MaxHealth => health?.MaxHealth ?? maxHealth;

        /// <summary>Device-visible P0A diagnostic state for the Water+Lightning boundary (P0ADiagnosticOverlay reads these). Not gameplay state.</summary>
        public DamageElement? LastHitElement { get; private set; }
        public bool LastReactionTriggered { get; private set; }
        public int BurstSpawnCount { get; private set; }

        void Awake()
        {
            controller = GetComponent<CharacterController>();
            knockbackReceiver = GetComponent<KnockbackReceiver>();
            cachedRenderer = GetComponentInChildren<Renderer>();
            EnsureHealth();
        }

        void EnsureHealth()
        {
            if (health == null)
            {
                health = new ActorHealth(maxHealth);
            }
        }

        public void ConfigureMaxHealth(int value)
        {
            maxHealth = value;
            EnsureHealth();
            health.SetMaxHealthAndRestore(value);
        }

        public void SetMaxHealthAndRestore(int value)
        {
            ConfigureMaxHealth(value);
        }

        public void SetInWaterZone(bool inWaterZone)
        {
            IsInWaterZone = inWaterZone;
        }

        public void TakeHit(HitInfo hit)
        {
            EnsureHealth();
            if (IsDefeated)
            {
                return;
            }

            bool justDefeated = health.ApplyDamage(hit.Damage);

            bool burstTriggered = ElementalReaction.TryTriggerConductiveBurst(IsInWaterZone, hit.Element);
            LastHitElement = hit.Element;
            LastReactionTriggered = burstTriggered;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[P0A_WATER] TAKE_HIT isInWaterZone={IsInWaterZone} element={hit.Element} burstTriggered={burstTriggered}");
#endif

            Vector3 appliedImpulse = KnockbackCalculator.ApplyReactionMultiplier(hit.KnockbackImpulse, burstTriggered, hit.ConductiveKnockbackMultiplier);
            knockbackReceiver.ApplyKnockback(appliedImpulse);

            if (cachedRenderer != null)
            {
                StartCoroutine(HitFeedbackFlash.FlashRoutine(cachedRenderer, hitFlashColor, hitFlashSeconds));
            }

            if (burstTriggered)
            {
                PrimitiveBurstVFX.SpawnAt(transform.position, conductiveBurstPeakRadius, conductiveBurstLifetimeSeconds, conductiveBurstColor);
                BurstSpawnCount++;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[P0A_WATER] BURST_SPAWNED count={BurstSpawnCount}");
#endif
            }

            Damaged?.Invoke(CurrentHealth, MaxHealth);

            if (justDefeated)
            {
                Defeated?.Invoke();
            }
        }

        /// <summary>Restores full health and repositions without a self-respawn timer; the caller (ArenaRunDirector) decides when.</summary>
        public void ResetCombatant(Vector3 position)
        {
            EnsureHealth();
            health.RestoreToFull();
            IsInWaterZone = false;
            LastHitElement = null;
            LastReactionTriggered = false;
            BurstSpawnCount = 0;

            if (controller != null)
            {
                controller.enabled = false;
                transform.position = position;
                controller.enabled = true;
            }
            else
            {
                transform.position = position;
            }

            Damaged?.Invoke(CurrentHealth, MaxHealth);
        }
    }
}
