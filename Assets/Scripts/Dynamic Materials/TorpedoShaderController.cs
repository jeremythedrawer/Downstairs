using System.Collections;
using UnityEngine;

public class TorpedoShaderController : RendererManager
{
    public static float time;
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
