Shader "Custom/tessalation"
{
    Properties
    {
        _Tess("Tessellation", Range(1,32)) = 4
        _FromColor ("FromColor", Color) = (1,1,1,1)
        _FromTex("From (RGB)", 2D) = "white" {}
        _ToColor("ToColor", Color) = (1,1,1,1)
        _ToTex("To (RGB)", 2D) = "white" {}
        _Splat("SplatMap", 2D) = "black" {}
        _Displacement("Displacement", Range(0, 1.0)) = 0.3
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance 
        #pragma target 4.6

        #include "Tessellation.cginc"

        struct appdata {
            float4 vertex : POSITION;
            float4 tangent : TANGENT;
            float3 normal : NORMAL;
            float2 texcoord : TEXCOORD0;
        };

        float _Tess;

        float4 tessDistance(appdata v0, appdata v1, appdata v2) {
            float minDist = 10.0;
            float maxDist = 25.0;
            return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
        }

        sampler2D _Splat;
        float _Displacement;

        void disp(inout appdata v)
        {
            float d = tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r * _Displacement;
            v.vertex.xyz -= v.normal * d;
            v.vertex.xyz += v.normal * _Displacement;
        }

        //end paste

        sampler2D _FromTex;
        fixed4 _FromColor;
        sampler2D _ToTex;
        fixed4 _ToColor;


        struct Input
        {
            float2 uv_FromTex;
            float2 uv_ToTex;
            float2 uv_Splat;

        };

        half _Glossiness;
        half _Metallic;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //half ammount = tex2Dlod(_Splat, float4(IN.uv_Splat, 0, 0).r);
            //fixed4 c = lerp(tex2D (_FromTex, IN.uv_FromTex) * _FromColor, tex2D (_ToTex, IN.uv_ToTex) * _ToColor, ammount);

            fixed4 c = tex2D(_FromTex, IN.uv_FromTex) * _FromColor;
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
