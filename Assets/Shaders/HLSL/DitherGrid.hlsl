float distLine(float2 a, float2 b) // code version of my Distance Line sub graph
{
    float2 distA = a - b;
    float distB = saturate(dot(a, distA) / dot(distA, distA));
    
    return length(a - distA * distB);

}

inline float unity_noise_randomValue(float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
}

inline float unity_noise_interpolate(float a, float b, float t)
{
    return (1.0 - t) * a + (t * b);
}

inline float unity_valueNoise(float2 uv)
{
    float2 i = floor(uv);
    float2 f = frac(uv);
    f = f * f * (3.0 - 2.0 * f);

    uv = abs(frac(uv) - 0.5);
    float2 c0 = i + float2(0.0, 0.0);
    float2 c1 = i + float2(1.0, 0.0);
    float2 c2 = i + float2(0.0, 1.0);
    float2 c3 = i + float2(1.0, 1.0);
    float r0 = unity_noise_randomValue(c0);
    float r1 = unity_noise_randomValue(c1);
    float r2 = unity_noise_randomValue(c2);
    float r3 = unity_noise_randomValue(c3);

    float bottomOfGrid = unity_noise_interpolate(r0, r1, f.x);
    float topOfGrid = unity_noise_interpolate(r2, r3, f.x);
    float t = unity_noise_interpolate(bottomOfGrid, topOfGrid, f.y);
    return t;
}

float unity_SimpleNoise(float2 UV, float Scale)
{
    float t = 0.0;

    float freq = pow(2.0, float(0));
    float amp = pow(0.5, float(3 - 0));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    freq = pow(2.0, float(1));
    amp = pow(0.5, float(3 - 1));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    freq = pow(2.0, float(2));
    amp = pow(0.5, float(3 - 2));
    t += unity_valueNoise(float2(UV.x * Scale / freq, UV.y * Scale / freq)) * amp;

    return t;
}

float3 worldGrid(in float2 worldPos, in float scale)
{
    float2 p = worldPos * scale;
    float2 id = floor(1 / (scale * 2) + p);
    
    float2 gridUv = abs(frac(p) - 0.5);
    float gridContour = distLine(gridUv.x, gridUv.y);
    
    return float3(id, gridContour);
}
float4 ditherGrid(in float SDFShape, in float2 worldPos, in float scale, in float t)
{
    float3 worldGrid = worldGrid(worldPos, scale);
    
    float2 gridId = worldGrid.xy;
    float gridContour = worldGrid.z;
    
    t = sin(t) * 0.1;
    gridId += t;
    float noiseId = unity_SimpleNoise(gridId, 10);
    
}