using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static PlayerBrain Instance { get; private set; }

    public List<TorpedoSpawner> torpedoSpawners;
    [SerializeField] private float torpedoCoolDownTime = 0.1f;
    private float currentTorpCoolDownTime = 0;

    public static Vector2 currentPos;
    public static Vector2 currentDir;


    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        MoveInputs();
        currentPos = CameraController.mainCam.WorldToViewportPoint(transform.position);
        currentDir = transform.up;

        currentTorpCoolDownTime -= Time.deltaTime;

        if (movementController.shootInput && currentTorpCoolDownTime <= 0f)
        {
            Shoot();
            currentTorpCoolDownTime = torpedoCoolDownTime;
        }
    }

    private void MoveInputs()
    {
        movementController.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementController.shootInput = Input.GetKey(KeyCode.Space);
    }

    public void Shoot()
    {
        foreach (TorpedoSpawner torpedoSpawner in torpedoSpawners)
        {
            torpedoSpawner.FireTorpedo();
        }
    }
}
