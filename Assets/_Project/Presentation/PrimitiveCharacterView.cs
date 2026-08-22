using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Replaceable full-body presentation: builds a child CharacterView under
    /// this transform. When a chibi sprite resource matching the actor's
    /// GameObject name exists (Player/Pursuer/Lancer), renders that as a
    /// single camera-facing SpriteRenderer; otherwise falls back to the
    /// original Head/Body/Arms/Legs built from Android-safe primitives only
    /// (Cube/Capsule - never Sphere, whose SphereCollider IL2CPP strips on
    /// Android, see PrimitiveBurstVFX). WeaponSocket[/Sword] is always built
    /// either way - SwordAttackView's thunder-stack visual feedback depends
    /// on it regardless of body representation. Gameplay code must target the
    /// actor root/combat components, never these child visuals directly.
    /// </summary>
    public sealed class PrimitiveCharacterView : MonoBehaviour
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        const string ChibiSpriteResourcePathFormat = "Textures/Characters/{0}_Chibi_01";

        // Matches the previous primitive body's approximate vertical
        // footprint (Head top ~1.02, LeftLeg/RightLeg bottom ~-0.9) so the
        // sprite swap preserves existing camera framing/ground alignment.
        const float ChibiSpriteWorldHeight = 1.92f;
        const float ChibiSpriteFootLocalY = -0.9f;

        // Attempted mitigation for the WaterZone occlusion artifact disclosed in
        // SLICE-007 (see SLICE-008's task file "Priority 2" for the corrected root
        // cause - WaterZone is fully opaque and depth-writes, so this cannot fully
        // fix a genuine depth occlusion; kept as a safe, zero-risk nudge pending a
        // properly-scoped follow-up).
        const int ChibiSpriteSortingOrder = 10;

        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static Material s_PrimitiveMaterial;

        Transform chibiSpriteFacing;

        public Transform WeaponSocket { get; private set; }

        /// <summary>(Re)builds the CharacterView hierarchy under this transform. Safe to call again to rebuild with a different tint/scale.</summary>
        public void Build(Color bodyColor, Color accentColor, bool armed, float visualScale)
        {
            Transform existingView = transform.Find("CharacterView");
            if (existingView != null)
            {
                DestroyGameObjectSafely(existingView.gameObject);
            }

            var view = new GameObject("CharacterView");
            view.transform.SetParent(transform, false);

            chibiSpriteFacing = null;
            Sprite chibiSprite = Resources.Load<Sprite>(string.Format(ChibiSpriteResourcePathFormat, gameObject.name));
            if (chibiSprite != null)
            {
                chibiSpriteFacing = BuildChibiSprite(view.transform, chibiSprite, visualScale);
            }
            else
            {
                BuildPart(view.transform, "Head", PrimitiveType.Cube, new Vector3(0f, 0.82f, 0f), new Vector3(0.42f, 0.4f, 0.4f), bodyColor, visualScale);
                BuildPart(view.transform, "Body", PrimitiveType.Capsule, new Vector3(0f, 0.15f, 0f), new Vector3(0.42f, 0.32f, 0.32f), bodyColor, visualScale);
                BuildPart(view.transform, "LeftArm", PrimitiveType.Capsule, new Vector3(-0.4f, 0.25f, 0f), new Vector3(0.16f, 0.3f, 0.16f), bodyColor, visualScale);
                BuildPart(view.transform, "RightArm", PrimitiveType.Capsule, new Vector3(0.4f, 0.25f, 0f), new Vector3(0.16f, 0.3f, 0.16f), bodyColor, visualScale);
                BuildPart(view.transform, "LeftLeg", PrimitiveType.Capsule, new Vector3(-0.18f, -0.55f, 0f), new Vector3(0.18f, 0.35f, 0.18f), bodyColor, visualScale);
                BuildPart(view.transform, "RightLeg", PrimitiveType.Capsule, new Vector3(0.18f, -0.55f, 0f), new Vector3(0.18f, 0.35f, 0.18f), bodyColor, visualScale);
            }

            var weaponSocketGO = new GameObject("WeaponSocket");
            weaponSocketGO.transform.SetParent(view.transform, false);
            weaponSocketGO.transform.localPosition = new Vector3(0.5f, 0.05f, 0.1f) * visualScale;
            weaponSocketGO.transform.localRotation = Quaternion.identity;
            WeaponSocket = weaponSocketGO.transform;

            if (armed)
            {
                BuildPart(WeaponSocket, "Sword", PrimitiveType.Cube, new Vector3(0f, 0.35f, 0f), new Vector3(0.06f, 0.55f, 0.1f), accentColor, 1f);
            }
        }

        /// <summary>Keeps the chibi sprite facing the fixed gameplay camera regardless of the actor root's movement/attack-facing rotation (PlayerController/EnemyCombatController rotate the root at runtime; the camera itself never rotates - see PlayerFollowCamera).</summary>
        void LateUpdate()
        {
            if (chibiSpriteFacing == null)
            {
                return;
            }

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            chibiSpriteFacing.rotation = mainCamera.transform.rotation;
        }

        static Transform BuildChibiSprite(Transform parent, Sprite sprite, float visualScale)
        {
            var go = new GameObject("ChibiSprite");
            go.transform.SetParent(parent, false);

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = ChibiSpriteSortingOrder;

            float nativeHeight = sprite.bounds.size.y;
            float scale = nativeHeight > 0f ? (ChibiSpriteWorldHeight * visualScale) / nativeHeight : visualScale;
            go.transform.localScale = Vector3.one * scale;
            go.transform.localPosition = new Vector3(0f, ChibiSpriteFootLocalY * visualScale, 0f);

            return go.transform;
        }

        static void BuildPart(Transform parent, string name, PrimitiveType primitiveType, Vector3 localPosition, Vector3 localScale, Color color, float visualScale)
        {
            GameObject part = GameObject.CreatePrimitive(primitiveType);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition * visualScale;
            part.transform.localScale = localScale * visualScale;

            // Visual-only child: gameplay owns collision exclusively on the actor root.
            DestroyComponentSafely(part.GetComponent<Collider>());

            var renderer = part.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = PrimitiveMaterial();

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor(TintColorPropertyId, color);
                renderer.SetPropertyBlock(block);
            }
        }

        static void DestroyComponentSafely(Object target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }

        static void DestroyGameObjectSafely(GameObject target) => DestroyComponentSafely(target);

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
