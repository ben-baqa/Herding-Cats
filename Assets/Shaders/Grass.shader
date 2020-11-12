Shader "Mark/Grass"{
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_WaveMap ("Noise Texture", 2D) = "white" {}
	}

	SubShader{
		Tags{ 
			"RenderType"="Transparent" 
			"Queue"="Transparent"
		}

		Blend SrcAlpha OneMinusSrcAlpha

		ZWrite off
		Cull off

		Pass{

			CGPROGRAM

			#include "UnityCG.cginc"

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainTex_ST;
            
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;

            sampler2D _WaveMap;
			float4 _WaveMap_ST;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
                float noise = tex2D(_NoiseTex, (i.uv + float2(0, _Time.y * 0.05 + i.uv.y)) * 0.2).r;
                noise = noise * 2 - 1.0;
                float2 uv = i.uv;
                float magnitude =  tex2D(_WaveMap, i.uv + float2(noise * 0.02, 0)).r;
                uv.x += noise * 0.02 * magnitude;
				fixed4 col = tex2D(_MainTex, uv);
				col *= i.color;
				return col;
                // return noise;
			}

			ENDCG
		}
	}
}
