using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>Primitive placeholder hit feedback: brief renderer color flash.</summary>
    public static class HitFeedbackFlash
    {
        public static IEnumerator FlashRoutine(Renderer target, Color flashColor, float seconds)
        {
            if (target == null)
            {
                yield break;
            }

            Color original = target.material.color;
            target.material.color = flashColor;

            yield return new WaitForSeconds(seconds);

            if (target != null)
            {
                target.material.color = original;
            }
        }
    }
}
