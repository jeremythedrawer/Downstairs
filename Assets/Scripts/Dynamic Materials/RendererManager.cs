using UnityEngine;

public abstract class RendererManager : MonoBehaviour
{
    protected Renderer objectRenderer;
    protected Material material;
    protected MaterialPropertyBlock mpb;

    public virtual void OnEnable()
    {
        objectRenderer = GetComponent<Renderer>();
        material = objectRenderer.material;
        mpb = new MaterialPropertyBlock();
    }

   // public abstract void UpdateMaterial();
}
