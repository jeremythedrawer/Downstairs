Shader "Unlit/s_ditherWorldGrid"
{
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

        float _gridScale;

        float4 ReconstructWorldPos(float2 uv)
        {
            float depth = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).r;
            float4 clipPos = float4(uv * 2 - 1, depth, 1);
            float4 worldPos = mul(_InvViewProjMatrix, clipPos);
            return worldPos / worldPos.w;
        }

        float distLine(float2 a, float2 b) // code version of my Distance Line sub graph
        {
            float2 distA = a - b;
            float distB = saturate(dot(a, distA) / dot(distA, distA));
    
            return length(a - distA * distB);
        }

        float4 calc(Varyings input) : SV_Target
        {

            float2 worldPos = ReconstructWorldPos(input.texcoord).xy;

            float2 scaledWorldPos = worldPos * _gridScale;
            float2 worldGV = frac(scaledWorldPos);
            worldGV -= 0.5;
            worldGV = abs(worldGV);
            
            float gridContour = distLine(worldGV.x, worldGV.y);


            float2 gridID = 1/(_gridScale * 2) * _gridScale + scaledWorldPos;
            gridID = round(gridID);
            gridID /= _gridScale;

            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, gridID);
            return blit;
        }

        float4 apply(Varyings input) : SV_Target
        {
            return SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, input.texcoord);
        }

    ENDHLSL

    SubShader {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off
        Cull Off

        Stencil {
            ref 1
            comp equal
        }

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