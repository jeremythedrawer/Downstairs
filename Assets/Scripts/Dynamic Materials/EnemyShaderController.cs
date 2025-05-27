using UnityEngine;

public class EnemyShaderController : RendererManager
{
    public bool hit { get; set; }
    private readonly int hitID = Shader.PropertyToID("_hit");

    private void Update()
    {
        UpdateMaterial();
    }

    public virtual void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(hitID, hit ? 1 : 0);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
