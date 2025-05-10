using System.Collections;
using UnityEngine;

public class TorpedoShaderController : RendererManager
{
    public float time { get; set; }
    private readonly int timeID = Shader.PropertyToID("_time");

    private void Update()
    {
        UpdateMaterial();
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(timeID, time);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
