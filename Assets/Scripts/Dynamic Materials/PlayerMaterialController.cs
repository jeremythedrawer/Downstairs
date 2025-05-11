using UnityEngine;

public class PlayerMaterialController : RendererManager
{
    public float hit { get; set; }
    private readonly int hitID = Shader.PropertyToID("_hit");

    private void Update()
    {
        UpdateMaterial();
    }

    public override void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(hitID, hit);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
