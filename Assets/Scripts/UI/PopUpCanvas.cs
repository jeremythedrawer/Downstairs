using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpCanvas : ImageController
{
    public static PopUpCanvas instance {  get; private set; }
    public enum PopUpType
    { 
        Start,
        FirstFish,
        Flare,
        SonarPing,
        RadialScan
    }

    [Serializable]
    public class PopUpUI
    { 
        public PopUpType type;
        public List<Image> images;
        public Image backdrop;
    }

    public List<PopUpUI> popUpUIList;

    private Dictionary<PopUpType, PopUpUI> popUpUIDic;

    private bool firstOnPlay = true;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        popUpUIDic = new Dictionary<PopUpType, PopUpUI>();
        foreach (PopUpUI popUpUI in popUpUIList)
        {
            if (!popUpUIDic.ContainsKey(popUpUI.type))
            {
                popUpUIDic.Add(popUpUI.type, popUpUI);
            }
        }
    }

    private void Start()
    {
        foreach(PopUpUI popUpUI in popUpUIList)
        {
            foreach(Image image in popUpUI.images)
            {
                image.color = Color.clear;
            }
        }
    }
    private void OnEnable()
    {
        MenuButton.onPlay += ShowStartImages;
    }

    private void OnDisable()
    {
        MenuButton.onPlay -= ShowStartImages;
    }
    public void ShowStartImages()
    {
        if (firstOnPlay)
        {
            if (popUpUIDic.TryGetValue(PopUpType.Start, out PopUpUI startPopUpUI))
            {
                ShowPopUpImages(startPopUpUI, 3f, () => PlayerBrain.instance.movementController.canMove = false);
            }
        }
    }

    public void HideStartImages()
    {
        if (firstOnPlay)
        {
            if (popUpUIDic.TryGetValue(PopUpType.Start, out PopUpUI startPopUpUI))
            {
                HidePopUpImages(startPopUpUI, 1f);
                firstOnPlay = false;
            }
        }
    }
    public void ShowFirstFish()
    {
        if (popUpUIDic.TryGetValue(PopUpType.FirstFish, out PopUpUI firstFishPopUpUI))
        {
            if (firstFishPopUpUI.images[0].color != Color.clear) return;
            ShowPopUpImages(firstFishPopUpUI, 1f);
        }
    }

    public void HideFirstFish()
    {
        if (popUpUIDic.TryGetValue(PopUpType.FirstFish, out PopUpUI firstFishPopUpUI))
        {
            if (firstFishPopUpUI.images[0].color != Color.white) return;
            HidePopUpImages(firstFishPopUpUI, 1f);
        }
    }

    public void ShowFlare()
    {
        if (popUpUIDic.TryGetValue(PopUpType.Flare, out PopUpUI flarePopUpUI))
        {
            ShowPopUpImages(flarePopUpUI, 3f, () => PlayerBrain.instance.movementController.canMove = false);
        }
    }

    public void HideFlare()
    {
        if (popUpUIDic.TryGetValue(PopUpType.Flare, out PopUpUI flarePopUpUI))
        {
            HidePopUpImages(flarePopUpUI, time: 1f);
        }
    }

    public void ShowSonarPing()
    {
        if (popUpUIDic.TryGetValue(PopUpType.SonarPing, out PopUpUI sonarPingUI))
        {
            ShowPopUpImages(sonarPingUI, time: 3f, () => PlayerBrain.instance.movementController.canMove = false);
        }
    }

    public void HideSonarPing()
    {
        if (popUpUIDic.TryGetValue(PopUpType.SonarPing, out PopUpUI sonarPingUI))
        {
            HidePopUpImages(sonarPingUI, time: 1f);
        }
    }

    public void ShowRadialScan()
    {
        if (popUpUIDic.TryGetValue(PopUpType.RadialScan, out PopUpUI radialScanUI))
        {
            ShowPopUpImages(radialScanUI, 3f, () => PlayerBrain.instance.movementController.canMove = false);
        }
    }

    public void HideRadialScan()
    {
        if (popUpUIDic.TryGetValue(PopUpType.RadialScan, out PopUpUI radialScanUI))
        {
            HidePopUpImages(radialScanUI, 1f);
        }
    }
    private IEnumerator WaitToHide(PopUpUI popUpUI, float time, HideUIDelegate onHideUI = null)
    {
        yield return new WaitUntil(() => popUpUI.images[0].color == Color.white);
        HideUI(time, popUpUI.images, popUpUI.backdrop, onHideUI);
    }

    private IEnumerator WaitToShow(PopUpUI popUpUI, float time, ShowUIDelegate onShowUI = null)
    {
        yield return new WaitUntil(() => popUpUI.images[0].color == Color.clear);
        ShowUI(time, popUpUI.images, popUpUI.backdrop, onShowUI);
    }

    private void ShowPopUpImages(PopUpUI popUpUI, float time, ShowUIDelegate onShowUI = null)
    {
        StartCoroutine(WaitToShow(popUpUI, time, onShowUI));
    }

    private void HidePopUpImages(PopUpUI popUpUI, float time, HideUIDelegate onHideUI = null)
    {
        StartCoroutine(WaitToHide(popUpUI, time, onHideUI));
    }


}
