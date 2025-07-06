using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ImageController : MonoBehaviour
{
    public List<Image> images;
    public Image backdrop;

    private bool canExit;

    public delegate void HideUIDelegate();
    public delegate void ShowUIDelegate();
    protected void ShowUI(float time, ShowUIDelegate onShowUI = null)
    {
        StartCoroutine(ShowingUI(time, onShowUI));
    }

    public void HideUI(float time, HideUIDelegate onHideUI = null)
    {
        if (!canExit) return;
        StartCoroutine(HidingUI(time, onHideUI));
    }

    protected IEnumerator ShowingUI(float time, ShowUIDelegate onShowUI = null)
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
            elapsedTime += Time.deltaTime;
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

        if (SceneManager.GetActiveScene().buildIndex > 0) //skip if in menu scene
        {
            yield return new WaitUntil(() => PlayerBrain.instance != null);
            PlayerBrain.instance.movementController.canMove = false;
        }

        onShowUI?.Invoke();
        canExit = true;
    }

    protected IEnumerator HidingUI(float time, HideUIDelegate onHideUI = null)
    {
        PlayerBrain.instance.movementController.canMove = true;
        float elapsedTime = time;
        GlobalVolumeController.instance.ditherWorldGridVolume.gridScale.value = GlobalVolumeController.instance.inGameGridScale;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
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
        canExit = false;
        gameObject.SetActive(false);
    }
}
