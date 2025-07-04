using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasController : MonoBehaviour
{
    public Canvas canvas;
    public List<MenuButton> menuButtons;

    private void OnEnable()
    {
        MenuController.UpdateSelection(menuButtons);
    }

    private void Update()
    {
        MenuController.MenuInputs(menuButtons);
    }
}
