Shader "TI-Shader/PBR/TI-PBR-base" {
	Properties {
		[HDR] _Lightcolor ("Lightcolor", Vector) = (1,1,1,0)
		_Finaltint ("Final tint", Vector) = (1,1,1,0)
		[HDR] _Ambientcolor ("Ambient color", Vector) = (0,0,0,1)
		_SpeedU ("Speed U", Float) = 0
		_SpeedV ("Speed V", Float) = 0
		_MainTex ("Main Tex", 2D) = "white" {}
		_Normalmap ("Normal map", 2D) = "bump" {}
		_ESMmap ("ESM map", 2D) = "black" {}
		_Emissionvalue ("Emission value", Float) = 1
		[HDR] _Fresnelcolor ("Fresnel color", Vector) = (1,1,1,1)
		_Fresnelscale ("Fresnel scale", Float) = 0.2
		_Fresnelpower ("Fresnel power", Float) = 0.8
		_WFFmap ("WFF map", 2D) = "black" {}
		[HDR] _Fresnelcolormask ("Fresnel color mask", Vector) = (0,0,0,0)
		_Fresnalmaskscale ("Fresnal mask scale", Float) = 0.085
		_Fresnalmaskpower ("Fresnal mask power", Float) = 0.2
		[HDR] _Rimlight1color ("Rimlight 1 - color", Vector) = (1,1,1,0)
		_Rimlight1power ("Rimlight 1 - power", Float) = 3
		_Rimlight1dir ("Rimlight 1 - dir", Vector) = (0,0,0,0)
		[HDR] _Rimlight2color ("Rimlight 2 - color", Vector) = (1,1,1,0)
		_Rimlight2power ("Rimlight 2 - power", Float) = 3
		_Rimlight2dir ("Rimlight 2 - dir", Vector) = (0,0,0,0)
		_Specularcutoff ("Specular cutoff", Float) = 0.5
		_Specularsmoothness ("Specular smoothness", Float) = 0.05
		_Specularbrightness ("Specular brightness", Float) = 0.4
		_Specularhighlightvalue ("Specular highlight value", Float) = 0.1
		_Specularhighlightarea ("Specular highlight area", Float) = 0.3
		_ClipOffsetY ("Clip Offset Y", Float) = 0
		[HDR] _Emissivecolor ("Emissive color", Vector) = (0,0,0,0)
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