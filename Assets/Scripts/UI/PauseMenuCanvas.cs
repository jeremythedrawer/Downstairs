using UnityEngine;

public class PauseMenuCanvas : MainMenuCanvas
{
    protected override void OnEnable()
    {
        base.OnEnable();
        ShowMenuUI();
    }
}
