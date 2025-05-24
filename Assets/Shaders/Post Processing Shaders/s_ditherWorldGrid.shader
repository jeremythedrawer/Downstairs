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

        float sonarSDF(float2 uv, float2 screenPos, float minValue, float maxValue)
        {
            float2 aspScreenPos = aspectRatioPentile(screenPos);

            float2 p = (uv * aspScreenPos) - (_playerPos * aspScreenPos);
            float innerLength = saturate(1 - pow(length(p*2),1));

            p /= _sonarPingTime;
            float sonarPingLength = pow(length(p*0.25),4);
            float ping = saturate((1 - distance(sonarPingLength, 1 - sonarPingLength)) - _sonarPingTime);
            ping = saturate((ping * 2) + (innerLength));
            return lerp(minValue, maxValue, ping);
        }


        float4 SquareCoords(float2 uv)
        {
            float2 id = floor(uv);
            float2 local = abs(frac(uv));
            return float4(local, id);
        }

        float SquareDist(float2 uv)
        {
            float2 d = abs(uv) - 0.5;
            return max(d.x,d.y);
        }

        float DisplaceEffect(float4 squareCoord, float2 distortOrigin) //calculating how much the square should be diplaced by the sonar ripple
        {
            float2 diff = squareCoord.zw - distortOrigin;
            float dist = length(diff);

            float rippleRadius = _sonarPingTime * _gridScale * 12;

            float shellThickness = 10;

            float rippleBand = 1.0 - smoothstep(1, shellThickness, abs(dist - rippleRadius));

            return rippleBand;
        }

        float2 DisplaceSquare(float4 squareCoord, float2 distortOrigin)
        {
            float2 displacementDir = normalize(distortOrigin - squareCoord.zw);

            float t = lerp(0, 20, _sonarPingTime);
            float displacementAmount = t * DisplaceEffect(squareCoord, distortOrigin);
            return squareCoord.xy + displacementDir * displacementAmount;
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
            
            float2 disGrid = float2(0,0);


            float4 sc = SquareCoords(scaledTexCoord);
            float col = 0;

            float2 gridSpacePlayerPos = (_playerPos * screenPos) / _gridScale;

            int neighbourRange = 3;
            for(int x = -neighbourRange; x <= neighbourRange; x++)
            {
                for(int y = -neighbourRange; y<=neighbourRange; y++) // iterate through all neighbouring cells defined by neighbourRange
                {
                    float2 offset = float2(x,y);
                    float4 currSC = SquareCoords(scaledTexCoord - offset);

                    float2 displacedPos = DisplaceSquare(currSC, gridSpacePlayerPos) + offset;
                    float currDistFromSquare = max(-SquareDist(displacedPos), 0);

                    float t = _Time.y * 0.1;
                    float2 noiseUV = t + currSC.zw;
                    float gridNoise = SimpleNoise(noiseUV, 10);

                    col += currDistFromSquare * gridNoise;
                }
            }
           // return col.xxxx;
            float4 blit = SAMPLE_TEXTURE2D_X(_BlitTexture, point_clamp_sampler, gridTexCoord);
            blit *= 3;
            blit = saturate(pow(blit, 10));
            blit *= pow(sonarPing,1);
            float3 blitHSV = RGBToHSV(blit);
            float brightnessFactor = 1- saturate(blitHSV.z);
            float minThickness = 0.0005;
            float gridThicknessThreshold = lerp(minThickness, _gridThickness, brightnessFactor);
            float grid = smoothstep(gridThicknessThreshold, gridThicknessThreshold + (1 / _ScreenParams.x), saturate(col));

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