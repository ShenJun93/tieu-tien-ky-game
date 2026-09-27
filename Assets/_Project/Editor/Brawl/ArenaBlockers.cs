using System.Collections.Generic;
using System.Linq;
using TieuTienKy.Brawl;
using TieuTienKy.Combat;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TieuTienKy.EditorTools.Brawl
{
    /// <summary>
    /// CORE-1b-ii: turns the LOOK-1 arena props into gameplay. Grounded props become sim circles,
    /// so fighters stop walking through them. The paifang roof and beams fade while they hide the hero.
    /// </summary>
    public static class ArenaBlockers
    {
        const string FadeFolder = "Assets/_Project/Art/Xianxia/Materials";
        static readonly string[] GateTop = { "BeamLow", "Plaque", "BeamHigh", "Roof", "Eave", "Ridge" };

        /// <summary>One circle per grounded prop, and one per paifang pillar (the gate itself stays walkable).</summary>
        public static CircleObstacle[] FromProps(Rect playArea)
        {
            var clusters = GameObject.Find("PropClusters");
            var result = new List<CircleObstacle>();
            if (clusters == null) return result.ToArray();

            foreach (Transform prop in clusters.transform)
            {
                if (prop.name == "Paifang")
                {
                    foreach (Transform part in prop)
                        if (part.name == "Base") result.Add(Footprint(part.GetComponentsInChildren<Renderer>(), 0.9f));
                    continue;
                }
                var renderers = prop.GetComponentsInChildren<Renderer>();
                if (renderers.Length == 0) continue;
                if (Bounds(renderers).min.y > 0.6f) continue; // hanging lanterns do not block
                var circle = Footprint(renderers, 0.8f);
                if (!Touches(playArea, circle)) continue;
                result.Add(circle);
            }
            return result.ToArray();
        }

        /// <summary>Adds an <see cref="OccluderFade"/> to the paifang, with transparent twins of its roof materials.</summary>
        public static OccluderFade GateRoofFade(Transform cameraTarget)
        {
            var gate = GameObject.Find("Paifang");
            if (gate == null) return null;
            var renderers = gate.GetComponentsInChildren<Renderer>().Where(r => GateTop.Contains(r.name)).ToArray();
            var fades = renderers.Select(r => FadeTwin(r.sharedMaterial)).ToArray();
            var fade = gate.AddComponent<OccluderFade>();
            fade.Configure(renderers, fades, cameraTarget);
            return fade;
        }

        /// <summary>URP Lit, transparent alpha blend, same colours. Created once beside the source material.</summary>
        static Material FadeTwin(Material source)
        {
            string path = $"{FadeFolder}/{source.name}_Fade.mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;

            var m = new Material(source);
            m.SetFloat("_Surface", 1f);
            m.SetFloat("_Blend", 0f);
            m.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            m.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            m.SetFloat("_SrcBlendAlpha", (float)BlendMode.One);
            m.SetFloat("_DstBlendAlpha", (float)BlendMode.OneMinusSrcAlpha);
            m.SetFloat("_ZWrite", 0f);
            m.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            m.SetOverrideTag("RenderType", "Transparent");
            m.SetShaderPassEnabled("DepthOnly", false);
            m.renderQueue = (int)RenderQueue.Transparent;
            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        static CircleObstacle Footprint(Renderer[] renderers, float fill)
        {
            var b = Bounds(renderers);
            return new CircleObstacle(new Vector2(b.center.x, b.center.z), Mathf.Max(b.extents.x, b.extents.z) * fill);
        }

        static Bounds Bounds(Renderer[] renderers)
        {
            var b = renderers[0].bounds;
            foreach (var r in renderers) b.Encapsulate(r.bounds);
            return b;
        }

        static bool Touches(Rect area, CircleObstacle c) =>
            c.Center.x + c.Radius > area.xMin && c.Center.x - c.Radius < area.xMax &&
            c.Center.y + c.Radius > area.yMin && c.Center.y - c.Radius < area.yMax;
    }
}
