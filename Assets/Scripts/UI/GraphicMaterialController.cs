using UnityEngine;
using UnityEngine.UI;

public abstract class GraphicMaterialController : MonoBehaviour
{
    [SerializeField] protected Graphic graphic;
    protected Material material;

    protected void InstantiateMat()
    {
        if (graphic != null)
        {
            material = graphic.material;
            material = Instantiate(material);
            graphic.material = material;
        }
    }
}
