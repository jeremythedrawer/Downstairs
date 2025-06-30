using System.Collections;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    public static PlayerBrain instance { get; private set; }

    public SphereCollider sphereCollider;
    public MovementController movementController;
    public PlayerMaterialController playerMaterialController;
    public PlayerLightController lightController;
    public AudioManager audioManager;


    [Header("Power Up Checks")]
    public bool canSonarPing;
    public bool canFlare;
    public bool canRadialScan;

    public Vector2 currentPos => CameraController.mainCam.WorldToViewportPoint(transform.position);
    public Vector2 currentDir => transform.right;
    public bool sonarPingInput { get; set; }
    public bool flareInput { get; set; }
    public bool radialScanInput { get; set; }

    private bool inGodMode;
    private void Awake()
    {
        if (instance == null) instance = this;
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
        sonarPingInput = Input.GetKeyDown(KeyCode.I);
        flareInput = Input.GetKeyDown(KeyCode.O);
        radialScanInput = Input.GetKeyDown(KeyCode.P);
        movementController.rotationInput = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;
    }

    private void UseSonarPing()
    {
        if (sonarPingInput && canSonarPing)
        {
            lightController.SonarPing();
        }
    }

    private void UseFlare()
    {
        if(flareInput && canFlare)
        {
            lightController.Flare();
        }
    }

    private void UseRadialScan()
    {
        if (radialScanInput && canRadialScan)
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
                canSonarPing = true;
                canFlare = true;
                canRadialScan = true;
                inGodMode = true;
            }
            else
            {
                canSonarPing = false;
                canFlare = false;
                canRadialScan = false;
                inGodMode = false;
            }
        }
    }
}
