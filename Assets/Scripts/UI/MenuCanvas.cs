using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuCanvas : ImageController
{
    public static MenuCanvas instance {  get; private set; }
    public List<MenuButton> menuButtons;
    public List<Image> images;
    public Image backdrop;
    public bool stopTime;
    public float transitionTime = 3;

    protected virtual void OnEnable()
    {
        instance = this;
    }
    private void Update()
    {
        MenuCanvasController.MenuInputs(menuButtons);
    }

    protected void StartSelection()
    {
        MenuCanvasController.UpdateSelection(menuButtons);
        StopTime(stopTime);
    }

    public void HideMenuUI()
    {
        HideUI(transitionTime, images, backdrop, () => gameObject.SetActive(false));
    }
    private void StopTime(bool stopTime)
    {
        Time.timeScale = stopTime ? 0 : 1;
    }

    protected void ShowMenuUI()
    {
        ShowUI(transitionTime, images, backdrop, StartSelection);
    }
}
