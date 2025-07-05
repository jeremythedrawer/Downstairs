using UnityEngine;

public class InstructionCanvasController : ImageController
{
    private void Update()
    {
        TurnOffInstructionCanvas();
    }

    private void TurnOffInstructionCanvas()
    {
        if (PlayerBrain.instance != null && PlayerBrain.instance.uncoverInput)
        {
            HideUI(UIManager.instance.TurnOffInstructionCanvasController);
        }
    }
}
