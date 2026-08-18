using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Presentation-only sword swing bound to BasicAttack's anticipation ->
    /// impact -> recovery events. Never applies damage itself; the actual
    /// hit continues to happen only in BasicAttack.PerformAttack().
    /// </summary>
    public sealed class SwordAttackView : MonoBehaviour
    {
        [SerializeField] float windUpDegrees = -35f;
        [SerializeField] float swingDegrees = 90f;
        [SerializeField] float returnSeconds = 0.18f;

        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static readonly Color BaseLightningTint = new Color(0.65f, 0.9f, 1f);
        static readonly Color EnhancedLightningTint = new Color(0.7f, 0.5f, 1f);

        BasicAttack attack;
        Transform weaponSocket;
        Quaternion restRotation;
        Coroutine activeRoutine;

        Transform swordTransform;
        Vector3 swordBaseScale = Vector3.one;

        public void Initialize(BasicAttack basicAttack, Transform socket)
        {
            attack = basicAttack;
            weaponSocket = socket;
            restRotation = weaponSocket.localRotation;

            swordTransform = weaponSocket.Find("Sword");
            if (swordTransform != null)
            {
                swordBaseScale = swordTransform.localScale;
            }

            attack.AttackStarted += HandleAttackStarted;
            attack.AttackImpacted += HandleAttackImpacted;
            attack.AttackRecovered += HandleAttackRecovered;
        }

        /// <summary>
        /// Lôi Kiếm gameplay+presentation identity: the sword itself (not
        /// just the transient impact flash BasicAttack spawns) visibly
        /// grows/tints more electric with each stack, distinct from the
        /// Conductive Burst that only appears when a Lightning hit lands
        /// inside a Water Zone.
        /// </summary>
        public void SetLightningStacks(int thunderStacks)
        {
            if (swordTransform == null)
            {
                return;
            }

            swordTransform.localScale = swordBaseScale * BlessingPresentationMath.SwordAccentScale(thunderStacks);

            var renderer = swordTransform.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            Color tint = Color.Lerp(BaseLightningTint, EnhancedLightningTint, Mathf.Clamp01(thunderStacks / 3f));
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(TintColorPropertyId, tint);
            renderer.SetPropertyBlock(block);
        }

        void OnDestroy()
        {
            if (attack == null)
            {
                return;
            }

            attack.AttackStarted -= HandleAttackStarted;
            attack.AttackImpacted -= HandleAttackImpacted;
            attack.AttackRecovered -= HandleAttackRecovered;
        }

        void HandleAttackStarted() => PlayRoutine(RotateTo(Quaternion.Euler(windUpDegrees, 0f, 0f), 0.1f));
        void HandleAttackImpacted() => PlayRoutine(RotateTo(Quaternion.Euler(windUpDegrees + swingDegrees, 0f, 0f), 0.06f));
        void HandleAttackRecovered() => PlayRoutine(RotateTo(restRotation, returnSeconds));

        void PlayRoutine(IEnumerator routine)
        {
            if (activeRoutine != null)
            {
                StopCoroutine(activeRoutine);
            }

            activeRoutine = StartCoroutine(routine);
        }

        IEnumerator RotateTo(Quaternion target, float seconds)
        {
            Quaternion start = weaponSocket.localRotation;
            float elapsed = 0f;
            seconds = Mathf.Max(0.01f, seconds);

            while (elapsed < seconds)
            {
                elapsed += Time.deltaTime;
                weaponSocket.localRotation = Quaternion.Slerp(start, target, Mathf.Clamp01(elapsed / seconds));
                yield return null;
            }

            weaponSocket.localRotation = target;
        }
    }
}
