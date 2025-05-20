void ComputeTotalAttenuation_half(float3 WorldPosition, out float TotalAttenuation)
{
    TotalAttenuation = 0.0;

    
#ifndef SHADERGRAPH_PREVIEW

    uint pixelLightCount = GetAdditionalLightsCount();

    LIGHT_LOOP_BEGIN(pixelLightCount)

    uint perObjectLightIndex = GetPerObjectLightIndex(lightIndex);

    Light light = GetAdditionalPerObjectLight(perObjectLightIndex, WorldPosition);

    float atten = light.distanceAttenuation;
    float intensity = dot(light.color.rgb, float3(0.2126, 0.7152, 0.0722));
    TotalAttenuation += atten * intensity;

    LIGHT_LOOP_END

#endif
}