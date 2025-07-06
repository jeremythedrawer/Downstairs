using System.Collections;
using UnityEngine;

public class PlayerMaterialController : RendererManager
{
    private readonly int colorID = Shader.PropertyToID("_color");
    private readonly int glowingID = Shader.PropertyToID("_glowing");

    public void PowerUpMaterial()
    {
        StartCoroutine(PoweringUpMaterial());
    }
    private IEnumerator PoweringUpMaterial()
    {
        float powerUpTime = 0.75f;
        float elapsedTime = 0f;

        while (elapsedTime < powerUpTime)
        {
            objectRenderer.GetPropertyBlock(mpb);
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / powerUpTime;
            float intensity = Mathf.Lerp(3, 1, t);
            Color newColor = Color.white * intensity;

            mpb.SetColor(colorID, newColor);
            objectRenderer.SetPropertyBlock(mpb);

            yield return null;
        }
        // Restore original color after pulse
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(colorID, Color.white);
        objectRenderer.SetPropertyBlock(mpb);
    }

    public void GlowingMaterial(bool turnOn)
    {
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(glowingID, turnOn ? 1 : 0);
        objectRenderer.SetPropertyBlock(mpb);
    }
}
