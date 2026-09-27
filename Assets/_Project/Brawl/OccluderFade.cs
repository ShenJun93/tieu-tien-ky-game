using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>
    /// Fades a group of renderers (the gate roof and beams) while they stand between the gameplay
    /// camera and the local fighter. Opaque materials swap to transparent twins only while fading,
    /// so the arena keeps its opaque batching the rest of the time. Shadows stay as they are.
    /// </summary>
    public sealed class OccluderFade : MonoBehaviour
    {
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField] Renderer[] renderers = System.Array.Empty<Renderer>();
        [Tooltip("Transparent twin for each renderer's material, same order as the renderers.")]
        [SerializeField] Material[] fadeMaterials = System.Array.Empty<Material>();
        [SerializeField] Transform target;
        [Tooltip("Height above the target's feet that must stay visible (chest).")]
        [SerializeField] float targetHeight = 1.2f;
        [SerializeField, Range(0f, 1f)] float fadedAlpha = 0.25f;
        [SerializeField] float fadeSeconds = 0.15f;

        Material[] opaqueMaterials;
        MaterialPropertyBlock block;
        Bounds bounds;
        Camera view;
        float alpha = 1f;
        bool faded;

        public bool IsOccluding { get; private set; }
        public float Alpha => alpha;

        public void Configure(Renderer[] group, Material[] fades, Transform follow)
        {
            renderers = group;
            fadeMaterials = fades;
            target = follow;
        }

        void Awake() => EnsureInit();

        void EnsureInit()
        {
            if (block != null) return;
            block = new MaterialPropertyBlock();
            opaqueMaterials = new Material[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                opaqueMaterials[i] = renderers[i].sharedMaterial;
                if (i == 0) bounds = renderers[i].bounds;
                else bounds.Encapsulate(renderers[i].bounds);
            }
        }

        void LateUpdate()
        {
            if (target == null || renderers.Length == 0) return;
            if (view == null) view = Camera.main;
            if (view == null) return;

            Vector3 from = view.transform.position;
            Vector3 to = target.position + Vector3.up * targetHeight;
            var ray = new Ray(from, to - from);
            IsOccluding = bounds.IntersectRay(ray, out float hit) && hit < Vector3.Distance(from, to);

            float goal = IsOccluding ? fadedAlpha : 1f;
            SetAlpha(Mathf.MoveTowards(alpha, goal, (1f - fadedAlpha) / Mathf.Max(0.01f, fadeSeconds) * Time.deltaTime));
        }

        /// <summary>Applies an opacity right away. The match drives it from LateUpdate; tools use it for previews.</summary>
        public void SetAlpha(float value)
        {
            EnsureInit();
            alpha = Mathf.Clamp01(value);
            bool wantFaded = alpha < 0.999f;
            if (wantFaded != faded)
            {
                faded = wantFaded;
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].sharedMaterial = faded ? fadeMaterials[i] : opaqueMaterials[i];
                    if (!faded) renderers[i].SetPropertyBlock(null);
                }
            }
            if (!faded) return;
            for (int i = 0; i < renderers.Length; i++)
            {
                Color c = fadeMaterials[i].GetColor(BaseColor);
                c.a = alpha;
                renderers[i].GetPropertyBlock(block);
                block.SetColor(BaseColor, c);
                renderers[i].SetPropertyBlock(block);
            }
        }
    }
}
