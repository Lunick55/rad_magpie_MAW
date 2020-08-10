// Grass Shader v2.0
Shader "Hidden/Keronius/DrawPath"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Brush color", Color) = (1, 0, 0, 0)
		_PosToDraw ("Coordinate to draw", Vector) = (0, 0, 0, 0)
		_BrushSize ("Size", Range(1, 500)) = 1
		_Stregnth ("Strength of brush", Range(0,1)) = 1
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
				float4 vertex : SV_POSITION;
			};
			
			fixed4 _Color;
			fixed4 _PosToDraw;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _BrushSize;
			float _BrushStrength;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 previousColor = tex2D(_MainTex, i.uv);
				float draw = pow(saturate(1 - distance(i.uv, _PosToDraw.xy)), 500 / _BrushSize);
				fixed4 drawColor = _Color * (draw * _BrushStrength);

				return saturate(previousColor + drawColor);
			}
			ENDCG
		}
	}
}