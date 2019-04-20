Shader "Dinosaur_Shader" {
    Properties {  
	_MainTex ("Texture", 2D) = "white" {} 
	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
	_Detail ("Detail", 2D) = "gray" {}
 
    }  
    SubShader {  
      Tags { "RenderType" = "Opaque" }  
      CGPROGRAM  
      #pragma surface surf Lambert  
      struct Input
      {  
		float2 uv_MainTex;  
		float3 worldRefl;
		float3 viewDir;
		float2 uv_Detail;
      };  
		sampler2D _MainTex;   
		float _RimPower;
		float4 _RimColor; 
		sampler2D _Detail;
		
      void surf (Input IN, inout SurfaceOutput o) {  
		o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * .55;
		o.Albedo *= tex2D (_Detail, IN.uv_Detail).rgb * 2;
		half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
		o.Emission = _RimColor.rgb * 3 * pow (rim, _RimPower); 
      }  
      ENDCG  
    }   
    Fallback "Diffuse"  
}