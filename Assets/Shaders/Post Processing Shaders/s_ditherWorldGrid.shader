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

        float2 _flarePos;
        float _flareTime;

        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 screenPos = float2(_ScreenParams.x, _ScreenParams.y) ;
            float2 scaledAspectRatioUV = screenPos / _gridScale;

            float2 scaledTexCoord = input.texcoord * scaledAspectRatioUV;
            float2 id = round(scaledTexCoord);
            float2 gridTexCoord = id / scaledAspectRatioUV; // quantized UV

            float2 col = SAMPLE_TEXTURE2D(_GridComputeTex, sampler_GridComputeTex, input.texcoord).xy; // Compute Shader
           //return col.xxxx;
            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);

            blit = pow(blit, 1);
            float3 blitHSV = RGBToHSV(blit);
            float brightnessFactor = 1- blitHSV.z;
            float minThickness = 0.005;
            float gridThicknessThreshold = lerp(minThickness, _gridThickness, brightnessFactor);
            blit *= float4(1,2,2,1);
            //return blit;
            float grid = smoothstep(gridThicknessThreshold, gridThicknessThreshold + (1 / _ScreenParams.x), col.x); // Grid Mask
            //return col.xxxx;
            return max(blit * grid, (float4(0, 0.004, 0.007, 0)) * col.x) * grid;
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