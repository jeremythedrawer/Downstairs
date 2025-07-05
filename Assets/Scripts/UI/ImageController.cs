using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    public List<Image> images;
    public Image backdrop;
    protected float time = 3;

    public delegate void HideUIDelegate();
    protected virtual void OnEnable()
    {
        ShowUI();
    }
    protected Coroutine ShowUI()
    {
        return StartCoroutine(ShowingUI());
    }

    public Coroutine HideUI(HideUIDelegate onHideUI = null)
    {
        return StartCoroutine(HidingUI(onHideUI));
    }

    protected IEnumerator ShowingUI()
    {
        yield return new WaitUntil(() => PlayerBrain.instance != null);
        PlayerBrain.instance.movementController.canMove = false;
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

            Color backdropColor = new Color(0, 0, 0, t * 0.99f);
            Color color = new Color(1, 1, 1, t);

            foreach (Image image in images)
            {
                image.color = color;
            }
            backdrop.color = backdropColor;
            yield return null;
        }
        foreach (Image image in images)
        {
            image.color = Color.white;
        }
    }

    protected IEnumerator HidingUI(HideUIDelegate onHideUI = null)
    {
        PlayerBrain.instance.movementController.canMove = true;
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
            Color backdropColor = new Color(0, 0, 0, t);
            foreach (Image image in images)
            {
                image.color = color;
            }
            backdrop.color = backdropColor;
            yield return null;
        }

        foreach (Image image in images)
        {
            image.color = Color.clear;
        }
        backdrop.color = Color.clear;
        gameObject.SetActive(false);
        onHideUI?.Invoke();
    }
}
