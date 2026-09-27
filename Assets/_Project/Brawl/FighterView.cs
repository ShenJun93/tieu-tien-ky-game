using TieuTienKy.Combat;
using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Presentation of one fighter. It places the model at the interpolated simulation position,
    /// turns it toward its facing, and plays the animator states the simulation implies: locomotion,
    /// combo swings, hit reactions and defeat. Hitstop freezes the pose, and a hit flashes the body
    /// white. It never changes the simulation.
    /// </summary>
    public sealed class FighterView : MonoBehaviour
    {
        public const string LocomotionState = "Locomotion";
        public const string HitState = "Hit";
        public const string DeathState = "Death";
        public static string AttackState(int comboStep) => "Attack" + comboStep;

        /// <summary>The animator contract every <see cref="CharacterDefinition"/> controller must meet.</summary>
        public static readonly string[] RequiredStates = { LocomotionState, "Attack1", "Attack2", "Attack3", HitState, DeathState };

        static readonly int SpeedParam = Animator.StringToHash("Speed");
        static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        [SerializeField] Animator animator;
        [SerializeField] float turnSpeedDegrees = 900f;
        [SerializeField] float attackTurnSpeedDegrees = 2400f;
        [SerializeField] float flashSeconds = 0.12f;

        Renderer[] renderers;
        MaterialPropertyBlock block;
        int shownStep;
        int shownHits;
        bool shownDefeated;
        bool reacting;
        float flash;
        float appliedFlash = -1f;

        public void Bind(Animator target) => animator = target;

        void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>(true);
            block = new MaterialPropertyBlock();
        }

        public void Present(Vector2 position, FighterState state, float normalizedSpeed)
        {
            var t = transform;
            t.position = new Vector3(position.x, t.position.y, position.y);
            if (state.Facing.sqrMagnitude > 1e-4f && !state.Defeated)
            {
                var target = Quaternion.LookRotation(new Vector3(state.Facing.x, 0f, state.Facing.y), Vector3.up);
                float turn = state.IsAttacking ? attackTurnSpeedDegrees : turnSpeedDegrees;
                t.rotation = Quaternion.RotateTowards(t.rotation, target, turn * Time.deltaTime);
            }

            if (animator != null) Animate(state, normalizedSpeed);

            if (state.HitsTaken != shownHits)
            {
                shownHits = state.HitsTaken;
                flash = flashSeconds;
            }
            UpdateFlash();
        }

        void Animate(FighterState state, float normalizedSpeed)
        {
            animator.speed = state.Hitstop > 0 ? 0f : 1f;
            animator.SetFloat(SpeedParam, normalizedSpeed, 0.08f, Time.deltaTime);

            if (state.Defeated != shownDefeated)
            {
                shownDefeated = state.Defeated;
                shownStep = 0;
                reacting = false;
                Play(state.Defeated ? DeathState : LocomotionState, 0.08f);
                return;
            }
            if (state.Defeated) return;

            if (state.HitsTaken != shownHits)
            {
                reacting = true;
                shownStep = 0;
                Play(HitState, 0.03f);
            }
            else if (state.ComboStep != shownStep)
            {
                shownStep = state.ComboStep;
                if (shownStep > 0) Play(AttackState(shownStep), 0.04f);
                else if (!reacting) Play(LocomotionState, 0.12f);
            }
            else if (reacting && state.Hitstun == 0 && state.Hitstop == 0)
            {
                reacting = false;
                Play(LocomotionState, 0.15f);
            }
        }

        void Play(string stateName, float blendSeconds) => animator.CrossFadeInFixedTime(stateName, blendSeconds, 0);

        void UpdateFlash()
        {
            float amount = flash > 0f ? Mathf.Clamp01(flash / flashSeconds) : 0f;
            flash = Mathf.Max(0f, flash - Time.deltaTime);
            if (Mathf.Approximately(amount, appliedFlash)) return;
            appliedFlash = amount;
            foreach (var r in renderers)
            {
                r.GetPropertyBlock(block);
                block.SetFloat(FlashAmount, amount);
                r.SetPropertyBlock(block);
            }
        }
    }
}
