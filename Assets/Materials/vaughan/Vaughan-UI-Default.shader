// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/Vaughan"
{
    Properties
    {

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

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			float2 random2(float2 p)
			{
				return frac(sin(float2(dot(p,float2(117.12,341.7)),dot(p,float2(269.5,123.3)))) * 43458.5453);
			}
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,1);
				float2 uv = i.uv;
				uv *= 6.0; //Scaling amount (larger number more cells can be seen)
				float2 iuv = floor(uv); //gets integer values no floating point
				float2 fuv = frac(uv); // gets only the fractional part
				float minDist = 1.0;  // minimun distance
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
																		// Vector between the pixel and the point
						float2 diff = neighbour + pointv - fuv;
						// Distance to the point
						float dist = length(diff);
						// Keep the closer distance
						minDist = min(minDist, dist);
					}
				}
				// Draw the min distance (distance field)
				col.r += minDist * minDist; // squared it to to make edges look sharper
				return col;
			}
        ENDCG
        }
    }
}
