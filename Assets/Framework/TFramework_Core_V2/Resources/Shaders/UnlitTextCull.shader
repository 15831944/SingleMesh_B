Shader "Tienchih/UnlitTextCull" {
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)
		_Offset("Offset",Range(-30,30)) = 0
	}
		SubShader
	{
		Tags{ "RenderType" = "Opaque" }


		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;

	};

	struct v2f
	{
		float3 uv : TEXCOORD0;

		float4 vertex : SV_POSITION;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Offset;
	float4 _Color;

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
		o.uv.z = _Offset - v.vertex.x;
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		clip(i.uv.z);
		fixed4 col = tex2D(_MainTex, i.uv.xy);
		col *= _Color;
		return col;
	}
		ENDCG
	}
	}
}
