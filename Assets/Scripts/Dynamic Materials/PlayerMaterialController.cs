using UnityEngine;

public class PlayerMaterialController : RendererManager
{
    private Material[] materials;
    private Color[] originalColors;
    private Color[] currentColors;

    private readonly int colorID = Shader.PropertyToID("_color");

    private void Start()
    {
        materials = objectRenderer.materials;  // material instances
        originalColors = new Color[materials.Length];
        currentColors = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].HasProperty(colorID))
            {
                originalColors[i] = materials[i].GetColor(colorID);
                currentColors[i] = originalColors[i];
            }
            else
            {
                originalColors[i] = Color.white;
                currentColors[i] = Color.white;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor(colorID, currentColors[i]);
        }
    }

    // Allow external scripts to set color on a specific material index
    public void SetColor(int index, Color color)
    {
        if (index >= 0 && index < currentColors.Length)
            currentColors[index] = color;
    }

    public Color GetOriginalColor(int index)
    {
        if (index >= 0 && index < originalColors.Length)
            return originalColors[index];
        return Color.white;
    }

    public int GetMaterialCount()
    {
        return materials.Length;
    }
}
