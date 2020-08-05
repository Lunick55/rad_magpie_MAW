// Copyright © 2018 Camilo Andres Carrillo Niño
// All Rights Reserved
// This product is protected by copyright and distributed under licenses restricting copying, distribution, and decompilation.

// Grass Shader v1.1

Shader "Keronius/Environment/Grass" {
	Properties {
		[Enum(Pointed, 0, Rectangular, 1)] _GrassShapeType("Grass Shape Type", Float) = 0

		_Size("Grass Size", Range(0, 10)) = 0.5
		_LowerWidth("Lower Width", Range(0, 1)) = 0.025
		_UpperWidth("Upper Width", Range(0, 1)) = 0.025

		_LeafCuts("Grass Subdivisions", Range(1, 10)) = 1
		_VisibleRange("Visible range", Range(1,1000)) = 75
		_LODDistance("Detail Distance", Range(0.1, 1000)) = 50
		_DetailReductionFactor("Detail Reduction Factor", Range(0, 0.99)) = 0.5

		[Enum(x1, 0, x2, 1)] _LeafMultiplier("Leafs multiplier", Float) = 0

		[Toggle] _EnableBasePlane("Active Base Plane", Float) = 1

		// Gradient Color
		[Toggle] _EnableGradient("Active Gradient", Float) = 0		
		_BaseColor("Base Color", Color) = (0, 0, 0, 1)
		_MiddleColor("Mid Color", Color) = (1, 1, 1, 1)
		_TopColor("Top Color", Color) = (1, 1, 1, 1)
		_GradientCenter("Middle Gradient", Range(0.001, 0.999)) = 0.5

		// Noise
		[Toggle] _EnableNoise("Active Noise", Float) = 0

		_NoiseTex("Noise Texture", 2D) = "white" { }
		_NoiseStrength("Noise strength", Range(-0.5, 0.5)) = 1
		[Toggle] _EnableRandomLength("Random Length", Float) = 0
		_LengthIntensity("Length Intensity", Range(1, 5)) = 1
		_LengthMaxDiference("Length max range", Range(0, 1)) = 0

		// Wind
		[Toggle] _EnableWind("Active Wind", Float) = 0

		_WindTex("Wind Texture", 2D) = "black" { }
		[Enum(All, 0, Reds, 1, Greens, 2, Blues, 3)] _WindTexCol("Colors to look for", Float) = 0

		_WindStrength("Wind strength", Range(-1, 1)) = 1
		_WindSpeed("Wind speed", Float) = 0		

		// Mask
		[Toggle] _EnableMask("Enable Mask", Float) = 0
		[Toggle] _AffectBase("Enable Mask", Float) = 0
		_MaskTex("Mask Texture", 2D) = "white" {}
 
		// Gravity
		[Toggle] _EnablePressure("Enable pressure", Float) = 0

		_PressureTex("Pressure Texture", 2D) = "white" { }
		_PressureStrength("Pressure strenght", Range(0, 1)) = 1
	}

	SubShader {
		Cull off

		Pass {
			Tags {"LightMode"="ForwardBase"}

			CGPROGRAM
			#pragma target 5.0
			#pragma vertex vertexShader
			#pragma geometry geometryShader
			#pragma fragment fragmentShader
			
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

			#include "Grass.cginc"

			ENDCG
		}
		
		Pass {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
			#pragma target 5.0
            #pragma vertex vertex
			#pragma geometry geometry
            #pragma fragment fragment
			
            #include "UnityCG.cginc"

            #pragma multi_compile_shadowcaster

           	#include "GrassShadows.cginc"

            ENDCG
        }
	}
	
    CustomEditor "GrassGUI"
}
