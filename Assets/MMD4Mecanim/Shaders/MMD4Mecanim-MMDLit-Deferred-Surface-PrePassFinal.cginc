#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_PREPASSFINAL
#include "UnityCG.cginc"
#include "Lighting.cginc"

#define INTERNAL_DATA
#define WorldReflectionVector(data,normal) data.worldRefl
#define WorldNormalVector(data,normal) normal

#include "MMD4Mecanim-MMDLit-Deferred-Surface-Lighting.cginc"

struct v2f_surf
{
	float4 pos : SV_POSITION;
	float2 pack0 : TEXCOORD0;
	float4 screen : TEXCOORD1;
	half3 normal : TEXCOORD2;
	half3 mmd_uvwSphere : TEXCOORD3;
	#ifdef LIGHTMAP_OFF
	half3 vlight : TEXCOORD4;
	half3 viewDir : TEXCOORD5;
	#else
	float2 lmap : TEXCOORD4;
	#ifdef DIRLIGHTMAP_OFF
	float4 lmapFadePos : TEXCOORD5;
	#else
	half3 viewDir : TEXCOORD5;
	#endif
	#endif
};

#ifndef LIGHTMAP_OFF
float4 unity_LightmapST;
#endif
float4 _MainTex_ST;

inline v2f_surf vert_core(appdata_full v)
{
	v2f_surf o;
	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	float3 worldN = mul((float3x3)_Object2World, SCALED_NORMAL);
	o.normal = worldN;
	o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
	#ifdef SPHEREMAP_OFF
	o.mmd_uvwSphere = 0;
	#else
	half3 norm = normalize(mul((float3x3)UNITY_MATRIX_MV, v.normal));
	half3 eye = normalize(mul(UNITY_MATRIX_MV, v.vertex).xyz);
	o.mmd_uvwSphere = reflect(eye, norm);
	#endif
	o.screen = ComputeScreenPos(o.pos);

	#ifndef LIGHTMAP_OFF
	o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	#ifdef DIRLIGHTMAP_OFF
	o.lmapFadePos.xyz = (mul(_Object2World, v.vertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w;
	o.lmapFadePos.w = (-mul(UNITY_MATRIX_MV, v.vertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w);
	#endif
	#else
	o.vlight = ShadeSH9(float4(worldN, 1.0));
	#endif

	#ifndef DIRLIGHTMAP_OFF
	TANGENT_SPACE_ROTATION;
	o.viewDir = mul(rotation, ObjSpaceViewDir(v.vertex));
	#else
	#ifdef LIGHTMAP_OFF
	o.viewDir = (half3)WorldSpaceViewDir(v.vertex);
	#endif
	#endif
	return o;
}

v2f_surf vert_fast(appdata_full v)
{
	return vert_core(v);
}

v2f_surf vert_surf(appdata_full v)
{
	return vert_core(v);
}

sampler2D _LightBuffer;
#if defined (SHADER_API_XBOX360) && defined (HDR_LIGHT_PREPASS_ON)
sampler2D _LightSpecBuffer;
#endif
#ifndef LIGHTMAP_OFF
sampler2D unity_Lightmap;
sampler2D unity_LightmapInd;
float4 unity_LightmapFade;
#endif

inline half4 frag_core(v2f_surf IN, half4 albedo)
{
	half4 light = tex2Dproj(_LightBuffer, UNITY_PROJ_COORD(IN.screen));
	#if defined (SHADER_API_GLES) || defined (SHADER_API_GLES3)
	light = max(light, half4(0.001));
	#endif
	#ifndef HDR_LIGHT_PREPASS_ON
	light = -log2(light);
	#endif
	#if defined (SHADER_API_XBOX360) && defined (HDR_LIGHT_PREPASS_ON)
	//light.w = tex2Dproj(_LightSpecBuffer, UNITY_PROJ_COORD(IN.screen)).r;
	#endif

	#ifndef LIGHTMAP_OFF
	// Pending: Sphere for Lightmap.(Now disabled)
	#ifdef DIRLIGHTMAP_OFF
	half4 lmtex = tex2D(unity_Lightmap, IN.lmap.xy);
	half4 lmtex2 = tex2D(unity_LightmapInd, IN.lmap.xy);
	half lmFade = length(IN.lmapFadePos) * unity_LightmapFade.z + unity_LightmapFade.w;
	half3 lmFull = MMDLit_DecodeLightmap(lmtex);
	half3 lmIndirect = MMDLit_DecodeLightmap(lmtex2);
	half3 lm = lerp (lmIndirect, lmFull, saturate(lmFade));
	light.rgb += lm;
	half3 c = MMDLit_Lightmap(
		(half3)albedo,
		(half3)light);
	#else
	half4 lmtex = tex2D(unity_Lightmap, IN.lmap.xy);
	half4 lmIndTex = tex2D(unity_LightmapInd, IN.lmap.xy);
	//half4 lm = MMDLit_DirLightmap(lmtex, lmIndTex, IN.normal, 0);
	//light += lm;
	half3 c = MMDLit_DirLightmap(
		(half3)albedo,
		IN.normal,
		lmtex,
		lmIndTex,
		normalize(IN.viewDir),
		(half3)light,
		0);
	#endif
	#else
	light.rgb += IN.vlight;
	half3 c = MMDLit_Lighting(
		(half3)albedo,
		IN.mmd_uvwSphere,
		IN.normal,
		_DefLightColor0,
		_DefLightDir0,
		IN.viewDir,
		light);
	#endif

	return half4(c, albedo.a);
}

half4 frag_fast(v2f_surf IN) : COLOR
{
	half4 albedo = MMDLit_GetAlbedo(IN.pack0.xy);
	return frag_core(IN, albedo);
}

half4 frag_surf(v2f_surf IN) : COLOR
{
	half4 albedo = MMDLit_GetAlbedo(IN.pack0.xy);
	albedo.a *= _Color.a;
	return frag_core(IN, albedo);
}
