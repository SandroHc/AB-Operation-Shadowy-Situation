#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
#define UNITY_PASS_SHADOWCOLLECTOR
#define SHADOW_COLLECTOR_PASS
#include "UnityCG.cginc"
#include "Lighting.cginc"

#define INTERNAL_DATA
#define WorldReflectionVector(data,normal) data.worldRefl
#define WorldNormalVector(data,normal) normal

half4 _Color;
sampler2D _MainTex;

struct v2f_surf {
  V2F_SHADOW_COLLECTOR;
  float2 pack0 : TEXCOORD5;
};
v2f_surf vert_surf (appdata_full v) {
  v2f_surf o;
  o.pack0.xy = v.texcoord;
  TRANSFER_SHADOW_COLLECTOR(o)
  return o;
}
fixed4 frag_surf (v2f_surf IN) : COLOR {
  half4 albedo = (half4)tex2D(_MainTex, IN.pack0.xy);
  albedo.a *= _Color.a; // for Transparency
  // Warning: Maybe crash Android / Adreano 205
  clip(albedo.a - (0.1 / 255.0)); // Like as MMD

  SHADOW_COLLECTOR_FRAGMENT(IN)
}

fixed4 frag_fast (v2f_surf IN) : COLOR {
  SHADOW_COLLECTOR_FRAGMENT(IN)
}
