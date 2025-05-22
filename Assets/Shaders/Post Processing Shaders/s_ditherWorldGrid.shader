Shader "Unlit/s_ditherWorldGrid"
{
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        #include "Assets/Shaders/HLSL/HelperShaderFunctions.hlsl"
        #include "Assets/Shaders/HLSL/NoiseFunctions.hlsl"

        float _gridScale;
        float _gridFallOff;
        float _gridThickness;
        float _sonarPingTime;
        float2 _playerPos;

        float2 aspectRatioPentile(float2 wh)
        {
            float2 w = float2(wh.x / wh.y, 1);
            float2 h = float2(1, wh.y / wh.x);
            return max(w,h);
        }

        float sonarSDF(float2 uv, float2 screenPos)
        {
            float2 aspScreenPos = aspectRatioPentile(screenPos);

            float2 p = (uv * aspScreenPos) - (_playerPos * aspScreenPos);
            float innerLength = saturate(1 - pow(length(p*4),2));

            p /= _sonarPingTime;
            float sonarPingLength = pow(length(p*0.75),2);
            float ping = saturate((1 - distance(sonarPingLength, 1 - sonarPingLength)) - _sonarPingTime);
            return saturate((ping * 2) + innerLength);
        }


        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 screenPos = float2(_ScreenParams.x, _ScreenParams.y);
            float2 scaledAspectRatioUV = screenPos / _gridScale;

            float2 scaledTexCoord = input.texcoord * scaledAspectRatioUV;
            float2 roundTexCoord = round(scaledTexCoord);
            float2 gridTexCoord = roundTexCoord / scaledAspectRatioUV;
            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);
            float sonarPing = sonarSDF(gridTexCoord, screenPos);
            //return sonarPing;
            blit *= sonarPing;
            float2 gv = abs(frac(scaledTexCoord) - 0.5);
            float gridSDF = DistLine(gv.x, gv.y);

            float t = _Time.y * 0.1;
            float2 noiseUV = t + roundTexCoord;
            float gridNoise = SimpleNoise(noiseUV, 10);
            gridSDF *= gridNoise;

            float3 blitHSV = RGBToHSV(blit);
            float minFalloff = 0.1;
            float brightnessFactor = pow(saturate(1.0 - blitHSV.z), _gridFallOff);
            brightnessFactor = max(brightnessFactor, minFalloff);

            float gridThicknessThreshold = _gridThickness * brightnessFactor;

            float grid = smoothstep(gridThicknessThreshold, gridThicknessThreshold + (1 / _ScreenParams.x), gridSDF);

            float4 output = blit * grid.xxxx;
            return output;
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