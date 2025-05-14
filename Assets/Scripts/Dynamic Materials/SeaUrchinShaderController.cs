using UnityEngine;

public class SeaUrchinShaderController : EnemyShaderController
{
    public float time { get; set; }
    private readonly int timeID = Shader.PropertyToID("_time");

    private void Update()
    {
        UpdateMaterial();
    }

    public override void UpdateMaterial()
    {
        base.UpdateMaterial();
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat(timeID, time);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
