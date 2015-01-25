#include "MMD4Mecanim-MMDLit-Lighting.cginc"

#define SUPPORT_SELFSHADOW

half4 _Color;
half4 _Ambient;
half4 _Specular;
half _Shininess;
half _ShadowLum;
half _SelfShadow;
sampler2D _MainTex;
sampler2D _ToonTex;

samplerCUBE _SphereCube;
half _SphereAdd;
half _SphereMul;

half4 _Emissive;

half4 _DefLightDir0;
half4 _DefLightColor0;

half4 _TempDiffuse;
half4 _TempAmbient;
half4 _TempAmbientL;

#ifdef UNITY_PASS_PREPASSFINAL
half4 unity_Ambient;
#endif

inline half3 MMDLit_GetAmbient()
{
#ifdef UNITY_PASS_PREPASSFINAL
	return (half3)unity_Ambient;
#else
	return 0;
#endif
}

//------------------------------------------------------------------------------------------------------------------------

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
	return tex2D(_MainTex, uv_MainTex);
}

inline half MMDLit_GetToonShadow(half toonRefl)
{
	half toonShadow = toonRefl * 2.0;
	return (half)saturate(toonShadow * toonShadow - 1.0);
}

// for Lightmap / DirLightmap
inline half3 MMDLit_GetRamp_Lightmap()
{
	#ifdef SELFSHADOW_ON
	half3 ramp = 1.0;
	#else
	half3 ramp = (half3)tex2D(_ToonTex, half2(1.0, 1.0));
	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else // SELFSHADOW_LEGACY
	ramp = ramp * (1.0 - _SelfShadow) + _SelfShadow; // _SelfShadow = 1.0 as White
	#endif
	#endif

	// No shadowStr, because included lightColor.
	return ramp;
}

// DirLightmap
inline half3 MMDLit_GetRamp_DirLightmap(half NdotL, half lambertStr)
{
	#ifdef SELFSHADOW_ON
	half refl = 1.0;
	#else
	half refl = (NdotL * 0.5 + 0.5);
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else // SELFSHADOW_LEGACY
	half selfShadowStrInv = 1.0 - _SelfShadow;
	refl = refl * selfShadowStrInv; // _SelfShadow = 1.0 as 0
	#endif
	#endif
	
	half3 ramp = (half3)tex2D(_ToonTex, half2(refl, refl));
	
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else
	half3 rampSS = (1.0 - lambertStr) * ramp + lambertStr;
	#ifdef SELFSHADOW_ON
	ramp = rampSS;
	#else // SELFSHADOW_LEGACY
	ramp = rampSS * _SelfShadow + ramp * selfShadowStrInv;
	#endif
	#endif
	
	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);
	// No shadowStr, because included lightColor.
	return ramp;
}

inline half3 MMDLit_Lighting(
	half3 albedo,
	half3 mmd_uvwSphere,
	half3 normal,
	half3 lightColor0,
	half3 lightDir,
	half3 viewDir,
	half3 light)
{
	half3 lightColor = lightColor0; // Premultiply lightColor x atten x 2.0
	half3 globalAmbient = MMDLit_GetAmbient();
	half3 globalLight = min(globalAmbient + lightColor, 1.0);

	half NdotL = dot(normal, lightDir);
	half lambertStr = max(NdotL, 0.0);
	half3 lambertLight = globalAmbient + lightColor * lambertStr;
	half3 addLight = max(light - lambertLight, 0.0);

	half shadowBias = 2.0; // 1.0 -
	half3 lightShadow = (light.rgb - globalAmbient) * shadowBias / max(globalLight - globalAmbient, 0.0001);
	half refl = MMDLit_Luminance(min(lightShadow, half3(1,1,1))); // SelfShadow
	half refl2 = NdotL * 0.5 + 0.5; // Lambert
	refl = min(refl, refl2); // = Lambert * shadowAtten
	half toonRefl = refl;

	#ifdef SELFSHADOW_OFF
	// Nothing
	#elif defined(SELFSHADOW_ON)
	refl = 0;
	#else // SELFSHADOW_LEGACY
	half selfShadowStrInv = 1.0 - _SelfShadow;
	refl = refl * selfShadowStrInv; // _SelfShadow = 1.0 as 0
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
	ramp = rampSS * _SelfShadow + ramp * selfShadowStrInv;
	#endif
	#endif

	ramp = saturate(1.0 - (1.0 - ramp) * _ShadowLum);

	half3 diffuseLight = lightColor;

	// Toon mapping for addLight.
	half alLen = max(max(addLight.x, addLight.y), addLight.z);
	half alRefl = alLen;

	#ifdef SELFSHADOW_OFF
	// Nothing.
	#elif defined(SELFSHADOW_ON)
	alRefl = 0;
	#else // SELFSHADOW_LEGACY
	alRefl = alRefl * selfShadowStrInv; // _SelfShadow = 1.0 as 0
	#endif
	
	half3 alRamp = (half3)tex2D(_ToonTex, half2(alRefl, alRefl));
	#ifdef SELFSHADOW_OFF
	// Nothing.
	#else
	half alToonShadow = MMDLit_GetToonShadow(alRefl);
	half3 alRampSS = (1.0 - alToonShadow) * alRamp + alToonShadow;
	#ifdef SELFSHADOW_ON
	alRamp = alRampSS;
	#else // SELFSHADOW_LEGACY
	alRamp = alRampSS * _SelfShadow + alRamp * selfShadowStrInv;
	#endif
	#endif

	alRamp = saturate(1.0 - (1.0 - alRamp) * _ShadowLum);

	half3 baseC;
	half3 baseD;
	MMDLit_GetBaseColor(albedo, _TempDiffuse, mmd_uvwSphere, baseC, baseD);

	half3 c = (diffuseLight * ramp + addLight * alRamp) * baseD;
	c += _TempAmbient * baseC;

	#ifdef SPECULAR_OFF
	// Nothing.
	#else
	refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (half3)_Specular * refl * lightColor;
	#endif
	
	// AutoLuminous
	c += baseC * (half3)_Emissive;
	return c;
}

inline half3 MMDLit_Lightmap(
	half3 albedo,
	half3 light)
{
	half3 ramp = MMDLit_GetRamp_Lightmap();
	
	half3 c = _TempDiffuse * light * ramp + _TempAmbient;
	c *= albedo;
	return c;
}

inline half3 MMDLit_DirLightmap(
	half3 albedo,
	half3 normal,
	half4 color,
	half4 scale,
	half3 viewDir,
	half3 light,
	bool surfFuncWritesNormal)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	half3 lm = MMDLit_DirLightmapDiffuse (unity_DirBasis, color, scale, normal, surfFuncWritesNormal, scalePerBasisVector);
	half3 lightDir = normalize(scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);

	light += lm;

	half NdotL = dot(normal, lightDir);
	half lambertStr = max(NdotL, 0.0);
	half3 ramp = MMDLit_GetRamp_DirLightmap(NdotL, lambertStr);
	
	half3 c = _TempDiffuse * light * ramp + _TempAmbient;
	c *= albedo;

	#ifdef SPECULAR_OFF
	// Nothing.
	#else
	half refl = MMDLit_SpecularRefl(normal, lightDir, viewDir, _Shininess);
	c += (half3)_Specular * refl * light;
	#endif
	return c;
}

inline half4 MMDLit_DirLightmap(
	half4 color,
	half4 scale,
	half3 normal,
	bool surfFuncWritesNormal)
{
	UNITY_DIRBASIS
	half3 scalePerBasisVector;
	half3 lm = MMDLit_DirLightmapDiffuse(unity_DirBasis, color, scale, normal, surfFuncWritesNormal, scalePerBasisVector);
	return half4(lm, 0);
}
