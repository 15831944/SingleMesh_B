// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tienchih/DoubleFaceTransparent" {
	Properties{
		_ColorFront("Color Tint", Color) = (1, 1, 1, 1)
		_MainTexFront("Main Tex Front", 2D) = "white" {}
		_AlphaScaleFront("Alpha Scale Front", Range(0, 1)) = 1

	    _ColorBack("Color Tint", Color) = (1, 1, 1, 1)
		_MainTexBack("Main Tex Back", 2D) = "white" {}
		_AlphaScaleBack("Alpha Scale Back", Range(0, 1)) = 1
		}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Pass{
		//Tags{ "LightMode" = "ForwardBase" }

		// First pass renders only back faces 
		Cull Front

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "Lighting.cginc"

		fixed4 _ColorFront;
	sampler2D _MainTexFront;
	float4 _MainTexFront_ST;
	fixed _AlphaScaleFront;

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float2 uv : TEXCOORD2;
	};

	v2f vert(a2v v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.worldNormal = UnityObjectToWorldNormal(v.normal);

		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		o.uv = TRANSFORM_TEX(v.texcoord, _MainTexFront);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
		fixed3 worldNormal = normalize(i.worldNormal);
	fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

	fixed4 texColor = tex2D(_MainTexFront, i.uv);

	fixed3 albedo = texColor.rgb * _ColorFront.rgb;

	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

	//fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
	fixed3 diffuse = albedo;
	return fixed4(ambient + diffuse, texColor.a * _AlphaScaleFront);
	}

		ENDCG
	}

		Pass{
		//Tags{ "LightMode" = "ForwardBase" }

		// Second pass renders only front faces 
		Cull Back

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "Lighting.cginc"

		fixed4 _ColorBack;
	sampler2D _MainTexBack;
	float4 _MainTexBack_ST;
	fixed _AlphaScaleBack;

	struct a2v {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float3 worldNormal : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
		float2 uv : TEXCOORD2;
	};

	v2f vert(a2v v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		o.worldNormal = UnityObjectToWorldNormal(v.normal);

		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

		o.uv = TRANSFORM_TEX(v.texcoord, _MainTexBack);

		return o;
	}

	fixed4 frag(v2f i) : SV_Target{
		fixed3 worldNormal = normalize(i.worldNormal);
	fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));

	fixed4 texColor = tex2D(_MainTexBack, i.uv);

	fixed3 albedo = texColor.rgb * _ColorBack.rgb;

	fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;

	//fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));
	fixed3 diffuse = albedo;
	return fixed4(ambient + diffuse, texColor.a * _AlphaScaleBack);
	}

		ENDCG
	}
	}
		FallBack "Transparent/VertexLit"
}
