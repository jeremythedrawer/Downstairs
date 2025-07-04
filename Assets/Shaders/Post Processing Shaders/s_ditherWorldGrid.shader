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
        float _gridFallOff;

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

            float3 blitHSV = RGBToHSV(blit);

            float brightnessFactor = max(blitHSV.z, 0.1);
            float gridSDF = pow(brightnessFactor, _gridFallOff) * col.x;
            
          // return gridSDF;
            float grid = step(0.001, gridSDF); // Grid Mask
           // return grid;
            return max(blit * grid, col.y * grid * float4(0, 0.005, 0.005, 1));
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