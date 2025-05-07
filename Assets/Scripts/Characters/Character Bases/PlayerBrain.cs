using System.Collections;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static Vector2 currentPos;

    private void Update()
    {
        MoveInputs();
        currentPos = CameraController.mainCam.WorldToViewportPoint(transform.position);

        if (movementController.shootInput && TorpedoShaderController.time == 0f)
        {
            Shoot();
        }
    }

    private void MoveInputs()
    {
        movementController.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementController.shootInput = Input.GetKey(KeyCode.Space);
    }

    public void Shoot()
    {
        StartCoroutine(Shooting());
    }

    private IEnumerator Shooting()
    {
        while (TorpedoShaderController.time < 1f)
        {
            TorpedoShaderController.time += Time.deltaTime;
            yield return null;
        }
        TorpedoShaderController.time = 0f;
    }

}
