using System;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Narrow, replaceable boundary between gameplay and the animated
    /// character rig. Gameplay must call only these semantic methods and
    /// read only the typed socket properties - never transform.Find/
    /// GameObject.Find into the rig hierarchy. All rig wiring (Animator,
    /// sockets, tinted renderers) happens once via Configure(), called by
    /// the component that builds/owns this presentation instance (an
    /// editor-authored prefab or a runtime builder), so this class itself
    /// never depends on child names or paths.
    /// </summary>
    public sealed class CharacterPresentation : MonoBehaviour
    {
        static readonly int MoveSpeedParam = Animator.StringToHash("MoveSpeed");
        static readonly int BasicAttackParam = Animator.StringToHash("BasicAttack");
        static readonly int CastParam = Animator.StringToHash("Cast");
        static readonly int MobilityParam = Animator.StringToHash("Mobility");
        static readonly int HitParam = Animator.StringToHash("Hit");
        static readonly int DeathParam = Animator.StringToHash("Death");
        static readonly int BlessingTintColorId = Shader.PropertyToID("_Color");

        [SerializeField] Animator animator;
        [SerializeField] Transform weaponSocket;
        [SerializeField] Transform bodyVfxSocket;
        [SerializeField] Transform feetVfxSocket;
        [SerializeField] Transform castVfxSocket;
        [SerializeField] Renderer[] tintableRenderers = Array.Empty<Renderer>();

        public Transform WeaponSocket => weaponSocket;
        public Transform BodyVfxSocket => bodyVfxSocket;
        public Transform FeetVfxSocket => feetVfxSocket;
        public Transform CastVfxSocket => castVfxSocket;
        public bool IsConfigured => animator != null;

        public event Action<Vector3> ImpactRequested;

        /// <summary>Wires this presentation instance to its rig. Called exactly once by whatever builds the rig (editor-authored prefab fields are already wired and never need this call at runtime).</summary>
        public void Configure(Animator animatorRef, Transform weapon, Transform bodyVfx, Transform feetVfx, Transform castVfx, Renderer[] renderers)
        {
            animator = animatorRef;
            weaponSocket = weapon;
            bodyVfxSocket = bodyVfx;
            feetVfxSocket = feetVfx;
            castVfxSocket = castVfx;
            tintableRenderers = renderers ?? Array.Empty<Renderer>();
        }

        /// <summary>0 = fully idle, 1 = full move speed. Drives the Idle/Run blend only; never affects gameplay movement itself.</summary>
        public void SetMovement(float normalizedSpeed)
        {
            if (animator != null)
            {
                animator.SetFloat(MoveSpeedParam, Mathf.Clamp01(normalizedSpeed));
            }
        }

        public void PlayBasicAttack() => Trigger(BasicAttackParam);
        public void PlayCast() => Trigger(CastParam);
        public void PlayMobility() => Trigger(MobilityParam);
        public void PlayHit() => Trigger(HitParam);
        public void PlayDeath() => Trigger(DeathParam);

        /// <summary>Requests a one-shot impact reaction at a world point (weapon contact / skill landing). Presentation-only: never applies damage.</summary>
        public void PlayImpact(Vector3 worldPoint) => ImpactRequested?.Invoke(worldPoint);

        /// <summary>Tints every rig renderer toward `tint` at `intensity01` (0 = untouched, 1 = full tint) - the shared visual escalation hook every blessing uses.</summary>
        public void SetBlessingVisual(Color tint, float intensity01)
        {
            float clamped = Mathf.Clamp01(intensity01);
            var block = new MaterialPropertyBlock();
            foreach (Renderer renderer in tintableRenderers)
            {
                if (renderer == null)
                {
                    continue;
                }

                renderer.GetPropertyBlock(block);
                block.SetColor(BlessingTintColorId, Color.Lerp(Color.white, tint, clamped));
                renderer.SetPropertyBlock(block);
            }
        }

        void Trigger(int parameterHash)
        {
            if (animator != null)
            {
                animator.SetTrigger(parameterHash);
            }
        }
    }
}
