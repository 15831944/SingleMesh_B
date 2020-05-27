Shader "Unlit/MyUnlitShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RotateNum("旋转角度",Range(-360,360)) = 0
		_UA("旋转中点x",Float) = 0.5
		_UB("旋转中点y",Float) = 0.5
		_CenterX("平移x",float) = 0
		_CenterY("平移y",float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _RotateNum;
			float _UA;
			float _UB;
			float _CenterX;
			float _CenterY;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				//计算旋转的坐标
				float Rote = (_RotateNum * 3.1415926)/180;
				float sinNum = sin(Rote);
				float cosNum = cos(Rote);
				 float2 di = float2(_UA,_UB);
				float2 uv = mul(float3(i.uv - di,1),float3x3(1,0,0,0,1,0,_CenterX,_CenterY,1)).xy;
				//计算旋转之后的坐标，需要乘旋转矩阵
				uv = mul(uv,float2x2(cosNum,-sinNum,sinNum,cosNum));
				fixed4 col = tex2D(_MainTex,TRANSFORM_TEX(uv,_MainTex));
				return col;
			}
			ENDCG
		}
	}
}
