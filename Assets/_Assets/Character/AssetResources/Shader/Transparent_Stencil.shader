Shader "My Shader/UI/Transparent_Stencil" {
	Properties {
		[PerRendererData] _MainTex ("Particle Texture", 2D) = "white" {}
		[Space(15)] [Header(Blend Factors)] [Space(10)] _AlphaClip ("Alpha Clip Threshold", Range(0, 1)) = 0.001
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrcFactor ("Blend Src Factor", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDstFactor ("Blend Dst Factor", Float) = 0
		[Space(15)] [Header(Stencil Properties)] [Space(10)] _Stencil ("Stencil Ref ID", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comp", Float) = 1
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Pass Op", Float) = 1
		_StencilReadMask ("Read Mask", Range(0, 255)) = 255
		_StencilWriteMask ("Write Mask", Range(0, 255)) = 255
		_ColorMask ("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
		[Toggle(UNITY_UI_CLIP_RECT)] _UseUIClipRect ("Use Clip Rect", Float) = 0
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
	Fallback "Mobile/Particles/Additive"
}