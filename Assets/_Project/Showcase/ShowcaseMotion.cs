using UnityEngine;

namespace TieuTienKy.Showcase
{
    /// <summary>
    /// LOOK-1f showcase only: keeps the frozen combat beat alive on the phone. It is
    /// presentation, not gameplay, and the showcase APK is its only user.
    /// </summary>
    public sealed class ShowcaseMotion : MonoBehaviour
    {
        public enum Mode { Spin, Pulse, Grow, Flash, AnimatorLoop }

        [SerializeField] Mode mode = Mode.Pulse;
        [SerializeField] float period = 1.2f;
        [SerializeField] float amount = 0.15f;
        [SerializeField] float phase;

        static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        Vector3 baseScale;
        Animator animator;
        Renderer[] renderers;
        MaterialPropertyBlock block;

        public void Configure(Mode newMode, float newPeriod, float newAmount, float newPhase = 0f)
        {
            mode = newMode;
            period = newPeriod;
            amount = newAmount;
            phase = newPhase;
        }

        void Awake()
        {
            baseScale = transform.localScale;
            animator = GetComponent<Animator>();
            renderers = GetComponentsInChildren<Renderer>();
            block = new MaterialPropertyBlock();
        }

        void Update()
        {
            float t = Mathf.Repeat(Time.time / period + phase, 1f);
            switch (mode)
            {
                case Mode.Spin:
                    transform.Rotate(0f, 0f, 360f * amount * Time.deltaTime, Space.Self);
                    break;
                case Mode.Pulse:
                    transform.localScale = baseScale * (1f + amount * Mathf.Sin(t * Mathf.PI * 2f));
                    break;
                case Mode.Grow:
                    // Telegraph fill grows from the origin, then resets (teardown 3.5).
                    transform.localScale = baseScale * Mathf.Lerp(amount, 1f, t);
                    break;
                case Mode.Flash:
                    // A short white hit flash once per period (COMBAT_BAR hit feedback).
                    float flash = t < 0.08f ? 0.8f : 0f;
                    foreach (var r in renderers)
                    {
                        r.GetPropertyBlock(block);
                        block.SetFloat(FlashAmount, flash);
                        r.SetPropertyBlock(block);
                    }
                    break;
                case Mode.AnimatorLoop:
                    if (animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f + amount)
                        animator.Play(0, 0, 0f);
                    break;
            }
        }
    }
}
