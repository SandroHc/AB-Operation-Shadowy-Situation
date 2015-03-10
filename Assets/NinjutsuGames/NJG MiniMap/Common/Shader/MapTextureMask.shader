Shader "NinjutsuGames/Map TextureMask"
{
	Properties {
		_Color ("_Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Mask ("Culling Mask", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	/*SubShader
	{
		LOD 100

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Offset -1, -1
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				
				#include "UnityCG.cginc"
	
				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
	
				struct v2f
				{
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					fixed4 color : COLOR;
				};
	
				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4x4 _Matrix;
				
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					o.color = v.color;
					return o;
				}
				
				fixed4 frag (v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, mul(_Matrix, float4(i.texcoord, 0, 0)).xy) * i.color;
					return col;
				}
			ENDCG
		}
	}*/
	SubShader { 

		LOD 100
		Tags
		{
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent"
		}		

		Pass {
			
			Cull Off
			Lighting Off
			ZWrite Off
			Fog { Mode Off }
			Offset -1, -1
			ColorMask RGB
			AlphaTest Greater .01
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse

			SetTexture [_Mask] { 
				combine texture 
			}

			SetTexture [_MainTex] { 
				constantColor [_Color]
         		matrix [_Matrix] combine texture * constant , previous * constant 
			}
		}

		/*LOD 100
		Tags
		{
			"Queue" = "Background" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Background"
		}

		Pass 
		{
			
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest LEqual [_Cutoff]
			SetTexture [_Mask] { combine texture }
		}*/
	}

	/*SubShader 
	{ 

		LOD 100
		Tags
		{
			"Queue" = "Background" 
			"IgnoreProjector" = "True" 
			"RenderType" = "Background"
		}	

		Pass 
		{
			ZTest Always
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest LEqual [_Cutoff]
			SetTexture [_Mask] { combine texture }
		}
	}*/
}
