using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Player-attached blessing presentation for Phong Hành (wind accent at
    /// the feet) and Hộ Thể (body ward aura), plus forwarding Lôi Kiếm stacks
    /// to SwordAttackView/BasicAttack (which own the sword/attack-impact
    /// presentation directly). One narrow component per RunBlessingState's
    /// three blessings' visible identity - not a generic VFX framework.
    /// Built only from Android-safe primitives (Cube/Capsule), colliders
    /// stripped, matching PrimitiveCharacterView's own convention.
    /// </summary>
    [RequireComponent(typeof(Combatant))]
    public sealed class PlayerBlessingPresentation : MonoBehaviour
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static readonly Color WindRingColor = new Color(0.75f, 0.95f, 0.85f, 0.55f);
        static readonly Color WardAuraColor = new Color(0.9f, 0.85f, 0.35f, 0.32f);
        static readonly Color WardHitFlashColor = new Color(1f, 1f, 1f, 0.95f);
        const float WardHitFlashSeconds = 0.12f;
        const float RingSpinDegreesPerSecond = 160f;

        static Material s_PrimitiveMaterial;

        Combatant combatant;
        SwordAttackView swordView;
        BasicAttack basicAttack;

        GameObject windRing;
        GameObject wardAura;
        Renderer wardAuraRenderer;
        int wardStacks;

        void Awake()
        {
            combatant = GetComponent<Combatant>();
            swordView = GetComponent<SwordAttackView>();
            basicAttack = GetComponent<BasicAttack>();
            combatant.Damaged += HandleDamaged;
        }

        void OnDestroy()
        {
            if (combatant != null)
            {
                combatant.Damaged -= HandleDamaged;
            }
        }

        void Update()
        {
            if (windRing != null)
            {
                windRing.transform.Rotate(Vector3.up, RingSpinDegreesPerSecond * Time.deltaTime, Space.World);
            }
        }

        /// <summary>Applies the current RunBlessingState stack counts to every blessing's presentation. Called on every blessing pick and on run restart (0,0,0).</summary>
        public void ApplyStacks(int thunderStacks, int windStacks, int wardStacksValue)
        {
            swordView?.SetLightningStacks(thunderStacks);
            basicAttack?.SetLightningStacks(thunderStacks);

            wardStacks = wardStacksValue;
            UpdateWindRing(windStacks);
            UpdateWardAura(wardStacksValue);
        }

        void UpdateWindRing(int windStacks)
        {
            if (!BlessingPresentationMath.WindRingVisible(windStacks))
            {
                if (windRing != null)
                {
                    Destroy(windRing);
                    windRing = null;
                }
                return;
            }

            float scale = BlessingPresentationMath.WindRingScale(windStacks);

            if (windRing == null)
            {
                windRing = GameObject.CreatePrimitive(PrimitiveType.Cube);
                windRing.name = "PhongHanhWindRing";
                Destroy(windRing.GetComponent<Collider>());
                windRing.transform.SetParent(transform, false);
                windRing.transform.localPosition = new Vector3(0f, -0.95f, 0f);
                Tint(windRing, WindRingColor);
            }

            windRing.transform.localScale = new Vector3(scale, 0.04f, scale);
        }

        void UpdateWardAura(int wardStacksValue)
        {
            if (!BlessingPresentationMath.WardAuraVisible(wardStacksValue))
            {
                if (wardAura != null)
                {
                    Destroy(wardAura);
                    wardAura = null;
                    wardAuraRenderer = null;
                }
                return;
            }

            float scale = BlessingPresentationMath.WardAuraScale(wardStacksValue);

            if (wardAura == null)
            {
                wardAura = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                wardAura.name = "HoTheWardAura";
                Destroy(wardAura.GetComponent<Collider>());
                wardAura.transform.SetParent(transform, false);
                wardAura.transform.localPosition = Vector3.zero;
                wardAuraRenderer = wardAura.GetComponent<Renderer>();
                Tint(wardAura, WardAuraColor);
            }

            wardAura.transform.localScale = new Vector3(scale, scale, scale);
        }

        void HandleDamaged(int currentHealth, int maxHealth)
        {
            if (wardStacks <= 0 || wardAuraRenderer == null)
            {
                return;
            }

            StartCoroutine(HitFeedbackFlash.FlashRoutine(wardAuraRenderer, WardHitFlashColor, WardHitFlashSeconds));
        }

        static void Tint(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            renderer.sharedMaterial = PrimitiveMaterial();

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(TintColorPropertyId, color);
            renderer.SetPropertyBlock(block);
        }

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
