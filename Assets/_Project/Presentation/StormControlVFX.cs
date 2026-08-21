using System.Collections;
using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// PRODUCT_PROOF_SLICE_006_STORM_CONTROL_HERO_VFX: a bespoke 5-beat authored
    /// effect for exactly one skill (Storm Control), testing composition/
    /// sequencing as the axis instead of the mechanism/material axes Slices
    /// 002-005 already exhausted on the shared PrimitiveBurstVFX.SpawnAt burst.
    /// Sequence (CAUSE -> REACTION -> PAYOFF -> DECAY, each a different
    /// silhouette): ignition flash -> water ripple -> lightning branch ->
    /// expanding shock ring -> residual sparkle (the last beat reuses
    /// PrimitiveBurstVFX.SpawnAt/ParticleGlow_01 unmodified, per the task).
    ///
    /// Exactly two shared material assets across all four new textures
    /// (P0A_StormControlAlpha / P0A_StormControlAdditive), swapped via
    /// MaterialPropertyBlock.SetTexture the same way every other primitive
    /// VFX in this codebase swaps _Color - no material instancing, no
    /// renderer.material. Each layer is a flat GameObject.CreatePrimitive
    /// Quad lying on the XZ ground plane (rotated like
    /// PrimitiveTelegraphVFX's warning markers), since PlayerFollowCamera is
    /// a fixed elevated angled-down view, not a billboard-facing one.
    ///
    /// Beats animate on Time.unscaledDeltaTime (like HitStop.Routine's
    /// WaitForSecondsRealtime) so they stay visible through the
    /// near-zero-timescale Storm Control hit-stop window
    /// (LoiTramSkill.stormHitStopSeconds/TimeScale) instead of freezing with
    /// it. Beat timing is authored short enough that Beat 3/4 (lightning +
    /// shock ring, the visual peak) land inside that same ~0.11s hit-stop
    /// window as the already-existing CombatAudio/PlayerFollowCamera impulse
    /// fired synchronously at the ApplyStormControlPulse call site.
    /// </summary>
    public static class StormControlVFX
    {
        const string AlphaMaterialResourcePath = "Materials/P0A_StormControlAlpha";
        const string AdditiveMaterialResourcePath = "Materials/P0A_StormControlAdditive";
        const string IgnitionTexturePath = "Textures/VFX/StormControl_IgnitionFlash_01";
        const string WaterRippleTexturePath = "Textures/VFX/StormControl_WaterRipple_01";
        const string LightningTexturePath = "Textures/VFX/StormControl_LightningBranch_01";
        const string ShockRingTexturePath = "Textures/VFX/StormControl_ShockRing_01";

        // Beat start times are seconds-from-cast; each beat's own duration is
        // how long it takes to fully fade after starting. Beat 3/4 (0.08s,
        // 0.12s) land inside LoiTramSkill's stormHitStopSeconds (0.11s)
        // window around the cast instant, per the sync-the-peak requirement.
        const float IgnitionStartSeconds = 0f;
        const float IgnitionDurationSeconds = 0.09f;
        const float WaterRippleStartSeconds = 0.02f;
        const float WaterRippleDurationSeconds = 0.26f;
        const float LightningStartSeconds = 0.08f;
        const float LightningDurationSeconds = 0.09f;
        const float ShockRingStartSeconds = 0.12f;
        const float ShockRingDurationSeconds = 0.22f;
        const float ResidualStartSeconds = 0.30f;

        // Radius factors are relative to runStyle.StormPulseRadius (the same
        // world-space value ApplyStormControlPulse already uses for
        // Physics.OverlapSphere) - Beat 4 uses exactly 1x per the task's
        // "world-space scale is real" requirement.
        const float IgnitionRadiusFactor = 0.32f;
        const float LightningRadiusFactor = 0.6f;
        const float WaterRippleRadiusFactor = 0.85f;
        const float ShockRingRadiusFactor = 1f;
        const float ResidualRadiusFactor = 0.45f;

        const float IgnitionStartScaleFactor = 0.7f;
        const float LightningStartScaleFactor = 0.85f;
        const float WaterRippleStartScaleFactor = 0.2f;
        const float ShockRingStartScaleFactor = 0.08f;

        const float IgnitionFadeStartFraction = 0.2f;
        const float LightningFadeStartFraction = 0.3f;
        const float WaterRippleFadeStartFraction = 0.55f;
        const float ShockRingFadeStartFraction = 0.4f;

        const float IgnitionHeight = 0.05f;
        const float LightningHeight = 0.04f;
        const float ShockRingHeight = 0.03f;
        const float WaterRippleHeight = 0.02f;

        static readonly Color IgnitionColor = Color.white;
        static readonly Color LightningColor = Color.white;
        static readonly Color WaterRippleColor = new Color(0.35f, 0.75f, 1f, 1f);
        static readonly Color ShockRingColor = new Color(0.78f, 0.95f, 1f, 1f);

        static readonly int MainTexPropertyId = Shader.PropertyToID("_MainTex");
        static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        static Material s_AlphaMaterial;
        static Material s_AdditiveMaterial;
        static Texture s_IgnitionTexture;
        static Texture s_WaterRippleTexture;
        static Texture s_LightningTexture;
        static Texture s_ShockRingTexture;

        public static void SpawnAt(Vector3 origin, float pulseRadius, float residualLifetimeSeconds, Color residualColor)
        {
            var host = new GameObject("StormControlVFX_Runner");
            host.transform.position = origin;

            var runner = host.AddComponent<StormControlVFXRunner>();
            runner.Begin(origin, pulseRadius, residualLifetimeSeconds, residualColor);
        }

        static Material AlphaMaterial()
        {
            if (s_AlphaMaterial == null)
            {
                s_AlphaMaterial = Resources.Load<Material>(AlphaMaterialResourcePath);
            }

            return s_AlphaMaterial;
        }

        static Material AdditiveMaterial()
        {
            if (s_AdditiveMaterial == null)
            {
                s_AdditiveMaterial = Resources.Load<Material>(AdditiveMaterialResourcePath);
            }

            return s_AdditiveMaterial;
        }

        static Texture IgnitionTexture()
        {
            if (s_IgnitionTexture == null)
            {
                s_IgnitionTexture = Resources.Load<Texture>(IgnitionTexturePath);
            }

            return s_IgnitionTexture;
        }

        static Texture WaterRippleTexture()
        {
            if (s_WaterRippleTexture == null)
            {
                s_WaterRippleTexture = Resources.Load<Texture>(WaterRippleTexturePath);
            }

            return s_WaterRippleTexture;
        }

        static Texture LightningTexture()
        {
            if (s_LightningTexture == null)
            {
                s_LightningTexture = Resources.Load<Texture>(LightningTexturePath);
            }

            return s_LightningTexture;
        }

        static Texture ShockRingTexture()
        {
            if (s_ShockRingTexture == null)
            {
                s_ShockRingTexture = Resources.Load<Texture>(ShockRingTexturePath);
            }

            return s_ShockRingTexture;
        }

        static GameObject SpawnLayer(
            string name,
            Material material,
            Texture texture,
            Color color,
            Vector3 origin,
            float height,
            float radius,
            float startScaleFactor,
            float durationSeconds,
            float fadeStartFraction,
            bool randomYRotation)
        {
            var layer = GameObject.CreatePrimitive(PrimitiveType.Quad);
            layer.name = name;

            // Application.isPlaying guard matches PrimitiveBurstVFX's delayed
            // Destroy: Object.Destroy on a component logs "Destroy may not be
            // called from edit mode" outside Play Mode (an EditMode-only
            // failure this project's EditMode tests do exercise), so an
            // immediate DestroyImmediate is used there instead.
            Collider layerCollider = layer.GetComponent<Collider>();
            if (Application.isPlaying)
            {
                Object.Destroy(layerCollider);
            }
            else
            {
                Object.DestroyImmediate(layerCollider);
            }

            float yRotation = randomYRotation ? Random.Range(0f, 360f) : 0f;
            layer.transform.SetPositionAndRotation(origin + Vector3.up * height, Quaternion.Euler(90f, yRotation, 0f));

            float endScale = radius * 2f;
            float startScale = endScale * startScaleFactor;
            layer.transform.localScale = new Vector3(startScale, startScale, 1f);

            var renderer = layer.GetComponent<Renderer>();
            renderer.sharedMaterial = material;

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetTexture(MainTexPropertyId, texture);
            block.SetColor(ColorPropertyId, color);
            renderer.SetPropertyBlock(block);

            StormControlVFXLayerAnimator animator = layer.AddComponent<StormControlVFXLayerAnimator>();
            animator.Begin(renderer, color, durationSeconds, startScale, endScale, fadeStartFraction);

            return layer;
        }

        sealed class StormControlVFXRunner : MonoBehaviour
        {
            public void Begin(Vector3 origin, float pulseRadius, float residualLifetimeSeconds, Color residualColor)
            {
                StartCoroutine(Sequence(origin, pulseRadius, residualLifetimeSeconds, residualColor));
            }

            IEnumerator Sequence(Vector3 origin, float pulseRadius, float residualLifetimeSeconds, Color residualColor)
            {
                SpawnLayer(
                    "StormControlVFX_Ignition", AdditiveMaterial(), IgnitionTexture(), IgnitionColor,
                    origin, IgnitionHeight, pulseRadius * IgnitionRadiusFactor, IgnitionStartScaleFactor,
                    IgnitionDurationSeconds, IgnitionFadeStartFraction, randomYRotation: true);

                yield return new WaitForSecondsRealtime(WaterRippleStartSeconds - IgnitionStartSeconds);

                SpawnLayer(
                    "StormControlVFX_WaterRipple", AlphaMaterial(), WaterRippleTexture(), WaterRippleColor,
                    origin, WaterRippleHeight, pulseRadius * WaterRippleRadiusFactor, WaterRippleStartScaleFactor,
                    WaterRippleDurationSeconds, WaterRippleFadeStartFraction, randomYRotation: false);

                yield return new WaitForSecondsRealtime(LightningStartSeconds - WaterRippleStartSeconds);

                SpawnLayer(
                    "StormControlVFX_Lightning", AdditiveMaterial(), LightningTexture(), LightningColor,
                    origin, LightningHeight, pulseRadius * LightningRadiusFactor, LightningStartScaleFactor,
                    LightningDurationSeconds, LightningFadeStartFraction, randomYRotation: true);

                yield return new WaitForSecondsRealtime(ShockRingStartSeconds - LightningStartSeconds);

                SpawnLayer(
                    "StormControlVFX_ShockRing", AlphaMaterial(), ShockRingTexture(), ShockRingColor,
                    origin, ShockRingHeight, pulseRadius * ShockRingRadiusFactor, ShockRingStartScaleFactor,
                    ShockRingDurationSeconds, ShockRingFadeStartFraction, randomYRotation: false);

                yield return new WaitForSecondsRealtime(ResidualStartSeconds - ShockRingStartSeconds);

                PrimitiveBurstVFX.SpawnAt(origin, pulseRadius * ResidualRadiusFactor, residualLifetimeSeconds, residualColor);

                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Ease-out scale grow (startScale -> endScale) plus a linear alpha/
        /// brightness fade that only begins at fadeStartFraction of the
        /// layer's own duration - "grow, then fade" for the ground-bound
        /// beats (water ripple, shock ring), "quick pop, then fade" for the
        /// instantaneous beats (ignition, lightning). Fading all four color
        /// channels together works for both blend modes: the alpha material
        /// reads _Color.a, the additive material effectively reads
        /// _Color.rgb (Blend One One ignores alpha).
        /// </summary>
        sealed class StormControlVFXLayerAnimator : MonoBehaviour
        {
            static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

            Renderer targetRenderer;
            Color baseColor;
            float durationSeconds;
            float startScale;
            float endScale;
            float fadeStartFraction;
            float elapsedSeconds;
            MaterialPropertyBlock block;

            public void Begin(Renderer renderer, Color color, float duration, float startScale, float endScale, float fadeStartFraction)
            {
                targetRenderer = renderer;
                baseColor = color;
                durationSeconds = Mathf.Max(duration, 0.01f);
                this.startScale = startScale;
                this.endScale = endScale;
                this.fadeStartFraction = fadeStartFraction;
                block = new MaterialPropertyBlock();
            }

            void Update()
            {
                elapsedSeconds += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsedSeconds / durationSeconds);

                float scaleT = 1f - (1f - t) * (1f - t);
                float currentScale = Mathf.Lerp(startScale, endScale, scaleT);
                transform.localScale = new Vector3(currentScale, currentScale, 1f);

                float fadeSpan = Mathf.Max(0.0001f, 1f - fadeStartFraction);
                float fade = t < fadeStartFraction ? 1f : Mathf.Clamp01(1f - (t - fadeStartFraction) / fadeSpan);

                targetRenderer.GetPropertyBlock(block);
                block.SetColor(ColorPropertyId, new Color(baseColor.r * fade, baseColor.g * fade, baseColor.b * fade, baseColor.a * fade));
                targetRenderer.SetPropertyBlock(block);

                if (t >= 1f)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        enabled = false;
                    }
                }
            }
        }
    }
}
