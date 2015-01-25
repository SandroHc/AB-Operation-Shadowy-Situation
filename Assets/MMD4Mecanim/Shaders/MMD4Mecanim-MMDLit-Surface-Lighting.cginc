#include "MMD4Mecanim-MMDLit-Lighting.cginc"

half4 _Color;
half4 _Specular;
half4 _Ambient;
half _Shininess;
half _ShadowLum;
half _SelfShadow;

sampler2D _MainTex;
sampler2D _ToonTex;

half _AddLightToonCen;
half _AddLightToonMin;

half4 _Emissive;

samplerCUBE _SphereCube;
half _SphereAdd; // Legacy
half _SphereMul; // Legacy

half4 _TempDiffuse;
half4 _TempAmbient;
half4 _TempAmbientL;

inline void MMDLit_GetBaseColor(
	half3 albedo,
	half3 tempDiffuse,
	half3 uvw_Sphere,
	out half3 baseC,
	out half3 baseD)
{
	#ifdef SPHEREMAP_LEGACY
	half3 sph = (half3)texCUBE(_SphereCube, uvw_Sphere);
	half3 sphMul = sph * _SphereMul + (1.0 - _SphereMul);
	half3 sphAdd = sph * _SphereAdd;
	baseC = albedo * sphMul;
	baseD = baseC * tempDiffuse + sphAdd; // for Diffuse only.
	baseC += sphAdd;
	#elif defined(SPHEREMAP_MUL)
	half3 sph = (half3)texCUBE(_SphereCube, uvw_Sphere);
	baseC = albedo * sph;
	baseD = baseC * tempDiffuse; // for Diffuse only.
	#elif defined(SPHEREMAP_ADD)
	half3 sph = (half3)texCUBE(_SphereCube, uvw_Sphere);
	baseC = albedo + sph;
	baseD = albedo * tempDiffuse + sph; // for Diffuse only.
	#else
	baseC = albedo;
	baseD = albedo * tempDiffuse;
	#endif
}

inline half4 MMDLit_GetAlbedo(float2 uv_MainTex)
{
	return (half4)tex2D(_MainTex, uv_MainTex);
}

inline half MMDLit_GetToolRefl(half NdotL)
{
	return NdotL * 0.5 + 0.5;
}

inline half MMDLit_GetToonShadow(half toonRefl)
{
	half toonShadow = toonRefl * 2.0;
	return (half)saturate(toonShadow * toonShadow - 1.0);
}

inline half MMDLit_GetForwardAddStr(half toonRefl)
{
	half toonShadow = (toonRefl - _AddLightToonCen) * 2.0;
	return (half)clamp(toonShadow * toonShadow - 1.0, _AddLightToonMin, 1.0);
}

// for ForwardBase
inline half3 MMDLit_GetRamp(half NdotL, half shadowAtten)
{
	half refl = (NdotL * 0.5 + 0.5) * shadowAtten;
	half toonRefl = refl;
	
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#elif SELFSHADOW_ON
	refl = 0;
	#else // SELFSHADOW_LEGACY
	half selfShadowInv = 1.0 - _SelfShadow;
	refl = refl * selfShadowInv; // _SelfShadow = 1.0 as 0
	#endif
	
	half3 ramp = (half3)tex2D(_ToonTex, half2(refl, refl));

	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else
	half toonShadow = MMDLit_GetToonShadow(toonRefl);
	half3 rampSS = (1.0 - toonShadow) * ramp + toonShadow;
	#ifdef SELFSHADOW_ON
	ramp = rampSS;
	#else // SELFSHADOW_LEGACY
	ramp = rampSS * _SelfShadow + ramp * selfShadowInv;
	#endif
	#endif

	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	return ramp;
}

// for ForwardAdd
inline half3 MMDLit_GetRamp_Add(half toonRefl, half toonShadow)
{
	half refl = toonRefl;
	
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#elif SELFSHADOW_ON
	refl = 0;
	#else // SELFSHADOW_LEGACY
	half selfShadowInv = 1.0 - _SelfShadow;
	refl = refl * selfShadowInv; // _SelfShadow = 1.0 as 0
	#endif
	
	half3 ramp = (half3)tex2D(_ToonTex, half2(refl, refl));
	
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else
	half3 rampSS = (1.0 - toonShadow) * ramp + toonShadow;
	#ifdef SELFSHADOW_ON
	ramp = rampSS;
	#else // SELFSHADOW_LEGACY
	ramp = rampSS * _SelfShadow + ramp * selfShadowInv;
	#endif
	#endif
	
	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	return ramp;
}

// for Lightmap, DirLightmap
inline half3 MMDLit_GetRamp_Lightmap()
{
	#ifdef SELFSHADOW_ON
	half3 ramp = 1.0;
	#else
	half3 ramp = tex2D(_ToonTex, float2(1.0, 1.0));
	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	#endif
	
	#ifdef SELFSHADOW_LEGACY
	ramp = ramp * (1.0 - _SelfShadow) + _SelfShadow; // _SelfShadow = 1.0 as White
	#endif
	
	// No shadowStr, because included lightColor.
	return (half3)ramp;
}

// DirLightmap
inline half3 MMDLit_GetRamp_DirLightmap(half NdotL, half lambertStr)
{
	#ifdef SELFSHADOW_ON
	half refl = 0;
	#else
	half refl = (NdotL * 0.5 + 0.5);
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else // SELFSHADOW_LEGACY
	half selfShadowInv = 1.0 - _SelfShadow;
	refl = refl * selfShadowInv; // _SelfShadow = 1.0 as 0
	#endif
	#endif
	
	half3 ramp = (half3)tex2D(_ToonTex, half2(refl, refl));

	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else
	half3 rampSS = (1.0 - lambertStr) * ramp + lambertStr; // memo: Not use toonShadow.
	#ifdef SELFSHADOW_ON
	ramp = rampSS;
	#else //SELFSHADOW_LEGACY
	ramp = rampSS * _SelfShadow + ramp * selfShadowInv;
	#endif
	#endif

	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	// No shadowStr, because included lightColor.
	return ramp;
}

// for FORWARD_BASE
inline half3 MMDLit_Lighting(
	half3 albedo,
	half3 uvw_Sphere,
	half NdotL,
	half3 normal,
	half3 lightDir,
	half3 viewDir,
	half atten,
	half shadowAtten,
	out half3 baseC)
{
	half3 ramp = MMDLit_GetRamp(NdotL, shadowAtten);
	half3 lightColor = (half3)_LightColor0 * atten * 2.0;

	half3 baseD;
	MMDLit_GetBaseColor(albedo, _TempDiffuse, uvw_Sphere, baseC, baseD);
	
	half3 c = baseD * lightColor * ramp;
	
	#ifdef SPECULAR_OFF
	// Nothing.
	#else
	half refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (half3)_Specular * lightColor * refl;
	#endif

	#ifdef EMISSIVE_OFF
	// Nothing.
	#else
	// AutoLuminous
	c += baseC * (half3)_Emissive;
	#endif
	return c;
}

// for FORWARD_ADD
inline half3 MMDLit_Lighting_Add(
	half3 albedo,
	half3 uvw_Sphere,
	half toonRefl,
	half toonShadow,
	half3 normal,
	half3 lightDir,
	half3 viewDir,
	half atten)
{
	half3 ramp = MMDLit_GetRamp_Add(toonRefl, toonShadow);
	half3 lightColor = (half3)_LightColor0 * atten * 2.0;

	half3 baseC;
	half3 baseD;
	MMDLit_GetBaseColor(albedo, _TempDiffuse, uvw_Sphere, baseC, baseD);

	half3 c = baseD * lightColor * ramp;

	#ifdef SPECULAR_OFF
	// Nothing.
	#else
	half refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (half3)_Specular * lightColor * refl;
	#endif
	
	#ifdef EMISSIVE_OFF
	// Nothing.
	#else
	// AutoLuminous
	c += baseC * (half3)_Emissive;
	#endif
	return c;
}

inline half MMDLit_MulAtten(half atten, half shadowAtten)
{
	return atten * shadowAtten;
}

inline half3 MMDLit_Lightmap(half4 lmtex)
{
	half3 lm = MMDLit_DecodeLightmap(lmtex);
	// lm = lightColor = _LightColor0.rgb * atten * 2.0
	half3 ramp = MMDLit_GetRamp_Lightmap();

	return _TempDiffuse * lm * ramp + _TempAmbient;
}

inline half3 MMDLit_DirLightmap(
	half3 normal,
	half4 color,
	half4 scale,
	half3 viewDir,
	bool surfFuncWritesNormal,
	out half3 specColor)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	half3 lm = MMDLit_DirLightmapDiffuse (unity_DirBasis, color, scale, normal, surfFuncWritesNormal, scalePerBasisVector);
	half3 lightDir = normalize(scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
	// lm = lightColor = _LightColor0.rgb * atten * 2.0

	half NdotL = dot(normal, lightDir);
	half lambertStr = max(NdotL, 0.0);
	half3 ramp = MMDLit_GetRamp_DirLightmap(NdotL, lambertStr);

	half3 c = _TempDiffuse * lm * ramp + _TempAmbient;

	#ifdef SPECULAR_OFF
	specColor = 0;
	#else
	half refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	specColor = (half3)_Specular * lm * refl;
	#endif
	return c;
}
