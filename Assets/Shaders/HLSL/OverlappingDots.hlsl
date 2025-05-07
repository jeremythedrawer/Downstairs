void OverlappingDots_float(float2 uv, float dotCount, float time, float totalScale, out float output)
{
    output = 0;
    float centerX = 0.5;
    float baseY = 0.0; // start from bottom edge

    float dotHeights[64]; // assumes max 64 dots
    float totalStackHeight = 0.0;

    // Precompute dot scales and heights
    for (int i = 0; i < dotCount; i++)
    {
        float scale = 1.0 - (i / dotCount);
        dotHeights[i] = scale * totalScale * 0.1; // adjust 0.1 if needed
        totalStackHeight += dotHeights[i];
    }


    float packedHeight = totalStackHeight;

    float accumulatedStack = 0.0;
    for (int i = 0; i < dotCount; i++)
    {
        float scale = 1.0 - (i / dotCount);
        float2 scaledUV = ((uv - float2(0.5, 0.5)) * (scale * 0.5) / totalScale) + float2(0.5, 0.0);

        // --- time = 1: normalized compressed stacked Y ---
        accumulatedStack += dotHeights[i];
        float normalizedY = baseY + (accumulatedStack / packedHeight) * (1.0 - baseY);

        // --- blend between collapsed and packed ---
        float currentY = lerp(baseY, normalizedY, time);

        float2 dotPos = float2(centerX, currentY);
        float dist = distance(scaledUV, dotPos);

        output += smoothstep(0.1, 0.0, dist);
    }
}
