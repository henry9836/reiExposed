Shader "Cutsom/Heat"
{
    Properties
    {
        _Noise("Noise", 2D) = "white" {}
        _StrengthFilter("Strength Filter", 2D) = "white" {}
        _Strength("Distort Strength", float) = 1.0
        _Speed("Distort Speed", float) = 1.0
        _BackgroundTexture("BGtex Filter", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent"
            "DisableBatching" = "True"
        }

        Pass
        {
            ZTest Always


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _Noise;
            sampler2D _StrengthFilter;
            sampler2D _BackgroundTexture;
            float     _Strength;
            float     _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 texCoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 grabPos : TEXCOORD0;
            };

            void Unity_SceneColor_float(float4 UV, out float3 Out)
            {
                Out = SHADERGRAPH_SAMPLE_SCENE_COLOR(UV);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 pos = v.vertex;

                pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(pos.x, pos.z, 0, 0));
                o.pos = pos;
                o.grabPos = ComputeGrabScreenPos(o.pos);



                float noise = tex2Dlod(_Noise, float4(v.texCoord, 0)).rgb;
                float3 filt = tex2Dlod(_StrengthFilter, float4(v.texCoord, 0)).rgb;
                o.grabPos.x += cos(noise * _Time.x * _Speed) * filt * _Strength;
                o.grabPos.y += sin(noise * _Time.x * _Speed) * filt * _Strength;

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                float3 col;
                Unity_SceneColor_float(i.grabPos, col);
                return tex2Dproj(col, i.grabPos);
            }

            ENDCG
        }
    }

}
