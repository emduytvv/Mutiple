Shader "UI/GradientShader" {
	Properties {
		_TopLeftColor ("TopLeftColor", Vector) = (1,1,1,1)
		_TopRightColor ("TopRightColor", Vector) = (1,1,1,1)
		_BotLeftColor ("BotLeftColor", Vector) = (1,1,1,1)
		_BotRightColor ("BotRightColor", Vector) = (1,1,1,1)
		_LeftStartGradient ("LeftStartGradient", Range(0, 1)) = 0
		_RightEndGradient ("RightEndGradient", Range(0, 1)) = 1
		_TopEndGradient ("TopEndGradient", Range(0, 1)) = 0
		_BotStartGradient ("BotStartGradient", Range(0, 1)) = 1
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
}