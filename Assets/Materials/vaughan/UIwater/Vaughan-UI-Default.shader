Shader "Custom/UIvoroni"
{
    Properties
    {
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Power("voronisize", Float) = 6

		
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

		Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest[unity_GUIZTestMode]
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask[_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
			#pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

			#pragma multi_compile_local _ UNITY_UI_CLIP_RECT
			#pragma multi_compile_local _ UNITY_UI_ALPHACLIP

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO

			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _TextureSampleAdd;
			fixed4 _Color;
			float4 _ClipRect;
			float _Power;

			v2f vert(appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.worldPosition = v.vertex;
				o.color = v.color;
				
				return o;
			}

			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3)))) * 43458.5453);
			}

			float calcvoroni(float minDist, v2f i)
			{
				float2 texcoord = i.texcoord;
				texcoord *= 10.0f; //Scaling amount (larger number more cells can be seen, 6.0 defualt)
				float2 iuv = floor(texcoord); //gets integer values no floating point
				float2 fuv = frac(texcoord); // gets only the fractional part

				for (int y = -1; y <= 1; y++)
				{
					for (int x = -1; x <= 1; x++)
					{
						// Position of neighbour on the grid
						float2 neighbour = float2(float(x), float(y));
						// Random position from current + neighbour place in the grid
						float2 pointv = random2(iuv + neighbour);
						// Move the point with time
						pointv = 0.5 + 0.5 * sin(_Time.z + 6.2236 * pointv);//each point moves in a certain way
						//pointv.y = frac(_Time.x);
						// Vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Distance to the point
						float dist = length(diff);
						// Keep the closer distance
						minDist = min(minDist, dist);
					}
				}

				return minDist;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//START voroni 
				float minDist = 1.0;  // minimun distance
				minDist = calcvoroni(minDist, i);
				fixed4 col = fixed4(0, 0, 0, _Color.a);
				// Draw the min distance (distance field)
				col.rgb += -1 * (minDist * minDist * minDist * minDist * minDist * _Power) ; // squared it to to make edges look sharper
				//END voroni

				//START sprite
				half4 color = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
#ifdef UNITY_UI_CLIP_RECT
				color.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
#endif
#ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
#endif
				//END sprite


				half4 testcolor = half4(0.0f, 0.0f, 0.0f, 1.0f);
				half4 testcolor2 = half4(1.0f, 1.0f, 1.0f, 1.0f);

				float2 fromCenter = abs(i.texcoord - (0.5f, 0.5f));
				//float2 fromEdge = fromCenter - 0.5f;

				float dist2 = abs((((i.texcoord.x - 0.5f)) * (i.texcoord.y - 0.5f)) * 0.02f);

				float dist = clamp(0.0f, 0.0f, 1.0f);
				//fromEdge.x /= length(float2(ddx(i.vertex.x), ddy(i.vertex.x)));
				//fromEdge.y /= length(float2(ddx(i.vertex.y), ddy(i.vertex.y)));
				//float distance = abs(min(max(fromEdge.x, fromEdge.y), 0.0f) + length(max(fromEdge, 0.0f)));

				testcolor = lerp(testcolor, testcolor2, dist2);

				//half4 testcolor =  half4(0.0f, 0.0f, 0.0f, 1.0f);
				//return testcolor;


				// START add
				testcolor *= half4(100.1f, 100.1f, 100.1f, 1.0f);


				col.rgb *= testcolor.r;

				col.rgb += color.rgb;
				col *= _Color;
				col.a *= color.a;
				//END add

				col.x = clamp(col.x, 0.0f, 1.0f);
				col.y = clamp(col.y, 0.0f, 1.0f);
				col.z = clamp(col.z, 0.0f, 1.0f);
				col.a = clamp(col.a, 0.0f, 1.0f);


				return col;
			}
        ENDCG
        }
    }
}
