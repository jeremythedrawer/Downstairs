using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    public static PlayerBrain instance { get; private set; }

    public static PowerUp lastPowerUp;

    public SphereCollider sphereCollider;
    public MovementController movementController;
    public PlayerMaterialController playerMaterialController;
    public PlayerLightController lightController;
    public AudioManager audioManager;

    [Header("Fish Detection")]
    public float fishDetectionRadius = 5f;
    public LayerMask fishToFindLayer;

    [Header("Power Up Checks")]
    public bool canSonarPing;
    public bool canFlare;
    public bool canRadialScan;

    public Vector2 currentPos => CameraController.mainCam.WorldToViewportPoint(transform.position);
    public Vector2 currentDir => transform.right;
    public bool sonarPingInput { get; set; }
    public bool flareInput { get; set; }
    public bool radialScanInput { get; set; }
    public bool uncoverInput { get; set; }
    public bool menuInput { get; set; }

    private bool foundFirstFish;
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

        AquirePowerUp();

        UseSonarPing();
        UseFlare();
        UseRadialScan();

        OpenInGameMenu();

        TurnOffStartUI();
        GodMode();
        UseUncoverFish();
    }

    private void FixedUpdate()
    {
        movementController.UpdatePos();
        movementController.UpdateRotation();
    }
    private void MoveInputs()
    {
        movementController.moveInput = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        movementController.rotationInput = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0;

        flareInput = Input.GetKeyDown(KeyCode.I);
        sonarPingInput = Input.GetKeyDown(KeyCode.O);
        radialScanInput = Input.GetKeyDown(KeyCode.P);

        uncoverInput = Input.GetKeyDown(KeyCode.Space);

        menuInput = Input.GetKeyDown(KeyCode.Tab);
    }


    private void AquirePowerUp()
    {
        if (lastPowerUp == null) return;
        switch (lastPowerUp.powerUpType)
        { 
            case PowerUp.PowerUpType.Flare:
            {
                if (flareInput && !canFlare)
                {
                    PopUpCanvas.instance.HideFlare();
                    canFlare = true;

                }
            }
            break;
            case PowerUp.PowerUpType.SonarPing:
            {
                if (sonarPingInput && !canSonarPing)
                {
                    PopUpCanvas.instance.HideSonarPing();
                    canSonarPing = true;

                }
            }
            break;
            case PowerUp.PowerUpType.RadialScan:
            {
                if (radialScanInput && !canRadialScan)
                {
                    PopUpCanvas.instance.HideRadialScan();
                    canRadialScan = true;
                }
            }
            break;
        }
    }
    private void UseFlare()
    {
        if(flareInput && canFlare)
        {
            lightController.Flare();
        }
    }

    private void UseSonarPing()
    {
        if (sonarPingInput && canSonarPing)
        {
            lightController.SonarPing();
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
        playerMaterialController.PowerUpMaterial();
    }

    private void UseUncoverFish()
    {
        if (MenuCanvas.instance != null && MenuCanvas.instance.isActiveAndEnabled) return;
        UncoverFish();
    }

    private void UncoverFish()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, fishDetectionRadius, fishToFindLayer);

        if (hits.Length > 0)
        {
            foreach (Collider c in hits)
            {
                if (c.TryGetComponent<SolitaryFish>(out SolitaryFish fish) && !fish.uncovered)
                {
                    if (uncoverInput)
                    {
                        StatsManager.instance.CheckOffFish(fish);

                        playerMaterialController.GlowingMaterial(turnOn: false);
                        playerMaterialController.PowerUpMaterial();

                        audioManager.cameraFlashAudioSource.PlayOneShot(audioManager.cameraFlashAudioSource.clip);

                        fish.materialController.FlashMaterial();
                        fish.materialController.GlowingMaterial(turnOn: false);

                        foundFirstFish = true;
                        PopUpCanvas.instance.HideFirstFish();
                    }
                    else
                    {
                        playerMaterialController.GlowingMaterial(turnOn: true);
                        if (!foundFirstFish)
                        {
                            PopUpCanvas.instance.ShowFirstFish();
                        }
                    }
                }
            }
        }
        else if (!foundFirstFish)
        {
            PopUpCanvas.instance.HideFirstFish();          
        }
    }  
    private void OpenInGameMenu()
    {
        if (menuInput)
        {
            MenuCanvasController.instance.TurnOnInGameMenu();
        }
    }

    private void TurnOffStartUI()
    {
        if (uncoverInput)
        {
            PopUpCanvas.instance.HideStartImages();
        }
    }
    private void OnDrawGizmosSelected()
    {
        DrawFishDetectionRadius();
    }
    private void DrawFishDetectionRadius()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fishDetectionRadius);
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
