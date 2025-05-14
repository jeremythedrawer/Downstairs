using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static PlayerBrain Instance { get; private set; }

    public List<TorpedoSpawner> torpedoSpawners;
    [SerializeField] private float torpedoCoolDownTime = 0.1f;
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
        healthManager.LooseHealth(boxCollider.bounds, enemyLayer);
    }
    private void MoveInputs()
    {
        movementController.moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementController.shootInput = Input.GetMouseButton(0);
        movementController.burstInput = Input.GetKeyDown(KeyCode.LeftShift);
    }

    private void Shoot()
    {
        currentTorpCoolDownTime -= Time.deltaTime;
        if (movementController.shootInput && currentTorpCoolDownTime <= 0f)
        {
            if (characterStats.doubleTorpedo)
            {
                foreach (TorpedoSpawner torpedoSpawner in torpedoSpawners)
                {
                    torpedoSpawner.FireTorpedo();
                }
            }
            else if (characterStats.singleTorpedo)
            {
                torpedoSpawners[0].FireTorpedo();
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
        float normSpeed = characterStats.runSpeed;
        while(elaspedTime < burstCoolDownTime)
        {
            elaspedTime += Time.deltaTime;
            characterStats.runSpeed = characterStats.burstPower;
            yield return null;
        }
        characterStats.runSpeed = normSpeed;
    }
}
