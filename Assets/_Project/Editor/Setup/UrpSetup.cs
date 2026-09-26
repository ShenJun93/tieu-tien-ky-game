using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TieuTienKy.EditorTools.Setup
{
    /// <summary>
    /// One-shot Built-in → URP migration (slice R0.3). Idempotent: re-running
    /// reuses existing tier assets. Batch usage:
    ///   Unity -batchmode -nographics -projectPath . -executeMethod TieuTienKy.EditorTools.Setup.UrpSetup.Apply -quit
    /// </summary>
    public static class UrpSetup
    {
        const string Folder = "Assets/_Project/Settings/Rendering";
        const string DefaultPostProcessData = "Packages/com.unity.render-pipelines.universal/Runtime/Data/PostProcessData.asset";
        const string ToonMaterialPath = "Assets/_Project/Materials/Toon_Prototype.mat";

        struct Tier
        {
            public string Name;
            public float RenderScale;
            public int Msaa;
            public bool Hdr;
            public bool Shadows;
            public float ShadowDistance;
            public bool SoftShadows;
        }

        // Low: Mali-G57-class phones (locked 30 fps). Mid: default. High: flagships.
        static readonly Tier Low = new Tier { Name = "Low", RenderScale = 0.8f, Msaa = 1, Hdr = false, Shadows = false, ShadowDistance = 0f, SoftShadows = false };
        static readonly Tier Mid = new Tier { Name = "Mid", RenderScale = 1f, Msaa = 2, Hdr = false, Shadows = true, ShadowDistance = 25f, SoftShadows = false };
        static readonly Tier High = new Tier { Name = "High", RenderScale = 1f, Msaa = 4, Hdr = true, Shadows = true, ShadowDistance = 35f, SoftShadows = true };

        [MenuItem("Tieu Tien Ky/Setup/Apply URP Migration")]
        public static void Apply()
        {
            EnsureFolder(Folder);
            var low = CreateTier(Low);
            var mid = CreateTier(Mid);
            var high = CreateTier(High);

            AssignPipelines(mid, new[] { low, low, mid, mid, high, high });

            // URP 17.3's Converters.RunInBatchMode throws (it instantiates the
            // abstract Base2DMaterialUpgrader), so Standard materials are upgraded here.
            UpgradeStandardMaterials("Assets/_Project");
            MoveIntoFolder("Assets/UniversalRenderPipelineGlobalSettings.asset");
            MoveIntoFolder("Assets/DefaultVolumeProfile.asset");

            CreateToonPrototypeMaterial();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[UrpSetup] URP active: {GraphicsSettings.defaultRenderPipeline?.name}; quality levels: {QualitySettings.names.Length}");
        }

        static UniversalRenderPipelineAsset CreateTier(Tier tier)
        {
            string assetPath = $"{Folder}/URP_{tier.Name}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(assetPath);
            if (existing != null)
                return existing;

            var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
            renderer.postProcessData = AssetDatabase.LoadAssetAtPath<PostProcessData>(DefaultPostProcessData);
            AssetDatabase.CreateAsset(renderer, $"{Folder}/URP_{tier.Name}_Renderer.asset");

            var asset = UniversalRenderPipelineAsset.Create(renderer);
            AssetDatabase.CreateAsset(asset, assetPath);
            asset.renderScale = tier.RenderScale;
            asset.msaaSampleCount = tier.Msaa;
            asset.supportsHDR = tier.Hdr;
            asset.shadowDistance = tier.ShadowDistance;
            asset.useSRPBatcher = true;

            var so = new SerializedObject(asset);
            so.FindProperty("m_MainLightShadowsSupported").boolValue = tier.Shadows;
            so.FindProperty("m_SoftShadowsSupported").boolValue = tier.SoftShadows;
            so.ApplyModifiedPropertiesWithoutUndo();

            EditorUtility.SetDirty(asset);
            EditorUtility.SetDirty(renderer);
            return asset;
        }

        static void AssignPipelines(RenderPipelineAsset defaultAsset, RenderPipelineAsset[] perQualityLevel)
        {
            var graphics = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
            graphics.FindProperty("m_CustomRenderPipeline").objectReferenceValue = defaultAsset;
            graphics.ApplyModifiedPropertiesWithoutUndo();
            GraphicsSettings.defaultRenderPipeline = defaultAsset;

            var quality = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/QualitySettings.asset")[0]);
            var levels = quality.FindProperty("m_QualitySettings");
            for (int i = 0; i < levels.arraySize; i++)
            {
                var pipeline = perQualityLevel[Mathf.Min(i, perQualityLevel.Length - 1)];
                levels.GetArrayElementAtIndex(i).FindPropertyRelative("customRenderPipeline").objectReferenceValue = pipeline;
            }
            quality.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Built-in Standard → URP Lit, keeping colour, albedo, smoothness and metallic.</summary>
        static void UpgradeStandardMaterials(string root)
        {
            var lit = Shader.Find("Universal Render Pipeline/Lit");
            foreach (string guid in AssetDatabase.FindAssets("t:Material", new[] { root }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null || material.shader == null || material.shader.name != "Standard")
                    continue;

                Color color = material.HasProperty("_Color") ? material.GetColor("_Color") : Color.white;
                Texture albedo = material.HasProperty("_MainTex") ? material.GetTexture("_MainTex") : null;
                float smoothness = material.HasProperty("_Glossiness") ? material.GetFloat("_Glossiness") : 0.5f;
                float metallic = material.HasProperty("_Metallic") ? material.GetFloat("_Metallic") : 0f;

                material.shader = lit;
                material.SetColor("_BaseColor", color);
                material.SetTexture("_BaseMap", albedo);
                material.SetFloat("_Smoothness", smoothness);
                material.SetFloat("_Metallic", metallic);
                EditorUtility.SetDirty(material);
                Debug.Log($"[UrpSetup] Standard → URP Lit: {path}");
            }
        }

        static void MoveIntoFolder(string assetPath)
        {
            if (AssetDatabase.LoadMainAssetAtPath(assetPath) == null)
                return;
            string target = $"{Folder}/{System.IO.Path.GetFileName(assetPath)}";
            string error = AssetDatabase.MoveAsset(assetPath, target);
            if (!string.IsNullOrEmpty(error))
                Debug.LogWarning($"[UrpSetup] Could not move {assetPath}: {error}");
        }

        static void CreateToonPrototypeMaterial()
        {
            if (AssetDatabase.LoadAssetAtPath<Material>(ToonMaterialPath) != null)
                return;
            var shader = Shader.Find("TieuTienKy/ToonPrototype");
            if (shader == null)
            {
                Debug.LogError("[UrpSetup] TieuTienKy/ToonPrototype shader not found");
                return;
            }
            AssetDatabase.CreateAsset(new Material(shader) { name = "Toon_Prototype" }, ToonMaterialPath);
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;
            string parent = System.IO.Path.GetDirectoryName(path).Replace('\\', '/');
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, System.IO.Path.GetFileName(path));
        }
    }
}
