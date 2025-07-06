using System.Collections;
using UnityEngine;

public class SolitaryFishMaterialController : RendererManager
{
    private readonly int glowingID = Shader.PropertyToID("_glowing");
    private readonly int flashIntensityID = Shader.PropertyToID("_flashIntensity");
    public void GlowingMaterial(bool turnOn)
    {
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(glowingID, turnOn ? 1 : 0);
        objectRenderer.SetPropertyBlock(mpb);
    }

    public void FlashMaterial()
    {
        StartCoroutine(FlashingMaterial());
    }
    private IEnumerator FlashingMaterial()
    {
        float flashTime = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            objectRenderer.GetPropertyBlock(mpb);
            elapsedTime += Time.deltaTime;
            float t = 1 - (elapsedTime / flashTime);
            mpb.SetFloat(flashIntensityID, t);
            objectRenderer.SetPropertyBlock(mpb);

            yield return null;
        }
        // Restore original color after pulse
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(flashIntensityID, 0);
        objectRenderer.SetPropertyBlock(mpb);
    }
}
