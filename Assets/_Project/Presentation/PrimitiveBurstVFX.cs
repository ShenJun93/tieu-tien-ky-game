using UnityEngine;

namespace TieuTienKy.Gameplay
{
    /// <summary>
    /// "Conductive Burst" visual: a genuine UnityEngine.ParticleSystem radial
    /// burst - the technique PRODUCT_PROOF_SLICE_003_VFX_TECHNIQUE originally
    /// targeted but could not reach, because com.unity.modules.particlesystem
    /// was absent from Packages/manifest.json and Packages/ was that task's
    /// own defensive forbidden path. Escalated per
    /// PRODUCT_PROOF_SLICE_004_VFX_PARTICLESYSTEM after Slice 003's manually
    /// coroutine-tweened cube-fragment fallback read as "vẫn ổn như cũ,
    /// không tệ hơn, chỉ chưa 'đẹp/nổi bật' hơn" on the Human physical gate -
    /// no regression, but no product improvement either.
    ///
    /// PRODUCT_PROOF_SLICE_005_VFX_TEXTURED_SHADER escalated again after
    /// Slice 004's real ParticleSystem burst hit the same verdict a third
    /// time: the evidence diagnosed the actual ceiling as content, not
    /// technique - every burst rendered through P0A_Unlit, a flat-color-only,
    /// RenderType=Opaque shader with no texture sampling or blending, so no
    /// mechanism could ever look soft. This burst now renders a Human-
    /// provided transparent glow/sparkle texture through a new, separate,
    /// alpha-blended shader (TieuTienKy/P0A_UnlitTexturedAlpha) instead of a
    /// flat-tinted quad, with a ColorOverLifetime alpha fade the old opaque
    /// shader could never support (only a size-to-zero shrink could fake
    /// disappearing before).
    ///
    /// Simulation (radial travel, size decay, tumble, a light gravity pull)
    /// is owned entirely by ParticleSystem itself, not a hand-rolled
    /// coroutine - real per-particle physics.
    ///
    /// Tinted via MaterialPropertyBlock on the shared P0A_ParticleGlow
    /// material, exactly like GreyboxSceneBootstrapper.Tint and every prior
    /// slice's technique - never renderer.material, which would leak a
    /// material instance. P0A_UnlitTexturedAlpha's frag() multiplies the
    /// sampled texture by the material's constant _Color and never reads a
    /// vertex/particle color, so ParticleSystem.MainModule.startColor alone
    /// would have no visible effect here; the property block is what
    /// actually tints the burst.
    /// </summary>
    public static class PrimitiveBurstVFX
    {
        const string PrimitiveMaterialResourcePath = "Materials/P0A_ParticleGlow";
        const int ParticleCount = 20;
        const float ParticleSize = 0.35f;
        const float GravityModifier = 0.3f;
        const float SpinDegreesPerSecond = 720f;

        static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
        static Material s_PrimitiveMaterial;

        public static void SpawnAt(Vector3 position, float peakRadius, float lifetimeSeconds, Color color)
        {
            var burst = new GameObject("ConductiveBurstVFX_Primitive");
            burst.transform.position = position;

            var system = burst.AddComponent<ParticleSystem>();

            // ParticleSystem defaults to playOnAwake=true, so AddComponent
            // already starts it playing (with default settings) before any
            // of the configuration below runs. Stop it first - Unity
            // rejects module changes like MainModule.duration while a
            // system is still playing.
            system.Stop(withChildren: true, stopBehavior: ParticleSystemStopBehavior.StopEmittingAndClear);

            var renderer = burst.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = PrimitiveMaterial();

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetColor(ColorPropertyId, color);
            renderer.SetPropertyBlock(block);

            ParticleSystem.MainModule main = system.main;
            main.loop = false;
            main.playOnAwake = false;
            main.maxParticles = ParticleCount;
            main.duration = lifetimeSeconds;
            main.startLifetime = lifetimeSeconds;
            main.startSpeed = peakRadius / lifetimeSeconds;
            main.startSize = ParticleSize;
            main.gravityModifier = GravityModifier;

            ParticleSystem.EmissionModule emission = system.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)ParticleCount) });

            ParticleSystem.ShapeModule shape = system.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.01f;

            ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = system.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

            // Real alpha fade, only possible now that the material actually
            // blends - the old opaque P0A_Unlit path could only fake this by
            // shrinking size to zero.
            ParticleSystem.ColorOverLifetimeModule colorOverLifetime = system.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var fadeGradient = new Gradient();
            fadeGradient.SetKeys(
                new[] { new GradientColorKey(Color.white, 0f) },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(fadeGradient);

            ParticleSystem.RotationOverLifetimeModule rotationOverLifetime = system.rotationOverLifetime;
            rotationOverLifetime.enabled = true;
            rotationOverLifetime.z = new ParticleSystem.MinMaxCurve(-SpinDegreesPerSecond, SpinDegreesPerSecond);

            system.Play();

            // Object.Destroy(obj, delay) unconditionally errors outside Play
            // Mode ("Destroy may not be called from edit mode"). The old
            // coroutine-based fragment technique never hit this in EditMode
            // tests only because its final Destroy() sat behind a
            // yield-return-null loop the synchronous test harness never
            // pumped to completion; callers there (e.g.
            // WaterZoneLightningIntegrationTests) already rely on the burst
            // object surviving past SpawnAt and being cleaned up by the
            // test's own TearDown. Guarding here preserves that exact
            // behavior while giving the real (Play Mode / on-device) path
            // proper delayed cleanup.
            if (Application.isPlaying)
            {
                Object.Destroy(burst, lifetimeSeconds);
            }
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
