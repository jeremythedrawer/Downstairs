using UnityEngine;

public class SchoolFishMaterialController : RendererManager
{
    public float speed { get; set; }
    private readonly int speedID = Shader.PropertyToID("_speed");

    public void SetNewSpeed(float newSpeed)
    {
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(speedID, newSpeed);
        objectRenderer.SetPropertyBlock(mpb);
    }
}
