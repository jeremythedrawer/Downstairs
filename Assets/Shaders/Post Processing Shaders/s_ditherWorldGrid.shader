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
        float2 _gridWorldOffset;
        static const float GRID_STEPS[4] = { 1.0, 0.5, 0.25, 0.125 };

        float GetQuantizedGridScale(float brightness)
        {
            int index = clamp(int((1.0 - brightness) * 4.0), 0, 3);
            return GRID_STEPS[index];
        }




        SamplerState point_clamp_sampler;

        float4 calc(Varyings input) : SV_Target
        {
            float2 screenCenter = float2(0.5, 0.5);
            float2 uvToCenter = input.texcoord - screenCenter;
            float distToCenter = max(abs(uvToCenter.x), abs(uvToCenter.y));
            float normalizedDist = pow(saturate(distToCenter / 0.5),1.5);
            float quantizedGridSize = GetQuantizedGridScale(normalizedDist);
            //return quantizedGridSize.xxxx;
            float finalGridScale = _gridScale * quantizedGridSize;

            float2 pixelSize = 1.0 / _ScreenParams.xy;
            float2 pixelBlockSize = pixelSize * finalGridScale;
            float2 snappedUV = round(input.texcoord / pixelBlockSize) * pixelBlockSize;

            pixelBlockSize = pixelSize * finalGridScale;
            snappedUV = round(input.texcoord / pixelBlockSize) * pixelBlockSize;

            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, snappedUV);


            float2 gridOffsetUV =  (_gridWorldOffset / _ScreenParams.xy);
            float2 scaledAspectRatioUV = float2(_ScreenParams.x, _ScreenParams.y) / finalGridScale;


            float2 gridOrigin = screenCenter * scaledAspectRatioUV;
            float2 scaledTexCoord = ((input.texcoord + gridOffsetUV) * scaledAspectRatioUV) - gridOrigin;
            float2 roundTexCoord = round(scaledTexCoord);
            float2 gridTexCoord = (roundTexCoord + gridOrigin) / scaledAspectRatioUV;
            blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);

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