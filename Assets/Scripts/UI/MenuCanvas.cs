using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : ImageController
{
    public static MenuCanvas instance {  get; private set; }
    public List<MenuButton> menuButtons;
    public bool stopTime;
    public float transitionTime = 3;
    private void OnEnable()
    {
        ShowUI(transitionTime, StartSelection);
        instance = this;
    }

    private void Update()
    {
        if(!canExit) return;
        CanvasController.MenuInputs(menuButtons);
    }

    private void StartSelection()
    {
        CanvasController.UpdateSelection(menuButtons);
        StopTime(stopTime);
    }

    private void StopTime(bool stopTime)
    {
        Time.timeScale = stopTime ? 0 : 1;
    }
}
