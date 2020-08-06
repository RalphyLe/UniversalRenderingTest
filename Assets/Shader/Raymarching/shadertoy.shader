Shader "Custom/RayMarching/shadertoy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

HLSLINCLUDE
    #include "DistanceFunction.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

    CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float _MaxIterations;

    CBUFFER_END

    int Max_MARCHING_STEP = 255;
    int MIN_DIST = 0.0;
    float MAX_DIST = 100.0;
    float EPSILON = 0.0001;

    struct appdata
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f
    {
        float2 uv : TEXCOORD0;
        float3 ray : TEXCOORD1;
        float4 vertex : SV_POSITION;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    struct Ray{
        float3 origin;
        float3 direction;
    };

    struct RayHit{
        float4 position;
        float3 normal;
        float3 color;
    }

    Ray createRay(float3 origin,float3 direction){
        Ray ray;
        ray.origin = origin;
        ray.direction = direction;
        return ray;
    }

    RayHit createRayHit(){
        RayHit hit;
        hit.position = float4(0.0f,0.0f,0.0f,0.0f);
        hit.normal = float3(0.0f,0.0f,0.0f,0.0f);
        hit.color = float3(0.0f,0.0f,0.0f,0.0f)
    }

    v2f vert (appdata v)
    {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex = TransformObjectToWorld(v.vertex);
        o.uv = v.uv;
        return o;
    }

	TEXTURE2D(_MainTex);
    SAMPLER(sampler_MainTex);

    fixed4 frag (v2f i) : SV_Target
    {
        float2 size = iResolution.xy;
        
        float3 worldPosition = i.vertext;
        float3 worldSpaceCameraPos = GetCameraPositonWS();
        float3 viewDirection = normalize(i.worldPos - worldSpaceCameraPos);
        Ray ray = createRay(worldSpaceCameraPos,viewDirection);
        return raymarch(ray,1);
    }

    float4 raymarch(Ray ray,float start){
        float t=start;
        float4 result;
        for(int i=0; i<256;i++){
            if(t>250){
                result = float4(0.0,0.0,0.0,0.0);
                break;
            }

            p = ray.origin + ray.direction*t;
            float d = sdSphere(p,1.0)
            if(d < 0.01){
                result = float4(1.0,0,0,1.0);
                break;
            }
            t +=d;
        }
        return result;
    }
ENDHLSL

    SubShader
    {
        Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}
        LOD 100
		ZTest Always Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            ENDHLSL
        }
    }
}
