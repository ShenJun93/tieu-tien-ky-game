using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Placeholder "Conductive Burst" visual: a primitive sphere that scales up
    /// and disappears. No VFX Graph/particle asset dependency for the spike.
    /// </summary>
    public static class PrimitiveBurstVFX
    {
        public static void SpawnAt(Vector3 position, float peakRadius, float lifetimeSeconds, Color color)
        {
            var burst = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            burst.name = "ConductiveBurstVFX_Primitive";
            Object.Destroy(burst.GetComponent<Collider>());

            burst.transform.position = position;
            burst.transform.localScale = Vector3.one * 0.05f;

            var renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }

            burst.AddComponent<BurstPulseRunner>().Begin(burst.transform, peakRadius, lifetimeSeconds);
        }

        sealed class BurstPulseRunner : MonoBehaviour
        {
            public void Begin(Transform target, float peakRadius, float lifetimeSeconds)
            {
                StartCoroutine(Run(target, peakRadius, lifetimeSeconds));
            }

            IEnumerator Run(Transform target, float peakRadius, float lifetimeSeconds)
            {
                float elapsed = 0f;
                while (elapsed < lifetimeSeconds)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / lifetimeSeconds);
                    float scale = Mathf.Lerp(0.05f, peakRadius * 2f, t);
                    if (target != null)
                    {
                        target.localScale = Vector3.one * scale;
                    }
                    yield return null;
                }

                if (target != null)
                {
                    Destroy(target.gameObject);
                }
            }
        }
    }
}
