using System.Collections;
using UnityEngine;

public class PlayerBrain : CharacterBrain
{
    public static PlayerBrain Instance { get; private set; }

    public Vector2 currentPos => CameraController.mainCam.WorldToViewportPoint(transform.position);
    public Vector2 currentDir => transform.up;

    [SerializeField] private LayerMask hitLayer;

    private bool inGodMode;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void OnEnable()
    {
        PowerUp.onAquirePowerUp += PowerUpMaterial;
    }

    private void OnDisable()
    {
        PowerUp.onAquirePowerUp -= PowerUpMaterial;      
    }
    private void Update()
    {
        MoveInputs();
        UseSonarPing();
        UseFlare();
        UseRadialScan();
        GodMode();
    }
    private void MoveInputs()
    {
        movementController.moveInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        movementController.sonarPingInput = Input.GetKeyDown(KeyCode.I);
        movementController.flareInput = Input.GetKeyDown(KeyCode.O);
        movementController.radialScanInput = Input.GetKeyDown(KeyCode.P);
        movementController.rotationInput = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;
    }

    private void UseSonarPing()
    {
        if (movementController.sonarPingInput && characterStats.canSonarPing)
        {
            lightController.SonarPing();
        }
    }

    private void UseFlare()
    {
        if(movementController.flareInput && characterStats.canFlare)
        {
            lightController.Flare();
        }
    }

    private void UseRadialScan()
    {
        if (movementController.radialScanInput && characterStats.canRadialScan)
        {
            lightController.RadialScan();
        }
    }

    private void PowerUpMaterial()
    {
        int materialCount = playerMaterialController.GetMaterialCount();

        for (int i = 0; i < materialCount; i++)
        {
            StartCoroutine(PoweringUpMaterial(i));
        }
    }

    private IEnumerator PoweringUpMaterial(int materialIndex)
    {
        float powerUpTime = 0.5f;
        float elapsedTime = 0f;
        Color originalColor = playerMaterialController.GetOriginalColor(materialIndex);

        while (elapsedTime < powerUpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / powerUpTime;

            float intensity = Mathf.Lerp(40, 1, t);

            Color pulsedColor = originalColor * intensity;

            playerMaterialController.SetColor(materialIndex, pulsedColor);

            yield return null;
        }
        // Restore original color after pulse
        playerMaterialController.SetColor(materialIndex, originalColor);
    }

    private void GodMode()
    {
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (!inGodMode)
            {
                characterStats.canSonarPing = true;
                characterStats.canFlare = true;
                characterStats.canRadialScan = true;
                inGodMode = true;
            }
            else
            {
                characterStats.canSonarPing = false;
                characterStats.canFlare = false;
                characterStats.canRadialScan = false;
                inGodMode = false;
            }
        }
    }
}
