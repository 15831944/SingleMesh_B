Shader "Tienchih/FlashLight" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FluLightTex("FluLightTex" , 2D) = "white"{}
		_FluColor("FluColor" , color) = (1,1,1,1)
		_FrontierRange("_FrontierRange" ,float) = 0.9
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert 

		sampler2D _MainTex;
		sampler2D _FluLightTex;
		half4 _FluColor;
		float _FrontierRange;

		struct Input {
			float2 uv_MainTex;
			float2 uv_FluLightTex;
			float3 viewDir;
		};
		
		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;	
			IN.viewDir = normalize(IN.viewDir);
		//	o.Normal = normalize(o.Normal);
			float NdotV = dot(o.Normal , IN.viewDir);
			if(NdotV<_FrontierRange)
			{
				float2 fluUV=IN.uv_FluLightTex;
				fluUV +=_Time.y*0.5;
				o.Emission = _FluColor.rgb*lerp(0,1,(_FrontierRange-NdotV)/(1-_FrontierRange));
				o.Emission*=tex2D (_FluLightTex, fluUV); 
			}
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
