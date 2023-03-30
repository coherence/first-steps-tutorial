//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━																												
// Copyright 2020, Alexander Ameye, All rights reserved.
// https://alexander-ameye.gitbook.io/stylized-water/
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━	

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▘▘▜ 																													    
//   LIGHTING MODELS																														    
// ▙ ▖▖▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

half LightingSpecular(half3 L, half3 N, half3 V, half smoothness)
{
    float3 H = SafeNormalize(float3(L) + float3(V));
    half NdotH = saturate(dot(N, H));
    return pow(NdotH, smoothness);
}

half LightingLambert(half3 L, half3 N)
{
    return saturate(dot(N,L));
}


//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▘▘▘▘▘▘▜ 																													    
//   LIGHTING FUNCTIONS																														    
// ▙ ▖▖▖▖▖▖▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

void MainLighting_half(half3 positionWS, half3 normalWS, half3 viewWS, half smoothness, out half specular, out half diffuse)
{
    specular = 0;
    diffuse = 0;
    
    #ifndef SHADERGRAPH_PREVIEW
        smoothness = exp2(10 * smoothness + 1);
        
        normalWS = normalize(normalWS);
        viewWS = SafeNormalize(viewWS);

        Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));

        diffuse = LightingLambert(mainLight.direction, normalWS);
        //half diffuse_hard = smoothstep(0.005,0.01,diffuse_soft);
        //half diffuse_term = lerp(diffuse_soft, diffuse_hard, hardness);

        specular = LightingSpecular(mainLight.direction, normalWS, viewWS, smoothness);
        //half specular_hard = smoothstep(0.005,0.01,specular_soft);
        //half specular_term = lerp(specular_soft, specular_hard, hardness);

        //specular = specular_term * specular_color;
        //diffuse = diffuse_term * diffuse_color;
    #endif
}

void AdditionalLighting_half(half3 positionWS, half3 normalWS, half3 viewWS, half smoothness, half hardness, out half3 specular, out half3 diffuse)
{
    specular = 0;
    diffuse = 0;

    #ifndef SHADERGRAPH_PREVIEW
        smoothness = exp2(10 * smoothness + 1);

        normalWS = normalize(normalWS);
        viewWS = SafeNormalize(viewWS);

        // additional lights
        int pixelLightCount = GetAdditionalLightsCount();
        for (int i = 0; i < pixelLightCount; ++i)
        {
            Light light = GetAdditionalLight(i, positionWS);
            half3 attenuatedLight = light.color * light.distanceAttenuation * light.shadowAttenuation;

            diffuse += LightingLambert(attenuatedLight, light.direction, normalWS);

            half specular_soft = LightingSpecular(light.direction, normalWS, viewWS, smoothness);
            half specular_hard = smoothstep(0.005,0.01,specular_soft);
            half specular_term = lerp(specular_soft, specular_hard, hardness);

            specular += specular_term * attenuatedLight;
        }
    #endif
}

void MainLight_half(half3 positionWS, out half3 Direction, out half3 Color)
{
    #if SHADERGRAPH_PREVIEW
        Direction = half3(0.5, 0.5, 0);
        Color = 1;
    #else
        Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));
        Direction = mainLight.direction;
        Color = mainLight.color;
    #endif
}

//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
// ▛ ▘▘▘▘▜ 																													    
//   NORMALS																														    
// ▙ ▖▖▖▖▟	 				
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

float2 Panner(float2 uv, float direction, float speed, float2 offset, float tiling)
{
    direction = direction * 2 - 1;
    float2 d = normalize(float2(cos(3.14 * direction), sin(3.14 * direction)));
    return  (d * _Time.y * speed) + (uv * tiling) + offset;
}

void NormalsUV_half(half2 UV, half2 movement, out half2 UV1, out half2 UV2)
{
    half speed1 = movement.x * -0.5;
    half speed2 = movement.x;

    half tiling1 = movement.y * 0.5;
    half tiling2 = movement.y;

    UV1 = Panner(UV, 1, speed1, 0, 1/tiling1);
    UV2 = Panner(UV, 1, speed2, 0, 1/tiling2);
}