Shader "TW/UI/UI_CustomBlend_Animated_Soft" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[Space(10)] [Header(Blend Settings)] [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Float) = 10
		[Space(10)] [Header(Render Settings)] [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
		[Space(10)] [Header(Color Adjustments)] [HDR] _Colormul ("Color mul", Vector) = (1,1,1,1)
		[HDR] _Coloradd ("Color add", Vector) = (0,0,0,1)
		_Emissive ("Emissive", Float) = 1
		[Toggle] _Vertexadd ("Vertex add?", Float) = 0
		_Preadd ("Pre-add", Vector) = (0,0,0,0)
		[Space(10)] [Header(Gradient Settings)] _GradientTex ("Gradient Tex", 2D) = "gray" {}
		[Toggle] _IsAlphaGradient ("Is Alpha Gradient?", Float) = 0
		_Intensity ("Gradient intensive", Float) = 0
		[Space(10)] [Header(Shine Animation)] [Toggle] _UseShineAnim ("Use Shine Animation?", Float) = 0
		_ShineValueA ("Shine value A", Float) = 1
		_ShineValueB ("Shine value B", Float) = 1.5
		_DurationShine ("Duration shine (A→B→A)", Float) = 1
		_IntervalShine ("Interval between cycles", Float) = 0
		[Enum(Sine,0,Triangle,1,Square,2,Sawtooth,3,Reverse Sawtooth,4)] _ShineWaveType ("Shine Wave Type", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
			float4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy) * _Color;
			}

			ENDHLSL
		}
	}
	Fallback "UI/Default"
	//CustomEditor "ASEMaterialInspector"
}