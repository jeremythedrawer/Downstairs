using UnityEngine;

public class FishShaderController : RendererManager
{
    public float speed { get; set; }
    private readonly int speedID = Shader.PropertyToID("_speed");

    private void Update()
    {
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(speedID, speed);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
