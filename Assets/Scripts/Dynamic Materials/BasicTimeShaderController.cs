using UnityEngine;

public class BasicTimeShaderController : RendererManager
{
    public float time { get; set; }
    private readonly int timeID = Shader.PropertyToID("_time");

    private void Update()
    {
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(timeID, time);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
