using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Simple obstacle/hazard. A Combatant being knocked back into this
    /// reports the impact via OnImpact for a primitive, readable reaction.
    /// </summary>
    public sealed class HazardObstacle : MonoBehaviour
    {
        [SerializeField] Color impactFlashColor = Color.red;
        [SerializeField] float impactFlashSeconds = 0.15f;

        Renderer cachedRenderer;

        void Awake()
        {
            cachedRenderer = GetComponent<Renderer>();
        }

        public void OnImpact(Combatant source, Vector3 hitPoint)
        {
            if (cachedRenderer != null)
            {
                StopAllCoroutines();
                StartCoroutine(HitFeedbackFlash.FlashRoutine(cachedRenderer, impactFlashColor, impactFlashSeconds));
            }
        }
    }
}
