using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : ImageController
{
    public static MenuCanvas instance {  get; private set; }
    public List<MenuButton> menuButtons;
    public List<Image> images;
    public Image backdrop;
    public bool stopTime;
    public float transitionTime = 3;
    private void OnEnable()
    {
        ShowUI(transitionTime, images, backdrop, StartSelection);
        instance = this;
    }

    private void Update()
    {
        MenuCanvasController.MenuInputs(menuButtons);
    }

    private void StartSelection()
    {
        MenuCanvasController.UpdateSelection(menuButtons);
        StopTime(stopTime);
    }

    public void HideMenuUI()
    {
        HideUI(transitionTime, images, backdrop);
    }
    private void StopTime(bool stopTime)
    {
        Time.timeScale = stopTime ? 0 : 1;
    }
}
