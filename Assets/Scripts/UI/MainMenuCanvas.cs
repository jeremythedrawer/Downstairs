using UnityEngine;

public class MainMenuCanvas : MenuCanvas
{
    private void Start()
    {
        ShowMenuUI();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartSelection();

    }
}
