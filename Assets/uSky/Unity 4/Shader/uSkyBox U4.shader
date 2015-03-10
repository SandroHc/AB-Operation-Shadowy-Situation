Shader "Skybox/uSkyBox U4" 
{
Properties {

[HideInInspector]	_SkyMultiplier("_SkyMultiplier", Vector) = (1, 4, 0.75, 0)
[HideInInspector]	_betaR("BetaR", Vector) = (0.0058, 0.0136, 0.0331, 0)
[HideInInspector]	_betaM("BetaM", Vector) = (0.004, 0.005, 0.006, 0)
[HideInInspector]	_SkyTint ("Tint", Vector) = (1, 1, 1, 1)
[HideInInspector]	_GroundColor ("Ground Color", Vector) = (0.2, 0.6, 1.4, 0)
//[HideInInspector]	_SunDir ("Sun Direction", Vector) = (0.321,0.766,-0.557,0)

[HideInInspector]	_NightHorizonColor("_Night Horizon Color", Vector) = (0.43, 0.47, 0.5, 1)
[HideInInspector]	_NightZenithColor("Night Zenith Color", Vector) = (0.00532, 0.00707, .001, 0)
[HideInInspector]	_MoonOuterCorona("Moon Outer Corona Color", Vector) = (0.73, 0.89, 1, 1)
[HideInInspector]	_colorCorrection ("Color Correction", Vector) = (1,1,0,0)

//					_MoonSize("Moon Size", Range(0.01,1.0)) = 0.15
[NoScaleOffset]		_MoonSampler ("Moon",2D) = "black" {}
	
[HideInInspector]	_OuterSpaceIntensity("Outer Space Intensity",Range(0, 2)) = 0.25
[NoScaleOffset]		_OuterSpaceCube("Outer Space Cubemap", Cube) = "black" {}
}
SubShader 
	{
		Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox"}
		ZWrite Off  Fog { Mode Off }

		CGINCLUDE
//		#include "UnityCG.cginc"
//		#include "Lighting.cginc"
		
		struct appdata_t {
			float4 vertex 			: POSITION;
		};
		
		struct v2f 
		{
			float4	pos 			: SV_POSITION;
			float3	worldPos 		: TEXCOORD0;
			#ifdef NIGHTSKY_ON
			float3	cubeSamplerTC	: TEXCOORD1;
			#endif
		};
		
		uniform half 		_SunSize, _OuterSpaceIntensity;
		uniform half2		_colorCorrection;
		uniform half3		_betaR, _betaM, _miePhase_g, _mieConst; 
		uniform half4		_SkyTint, _NightZenithColor,_GroundColor;
		uniform float3		_SunDir;
		
		// x = Sunset, y = Day, z = Rayleigh, w = Night 
		uniform half4		_SkyMultiplier;
		#ifdef NIGHTSKY_ON
		sampler2D			_MoonSampler;
		samplerCUBE			_OuterSpaceCube;
		uniform half4		_NightHorizonColor, _MoonInnerCorona, _MoonOuterCorona;
		uniform float4x4	_Moon_wtl;
		uniform float		_MoonSize;
		#endif
		
		v2f vert(appdata_t v)
		{
			v2f OUT;
			OUT.pos = mul(UNITY_MATRIX_MVP, v.vertex);

//			OUT.worldPos = normalize(mul((float3x3)_Object2World, v.vertex.xyz));
			OUT.worldPos = v.vertex.xyz;
			
			#ifdef NIGHTSKY_ON
			OUT.cubeSamplerTC = v.vertex.xyz * float3(-1,1,1);
			#endif
			return OUT;
		}
			
		half4 frag(v2f IN) : SV_Target
		{
		    float3 pos = normalize( IN.worldPos );
			float cosTheta = dot( pos, _SunDir); // need precision float in iOS
			half cosine = cosTheta;
			
			half t = pos.y;
			half upperSky = max(0.0,t);
			half3 zenithAngle = pow(max(6e-2, t + 6e-2), _SkyTint.w) / _SkyTint.xyz
								+ (max(0.0, -t) * _GroundColor.xyz);
			
			// optical depth
			half3 sR = 8.0 / zenithAngle ;
			half3 sM = 1.2 / zenithAngle ;
			
			// gradient
			half3 gr = _NightZenithColor.xyz * sR;
			gr *= (2 - gr);
			
			// sky color
			half3 extinction = exp(-( _betaR * sR + _betaM * sM ));

			half3 rayleigh = lerp( extinction * gr, 1 - extinction, _SkyMultiplier.x );
			half3 mie = rayleigh * sM / rayleigh.r * _mieConst ;

			// scattering phase
			half miePhase = _miePhase_g.x * pow( _miePhase_g.y - _miePhase_g.z * cosine, -1.5 );

			half3 inScatter = ( rayleigh * _SkyMultiplier.z + mie * miePhase ); 
			inScatter *= ( 1.0 + cosine * cosine ) * _SkyMultiplier.y;
			
			// add sun
			half sun = min(1e3, pow((1-cosine)* _SunSize, -1.5 ));
			inScatter += sun * min(mie,upperSky)* extinction ;

// --------------------------------------------------------------------------------
			// night sky
			
			half moonMask = 0.0;
			#ifdef NIGHTSKY_ON

			// night horizontal gradient
			inScatter += _NightHorizonColor.xyz * gr;
			
			float2 moonTC = float2( dot( _Moon_wtl[0].xyz, pos.xyz), dot( _Moon_wtl[1].xyz, pos.xyz) )/_MoonSize + 0.5;
			
			// add moon and outer space
			half4 moonAlbedo = tex2D( _MoonSampler, moonTC );
			moonMask = moonAlbedo.a;
			half3 spaceAlbedo = texCUBE (_OuterSpaceCube, IN.cubeSamplerTC).rgb * (1 - moonMask) * _OuterSpaceIntensity;
			inScatter += ( moonAlbedo.rgb + spaceAlbedo.rgb )* upperSky * _SkyMultiplier.w;

			// add inner and outer moon corona
			float moonDir = 1 + dot( pos, _Moon_wtl[2].xyz);
			half m = moonDir;
			inScatter += _MoonInnerCorona.xyz * (1.0 / (1.05 + m * _MoonInnerCorona.w));
			inScatter += _MoonOuterCorona.xyz * (1.0 / (1.05 + m * _MoonOuterCorona.w));
			
			#endif
// --------------------------------------------------------------------------------		

			// tonemapping
			#ifndef USKY_HDR_ON
			inScatter = 1 - exp(-1 * inScatter);															// clear sky	(4 ALU)
//			inScatter = inScatter < 1.413 ? pow(inScatter * 0.38317, 1.0 / 2.2) : 1.0 - exp(-inScatter);	// standard sky	(10 AlU)
			#endif
			
			// color correction
			inScatter = pow(inScatter * _colorCorrection.x, _colorCorrection.y);
			
			half alpha = lerp( moonMask, 1.0, _SkyMultiplier.x);
			
			return half4(inScatter, alpha); 
		}

		ENDCG
// --------------------------------------------------------------------------------
// --------------------------------------------------------------------------------				
	Pass{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile NIGHTSKY_ON NIGHTSKY_OFF
			#pragma multi_compile USKY_HDR_ON USKY_HDR_OFF
			#pragma target 3.0
			ENDCG
    	}
	}
	
	Fallback Off
}