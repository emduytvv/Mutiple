Shader "TI-Shader/Mesh-Particle/MeshVFX_fire_offsetUV" {
	Properties {
		_InvFade ("Soft Particles Factor", Float) = 1
		[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Float) = 10
		_MainTex ("Main Tex", 2D) = "white" {}
		[Toggle] _IsGstep ("Is G step?", Float) = 0
		_blend_top_bot ("blend_top_bot", Range(-0.5, 1.5)) = 0.5
		_Noise ("Noise", 2D) = "white" {}
		[HDR] _top_color ("top_color", Vector) = (0,0.2019179,0.9794025,0)
		[HDR] _bot_color ("bot_color", Vector) = (0,0.7921569,0.7426244,0)
		[HDR] _in_color ("in_color", Vector) = (0.01557495,0.3432865,0.4716981,1)
		_Tilingsmallnoise ("Tiling small noise", Float) = 0.4
		_Tilingbignoise ("Tiling big  noise", Float) = 0.4
		_Noisesmallspeed ("Noise small speed", Vector) = (0,0,0,0)
		_Noisebigspeed ("Noise big speed", Vector) = (0,0,0,0)
		[HDR] _Bordercolor ("Border color", Vector) = (1,1,1,0)
		_Innerwidth ("Inner width", Float) = 0
		_Borderwidth ("Border width", Float) = 0
		_Step ("Step", Range(0, 0.01)) = 0.008835083
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