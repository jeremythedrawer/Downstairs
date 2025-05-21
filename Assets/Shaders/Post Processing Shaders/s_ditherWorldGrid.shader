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

        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 scaledAspectRatioUV = float2(_ScreenParams.x, _ScreenParams.y) / _gridScale;
            float2 scaledTexCoord = input.texcoord * scaledAspectRatioUV;
            float2 roundTexCoord = round(scaledTexCoord);

            float2 gridTexCoord = roundTexCoord / scaledAspectRatioUV;
            
            float2 gv = abs(frac(scaledTexCoord) - 0.5);
            float gridSDF = DistLine(gv.x, gv.y);

            float t = _Time.y * 0.1;
            float2 noiseUV = t + roundTexCoord;
            float gridNoise = SimpleNoise(noiseUV, 10);
            gridSDF *= gridNoise;

            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);

            float3 blitHSV = RGBToHSV(blit);
            float gridThicknessThreshold = max(_gridThickness * pow(1 - blitHSV.z, _gridFallOff), 0);

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