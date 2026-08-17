using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Placeholder "Conductive Burst" visual: a primitive cube that scales up
    /// and disappears. No VFX Graph/particle asset dependency for the spike.
    /// Uses Cube, not Sphere: CreatePrimitive(Sphere) needs SphereCollider,
    /// which IL2CPP strips on Android (nothing else in the project statically
    /// references it), throwing "Can't add component because class
    /// 'SphereCollider' doesn't exist!" the first time this runs on device.
    /// Cube only needs MeshFilter/MeshRenderer/BoxCollider, already preserved
    /// in link.xml and already used elsewhere in this scene (WaterZone,
    /// HazardObstacle) without issue.
    ///
    /// Tinted via the same project-owned P0A_Greybox material +
    /// MaterialPropertyBlock pattern GreyboxSceneBootstrapper.Tint uses -
    /// not renderer.material. CreatePrimitive's own default material
    /// references the built-in Standard shader, which nothing else in this
    /// project statically references, so IL2CPP/Android shader stripping
    /// drops it and the renderer falls back to Unity's pink "shader
    /// missing" material: the burst spawns and reacts correctly (confirmed
    /// on device), it just rendered pink instead of cyan. The shared
    /// material here already carries the proven-safe TieuTienKy/P0A_Unlit
    /// shader (see link.xml/GreyboxPrimitiveStrippingTests), so reusing it
    /// avoids a second unverified runtime material path.
    /// </summary>
    public static class PrimitiveBurstVFX
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
        static Material s_PrimitiveMaterial;

        public static void SpawnAt(Vector3 position, float peakRadius, float lifetimeSeconds, Color color)
        {
            var burst = GameObject.CreatePrimitive(PrimitiveType.Cube);
            burst.name = "ConductiveBurstVFX_Primitive";
            Object.Destroy(burst.GetComponent<Collider>());

            burst.transform.position = position;
            burst.transform.localScale = Vector3.one * 0.05f;

            var renderer = burst.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = PrimitiveMaterial();

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(ColorPropertyId, color);
                renderer.SetPropertyBlock(block);
            }

            burst.AddComponent<BurstPulseRunner>().Begin(burst.transform, peakRadius, lifetimeSeconds);
        }

        static Material PrimitiveMaterial()
        {
            if (s_PrimitiveMaterial == null)
            {
                s_PrimitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_PrimitiveMaterial;
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
