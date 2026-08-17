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

        BasicAttack attack;
        Transform weaponSocket;
        Quaternion restRotation;
        Coroutine activeRoutine;

        public void Initialize(BasicAttack basicAttack, Transform socket)
        {
            attack = basicAttack;
            weaponSocket = socket;
            restRotation = weaponSocket.localRotation;

            attack.AttackStarted += HandleAttackStarted;
            attack.AttackImpacted += HandleAttackImpacted;
            attack.AttackRecovered += HandleAttackRecovered;
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
