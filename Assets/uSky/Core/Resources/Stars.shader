// Note: This Star shader need to be inside Resources folder for mobile build!

Shader "Hidden/uSky/Stars" {
SubShader {
			Tags { "Queue"="Geometry+501" "RenderType"="Background" }

			Blend OneMinusDstAlpha  OneMinusSrcAlpha
			ZWrite Off Fog { Mode Off }

	Pass{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest	

		uniform half StarIntensity;
		uniform float2 _tab[8];
		
		struct appdata_t {
			float4 vertex : POSITION;
			float4 ColorAndMag : COLOR;
			float2 texcoord : TEXCOORD;
		};
		
		struct v2f 
		{
			float4 pos : SV_POSITION;
			half4 Color : COLOR;
			half2 uv : TEXCOORD;
		};	
			
		float GetFlickerAmount(in float2 pos)
		{
			float2 hash = frac(pos.xy * 256);
			float index = frac(hash.x + (hash.y + 1) * (_Time.x * 5)); 
			index *= 8;

			float f = frac(index)* 2.5; // multiply 2.5 to emphasize flickering effects
			int i = (int)index;

			return _tab[i].x + f * _tab[i].y;
		}	
		
		v2f vert(appdata_t v)
		{
			v2f OUT = (v2f)0;

			float3 t = v.vertex.xyz + _WorldSpaceCameraPos.xyz ; 
			OUT.pos = mul(UNITY_MATRIX_MVP, float4 (t, 1))  ;

			float appMag = 6.5 + v.ColorAndMag.w * (-1.44 -2.5);
			float brightness = GetFlickerAmount(v.vertex.xy) * pow(5.0, (-appMag -1.44)/ 2.5);
			
			OUT.Color = StarIntensity * float4(brightness * v.ColorAndMag.xyz, brightness );
			OUT.uv = 5 * v.texcoord.xy - 5 * float2(0.5, 0.5);
			
			return OUT;
		}

		half4 frag(v2f IN) : SV_Target
		{
			half2 distCenter = IN.uv.xy;
			half scale = exp(-dot(distCenter, distCenter));
			half3 colFinal = (IN.Color.xyz * scale + 5 * IN.Color.w * pow(scale, 10));
			return half4( colFinal,0);
		}
		ENDCG
	  }
	} 
}
