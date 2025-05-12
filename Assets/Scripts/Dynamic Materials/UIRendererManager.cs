using UnityEngine;
using UnityEngine.UI;

public abstract class UIRendererManager : MonoBehaviour
{
    [SerializeField] protected Graphic graphic;
    protected Material material;

    public virtual void Start()
    {
        if (graphic != null)
        {
            material = graphic.material;
            material = Instantiate(material);
            graphic.material = material;
        }
    }
}

