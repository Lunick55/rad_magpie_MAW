Shader "Roystan/ToonTransparent"
{
	Properties
	{
		_Color("Color", Color) = (0.5, 0.65, 1, 1)
		_MainTex("Main Texture", 2D) = "white" {}	
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range (0, 1)) = 0.716
		[HDR]
		_Emission("Emission", Color) = (0,0,0,1)

        _AlphaThreshold ("Alpha Cutoff", Range(0,1)) = 0.5
	}
	SubShader
	{
		Pass
		{
			Cull Off
			Tags
			{
				"LightMode" = "ForwardBase"
				//"PassFlags" = "OnlyDirectional"
			    "Queue" = "Transparent"
                "RenderType" = "Transparent"
                "PassFlags" = "OnlyDirectional"
                //"Queue"="AlphaTest" "RenderType"="TransparentCutout"
                //"IgnoreProjector"="True" 
			}
            //AlphaToMask On
            //Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float3 normal : NORMAL;
				float4 vertex : POSITION;				
				float4 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 worldNormal : NORMAL;
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 viewDir : TEXCOORD1;
				SHADOW_COORDS(2)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.viewDir = WorldSpaceViewDir(v.vertex);

                o.worldNormal *= lerp (1.0, -1.0, dot (o.viewDir, o.worldNormal) < 0.0);

				TRANSFER_SHADOW(o)
				return o;
			}
			
			float4 _Color;
			float4 _AmbientColor;
			float _Glossiness;
			float4 _SpecularColor;
			float4 _RimColor;
			float _RimAmount;
			float4 _Emission;
            half _AlphaThreshold;

			float4 frag (v2f i) : SV_Target
			{
                float4 sample = tex2D(_MainTex, i.uv);

                 if (sample.a < _AlphaThreshold)
                 {
                     discard;
                     return 0;
                 }
                
				float3 normal = normalize(i.worldNormal);
				float NdotL = dot(_WorldSpaceLightPos0, normal);

				float shadow = SHADOW_ATTENUATION(i);

				float lightIntensity = smoothstep(0,0.01,NdotL * shadow);
				float4 light = lightIntensity * _LightColor0;
				float3 viewDir = normalize(i.viewDir);

				float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
				float NdotH = dot(normal, halfVector);

				float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
				float specularIntensitySmooth = smoothstep(0.005,0.01, specularIntensity);
				float4 specular = specularIntensitySmooth * _SpecularColor;

				float4 rimDot = 1 - dot(viewDir, normal);
				float rimIntensity = rimDot * NdotL;
				rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
				float4 rim = rimIntensity * _RimColor;

				//float4 sample = tex2D(_MainTex, i.uv);

				return (_Color * sample * (_AmbientColor + light + specular + rim) ) + (_Emission);
			}
			ENDCG
		}

		//UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
        Pass
        {
            Name "ShadowCaster"
            Tags {"LightMode" = "ShadowCaster"}

            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #pragma milti_compile_instancing

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _AlphaThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv * _MainTex_ST.xy) + _MainTex_ST.zw;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float a = tex2D (_MainTex, i.uv).a;
                if (a < _AlphaThreshold)
                    discard;

                return 0;
            }
            ENDCG
        }
	}
}