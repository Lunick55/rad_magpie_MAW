

Shader "ToonBetter" {
 
    Properties {
        [Header (Base Parameters)]
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color("Color", Color) = (0.5, 0.65, 1, 1)
        _Emission("Emission", Color) = (0,0,0,1)

        [Header(Lighting Parameters)]
        _ShadowTint ("Shadow Color", Color) = (0.5,0.5,0.5,1)
        _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
        _RimAmount ("Rim Amount", Range(0,1)) = 0.5
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

    }
 
    SubShader {

        Tags {"RenderType" = "Opaque" "Queue" = "Geometry"}
        
        CGPROGRAM
        #pragma target 3.0
        #pragma surface surf Stepped addshadow

        sampler2D _MainTex;
        fixed4 _Color;
        float4 _RimColor;
        float _RimAmount;
        float _RimThreshold;

        half3 _Emission;

        float3 _ShadowTint;

        float4 LightingStepped (SurfaceOutput s, float3 lightDir, half3 viewDir, float shadowAttenuation)
        {
            float towardsLight = dot(s.Normal, lightDir);
            float towardsLightChange = fwidth(towardsLight);
            float lightIntensity = smoothstep(0, towardsLightChange, towardsLight);
        #ifdef USING_DIRECTIONAL_LIGHT
            float attenuationChange = fwidth(shadowAttenuation) * 0.5;
            float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, shadowAttenuation);
        #else
            float attenuationChange = fwidth(shadowAttenuation);
            float shadow = smoothstep(0, attenuationChange, shadowAttenuation);
        #endif
            lightIntensity = lightIntensity * shadow;

            float3 shadowColor = s.Albedo * _ShadowTint;
            float4 color;
            color.rgb = lerp(shadowColor, s.Albedo, lightIntensity) * _LightColor0.rgb;
            color.a = s.Alpha;
            return color;

        }

        //Structs
        struct Input 
        {
            float2 uv_MainTex;
            float3 viewDir;
        };

            //Surface Shader
            void surf (Input i, inout SurfaceOutput o) 
            {
                float NdotL = dot(_WorldSpaceLightPos0, o.Normal);
                float lightIntensity = smoothstep(0, 0.01, NdotL);


                fixed4 col = tex2D(_MainTex, i.uv_MainTex);
                col *= _Color;
                
                //rim lighting
                
                float4 rimDot = 1 - dot(i.viewDir, o.Normal);
                //float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor;
                col += rim;
                

                o.Albedo = col.rgb;
                o.Alpha = col.a;
                o.Emission = _Emission;
            }

        ENDCG
        }
     
    Fallback "Standard"
} 
