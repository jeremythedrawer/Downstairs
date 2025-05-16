using UnityEngine;

public class SmallFishShaderController : RendererManager
{
    public float speed { get; set; }
    private readonly int speedID = Shader.PropertyToID("_speed");

    private void Update()
    {
        UpdateMaterial();
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(speedID, speed);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
