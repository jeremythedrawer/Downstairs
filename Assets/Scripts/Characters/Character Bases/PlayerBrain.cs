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

    private bool inGodMode;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void OnEnable()
    {
        PowerUp.onAquirePowerUp += AquirePowerUp;
    }

    private void OnDisable()
    {
        PowerUp.onAquirePowerUp -= AquirePowerUp;      
    }
    private void Update()
    {
        MoveInputs();
        movementController.UpdatePos();
        movementController.UpdateRotation();

        UseSonarPing();
        UseFlare();
        UseRadialScan();

        UseUncoverFish();

        OpenInGameMenu();

        GodMode();
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


    private void AquirePowerUp()
    {
        playerMaterialController.PowerUpMaterial();
    }

    private void UseUncoverFish()
    {
        if (MenuCanvas.instance.isActiveAndEnabled) return;
        UncoverFish();
    }
    private void UncoverFish()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, fishDetectionRadius, fishToFindLayer);

        if (hits.Length > 0)
        {
            foreach (Collider c in hits)
            {
                if (c.TryGetComponent<SolitaryFish>(out SolitaryFish fish))
                {
                    if(!fish.uncovered)
                    {
                        if (uncoverInput)
                        {
                            StatsManager.instance.CheckOffFish(fish);
                            playerMaterialController.GlowingMaterial(turnOn: false);
                            playerMaterialController.PowerUpMaterial();

                            audioManager.cameraFlashAudioSource.Play();

                            fish.materialController.FlashMaterial();
                            fish.materialController.GlowingMaterial(turnOn: false);
                        }
                        else
                        {
                            playerMaterialController.GlowingMaterial(turnOn: true);
                        }
                    }
                }
            }
        }
    }

    private void OpenInGameMenu()
    {
        if (menuInput)
        {
            CanvasController.instance.TurnOnInGameMenu();
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
