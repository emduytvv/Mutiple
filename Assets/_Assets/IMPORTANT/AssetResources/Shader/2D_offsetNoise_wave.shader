Shader "TW/2D_offsetNoise_wave" {
	Properties {
		_InvFade ("Soft Particles Factor", Float) = 1
		[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Float) = 10
		[HDR] _Colormul ("Color mul", Vector) = (1,1,1,1)
		[HDR] _Coloradd ("Color add", Vector) = (0,0,0,1)
		_Emissive ("Emissive", Float) = 1
		_MainTex ("Main Tex", 2D) = "white" {}
		_Noisemap ("Noise map", 2D) = "white" {}
		[HDR] _Noisecolor ("Noise color", Vector) = (1,1,1,0)
		[Toggle] _Isadd ("Is add?", Float) = 1
		[HideInInspector] _texcoord ("", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy);
			}

			ENDHLSL
		}
	}
	//CustomEditor "ASEMaterialInspector"
}