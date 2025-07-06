using UnityEngine;

public class InstructionCanvas : ImageController
{
    private void OnEnable()
    {
        ShowUI(time: 3);
    }
    private void Update()
    {
        TurnOffInstructionCanvas();
    }

    private void TurnOffInstructionCanvas()
    {
        if (PlayerBrain.instance != null && PlayerBrain.instance.uncoverInput && canExit)
        {
            HideUI(time: 3, CanvasController.instance.TurnOffInstructionCanvas);
        }
    }
}
