Shader "Unlit/s_ditherWorldGrid"
{
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        #include "Assets/Shaders/HLSL/HelperShaderFunctions.hlsl"
        #include "Assets/Shaders/HLSL/NoiseFunctions.hlsl"


        TEXTURE2D(_GridComputeTex);
        SAMPLER(sampler_GridComputeTex);

        float _gridScale;
        float _gridThickness;
        float _sonarPingTime;
        float2 _playerPos;

        float2 aspectRatioPentile(float2 wh)
        {
            float2 w = float2(wh.x / wh.y, 1);
            float2 h = float2(1, wh.y / wh.x);
            return max(w,h);
        }

        float sonarSDF(float2 uv, float2 screenPos, float minValue, float maxValue)
        {
            float2 aspScreenPos = aspectRatioPentile(screenPos);

            float2 p = (uv * aspScreenPos) - (_playerPos * aspScreenPos);
            float innerLength = saturate(1 - pow(length(p*2),1));

            p /= _sonarPingTime;
            float sonarPingLength = pow(length(p*0.25),4);
            float ping = saturate((1 - distance(sonarPingLength, 1 - sonarPingLength)) - _sonarPingTime);
            ping = saturate(ping * 2 + innerLength);
            return lerp(minValue, maxValue, ping);
        }

        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 screenPos = float2(_ScreenParams.x, _ScreenParams.y) ;
            float2 scaledAspectRatioUV = screenPos / _gridScale;

            float2 scaledTexCoord = input.texcoord * scaledAspectRatioUV;
            float2 id = round(scaledTexCoord);
            float2 gridTexCoord = id / scaledAspectRatioUV;

            float sonarPing = sonarSDF(gridTexCoord, screenPos, 0, 1);

            float col = SAMPLE_TEXTURE2D(_GridComputeTex, sampler_GridComputeTex, input.texcoord).r;;
            //return col.xxxx;
            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);

            blit *= 3;
            blit = saturate(pow(blit, 10));
            blit *= pow(sonarPing,1);
            float3 blitHSV = RGBToHSV(blit);
            float brightnessFactor = 1- saturate(blitHSV.z);
            float minThickness = 0.0005;
            float gridThicknessThreshold = lerp(minThickness, _gridThickness, brightnessFactor);
            float grid = smoothstep(gridThicknessThreshold, gridThicknessThreshold + (1 / _ScreenParams.x), col);

            return blit * grid;
        }


        float4 apply(Varyings input) : SV_Target
        {
            return SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, input.texcoord);
        }

    ENDHLSL

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off
        Cull Off

        Pass { // pass 0
            Name "CalculateDitherWorldGridPass"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment calc
            ENDHLSL
        }

        Pass { // pass 1
            Name "ApplyDitherWorldGridPass"
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment apply
            ENDHLSL
        }
    }
}