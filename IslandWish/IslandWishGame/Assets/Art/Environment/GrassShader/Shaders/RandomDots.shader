// Grass Shader v2.0
Shader "Hidden/Keronius/RandomDots"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
			
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _DotsAmount;
			float _DotsOpacity;

			float random(float3 coord)
			{
				return frac(sin(dot(coord.xyz ,float3(12.9898,78.233,45.5432))) * 43758.5453);
			}
						
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float randValue = ceil(random(float3(i.uv.x, i.uv.y, 0) * _Time.x) - (1 - _DotsAmount));
				return saturate(col - ( randValue * _DotsOpacity));
			}
			ENDCG
		}
	}
}