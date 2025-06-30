using UnityEngine;

public class GridShaderController : RendererManager
{
    public Vector2 playerPos => PlayerBrain.instance.currentPos;
    private readonly int playerPosID = Shader.PropertyToID("_playerPos");

    private void Update()
    {
        UpdateMaterial();
    }

    public void UpdateMaterial()
    {
        if (material != null)
        {
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetVector(playerPosID, playerPos);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }
}
