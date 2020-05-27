Shader "Tienchih/WaterWave" {
	Properties{
		_WaterTex("水的纹理图：", 2D) = "white" {}
	_XSpeed("X轴方向的纹理滚动速度：",Range(0,10)) = 1
		_YSpeed("Y轴方向的纹理滚动速度：",Range(0,10)) = 1
	}
		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
#pragma surface surf Lambert
		sampler2D _WaterTex;
	fixed _XSpeed;
	fixed _YSpeed;
	struct Input {
		float2 uv_WaterTex;//注意uv后面的名必须跟你要获取uv值得纹理贴图名字要一样，否则没效果
	};

	void surf(Input IN, inout SurfaceOutput o) {

		fixed2 UV = IN.uv_WaterTex;

		fixed xValue = _XSpeed * _Time;//改变u坐标值 _Time为内置变量
		fixed yValue = _YSpeed * _Time;//改变V坐标值

		UV += fixed2(xValue,yValue);

		half4 c = tex2D(_WaterTex,UV);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
		FallBack "Diffuse"
}
