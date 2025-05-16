using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static PlayerBrain Instance { get; private set; }

    public TorpedoSpawner torpedoSpawner;
    [SerializeField] private float torpedoCoolDownTime = 5f;
    [SerializeField] private float burstCoolDownTime = 2f;
    private float currentTorpCoolDownTime;
    private float currentBurstCoolDownTime;

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
        Burst();
        healthManager.LooseHealth(capsuleCollider.bounds, enemyLayer);
    }
    private void MoveInputs()
    {
        movementController.moveInput = Input.GetKey(KeyCode.W) ? 1 : 0;
        movementController.shootInput = Input.GetMouseButtonDown(0);
        movementController.burstInput = Input.GetKeyDown(KeyCode.LeftShift);
        movementController.rotationInput = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;
    }

    private void Shoot()
    {
        currentTorpCoolDownTime -= Time.deltaTime;
        if (movementController.shootInput && currentTorpCoolDownTime <= 0f)
        {
            if (characterStats.canon)
            {
                torpedoSpawner.FireTorpedo();
            }
            currentTorpCoolDownTime = torpedoCoolDownTime;
        }
    }

    private void Burst()
    {
        currentBurstCoolDownTime -= Time.deltaTime;
        if (movementController.burstInput && currentBurstCoolDownTime <= 0f && characterStats.burst)
        {
            body.AddForce(transform.up * characterStats.burstPower, ForceMode.Impulse);
            StartCoroutine(Bursting());
            currentBurstCoolDownTime = burstCoolDownTime;
        }
    }

    private IEnumerator Bursting()
    {
        float elaspedTime = 0;
        float normSpeed = characterStats.linearSpeed;
        while(elaspedTime < burstCoolDownTime)
        {
            elaspedTime += Time.deltaTime;
            characterStats.linearSpeed = characterStats.burstPower;
            yield return null;
        }
        characterStats.linearSpeed = normSpeed;
    }
}
