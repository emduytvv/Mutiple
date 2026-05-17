Shader "My Shader/PBR/Cel-character-alpha" {
	Properties {
		_Finaltint ("Final tint", Vector) = (1,1,1,0)
		_Emissive ("Emissive", Float) = 1.1
		_UVSpeed ("UV Speed", Vector) = (0,0,0,0)
		_MainUVTiling ("Main UV Tiling", Vector) = (1,1,0,0)
		_MainUVOffset ("Main UV Offset ", Vector) = (0,0,0,0)
		_MainTex ("Main Tex", 2D) = "white" {}
		_ORMmap ("ORM map", 2D) = "black" {}
		_Normalmap ("Normal map", 2D) = "bump" {}
		_Fresnelcolor ("Fresnel color", Vector) = (1,1,1,0)
		_Fresnelscale ("Fresnel scale", Float) = 0.25
		_Fresnelpower ("Fresnel power", Float) = 3
		_Maskfresnelcolor ("Mask fresnel color", Vector) = (1,1,1,0)
		_Maskfresnelscale ("Mask fresnel scale", Float) = 0.25
		_Maskfresnelpower ("Mask fresnel power", Float) = 3
		_Fogcolor ("Fog color", Vector) = (0.6823117,0.5304824,0.8584906,0)
		_FogDistance ("Fog Distance", Float) = 15
		_FogFalloff ("Fog Falloff", Float) = 200
		_Smoothness ("Smoothness", Range(0, 1)) = 1
		_Glossness ("Glossness", Range(0, 1)) = 0
		[Toggle] _Hasnormal ("Has normal?", Float) = 1
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
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
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}