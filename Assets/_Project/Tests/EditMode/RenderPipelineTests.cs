using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TieuTienKy.Gameplay.Tests
{
    /// <summary>
    /// Slice R0.3: the project renders on URP in every quality level, and no
    /// project material is left on a shader URP cannot draw (pink/magenta).
    /// </summary>
    public class RenderPipelineTests
    {
        const string UrpAssetType = "UniversalRenderPipelineAsset";

        [Test]
        public void DefaultRenderPipelineIsUrp()
        {
            Assert.IsNotNull(GraphicsSettings.defaultRenderPipeline, "No default render pipeline: the project is still on Built-in.");
            Assert.AreEqual(UrpAssetType, GraphicsSettings.defaultRenderPipeline.GetType().Name);
        }

        [Test]
        public void EveryQualityLevelUsesUrp()
        {
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                var pipeline = QualitySettings.GetRenderPipelineAssetAt(i);
                Assert.IsNotNull(pipeline, $"Quality level '{QualitySettings.names[i]}' has no URP asset.");
                Assert.AreEqual(UrpAssetType, pipeline.GetType().Name, QualitySettings.names[i]);
            }
        }

        [Test]
        public void ProjectMaterialsUseUrpCompatibleShaders()
        {
            var problems = new List<string>();
            foreach (string guid in AssetDatabase.FindAssets("t:Material", new[] { "Assets/_Project" }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                var shader = material != null ? material.shader : null;
                if (shader == null || shader.name == "Hidden/InternalErrorShader" || shader.name == "Standard" || !shader.isSupported)
                    problems.Add($"{path} -> {(shader == null ? "<none>" : shader.name)}");
            }
            Assert.IsEmpty(problems, "Materials that would render pink on URP:\n" + string.Join("\n", problems));
        }

        [Test]
        public void ProjectShadersCompileWithoutErrors()
        {
            var problems = new List<string>();
            foreach (string guid in AssetDatabase.FindAssets("t:Shader", new[] { "Assets/_Project" }))
            {
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath(guid));
                if (shader != null && ShaderUtil.ShaderHasError(shader))
                    problems.Add(shader.name);
            }
            Assert.IsEmpty(problems, "Shaders with compile errors: " + string.Join(", ", problems));
        }
    }
}
