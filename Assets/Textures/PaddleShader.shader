Shader "Custom/URPOutlineShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range (0.0, 0.1)) = 0.03
    }

    SubShader
    {
        Tags {"Queue" = "Overlay" }

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode" = "Always"}

            Cull Front
            ZWrite On
            ZTest LEqual

            Offset 10,10

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            uniform float _OutlineWidth;
            uniform float4 _OutlineColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                float3 norm = TransformObjectToWorldNormal(v.normal);
                o.pos.xy += _OutlineWidth * norm.xy;
                o.color = _OutlineColor;
                return o;
            }

            half4 frag (v2f i) : SV_TARGET
            {
                return i.color;
            }
            ENDHLSL
        }

        Pass
        {
            Name "BASE"
            Tags {"LightMode" = "Always"}

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_TARGET
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
            }
            ENDHLSL
        }
    }
}
