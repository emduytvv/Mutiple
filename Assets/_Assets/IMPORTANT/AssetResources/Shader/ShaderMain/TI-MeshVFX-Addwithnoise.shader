Shader "Addwithnoise" {
	Properties {
		[HDR] _MainCol ("MainCol", Vector) = (1,1,1,0)
		_Outline ("Outline", Range(0, 10)) = 0
		[HDR] _ewew ("ewew", Vector) = (0.08962262,0.4940201,1,0)
		_Texture0 ("Texture 0", 2D) = "white" {}
		_Dissolve ("Dissolve", Range(0, 1)) = 0.08191914
		_EdgeWidth0 ("EdgeWidth 0", Range(-1, 0)) = -0.2163453
		_EdgeColor ("EdgeColor", Vector) = (1,1,1,0)
		[Toggle(_DARKORLIGHT_ON)] _DarkorLight ("Dark or Light ?", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	//CustomEditor "ASEMaterialInspector"
}