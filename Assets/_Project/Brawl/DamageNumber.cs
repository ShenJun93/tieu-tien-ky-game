using TMPro;
using UnityEngine;

namespace TieuTienKy.Brawl
{
    /// <summary>A pooled damage number: pops, rises and fades, always facing the camera.</summary>
    public sealed class DamageNumber : MonoBehaviour
    {
        const float Life = 0.75f;
        TextMeshPro text;
        Color color;
        Vector3 start;
        float size;
        float age;

        public void Show(Vector3 position, int damage, Color tint, float scale, int serial)
        {
            if (text == null) text = GetComponent<TextMeshPro>();
            // Spread consecutive numbers sideways so a combo does not stack them.
            start = position + Vector3.right * ((serial % 3) - 1) * 0.45f;
            text.text = damage.ToString();
            color = tint;
            size = scale;
            age = 0f;
            gameObject.SetActive(true);
            LateUpdate();
        }

        void LateUpdate()
        {
            age += Time.deltaTime;
            float k = Mathf.Clamp01(age / Life);
            float pop = k < 0.12f ? Mathf.Lerp(1.6f, 1f, k / 0.12f) : 1f;
            transform.position = start + Vector3.up * (1.4f * (1f - (1f - k) * (1f - k)));
            transform.localScale = Vector3.one * size * pop;
            if (Camera.main != null) transform.rotation = Camera.main.transform.rotation;
            var c = color;
            c.a = k < 0.6f ? 1f : Mathf.InverseLerp(1f, 0.6f, k);
            text.color = c;
            if (k >= 1f) gameObject.SetActive(false);
        }
    }
}
