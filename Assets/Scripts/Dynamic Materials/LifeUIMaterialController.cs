using UnityEngine;

public class LifeUIMaterialController : UIRendererManager
{
    private static float currentLife => PlayerBrain.Instance.healthManager.currentHealth * 0.01f;
    private readonly int currentLifeID = Shader.PropertyToID("_currentLife");
    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        if (material != null)
        {
            material.SetFloat(currentLifeID, currentLife);
        }
    }
}
