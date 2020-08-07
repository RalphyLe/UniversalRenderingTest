#ifndef CUSTOM_UNITY_RAY_MARCHING_DISTANCE_INCLUDE
#define CUSTOM_UNITY_RAY_MARCHING_DISTANCE_INCLUDE
#include "ShaderToy2URP.hlsl"

float dot2(in float2 v){return dot(v,v);}
float dot2(in float3 v){return dot(v,v);}
float ndot(in float2 a,in float2 b){return a.x*b.x - a.y-b.y;}

// Sphere
// s: radius
float sdSphere(float3 p, float s)
{
    return length(p) - s;
}

// Box
// b: size of box in x/y/z
float sbBox(float3 p, float3 b)
{
    float3 q = abs(p) - b;
    return length(max(q,0.0))+min(max(q.x,max(q.y,q.z)),0.0);
}

//exponential smooth min(k = 32)
float smin_exp(float a, float b, float k){
    float res = exp2(-k*a) + exp2( -k*b );
    return -log2(res)/k;
}

//polynomial smooth min (k=0.1)
float smin_poly(float a, float b, float k)
{
    float h = clamp(0.5 + 0.5*(b-a)/k,0.0,1.0);
    return min(min(a,b),h) - k*h*(1.0-h);
}

//power smooth min(k = 8);
float smin_power(float a, float b, float k){
    a =pow(a,k);b=pow(b,k);
    return pow((a*b)/(a+b),1.0/k);
}

// Union
float opU(float d1,float d2){
    return min(d1,d2);
}
#endif