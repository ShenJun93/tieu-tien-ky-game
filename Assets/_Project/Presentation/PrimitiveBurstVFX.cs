using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// "Conductive Burst" visual: a radial multi-fragment burst - several
    /// independent cube fragments flying outward with individual
    /// velocity/spin, instead of one cube scaling in place. Escalated per
    /// PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE after Slice 002 evidence showed
    /// parameter tuning on the single-scaling-cube technique has a low
    /// ceiling.
    ///
    /// The task's primary target technique was a real
    /// UnityEngine.ParticleSystem burst. That does not compile in this
    /// project: `com.unity.modules.particlesystem` is absent from
    /// Packages/manifest.json (only physics/physics2d/animation/audio
    /// built-in modules plus inputsystem/netcode/transport/ugui are
    /// referenced there), and Packages/ is a forbidden path for this task -
    /// enabling it is a package-manifest change this bounded slice does not
    /// authorize. This is the task's own pre-approved repair-budget
    /// fallback: several small Cube fragments with outward velocity, still
    /// built entirely from the proven-safe Cube-mesh + P0A_Unlit-material
    /// path, without introducing a new component type.
    ///
    /// Fragments use MeshFilter/MeshRenderer directly against the engine's
    /// built-in Cube mesh (Resources.GetBuiltinResource) rather than
    /// GameObject.CreatePrimitive(Cube), so no Collider is ever auto-added -
    /// a genuine simplification over the old create-then-destroy-the-auto-
    /// added-Collider dance, and it means MeshFilter/MeshRenderer (both
    /// already explicitly preserved in link.xml, and already exercised via
    /// CreatePrimitive elsewhere in the project) are the only component
    /// types this technique adds, with no new stripping risk.
    ///
    /// Tinted via MaterialPropertyBlock on the shared P0A_Greybox material
    /// (TieuTienKy/P0A_Unlit shader) exactly like
    /// GreyboxSceneBootstrapper.Tint - never renderer.material, which would
    /// leak a material instance.
    /// </summary>
    public static class PrimitiveBurstVFX
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        const int FragmentCount = 8;
        const float FragmentScale = 0.12f;

        static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
        static Material s_PrimitiveMaterial;
        static Mesh s_CubeMesh;

        public static void SpawnAt(Vector3 position, float peakRadius, float lifetimeSeconds, Color color)
        {
            var coordinator = new GameObject("ConductiveBurstVFX_Primitive");
            coordinator.transform.position = position;

            var fragments = new Transform[FragmentCount];
            for (int i = 0; i < FragmentCount; i++)
            {
                fragments[i] = SpawnFragment(coordinator.transform, color);
            }

            coordinator.AddComponent<BurstPulseRunner>().Begin(fragments, peakRadius, lifetimeSeconds);
        }

        static Transform SpawnFragment(Transform parent, Color color)
        {
            var fragment = new GameObject("ConductiveBurstVFX_Fragment");
            fragment.transform.SetParent(parent, worldPositionStays: false);
            fragment.transform.localPosition = Vector3.zero;
            fragment.transform.localScale = Vector3.one * FragmentScale;

            fragment.AddComponent<MeshFilter>().sharedMesh = CubeMesh();

            var renderer = fragment.AddComponent<MeshRenderer>();
            renderer.sharedMaterial = PrimitiveMaterial();

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(ColorPropertyId, color);
            renderer.SetPropertyBlock(block);

            return fragment.transform;
        }

        static Material PrimitiveMaterial()
        {
            if (s_PrimitiveMaterial == null)
            {
                s_PrimitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_PrimitiveMaterial;
        }

        static Mesh CubeMesh()
        {
            if (s_CubeMesh == null)
            {
                s_CubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            }

            return s_CubeMesh;
        }

        sealed class BurstPulseRunner : MonoBehaviour
        {
            public void Begin(Transform[] fragments, float peakRadius, float lifetimeSeconds)
            {
                StartCoroutine(Run(fragments, peakRadius, lifetimeSeconds));
            }

            IEnumerator Run(Transform[] fragments, float peakRadius, float lifetimeSeconds)
            {
                var directions = new Vector3[fragments.Length];
                for (int i = 0; i < fragments.Length; i++)
                {
                    directions[i] = Random.onUnitSphere;
                }

                float elapsed = 0f;
                while (elapsed < lifetimeSeconds)
                {
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / lifetimeSeconds);
                    float travel = Mathf.Lerp(0f, peakRadius, t);
                    float scale = Mathf.Lerp(FragmentScale, 0f, t);

                    for (int i = 0; i < fragments.Length; i++)
                    {
                        Transform fragment = fragments[i];
                        if (fragment == null)
                        {
                            continue;
                        }

                        fragment.localPosition = directions[i] * travel;
                        fragment.localScale = Vector3.one * scale;
                        fragment.Rotate(directions[i], 360f * Time.deltaTime, Space.World);
                    }

                    yield return null;
                }

                if (this != null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
