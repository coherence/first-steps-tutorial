//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▘▘▘▜ 																													    
//   UTILITY FUNCTIONS																													    
// ▙ ▖▖▖▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

float2 PanUV(float2 uv, float direction, float speed, float2 offset, float tiling)
{
    direction = direction * 2 - 1;
    float2 d = normalize(float2(cos(3.14 * direction), sin(3.14 * direction)));
    return  (d * _Time.y * speed) + (uv * tiling) + offset;
}

float2 DistortUV(float2 UV, float Amount)
{
    float time = _Time.y;
    
    UV.y += Amount * 0.01 * (sin(UV.x * 3.5 + time * 0.35) + sin(UV.x * 4.8 + time * 1.05) + sin(UV.x * 7.3 + time * 0.45)) / 3.0;
    UV.x += Amount * 0.12 * (sin(UV.y * 4.0 + time * 0.50) + sin(UV.y * 6.8 + time * 0.75) + sin(UV.y * 11.3 + time * 0.2)) / 3.0;
    UV.y += Amount * 0.12 * (sin(UV.x * 4.2 + time * 0.64) + sin(UV.x * 6.3 + time * 1.65) + sin(UV.x * 8.2 + time * 0.45)) / 3.0;

    return UV;
}

float4 SurfaceFoam(float2 uv, float2 movement, float2 offset, float scale, float3 sampling, SamplerState Sampler, Texture2D Texture)
{
    float direction = movement.x;
    float speed = movement.y;
    float cutoff = sampling.x;
    float softness = sampling.y;
    float distortion = sampling.z;

    float2 DistortedUV = DistortUV(PanUV(uv, direction, speed, offset, 1/scale), distortion);

    float edge1 = cutoff - softness;
    float edge2 = cutoff + softness;

    return smoothstep(edge1, edge2, SAMPLE_TEXTURE2D(Texture, Sampler, DistortedUV));
}

void SurfaceFoam_float(float2 uv, float4 movement, float2 scale, float2 offset, float3 sampling, SamplerState Sampler, Texture2D Texture, out float Primary, out float Secondary)
{
    Primary = SurfaceFoam(uv, movement.xy, 0, scale.x, sampling, Sampler, Texture).r;
    Secondary = SurfaceFoam(uv, movement.zw, offset, scale.y, sampling, Sampler, Texture).g;
}


//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▜ 																													    
//   SURFACE FOAM																														    
// ▙ ▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

// MOVEMENT
// x, z: direction 1, 2
// y, w: speed 1, 2

// TILNG AND OFFSET
// x, y: offset x, y
// z, w: scale 1, 2

void SurfaceFoamUV_half(half2 uv, half4 movement, half4 tilingandoffset, out half4 Out)
{
    half2 uv1 = PanUV(uv, movement.x, movement.y, 0, 1/tilingandoffset.z);
    half2 uv2 = PanUV(uv, movement.z, movement.w, tilingandoffset.xy, 1/tilingandoffset.w);

    Out = half4(uv1, uv2);
}

// SAMPLING
// x: cutoff
// y: distortion

void FoamSample_half(half4 uvs, half2 sampling, SamplerState Sampler, Texture2D tex, half2 shadowoffset, out half4 Out)
{
    half cutoff = sampling.x;
    half distortion = sampling.y;
    half2 uv1 = uvs.xy;
    half2 uv2 = uvs.zw;

    half foam1 =   saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv1,                distortion)).r));
    half foam2 =   saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv2,                distortion)).g));

    half shadow1 = saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv1 + shadowoffset, distortion)).r));
    half shadow2 = saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv2 + shadowoffset, distortion)).g));

    Out = half4(shadow1, shadow2, foam1, foam2);
}

void MobileFoamSample_half(half4 uvs, half2 sampling, SamplerState Sampler, Texture2D tex, out half2 Out)
{
    half cutoff = sampling.x;
    half distortion = sampling.y;
    half2 uv1 = uvs.xy;
    half2 uv2 = uvs.zw;

    half foam1 = saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv1, distortion)).r));
    half foam2 = saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv2, distortion)).g));

    Out = half2(foam1, foam2);
}

void MobileIntersectionFoamSample_half(half2 uv, half2 sampling, SamplerState Sampler, Texture2D tex, out half Out)
{
    half cutoff = sampling.x;
    half distortion = sampling.y;

    Out = saturate(step(cutoff, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uv, distortion)).r));
}

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▘▘▘▜ 																													    
//   INTERSECTION FOAM																														    
// ▙ ▖▖▖▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━


float2 ShoreMovement(float2 uv, float speed, float tiling)
{
    return (uv * tiling) + speed * _Time.y;
}

// movement
// x: direction
// y: speed

void IntersectionFoamUV_half(half2 uv, half2 movement, half scale, out half4 Directional, out half4 ByDepth)
{
    ByDepth = 0;

    half direction = movement.x;
    half speed = movement.y;

    half scale1 = scale;
    half scale2 = scale * 2;

    half2 uv1 = PanUV(uv, direction, speed, 0, 1/scale1);
    half2 uv2 = PanUV(uv, 0, 0, 0, 1/scale2);

    Directional = half4(uv1, uv2);
}

void MobileIntersectionFoamUV_half(half2 uv, half2 movement, half scale, out half2 Out)
{
    Out = PanUV(uv, movement.x, movement.y, 0, 1/scale);
}

void IntersectionFoamColor_half(half4 color, half foam1, half foam2, out half4 Foam)
{
    half primary_foam = foam1 * color.w;
    half secondary_foam = foam2 * color.w;

    half3 foam_color = primary_foam * color.rgb + (1-primary_foam) * secondary_foam * color.rgb;
    half transparency = primary_foam + (1-primary_foam) * secondary_foam;

    Foam = half4(foam_color, transparency);
}

void SampleSurfaceFoamTexture_half(half4 uvs, half2 sampling, SamplerState Sampler, Texture2D tex, out half Foam1, out half Foam2)
{
    Foam1 = saturate(step(sampling.x, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uvs.xy, sampling.y)).r));
    Foam2 = saturate(step(sampling.x, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uvs.zw, sampling.y)).g));
}

void SampleSurfaceFoamTextureShadows_half(half4 uvs, half2 sampling, SamplerState Sampler, Texture2D tex, half2 offset, out half Shadow1, out half Shadow2)
{
    Shadow1 = saturate(step(sampling.x, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uvs.xy + offset, sampling.y)).r));
    Shadow2 = saturate(step(sampling.x, SAMPLE_TEXTURE2D(tex, Sampler, DistortUV(uvs.zw + offset, sampling.y)).g));
}


void FoamColor_half(half4 foamcolor1, half4 foamcolor2, half foam1, half foam2, half shadow1, half shadow2, out half Shadow, out half4 Foam)
{
    half primary_foam = foam1 * foamcolor1.w;
    half secondary_foam = foam2 * foamcolor2.w;

    half3 color = primary_foam * foamcolor1.rgb + (1-primary_foam) * secondary_foam * foamcolor2.rgb;
    half transparency = primary_foam + (1-primary_foam) * secondary_foam;

    Foam = half4(color, transparency);

    half primary_shadow = shadow1 * foamcolor1.w;
    half secondary_shadow = shadow2 * foamcolor2.w;

    Shadow = primary_shadow + (1-primary_shadow) * secondary_shadow;
}

void MobileFoamColor_half(half4 color1, half4 color2, half2 foam, out half4 Out)
{
    foam.x *= color1.a;
    foam.y *= color2.a;

    half3 color = foam.x * color1.rgb + (1-foam.x) * foam.y * color2.rgb;
    half transparency = foam.x + (1-foam.x) * foam.y;

    Out = half4(color, transparency);
}

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▘▘▘▜ 																													    
//   INTERSECTION FOAM																														    
// ▙ ▖▖▖▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

// MOVEMENT
// x: direction 1
// y: speed 1
// z: direction 2
// w: speed 2

// TILNG AND OFFSET
// x: offset x
// y: offset y
// z: scale 1
// w: scale 2

void FoamUV_float(float2 uv, float4 movement, float2 offset, float2 scale, float depth, out float4 ByDepth, out float4 Directional)
{
    float direction1 = movement.x;
    float direction2 = movement.z;

    float speed1 = movement.y;
    float speed2 = movement.w;

    float scale1 = scale.x;
    float scale2 = scale.y;

    float2 shoreline_uv = float2(uv.x * 0.2, depth);

    ByDepth.xy = PanUV(shoreline_uv, 0.75, speed1, 0, 1/scale1);
    ByDepth.zw = PanUV(shoreline_uv, 0.75, speed2, 0, 1/scale2);

    Directional.xy = PanUV(uv, direction1, speed1, 0, 1/scale1);
    Directional.zw = PanUV(uv, direction2, speed2, offset, 1/scale2);
}

void SampleTexture_float(float4 uvs, float distortion, SamplerState Sampler, Texture2D Texture, out float R, out float G)
{
    R = SAMPLE_TEXTURE2D(Texture, Sampler, DistortUV(uvs.xy, distortion)).r;
    G = SAMPLE_TEXTURE2D(Texture, Sampler, DistortUV(uvs.zw, distortion)).g;
}