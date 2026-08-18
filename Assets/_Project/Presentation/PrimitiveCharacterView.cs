using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// Replaceable full-body primitive presentation: builds a child
    /// CharacterView (Head/Body/Arms/Legs/WeaponSocket[/Sword]) out of
    /// Android-safe primitives only (Cube/Capsule - never Sphere, whose
    /// SphereCollider IL2CPP strips on Android, see PrimitiveBurstVFX).
    /// Gameplay code must target the actor root/combat components, never
    /// these child meshes directly, so a future imported character model can
    /// replace CharacterView without touching combat/run logic.
    /// </summary>
    public sealed class PrimitiveCharacterView : MonoBehaviour
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_Greybox";
        static readonly int TintColorPropertyId = Shader.PropertyToID("_Color");
        static Material s_PrimitiveMaterial;

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

            BuildPart(view.transform, "Head", PrimitiveType.Cube, new Vector3(0f, 0.82f, 0f), new Vector3(0.42f, 0.4f, 0.4f), bodyColor, visualScale);
            BuildPart(view.transform, "Body", PrimitiveType.Capsule, new Vector3(0f, 0.15f, 0f), new Vector3(0.42f, 0.32f, 0.32f), bodyColor, visualScale);
            BuildPart(view.transform, "LeftArm", PrimitiveType.Capsule, new Vector3(-0.4f, 0.25f, 0f), new Vector3(0.16f, 0.3f, 0.16f), bodyColor, visualScale);
            BuildPart(view.transform, "RightArm", PrimitiveType.Capsule, new Vector3(0.4f, 0.25f, 0f), new Vector3(0.16f, 0.3f, 0.16f), bodyColor, visualScale);
            BuildPart(view.transform, "LeftLeg", PrimitiveType.Capsule, new Vector3(-0.18f, -0.55f, 0f), new Vector3(0.18f, 0.35f, 0.18f), bodyColor, visualScale);
            BuildPart(view.transform, "RightLeg", PrimitiveType.Capsule, new Vector3(0.18f, -0.55f, 0f), new Vector3(0.18f, 0.35f, 0.18f), bodyColor, visualScale);

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
