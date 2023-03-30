// Designed & Developed by Alexander Ameye
// https://alexander-ameye.gitbook.io/stylized-water/
// Version 1.1.0

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▜ 																													    
//   CAUSTICS																													    
// ▙ ▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

float3 RGBSplit(float Split, Texture2D Texture, SamplerState Sampler, float2 UV)
{
    float2 UVR = UV + float2(Split, Split);
    float2 UVG = UV + float2(Split, -Split);
    float2 UVB = UV + float2(-Split, -Split);

    float r = SAMPLE_TEXTURE2D(Texture, Sampler, UVR).r;
    float g = SAMPLE_TEXTURE2D(Texture, Sampler, UVG).g;
    float b = SAMPLE_TEXTURE2D(Texture, Sampler, UVB).b;

    return float3(r,g,b);
}

void Caustics_float(float2 uv, SamplerState Sampler, float speed, float scale, float split, Texture2D Texture, out float3 Out)
{
    split *= 0.01f;

    float3 tex1 = RGBSplit(split, Texture, Sampler, Panner(uv, 1, speed, float2(0,0), 1/scale));
    float3 tex2 = RGBSplit(split, Texture, Sampler, Panner(uv, 1, speed, float2(0,0), -1/scale));

    Out =  min(tex1, tex2);
}

void IVPFix_float(float4x4 In, out float4x4 Out)
{
    Out = In;
    #if UNITY_REVERSED_Z
        Out._12_22_32_42 = -In._12_22_32_42;              
    #endif
}

void DepthFix_float(float In, out float Out)
{
    Out = In;
    #ifndef UNITY_REVERSED_Z
        Out = In * 2 - 1;         
    #endif
}

void PerspectiveDivide_float(float4 In, out float3 Out)
{
    Out = In.rgb / In.a;
}

void Luminance_float(float3 In, out float Out)
{
    Out = Luminance(In);
}