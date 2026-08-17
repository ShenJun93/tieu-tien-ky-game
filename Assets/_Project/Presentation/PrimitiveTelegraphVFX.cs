using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// One temporary tinted primitive warning marker per enemy - readable
    /// melee/lunge telegraph without a VFX framework. Cube only (never
    /// Sphere - its SphereCollider is IL2CPP-stripped on Android, see
    /// PrimitiveBurstVFX). Pursuer warning is a short flattened footprint;
    /// Lancer warning is a long narrow lane aligned to the committed
    /// direction.
    /// </summary>
    public sealed class PrimitiveTelegraphVFX : MonoBehaviour
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static readonly Color PursuerWarningColor = new Color(1f, 0.45f, 0.1f, 0.85f);
        static readonly Color LancerWarningColor = new Color(1f, 0.2f, 0.15f, 0.85f);

        static Material s_PrimitiveMaterial;

        GameObject marker;

        public void Show(EnemyArchetype archetype, Transform origin, Vector3 forward)
        {
            Hide();

            marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = "TelegraphWarning_Primitive";
            Destroy(marker.GetComponent<Collider>());

            Vector3 flatForward = forward.sqrMagnitude > 0.0001f ? forward.normalized : origin.forward;
            Quaternion rotation = Quaternion.LookRotation(flatForward, Vector3.up);

            if (archetype == EnemyArchetype.Lancer)
            {
                marker.transform.SetPositionAndRotation(origin.position + flatForward * 1.6f, rotation);
                marker.transform.localScale = new Vector3(0.5f, 0.05f, 3.2f);
            }
            else
            {
                marker.transform.SetPositionAndRotation(origin.position + flatForward * 0.9f, rotation);
                marker.transform.localScale = new Vector3(1.1f, 0.05f, 1.1f);
            }

            var renderer = marker.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = PrimitiveMaterial();

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(TintColorPropertyId, archetype == EnemyArchetype.Lancer ? LancerWarningColor : PursuerWarningColor);
                renderer.SetPropertyBlock(block);
            }
        }

        public void Hide()
        {
            if (marker != null)
            {
                Destroy(marker);
                marker = null;
            }
        }

        void OnDestroy() => Hide();

        static Material PrimitiveMaterial()
        {
            if (s_PrimitiveMaterial == null)
            {
                s_PrimitiveMaterial = Resources.Load<Material>(PrimitiveMaterialResourcePath);
            }

            return s_PrimitiveMaterial;
        }
    }
}
