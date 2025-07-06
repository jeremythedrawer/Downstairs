using System.Collections;
using System.Linq;
using UnityEngine;

public class UnlockableIconMatCont : GraphicMaterialController
{
    public enum IconType
    {
        PelicanEel,
        Anglerfish,
        GiantSquid,
        Flare,
        SonarPing,
        RadialScan
    }
    public IconType type;

    public Color color = Color.grey;
    private readonly int colorId = Shader.PropertyToID("_color");

    private bool active;
    private void OnEnable()
    {
        UpdateGraphic();
    }
    private void CheckActivated()
    {
        switch(type)
        {
            case IconType.PelicanEel:
            {
               active = !StatsManager.instance.solitaryFishList.Any(fish => fish is PelicanEel);
            }
            break;
            case IconType.Anglerfish:
            {
                active = !StatsManager.instance.solitaryFishList.Any(fish => fish is Anglerfish);
            }
            break;
            case IconType.GiantSquid:
            {
                active = !StatsManager.instance.solitaryFishList.Any(fish => fish is GiantSquid);
            }
            break;
            case IconType.Flare:
            {
                active = PlayerBrain.instance.canFlare;
            }
            break;
            case IconType.SonarPing:
            {
                active = PlayerBrain.instance.canSonarPing;
            }
            break;
            case IconType.RadialScan:
            {
                active = PlayerBrain.instance.canRadialScan;
            }
            break;
        }
    }
    public void UpdateColor()
    {
        if (active)
        {
            material.SetColor(colorId, color);
        }
        else
        {
            material.SetColor(colorId, Color.grey);
        }
    }

    private void UpdateGraphic()
    {
        StartCoroutine(UpdatingGraphic());
    }
    private IEnumerator UpdatingGraphic()
    {
        InstantiateMat();
        yield return new WaitUntil(() => material != null);
        CheckActivated();
        UpdateColor();
    }

}
