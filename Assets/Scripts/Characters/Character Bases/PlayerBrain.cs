using System.Collections;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static PlayerBrain Instance { get; private set; }

    public Vector2 currentPos => CameraController.mainCam.WorldToViewportPoint(transform.position);
    public Vector2 currentDir => transform.up;

    [SerializeField] private LayerMask hitLayer;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        MoveInputs();
        UseSonarPing();
        healthManager.LooseHealth(sphereCollider.bounds, hitLayer);
    }
    private void MoveInputs()
    {
        movementController.moveInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        movementController.grabInput = Input.GetKeyDown(KeyCode.Space);
        movementController.sonarPingInput = Input.GetKeyDown(KeyCode.LeftShift);
        movementController.rotationInput = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;
    }

    private void UseSonarPing()
    {
        if (movementController.sonarPingInput && characterStats.canSonarPing)
        {
            lightController.SonarPing();
        }
    }

}
