Shader "Transparent/Cutout/Bumped Diffuse LR" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

	SubShader {
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 300
		Cull Off
		
		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff
		#pragma multi_compile_particle
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
		
//		Pass {
//			CGPROGRAM
//			#pragma vertex vert
//			#pragma fragment frag
//			#pragma multi_compile_particles
//			
//			#include "UnityCG.cginc"
//
//			sampler2D _MainTex;
//			fixed4 _TintColor;
//			
//			struct appdata_t {
//				float4 vertex : POSITION;
//				fixed4 color : COLOR;
//				float2 texcoord : TEXCOORD0;
//			};
//
//			struct v2f {
//				float4 vertex : SV_POSITION;
//				fixed4 color : COLOR;
//				float2 texcoord : TEXCOORD0;
//				#ifdef SOFTPARTICLES_ON
//				float4 projPos : TEXCOORD1;
//				#endif
//			};
//			
//			float4 _MainTex_ST;
//
//			v2f vert (appdata_t v)
//			{
//				v2f o;
//				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
//				#ifdef SOFTPARTICLES_ON
//				o.projPos = ComputeScreenPos (o.vertex);
//				COMPUTE_EYEDEPTH(o.projPos.z);
//				#endif
//				o.color = v.color;
//				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
//				return o;
//			}
//
//			sampler2D_float _CameraDepthTexture;
//			float _InvFade;
//			
//			fixed4 frag (v2f i) : SV_Target
//			{
//				#ifdef SOFTPARTICLES_ON
//				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
//				float partZ = i.projPos.z;
//				float fade = saturate (_InvFade * (sceneZ-partZ));
//				i.color.a *= fade;
//				#endif
//				
//				return 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
//			}
//			ENDCG 
//		}
	}

	FallBack "Transparent/Cutout/Diffuse"
}
