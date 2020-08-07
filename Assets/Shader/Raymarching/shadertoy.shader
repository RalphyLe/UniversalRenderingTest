Shader "Custom/RayMarching/shadertoy"
{

	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_offsets("Offset",vector) = (0,0,0,0)
	}

	//通过CGINCLUDE我们可以预定义一些下面在Pass中用到的struct以及函数，
	//这样在pass中只需要设置渲染状态以及调用函数,shader更加简洁明了
	HLSLINCLUDE

	    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "DistanceFunctions.hlsl"
		//#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _offsets;
        CBUFFER_END

        struct ver_blur{
            float4 positionOS : POSITION;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct v2f_blur{
	    	float4 pos : SV_POSITION;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_OUTPUT_STEREO
	    };

	    TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);


	    //vertex shader
	    v2f_blur vert_blur(ver_blur v)
	    {
	    	v2f_blur o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
	    	o.pos = TransformObjectToHClip(v.positionOS.xyz);
	    	o.uv = v.uv;
	    	return o;
	    }

		float4 GetWorldPos(float2 uv){
			float2 trans = uv*2 - float2(1,1);
			float4 clip = float4(trans,0,1);
			float4 view_pos = mul(UNITY_MATRIX_I_VP,clip);
			float4 world_pos = mul(unity_CameraToWorld,view_pos);
			return world_pos;
		}

		float4 raymarching(float3 origin,float3 normalview,float r){
			float t = 0;
			float4 result = float4(0,0,0,0);
			for(int i=1;i<255;i++){
				float3 p = origin + normalview*t;
				float d = sdSphere(p,r);
				if(d<0.01f){
					result = float4(1,0,0,1);
					break;
				}
				t+=d;
			}

			return result;
		}

	    //fragment shader
	    float4 frag_blur(v2f_blur i) : SV_Target
	    {
	    	float4 pos = GetWorldPos(i.uv);
			float3 origin = _WorldSpaceCameraPos;
			float3 dir = pos.xyz -origin;
			float4 color = raymarching(origin,normalize(dir),1.0);
	    	return color;
	    }

	ENDHLSL

	//开始SubShader
	SubShader
	{
        Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100
		ZTest Always Cull Off ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
		{
            Name "Gaussian Blur"
			//后处理效果一般都是这几个状态

			//使用上面定义的vertex和fragment shader
			HLSLPROGRAM
			    #pragma vertex vert_blur
		        #pragma fragment frag_blur
			ENDHLSL
		}

	}
	//后处理效果一般不给fallback，如果不支持，不显示后处理即可
}