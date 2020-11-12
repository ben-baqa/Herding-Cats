Shader "Mark/Grass Envelop"{
	Properties{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
        _GrassHeight ("Grass Height", Float) = 1.0
        _GrassMap ("Grass Map", 2D) = "white" {}
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

            sampler2D _GrassMap;
			float4 _GrassMap_ST;

            float _GrassHeight;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
                float3 worldPos : TEXCOORD1;
			};

			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;  
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
                float noise = tex2D(_NoiseTex, float2(i.worldPos.x + sin(_Time.x) * 0.1, 0.0)).r;
				fixed4 col = tex2D(_MainTex, i.uv);
                // float hasGrass = tex2D(_GrassMap, float2(1.0 + 0.5 * (i.worldPos.x / 5.75), 1.0 + 0.5 * (i.worldPos.y / 3.75))).r;
                float hasGrass = tex2D(_GrassMap, float2(0.5 + i.worldPos.x / 5.75 / 2, 0.5 + i.worldPos.y / 3.75 / 2)).r;
                col.a *= i.uv.y > (noise.r * _GrassHeight * hasGrass)? 1.0 : 0.0;
                // noise = noise * 2 - 1.0; 
                // float2 uv = i.uv;
                // float magnitude =  tex2D(_WaveMap, i.uv + float2(noise * 0.02, 0)).r;
                // uv.x += noise * 0.02 * magnitude;
				col *= i.color;
				return col;
                // return float4(hasGrass, hasGrass, hasGrass, 1.0);
			}

			ENDCG
		}
	}
}
