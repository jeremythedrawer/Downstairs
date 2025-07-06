using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCanvas : MonoBehaviour
{
    public List<PowerUpUI> powerUpUIList;
    public static PowerUp.PowerUpType activePowerUpType;
    public static PowerUp.PowerUpType prevPowerUpType;

    private PowerUpUI activePowerUpUI;
    private void OnEnable()
    {
        ShowActivePowerUpUI();
    }

    private void Update()
    {
        HideActivePowerUpUIInput();
    }
    private Coroutine ShowActivePowerUpUI()
    {
        return StartCoroutine(ShowingActivePowerUpUI());
    }

    private IEnumerator ShowingActivePowerUpUI()
    {
        yield return new WaitUntil(()=> activePowerUpType != prevPowerUpType);

        foreach (PowerUpUI powerUpUI in powerUpUIList)
        {
            if (powerUpUI.powerUpType == activePowerUpType)
            {
                powerUpUI.gameObject.SetActive(true);
                activePowerUpUI = powerUpUI;
                prevPowerUpType = activePowerUpType;
            }
        }
    }

    private void HideActivePowerUpUIInput()
    {
        switch(activePowerUpType)
        {
            case PowerUp.PowerUpType.Flare:
            {
                if (PlayerBrain.instance.flareInput)
                {
                    HideActivePowerUpUI();
                }
            }
            break;
            case PowerUp.PowerUpType.SonarPing:
            {
                if (PlayerBrain.instance.sonarPingInput)
                {
                    HideActivePowerUpUI();
                }
            }
            break;
            case PowerUp.PowerUpType.RadialScan:
            {
                if (PlayerBrain.instance.radialScanInput)
                {
                    HideActivePowerUpUI();
                }
            }
            break;
        }
    }

    private void HideActivePowerUpUI()
    {
        activePowerUpUI.HideUI(time: 1, CanvasController.instance.TurnOffPowerUpCanvas);
    }


}
