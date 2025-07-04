using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public List<Image> images;
    public PowerUp.PowerUpType powerUpType;
    private float time = 2;
    private void OnEnable()
    {
        ShowUI();
    }

    private Coroutine ShowUI()
    {
        return StartCoroutine(ShowingUI());
    }

    public Coroutine HideUI()
    {
        return StartCoroutine(HidingUI());
    }
    private IEnumerator ShowingUI()
    {
        foreach (Image image in images)
        {
            image.color = Color.clear;
        }
        float elapsedTime = 0;
        yield return new WaitForSeconds(1f);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / time, 2);
            float gridScale = Mathf.Lerp(GlobalVolumeController.instance.inGameGridScale, GlobalVolumeController.instance.menuGridScale, t);
            float centrelightSize = Mathf.Lerp(GlobalVolumeController.instance.inGameCenterLightSize, GlobalVolumeController.instance.menuCentreLightSize, t);

            GlobalVolumeController.instance.ditherWorldGridVolume.gridScale.value = gridScale;
            GlobalVolumeController.instance.ditherWorldGridVolume.centreLightSize.value = centrelightSize;
            Color color = new Color(1, 1, 1, t);

            foreach(Image image in images)
            {
                image.color = color;
            }
            yield return null;
        }
        foreach (Image image in images)
        {
            image.color = Color.white;
        }
    }

    private IEnumerator HidingUI()
    {
        float elapsedTime = time;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / time, 2);
            float gridScale = Mathf.Lerp(GlobalVolumeController.instance.inGameGridScale, GlobalVolumeController.instance.menuGridScale, t);
            float centrelightSize = Mathf.Lerp(GlobalVolumeController.instance.inGameCenterLightSize, GlobalVolumeController.instance.menuCentreLightSize, t);

            GlobalVolumeController.instance.ditherWorldGridVolume.gridScale.value = gridScale;
            GlobalVolumeController.instance.ditherWorldGridVolume.centreLightSize.value = centrelightSize;
            Color color = new Color(1, 1, 1, t);
            foreach (Image image in images)
            {
                image.color = color;
            }
            yield return null;
        }

        foreach (Image image in images)
        {
            image.color = Color.white;
        }
        gameObject.SetActive(false);
        UIManager.instance.TurnOffPowerUpCanvasController();
    }
}
