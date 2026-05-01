Shader "Shader Graphs/All_Shader_Gen_V1.1.3" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		[Toggle(_MAINTEX_ALPHA_ASSET)] _MAINTEX_ALPHA_ASSET ("MainTex Alpha Asset", Float) = 0
		_MainTex_Asset ("MainTex Asset", Float) = 0
		_Maintex_HDR ("Maintex HDR", Float) = 1
		[ToggleUI] _Polar_UV_Maintex ("Polar UV Maintex", Float) = 0
		[ToggleUI] _Use_Custom_Data_Pan_Main_Tex ("Use Custom Data Pan Main Tex", Float) = 0
		_Twist_UV_Main_Tex ("Twist UV Main Tex", Float) = 0
		[KeywordEnum(CustomData Pan X, CustomData Pan Y)] _PAN_MAIN_TEX_X_OR_Y ("Pan Main Tex X or Y", Float) = 0
		[ToggleUI] _Use_Custom_Data_HDR_Main_Tex ("Use Custom Data HDR Main Tex", Float) = 0
		_Distorsion_Tex_Main_Tex ("Distorsion Tex ( Main Tex)", 2D) = "white" {}
		_Distorsion_Value ("Distorsion Value", Float) = 0
		_Gradient_Map_Fill_Color ("Gradient Map Fill Color", 2D) = "white" {}
		_Use_Gradient_Map_Fill_Color ("Use  Gradient Map Fill Color", Float) = 0
		_ColorW ("ColorW", Vector) = (1,1,1,1)
		_ColorB ("ColorB", Vector) = (0,0,0,1)
		_NoiseTex ("NoiseTex", 2D) = "white" {}
		_MaskTex ("MaskTex", 2D) = "white" {}
		[ToggleUI] _Use_Custom_Data_Pan_Mask ("Use Custom Data Pan Mask", Float) = 0
		_Polar_UV_Noise_Tex ("Polar UV Noise Tex", Float) = 0
		[KeywordEnum(CustomData Pan X, CustomData Pan Y)] _PAN_MASK_TEX_X_OR_Y ("Pan Mask Tex X or Y", Float) = 0
		_Polar_UV_Mask_Tex ("Polar UV Mask Tex", Float) = 0
		_DissovleTex ("DissovleTex", 2D) = "white" {}
		_Dissovle_Edge_Smooth ("Dissovle_Edge_Smooth", Float) = 0
		_Edge_Scale ("Edge_Scale", Float) = 0
		_Polar_UV_Dissovle_Tex ("Polar UV Dissovle Tex", Float) = 0
		_Edge_Color ("Edge_Color", Vector) = (1,1,1,1)
		_Edge_Color_HDR ("Edge_Color HDR", Float) = 1
		[ToggleUI] _Use_Custom_Data_HDR_Edge_Color ("Use Custom Data HDR Edge Color", Float) = 0
		_Mask_for_Dissovle_Trail ("Mask for Dissovle Trail", 2D) = "black" {}
		[ToggleUI] _MainTex_FlipBook ("MainTex_FlipBook", Float) = 0
		_Flipbook_Speed ("Flipbook_Speed", Float) = 0
		_X ("X", Float) = 1
		_Y ("Y", Float) = 1
		_Color_BackFace ("Color_BackFace", Vector) = (1,1,1,0)
		[Toggle(_USE_TWO_FACE)] _USE_TWO_FACE ("Use two face", Float) = 0
		[HideInInspector] _BUILTIN_Surface ("Float", Float) = 1
		[HideInInspector] _BUILTIN_Blend ("Float", Float) = 0
		[HideInInspector] _BUILTIN_AlphaClip ("Float", Float) = 0
		[HideInInspector] _BUILTIN_SrcBlend ("Float", Float) = 1
		[HideInInspector] _BUILTIN_DstBlend ("Float", Float) = 0
		[HideInInspector] _BUILTIN_ZWrite ("Float", Float) = 0
		[HideInInspector] _BUILTIN_ZWriteControl ("Float", Float) = 0
		[HideInInspector] _BUILTIN_ZTest ("Float", Float) = 4
		[HideInInspector] _BUILTIN_CullMode ("Float", Float) = 0
		[HideInInspector] _BUILTIN_QueueOffset ("Float", Float) = 0
		[HideInInspector] _BUILTIN_QueueControl ("Float", Float) = -1
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
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
}