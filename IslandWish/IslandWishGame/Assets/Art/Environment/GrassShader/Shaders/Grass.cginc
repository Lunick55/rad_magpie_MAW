// Copyright © 2018 Camilo Andres Carrillo Niño
// All Rights Reserved
// This product is protected by copyright and distributed under licenses restricting copying, distribution, and decompilation.

// Grass Shader v1.1

// --- Variables ---
float _GrassShapeType;

float _Size;
float _LowerWidth;
float _UpperWidth;

float _LeafCuts;
int _LeafMultiplier;

float _VisibleRange;
float _LODDistance;
float _DetailReductionFactor;

float _EnableBasePlane;

// Gradient
float _EnableGradient;
fixed4 _BaseColor;
fixed4 _MiddleColor;
fixed4 _TopColor;
float _GradientCenter;

// Noise
float _EnableNoise;

sampler2D _NoiseTex;
float4 _NoiseTex_ST;
float _NoiseStrength;
float _EnableRandomLength;
float _LengthIntensity;
float _LengthMaxDiference;

// Wind
float _EnableWind;

sampler2D _WindTex;
float4 _WindTex_ST;
float _WindStrength;
float _WindSpeed;
float _WindTexCol;

// Mask
float _EnableMask;

float _AffectBase;
sampler2D _MaskTex;
float4 _MaskTex_ST;

// Pressure
float _EnablePressure;

sampler2D _PressureTex;
float4 _PressureTex_ST;
float _PressureStrength;

//Extra vars
static
const float PI_VALUE = float(3.14159265);
// --------------------------------------------

struct v2g {
	float4 pos: POSITION;
	float3 normal: NORMAL;
	float2 uv: TEXCOORD0;

	SHADOW_COORDS(1)
};

struct g2f {
	float4 pos: SV_POSITION;
	float3 normal: NORMAL;

	float nDistance: float;
	LIGHTING_COORDS(1, 2)
};

v2g vertexShader(appdata_base v) {
	v2g o;
	o.pos = v.vertex;
	o.normal = v.normal;
	o.uv = TRANSFORM_TEX(v.texcoord, _PressureTex);

	TRANSFER_SHADOW(o)
	return o;
}

float ChangeRange(float iA, float fA, float iB, float fB, float n) {
	float res = (fB - iB) / (fA - iA);
	return (iB + (res) * (n - iA));
}

float4 TransferVertexPos(float3 nPos) {
	return UnityObjectToClipPos(nPos);
}

void GenerateLeaf(triangle v2g IN[3], float4 TriangleMiddle, float3 TriangleNorm, float2 TriangleUv, inout TriangleStream < g2f > tristream) {
	g2f o = (g2f) 0;

	float4 VertexA = IN[0].pos;
	float4 VertexB = IN[1].pos;
	float4 VertexC = IN[2].pos;

	float4 TriangleCenter = TriangleMiddle;

	float3 normalFace = TriangleNorm;
	float2 uvCenter = TriangleUv;

	// Mask
	float CanPaintLeaf_Mask = 1;
	if (_EnableMask == 1) {
		float3 mask = tex2Dlod(_MaskTex, float4(_MaskTex_ST.xy * uvCenter.xy + _MaskTex_ST.zw, 0, 0)).xyz;
		float mF = mask;
		CanPaintLeaf_Mask = clamp(mF, 0, 1);
	}
	// End Mask

	if (_EnableBasePlane == 1 && (_AffectBase == 0 || (_AffectBase == 1 && CanPaintLeaf_Mask == 1))) {		
		o.pos = TransferVertexPos(VertexA);
		o.normal = normalFace;
		o.nDistance = 0;
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		tristream.Append(o);

		o.pos = TransferVertexPos(VertexB);
		o.normal = normalFace;
		o.nDistance = 0;
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		tristream.Append(o);

		o.pos = TransferVertexPos(VertexC);
		o.normal = normalFace;
		o.nDistance = 0;
		TRANSFER_VERTEX_TO_FRAGMENT(o);
		tristream.Append(o);

		tristream.RestartStrip();
	}

	if (CanPaintLeaf_Mask == 1) {
		float3x3 noiseRotationMatrix = float3x3(1.0f, 0, 0,
			0, 1.0f, 0,
			0, 0, 1.0f);

		float distFromCamera = length(ObjSpaceViewDir(IN[0].pos));
		float finalLeafCuts = _LeafCuts;

		if (distFromCamera > _LODDistance) {
			finalLeafCuts = round((_LeafCuts) * (1 - _DetailReductionFactor)) + 1;
		}

		if (distFromCamera > _VisibleRange) {
			finalLeafCuts = 0;
		}

		if (finalLeafCuts > 0) {
			float HeightPerCut = (_Size / finalLeafCuts);

			float3 noiseNormal = 0;

			if (_EnableNoise == 1) {
				float3 noise = tex2Dlod(_NoiseTex, float4(_NoiseTex_ST.xy * uvCenter.xy + _NoiseTex_ST.zw, 0, 0)).xyz;
				noiseNormal = noise * _NoiseStrength / 2;

				float noiseRotationAngel = radians(ChangeRange(0, 1, 0, 360, noise.r));

				float3 row0 = float3(cos(noiseRotationAngel), 0.0, sin(noiseRotationAngel));
				float3 row1 = float3(0.0, 1.0, 0.0);
				float3 row2 = float3(-sin(noiseRotationAngel), 0.0, cos(noiseRotationAngel));

				noiseRotationMatrix = float3x3(row0, row1, row2);

				if (_EnableRandomLength == 1) {
					float minInit = 0;
					float maxFina = _Size * _LengthIntensity;

					float maxSize = _Size * _LengthIntensity;
					float minSize = maxSize * (1 - _LengthMaxDiference);

					float dif = abs(maxSize - _Size);

					float finalLength = maxSize * _LengthIntensity * noise.r;
					finalLength = clamp(finalLength, minSize, maxSize);

					finalLength = ChangeRange(minInit, maxFina, minSize, maxSize, finalLength);
					HeightPerCut = finalLength / finalLeafCuts;
				}
			}

			float3 windNormal = 0;
			if (_EnableWind == 1) {
				float WindDiff = (_Time.w / 10) * _WindSpeed;

				float3 wind = normalize(tex2Dlod(_WindTex, float4(_WindTex_ST.xy * uvCenter.xy + _WindTex_ST.zw + WindDiff, 0, 0)).xyz);

				if (_WindTexCol == 1) {
					windNormal = sin(wind.r) * _WindStrength;
				} else if (_WindTexCol == 2) {
					windNormal = sin(wind.g) * _WindStrength;
				} else if (_WindTexCol == 3) {
					windNormal = sin(wind.b) * _WindStrength;
				} else {
					windNormal = sin(wind) * _WindStrength;
				}
			}			

			float3 pressureNormal = 1;

			if (_EnablePressure == 1) {
				float3 pressure = tex2Dlod(_PressureTex, float4(_PressureTex_ST.xy * uvCenter.xy + _PressureTex_ST.zw, 0, 0)).xyz;

				pressureNormal = pressure * (_PressureStrength);
				pressureNormal = 1 - (clamp(pressureNormal, 0, 1));
			}

			float diff = _UpperWidth - _LowerWidth;
			float sumThick = diff / finalLeafCuts;			

			for (int i = 0; i < finalLeafCuts; i++) {

				if (_GrassShapeType == 0 && (i + 1) == finalLeafCuts) {
					normalFace = normalize((normalFace + windNormal + noiseNormal)) * pressureNormal;
					float3 nextNormalFace = normalize((normalFace + windNormal + noiseNormal)) * pressureNormal;

					float3 widthDir = float3(normalize((VertexB - VertexA).xyz)) * (_LowerWidth + (i * sumThick));

					if (_EnableNoise == 1) {
						widthDir = mul(noiseRotationMatrix, widthDir);
					}

					float3 LeftLeafA = TriangleCenter - (widthDir) + (normalFace * i * HeightPerCut);
					float3 LeftLeafB = TriangleCenter + (nextNormalFace * (i + 1) * HeightPerCut);
					float3 LeftLeafC = TriangleCenter + (widthDir) + (normalFace * i * HeightPerCut);

					o.pos = TransferVertexPos(LeftLeafA);
					o.normal = normalFace;
					o.nDistance = i * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(LeftLeafB);
					o.normal = nextNormalFace;
					o.nDistance = (i + 1) * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(LeftLeafC);
					o.normal = normalFace;
					o.nDistance = i * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					tristream.RestartStrip();
				} else {
					normalFace = normalize((normalFace + windNormal + noiseNormal)) * pressureNormal;
					float3 nextNormalFace = normalize((normalFace + windNormal + noiseNormal)) * pressureNormal;

					float3 widthDir = float3(normalize((VertexB - VertexA).xyz)) * (_LowerWidth + (i * sumThick));
					float3 nextWidthDir = float3(normalize((VertexB - VertexA).xyz)) * (_LowerWidth + ((i + 1) * sumThick));

					if (_EnableNoise == 1) {
						widthDir = mul(noiseRotationMatrix, widthDir);
						nextWidthDir = mul(noiseRotationMatrix, nextWidthDir);
					}

					float3 LeftLeafA = TriangleCenter - (widthDir) + (normalFace * i * HeightPerCut);
					float3 LeftLeafB = TriangleCenter - (nextWidthDir) + (nextNormalFace * (i + 1) * HeightPerCut);
					float3 LeftLeafC = TriangleCenter + (widthDir) + (normalFace * i * HeightPerCut);

					float3 RightLeafA = TriangleCenter + (widthDir) + (normalFace * i * HeightPerCut);
					float3 RightLeafB = TriangleCenter - (nextWidthDir) + (nextNormalFace * (i + 1) * HeightPerCut);
					float3 RightLeafC = TriangleCenter + (nextWidthDir) + (nextNormalFace * (i + 1) * HeightPerCut);

					o.pos = TransferVertexPos(LeftLeafA);
					o.normal = normalFace;
					o.nDistance = i * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(LeftLeafB);
					o.normal = nextNormalFace;
					o.nDistance = (i + 1) * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(LeftLeafC);
					o.normal = normalFace;
					o.nDistance = i * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					tristream.RestartStrip();

					o.pos = TransferVertexPos(RightLeafA);
					o.normal = normalFace;
					o.nDistance = i * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(RightLeafB);
					o.normal = nextNormalFace;
					o.nDistance = (i + 1) * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					o.pos = TransferVertexPos(RightLeafC);
					o.normal = nextNormalFace;
					o.nDistance = (i + 1) * HeightPerCut;
					TRANSFER_VERTEX_TO_FRAGMENT(o);
					tristream.Append(o);

					tristream.RestartStrip();
				}
			}
		}
	}
}

void DoubleLeafs(triangle v2g IN[3], inout TriangleStream < g2f > tristream) {
	float4 VertexA = IN[0].pos;
	float4 VertexB = IN[1].pos;
	float4 VertexC = IN[2].pos;

	float3 NormalA = IN[0].normal;
	float3 NormalB = IN[1].normal;
	float3 NormalC = IN[2].normal;

	float2 UvA = IN[0].uv;
	float2 UvB = IN[1].uv;
	float2 UvC = IN[2].uv;

	float4 VertexAB = (VertexA + VertexB) / 2.f;
	float3 NormalAB = (NormalA + NormalB) / 2.f;
	float2 UvAB = (UvA + UvB) / 2.f;

	float4 VertexBC = (VertexB + VertexC) / 2.f;
	float3 NormalBC = (NormalB + NormalC) / 2.f;
	float2 UvBC = (UvB + UvC) / 2.f;

	float4 VertexAC = (VertexA + VertexC) / 2.f;
	float3 NormalAC = (NormalA + NormalC) / 2.f;
	float2 UvAC = (UvA + UvC) / 2.f;

	if (distance(VertexA, VertexB) > distance(VertexA, VertexC) && distance(VertexA, VertexB) > distance(VertexB, VertexC)) {
		float4 Triangle_1 = (VertexC + VertexAB + VertexA) / 3.f;
		float3 NormalTriangle_1 = (NormalC + NormalAB + NormalA) / 3.f;
		float2 UvTriangle_1 = (UvC + UvAB + UvA) / 3.f;

		float4 Triangle_2 = (VertexC + VertexAB + VertexB) / 3.f;
		float3 NormalTriangle_2 = (NormalC + NormalAB + NormalB) / 3.f;
		float2 UvTriangle_2 = (UvC + UvAB + UvB) / 3.f;

		GenerateLeaf(IN, Triangle_1, NormalTriangle_1, UvTriangle_1, tristream);
		GenerateLeaf(IN, Triangle_2, NormalTriangle_2, UvTriangle_2, tristream);
	} else if (distance(VertexB, VertexC) > distance(VertexA, VertexB) && distance(VertexB, VertexC) > distance(VertexA, VertexC)) {
		float4 Triangle_1 = (VertexA + VertexBC + VertexB) / 3.f;
		float3 NormalTriangle_1 = (NormalA + NormalBC + NormalB) / 3.f;
		float2 UvTriangle_1 = (UvA + UvBC + UvB) / 3.f;

		float4 Triangle_2 = (VertexA + VertexBC + VertexC) / 3.f;
		float3 NormalTriangle_2 = (NormalA + NormalBC + NormalC) / 3.f;
		float2 UvTriangle_2 = (UvA + UvBC + UvC) / 3.f;

		GenerateLeaf(IN, Triangle_1, NormalTriangle_1, UvTriangle_1, tristream);
		GenerateLeaf(IN, Triangle_2, NormalTriangle_2, UvTriangle_2, tristream);
	} else {
		float4 Triangle_1 = (VertexB + VertexAC + VertexC) / 3.f;
		float3 NormalTriangle_1 = (NormalB + NormalAC + NormalC) / 3.f;
		float2 UvTriangle_1 = (UvB + UvAC + UvC) / 3.f;

		float4 Triangle_2 = (VertexB + VertexAC + VertexA) / 3.f;
		float3 NormalTriangle_2 = (NormalB + NormalAC + NormalA) / 3.f;
		float2 UvTriangle_2 = (UvB + UvAC + UvA) / 3.f;

		GenerateLeaf(IN, Triangle_1, NormalTriangle_1, UvTriangle_1, tristream);
		GenerateLeaf(IN, Triangle_2, NormalTriangle_2, UvTriangle_2, tristream);
	}
}

void GenerateAllLeafs(triangle v2g IN[3], inout TriangleStream < g2f > tristream) {
	float4 VertexA = IN[0].pos;
	float4 VertexB = IN[1].pos;
	float4 VertexC = IN[2].pos;

	float3 NormalA = IN[0].normal;
	float3 NormalB = IN[1].normal;
	float3 NormalC = IN[2].normal;

	float2 UvA = IN[0].uv;
	float2 UvB = IN[1].uv;
	float2 UvC = IN[2].uv;

	float4 Triangle_1 = (VertexA + VertexB + VertexC) / 3.f;
	float3 NormalTriangle_1 = (NormalA + NormalB + NormalC) / 3.f;
	float2 UvTriangle_1 = (UvA + UvB + UvC) / 3.f;

	float distFromCamera = length(ObjSpaceViewDir(IN[0].pos));

	if (distFromCamera > _LODDistance) {
		GenerateLeaf(IN, Triangle_1, NormalTriangle_1, UvTriangle_1, tristream);
	} else {
		if (_LeafMultiplier == 0) {
			GenerateLeaf(IN, Triangle_1, NormalTriangle_1, UvTriangle_1, tristream);
		} else if (_LeafMultiplier == 1) {
			DoubleLeafs(IN, tristream);
		}
	}
}

[maxvertexcount(85)]
void geometryShader(triangle v2g IN[3], inout TriangleStream < g2f > tristream) {
	GenerateAllLeafs(IN, tristream);
}

fixed4 fragmentShader(g2f i): SV_Target {
	float fSize = _Size;

	if (_EnableNoise == 1 && _EnableRandomLength == 1) {
		fSize *= _LengthIntensity;
	}

	if (_EnableGradient == 1) {
		float FinalPos = i.nDistance;
		float FinalMid = _GradientCenter;
		FinalMid *= fSize;

		fixed4 col = lerp(_BaseColor, _MiddleColor, FinalPos / FinalMid) * step(FinalPos, FinalMid);
		col += lerp(_MiddleColor, _TopColor, (FinalPos - FinalMid) / (fSize - FinalMid)) * step(FinalMid, FinalPos);

		float3 worldNormal = UnityObjectToWorldNormal(i.normal);

		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		fixed3 diff = nl * _LightColor0.rgb;
		fixed3 ambientL = ShadeSH9(half4(worldNormal, 1));

		fixed shadow = SHADOW_ATTENUATION(i);

		fixed3 lighting = diff * shadow + ambientL;

		col.rgb *= lighting;
		return col;
	} else {
		fixed4 col = _BaseColor;

		float3 worldNormal = UnityObjectToWorldNormal(i.normal);

		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		fixed3 diff = nl * _LightColor0.rgb;
		fixed3 ambientL = ShadeSH9(half4(worldNormal, 1));

		fixed shadow = SHADOW_ATTENUATION(i);

		fixed3 lighting = diff * shadow + ambientL;

		col.rgb *= lighting;
		return col;
	}
}