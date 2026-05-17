Shader "TI-Shader/Mesh-Particle/Particle_offsetUV_RGB_ink_Optimize" {
	Properties {
		_InvFade ("Soft Particles Factor", Float) = 1
		[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 4
		[Enum(UnityEngine.Rendering.CullMode)] _Culling ("Culling", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Float) = 10
		_MainTex ("Main Tex", 2D) = "white" {}
		[Toggle] _IsGstep ("Is G step?", Float) = 1
		_Blend ("Blend ", Range(-0.5, 1.5)) = 0.5
		_BlendthresholdInv ("Blend threshold Inverse", Float) = 2
		[HDR] _Startcolor ("Start color", Vector) = (4,4,4,1)
		[HDR] _Finishcolor ("Finish color", Vector) = (0,0,0,0)
		[HDR] _Highlightcolor ("Highlight color", Vector) = (4,4,4,1)
		[HDR] _Bordercolor ("Border color", Vector) = (0,0,0,0)
		[Toggle] _Usemidcolor ("Use mid color?", Float) = 0
		[HDR] _Midcolor ("Mid color", Vector) = (0.5,0.5,0.5,0)
		_Innerwidth ("Inner width", Float) = 0.8
		_Borderwidth ("Border width", Float) = 0.1
		_Step ("Step", Float) = 0.01
		_Innerrange ("Inner range", Float) = 0.5
		_Fresnelcolor ("Fresnel color", Vector) = (0,0,0,0)
		_Fresnelpower ("Fresnel power", Float) = 0.01
		_Fresnelscale ("Fresnel scale", Float) = 0
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
	Fallback "Mobile/Particles/Alpha Blended"
	//CustomEditor "ASEMaterialInspector"
}