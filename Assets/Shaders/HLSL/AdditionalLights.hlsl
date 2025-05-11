TotalAttenuation = 0;
#ifndef SHADERGRAPH_PREVIEW

    uint pixelLightCount = GetAdditionalLightsCount();

    LIGHT_LOOP_BEGIN(pixelLightCount)
    
        lightIndex = GetPerObjectLightIndex(lightIndex);
        Light light = GetAdditionalPerObjectLight(lightIndex, WorldPosition);
    
        light.shadowAttenuation = AdditionalLightRealtimeShadow(lightIndex, WorldPosition, light.direction);
        float atten = light.distanceAttenuation * light.shadowAttenuation;

    
        TotalAttenuation += atten;

    LIGHT_LOOP_END
#endif