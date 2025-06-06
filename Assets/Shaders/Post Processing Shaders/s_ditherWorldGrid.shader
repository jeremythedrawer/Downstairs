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

        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 screenPos = float2(_ScreenParams.x, _ScreenParams.y) ;
            float2 scaledAspectRatioUV = screenPos / _gridScale;

            float2 scaledTexCoord = input.texcoord * scaledAspectRatioUV;
            float2 id = round(scaledTexCoord);
            float2 gridTexCoord = id / scaledAspectRatioUV; // quantized UV

            float2 col = SAMPLE_TEXTURE2D(_GridComputeTex, sampler_GridComputeTex, input.texcoord).xy; // Compute Shader
            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);

            float3 hardMix = step(1 - blit.rgb, float4(0.9,0.99,0.99,1));
            blit.rgb = (ceil(blit.rgb * 20) / 20) * (hardMix * 2);

            float3 blitHSV = RGBToHSV(blit);
            float brightnessFactor = 1- blitHSV.z;
            float minThickness = 0.005;
            float gridThicknessThreshold = lerp(minThickness, _gridThickness, brightnessFactor);
            float grid = smoothstep(gridThicknessThreshold, gridThicknessThreshold + (1 / _ScreenParams.x), col.x); // Grid Mask
            float4 background = col.y * float4(0,0.002, 0.005,1);
            return max(blit * grid, background);
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
    }
}