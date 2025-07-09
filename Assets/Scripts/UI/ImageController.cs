using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ImageController : MonoBehaviour
{
    public delegate void HideUIDelegate();
    public delegate void ShowUIDelegate();
    public void ShowUI(float time, List<Image> images, Image backdrop, ShowUIDelegate onShowUI = null)
    {
        StartCoroutine(ShowingUI(time, images, backdrop, onShowUI));
    }

    public void HideUI(float time, List<Image> images, Image backdrop, HideUIDelegate onHideUI = null)
    {
        StartCoroutine(HidingUI(time, images, backdrop, onHideUI));
    }

    protected IEnumerator ShowingUI(float time, List<Image> images, Image backdrop, ShowUIDelegate onShowUI = null)
    {
        yield return new WaitUntil(() => GlobalVolumeController.instance != null);
        foreach (Image image in images)
        {
            image.color = Color.clear;
        }
        float elapsedTime = 0;
        GlobalVolumeController.instance.ditherWorldGridVolume.gridScale.value = GlobalVolumeController.instance.menuGridScale;

        while (elapsedTime < time)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = Mathf.Pow(elapsedTime / time, 2);
            float centrelightSize = Mathf.Lerp(GlobalVolumeController.instance.inGameCenterLightSize, GlobalVolumeController.instance.menuCentreLightSize, t);

            GlobalVolumeController.instance.ditherWorldGridVolume.centreLightSize.value = centrelightSize;

            Color backdropColor = new Color(0, 0, 0, t * 0.99f);
            Color color = new Color(1, 1, 1, t);

            foreach (Image image in images)
            {
                image.color = color;
            }
            if (backdrop != null) backdrop.color = backdropColor;
            yield return null;
        }
        foreach (Image image in images)
        {
            image.color = Color.white;
        }

        onShowUI?.Invoke();
    }

    protected IEnumerator HidingUI(float time, List<Image> images, Image backdrop, HideUIDelegate onHideUI = null)
    {
        Time.timeScale = 1;
        PlayerBrain.instance.movementController.canMove = true;
        float elapsedTime = time;
        GlobalVolumeController.instance.ditherWorldGridVolume.gridScale.value = GlobalVolumeController.instance.inGameGridScale;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.unscaledDeltaTime;
            float t = Mathf.Pow(elapsedTime / time, 2);
            float centrelightSize = Mathf.Lerp(GlobalVolumeController.instance.inGameCenterLightSize, GlobalVolumeController.instance.menuCentreLightSize, t);

            GlobalVolumeController.instance.ditherWorldGridVolume.centreLightSize.value = centrelightSize;
            Color color = new Color(1, 1, 1, t);
            Color backdropColor = new Color(0, 0, 0, t);
            foreach (Image image in images)
            {
                image.color = color;
            }
            if (backdrop != null) backdrop.color = backdropColor;
            yield return null;
        }

        foreach (Image image in images)
        {
            image.color = Color.clear;
        }
        if (backdrop != null) backdrop.color = Color.clear;
        onHideUI?.Invoke();
    }
}
