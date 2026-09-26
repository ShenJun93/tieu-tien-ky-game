// Prototype URP toon shader for G2 (docs/ROADMAP.md). Two-band ramp lighting
// with a controllable shadow colour, rim light, main-light shadows and an
// inverted-hull outline. Mobile-cheap: one light, no normal maps.
Shader "TieuTienKy/ToonPrototype"
{
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _ShadowColor ("Shadow Color", Color) = (0.55, 0.5, 0.7, 1)
        _RampThreshold ("Ramp Threshold", Range(-1, 1)) = 0.1
        _RampSmoothness ("Ramp Smoothness", Range(0.001, 0.5)) = 0.05
        _RimColor ("Rim Color", Color) = (1, 0.95, 0.85, 1)
        _RimPower ("Rim Power", Range(0.5, 8)) = 4
        _RimStrength ("Rim Strength", Range(0, 1)) = 0.35
        _OutlineColor ("Outline Color", Color) = (0.08, 0.06, 0.1, 1)
        _OutlineWidth ("Outline Width", Range(0, 0.05)) = 0.01
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_BaseMap);
        SAMPLER(sampler_BaseMap);

        CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            half4 _BaseColor;
            half4 _ShadowColor;
            half _RampThreshold;
            half _RampSmoothness;
            half4 _RimColor;
            half _RimPower;
            half _RimStrength;
            half4 _OutlineColor;
            half _OutlineWidth;
        CBUFFER_END
        ENDHLSL

        Pass
        {
            Name "ToonForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                VertexPositionInputs pos = GetVertexPositionInputs(v.positionOS.xyz);
                o.positionCS = pos.positionCS;
                o.positionWS = pos.positionWS;
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.uv = TRANSFORM_TEX(v.uv, _BaseMap);
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, i.uv) * _BaseColor;
                float3 n = normalize(i.normalWS);
                Light light = GetMainLight(TransformWorldToShadowCoord(i.positionWS));

                half ndl = dot(n, light.direction) * light.shadowAttenuation;
                half lit = smoothstep(_RampThreshold - _RampSmoothness, _RampThreshold + _RampSmoothness, ndl);
                half3 color = albedo.rgb * lerp(_ShadowColor.rgb, light.color, lit);

                float3 viewDir = normalize(GetWorldSpaceViewDir(i.positionWS));
                half rim = pow(saturate(1.0 - dot(n, viewDir)), _RimPower) * _RimStrength * lit;
                color += _RimColor.rgb * rim;

                return half4(color, albedo.a);
            }
            ENDHLSL
        }

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct Attributes { float4 positionOS : POSITION; float3 normalOS : NORMAL; };
            struct Varyings { float4 positionCS : SV_POSITION; };

            Varyings vert(Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz + normalize(v.normalOS) * _OutlineWidth);
                return o;
            }

            half4 frag(Varyings i) : SV_Target { return _OutlineColor; }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
        UsePass "Universal Render Pipeline/Lit/DepthOnly"
    }
}
