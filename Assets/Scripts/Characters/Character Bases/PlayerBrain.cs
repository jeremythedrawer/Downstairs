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

    public Vector2 currentPos => CameraController.mainCam.WorldToViewportPoint(transform.position);
    public Vector2 currentDir => transform.up;

    [SerializeField] private LayerMask enemyLayer;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        MoveInputs();
        Shoot();
        healthManager.LooseHealth(boxCollider.bounds, enemyLayer);
    }
    private void MoveInputs()
    {
        movementController.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementController.shootInput = Input.GetMouseButton(0);
    }

    public void Shoot()
    {
        currentTorpCoolDownTime -= Time.deltaTime;
        if (movementController.shootInput && currentTorpCoolDownTime <= 0f)
        {
            foreach (TorpedoSpawner torpedoSpawner in torpedoSpawners)
            {
                torpedoSpawner.FireTorpedo();
                currentTorpCoolDownTime = torpedoCoolDownTime;
            }
        }
    }
}
