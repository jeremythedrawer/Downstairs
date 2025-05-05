using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static Vector2 currentPos;

    private void Update()
    {
        MoveInputs();
        currentPos = CameraController.mainCam.WorldToViewportPoint(transform.position);
    }

    private void MoveInputs()
    {
        movementController.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
