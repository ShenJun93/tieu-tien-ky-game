using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>A pooled additive sprite that grows and fades once (spark, slash).</summary>
    public sealed class VfxBurst : MonoBehaviour
    {
        static readonly int ColorId = Shader.PropertyToID("_Color");
        Renderer[] renderers;
        MaterialPropertyBlock block;
        Color baseColor;
        Vector3 startScale;
        Vector3 endScale;
        float age;
        float life;
        bool billboard;

        public void Play(Vector3 position, Quaternion rotation, float scale, float grow, float seconds, bool faceCamera)
        {
            if (renderers == null)
            {
                renderers = GetComponentsInChildren<Renderer>();
                block = new MaterialPropertyBlock();
                baseColor = renderers.Length > 0 && renderers[0].sharedMaterial.HasProperty(ColorId)
                    ? renderers[0].sharedMaterial.GetColor(ColorId) : Color.white;
            }
            transform.SetPositionAndRotation(position, rotation);
            startScale = Vector3.one * scale;
            endScale = startScale * grow;
            transform.localScale = startScale;
            age = 0f;
            life = seconds;
            billboard = faceCamera;
            gameObject.SetActive(true);
            Apply(1f);
        }

        void LateUpdate()
        {
            age += Time.deltaTime;
            float k = Mathf.Clamp01(age / life);
            transform.localScale = Vector3.Lerp(startScale, endScale, 1f - (1f - k) * (1f - k));
            if (billboard && Camera.main != null) transform.rotation = Camera.main.transform.rotation;
            Apply((1f - k) * (1f - k));
            if (k >= 1f) gameObject.SetActive(false);
        }

        void Apply(float strength)
        {
            // Additive: fading the colour toward black fades the sprite.
            foreach (var r in renderers)
            {
                r.GetPropertyBlock(block);
                block.SetColor(ColorId, baseColor * strength);
                r.SetPropertyBlock(block);
            }
        }
    }
}
