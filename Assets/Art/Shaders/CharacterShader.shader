Shader "Custom/CharacterShader" 
{
	Properties 
	{
		_BumpMap("Bumpmap", 2D) = "bump" { }
		_PrimaryColor ("Primary Color", Color) = (1,1,1,1)
		_SecondaryColor ("Secondary Color", Color) = (1,1,1,1)
		_InnerTex ("Inner Texture", 2D) = "white" { }
		_InnerMask("Inner Mask", 2D) = "white" { }
		_InnerTransparency ("Inner Transparency", Range(0, 1)) = 1
		_InnerSpecColor("Inner Specular Color", Color) = (1,1,1,1)
		_InnerRim ("Inner Rim", Range(0.03, 1)) = 0.625
		_OuterTex ("Outer Texture", 2D) = "white" { }
		_OuterColor("Outer Color", Color) = (1,1,1,1)
		_OuterTransparency ("Outer Transparency", Range(0, 1)) = 1
		_OuterSpecColor("Outer Specular Color", Color) = (1,1,1,1)
		_OuterRim ("Outer Rim", Range(0.03, 1)) = 0.625
      	_OuterExtrusionAmount ("Outer Extrusion Amount", Range(0, 2)) = 1
	}
	SubShader 
	{
		Tags 
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent" 
		}
		
		Pass
		{
	        ZWrite On
	        ColorMask 0
        }
		
		CGPROGRAM
		#pragma surface surf WrapSpecularLambert alpha
		struct Input 
		{
			float2 uv_InnerTex;
			float2 uv_BumpMap;
			float2 uv_InnerMask;
			float3 viewDir;
		};
		
		sampler2D _InnerTex;
		sampler2D _BumpMap;
		sampler2D _InnerMask;
		fixed4 _PrimaryColor;
		fixed4 _SecondaryColor;
		fixed4 _InnerSpecColor;
		fixed _InnerTransparency;
		fixed _InnerRim;
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 tex = tex2D(_InnerTex, IN.uv_InnerTex);
			fixed4 mask = tex2D(_InnerMask, IN.uv_InnerMask);
			o.Albedo = tex.rgb * (1 - mask.r - mask.g) + _PrimaryColor * tex.rgb * mask.r + _SecondaryColor * tex.rgb * mask.g;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Alpha = max(mask.a, tex.a * _InnerTransparency * pow(rim, _InnerRim));
			o.Gloss = _InnerSpecColor * mask.b;
			o.Specular = max(0.01, _InnerSpecColor.a);
		}
		
		half4 LightingWrapSpecularLambert (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
		{
			half3 h = normalize (lightDir + viewDir);
	        fixed diff = dot (s.Normal, lightDir) * 0.5 + 0.5;
	        float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
	        fixed4 c;
	        c.rgb = (s.Albedo * _LightColor0.rgb + _LightColor0.rgb * _InnerSpecColor.rgb * spec) * (diff * atten * 2);
	        c.a = s.Alpha;
	        return c;
	    }
		ENDCG
		
		CGPROGRAM
		#pragma surface surf BlinnPhong2 alpha vertex:vert	    
		struct Input
		{
			float2 uv_OuterTex;
			float2 uv_OuterMask;
			float3 viewDir;
		};
		
		sampler2D _OuterTex;
		fixed4 _OuterColor;
		fixed4 _OuterSpecColor;
		fixed _OuterTransparency;
		fixed _OuterExtrusionAmount;
		fixed _OuterRim;
		
	    void vert (inout appdata_full v)
	    {
			v.vertex.xyz += v.normal * _OuterExtrusionAmount;
		}
		
		void surf (Input IN, inout SurfaceOutput o) 
		{
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			fixed4 rimColor = _OuterColor * pow(rim, _OuterRim);
			fixed4 tex = tex2D(_OuterTex, IN.uv_OuterTex) * _OuterColor;
			o.Albedo = tex.rgb;
			o.Alpha = tex.a * _OuterTransparency * rimColor.a;
			o.Emission = rimColor.rgb;
			o.Gloss = _OuterSpecColor;
			o.Specular = max(0.01, _OuterSpecColor.a);
		}
		
		fixed4 LightingBlinnPhong2 (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			half3 h = normalize (lightDir + viewDir);
			fixed diff = max (0, dot (s.Normal, lightDir));
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _OuterSpecColor.rgb * spec) * (atten * 2);
			c.a = s.Alpha + _LightColor0.a * _OuterSpecColor.a * spec * atten;
			return c;
		}
		ENDCG
		
	} 
	Fallback "Specular"
}