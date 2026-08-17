using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Regression coverage for the Android IL2CPP stripping failure where
    /// GameObject.CreatePrimitive-created components (MeshCollider, etc.) and
    /// the runtime primitive material were missing from a stripped build.
    /// EditMode cannot exercise the linker/build pipeline itself, so these
    /// tests pin the deterministic parts: the bootstrapper still produces the
    /// exact component types those primitives require, and it tints them via
    /// a project-owned shared material plus a per-renderer
    /// MaterialPropertyBlock rather than instancing Renderer.material (which
    /// both leaks materials and logs an error in EditMode).
    /// </summary>
    public class GreyboxPrimitiveStrippingTests
    {
        static readonly int ColorPropertyId = Shader.PropertyToID("_Color");

        [TearDown]
        public void TearDown()
        {
            foreach (string name in new[] { "Ground", "WaterZone" })
            {
                var go = GameObject.Find(name);
                if (go != null)
                {
                    Object.DestroyImmediate(go);
                }
            }
        }

        [Test]
        public void BuildGround_PlanePrimitive_HasRuntimeRequiredMeshAndColliderComponents()
        {
            InvokeBuilder("BuildGround");

            var ground = GameObject.Find("Ground");
            Assert.IsNotNull(ground, "BuildGround must create a GameObject named 'Ground'.");
            Assert.IsNotNull(ground.GetComponent<MeshFilter>());
            Assert.IsNotNull(ground.GetComponent<MeshRenderer>());
            Assert.IsNotNull(ground.GetComponent<MeshCollider>());
        }

        [Test]
        public void BuildWaterZone_CubePrimitive_HasRuntimeRequiredMeshAndColliderComponents()
        {
            InvokeBuilder("BuildWaterZone", Vector3.zero, Vector3.one);

            var zone = GameObject.Find("WaterZone");
            Assert.IsNotNull(zone, "BuildWaterZone must create a GameObject named 'WaterZone'.");
            Assert.IsNotNull(zone.GetComponent<MeshFilter>());
            Assert.IsNotNull(zone.GetComponent<MeshRenderer>());
            Assert.IsNotNull(zone.GetComponent<BoxCollider>());
        }

        [Test]
        public void BuildGround_AssignsProjectOwnedMaterial_NotDefaultOrErrorShader()
        {
            InvokeBuilder("BuildGround");

            var renderer = GameObject.Find("Ground").GetComponent<MeshRenderer>();

            Assert.IsNotNull(renderer.sharedMaterial, "Primitive must not fall back to Unity's implicit default material.");
            Assert.IsNotNull(renderer.sharedMaterial.shader);
            Assert.AreEqual("TieuTienKy/P0A_Unlit", renderer.sharedMaterial.shader.name);
            Assert.AreNotEqual("Hidden/InternalErrorShader", renderer.sharedMaterial.shader.name);
        }

        [Test]
        public void Tint_DistinctPrimitives_RetainIndependentColorsWithoutMutatingSharedMaterial()
        {
            InvokeBuilder("BuildGround");
            InvokeBuilder("BuildWaterZone", Vector3.zero, Vector3.one);

            var groundRenderer = GameObject.Find("Ground").GetComponent<MeshRenderer>();
            var waterRenderer = GameObject.Find("WaterZone").GetComponent<MeshRenderer>();

            Assert.AreSame(
                groundRenderer.sharedMaterial,
                waterRenderer.sharedMaterial,
                "Both primitives should reuse the same project-owned material asset, not per-object instances.");

            Color groundTint = ReadPropertyBlockColor(groundRenderer);
            Color waterTint = ReadPropertyBlockColor(waterRenderer);

            Assert.AreEqual(new Color(0.55f, 0.55f, 0.55f), groundTint);
            Assert.AreEqual(new Color(0.2f, 0.5f, 0.9f, 0.6f), waterTint);
            Assert.AreNotEqual(groundTint, waterTint, "Distinct primitives must retain independent tints.");

            Assert.AreEqual(
                Color.white,
                groundRenderer.sharedMaterial.color,
                "The shared material asset's own color must not be mutated by per-object tinting.");
        }

        [Test]
        public void BuildGround_DoesNotLogEditModeMaterialInstantiationWarning()
        {
            var receivedLogs = new List<string>();
            void OnLog(string condition, string stackTrace, LogType type) => receivedLogs.Add(condition);

            Application.logMessageReceived += OnLog;
            try
            {
                InvokeBuilder("BuildGround");
            }
            finally
            {
                Application.logMessageReceived -= OnLog;
            }

            Assert.IsFalse(
                receivedLogs.Exists(message => message.Contains("Instantiating material due to calling renderer.material")),
                "Tinting a primitive must not instantiate a material in edit mode (leaks materials into the scene).");
        }

        static Color ReadPropertyBlockColor(Renderer renderer)
        {
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            return block.GetColor(ColorPropertyId);
        }

        static void InvokeBuilder(string methodName, params object[] args)
        {
            MethodInfo method = typeof(GreyboxSceneBootstrapper).GetMethod(
                methodName, BindingFlags.NonPublic | BindingFlags.Static);

            Assert.IsNotNull(method, $"Expected private static method '{methodName}' on GreyboxSceneBootstrapper.");
            method.Invoke(null, args);
        }
    }
}
