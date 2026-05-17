Shader "TW/2D_dissolve_extraTexture" {
	Properties {
		[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Float) = 10
		[HDR] _Colormul ("Color mul", Vector) = (1,1,1,1)
		[HDR] _Coloradd ("Color add", Vector) = (0,0,0,1)
		_Emissive ("Emissive", Float) = 1
		_MainTex ("Main Tex", 2D) = "white" {}
		[Toggle] _Dissolvealpha ("Dissolve alpha?", Float) = 1
		_Dissolveholecolor ("Dissolve hole color", Vector) = (0,0,0,0)
		_Dissolvetex ("Dissolve tex", 2D) = "white" {}
		_Dissolveprogress ("Dissolve progress", Float) = 1
		_Dissolvethreshold ("Dissolve threshold", Range(0, 1.5)) = 0
		_DissolveFalloff ("Dissolve Falloff", Range(0, 1)) = 0
		[HDR] _Noisecolor1 ("Noise color 1", Vector) = (1,1,1,0)
		[HDR] _Noisecolor2 ("Noise color 2", Vector) = (1,1,1,0)
		_Noisethreshold ("Noise threshold", Range(0, 1)) = 0.5
		_Noisefalloff ("Noise falloff", Range(-1, 1)) = 0.01
		_Noisedirection ("Noise direction", Float) = -0.2
		_Tilingsmallnoise ("Tiling small noise", Float) = 0.4
		_Tilingbignoise ("Tiling big  noise", Float) = 0.4
		_Speedsmallnoise ("Speed small noise", Vector) = (0,0,0,0)
		_Speedbignoise ("Speed big noise", Vector) = (0,0,0,0)
		[HDR] _Dissolvebordercolor ("Dissolve border  color", Vector) = (1,0,0,0)
		_Dissolveborderthreshold ("Dissolve border threshold", Range(0.5, 2)) = 0.5
		_Dissolveborderfalloff ("Dissolve border falloff", Range(0, 1)) = 0.01
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