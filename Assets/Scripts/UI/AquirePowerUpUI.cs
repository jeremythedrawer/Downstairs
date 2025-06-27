using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AquirePowerUpUI : MonoBehaviour
{
    public static AquirePowerUpUI instance;
    public Image powerUpImage;
    public float time = 3;

    [Serializable]
    public class PowerUpUI
    {
        public PowerUp.PowerUpType type;
        public Sprite sprite;
    }

    public List<PowerUpUI> powerUpUIList;

    private static Dictionary<PowerUp.PowerUpType, Sprite> powerUpUIDict;

    private static PowerUp.PowerUpType activeType;

    private bool hideUIFlag;
    private void Awake()
    {
        instance = this;

        powerUpUIDict = new Dictionary<PowerUp.PowerUpType, Sprite>();
        foreach(PowerUpUI powerUpUI in powerUpUIList)
        {
            if (!powerUpUIDict.ContainsKey(powerUpUI.type))
            {
                powerUpUIDict.Add(powerUpUI.type, powerUpUI.sprite);
            }
        }
    }
    private void OnEnable()
    {
        PowerUp.onAquirePowerUp += ShowUI;
    }

    private void OnDisable()
    {
        PowerUp.onAquirePowerUp -= ShowUI;
    }


    private void Update()
    {
        HideUI();
    }
    private void ShowUI()
    {
        StartCoroutine(ShowingUI());
    }

    private void HideUI()
    {
        if (activeType == PowerUp.PowerUpType.None || powerUpImage.color != Color.white) return;

        switch (activeType)
        {
            case PowerUp.PowerUpType.SonarPing:
            {
                if (PlayerBrain.Instance.movementController.sonarPingInput)
                {
                    if (!hideUIFlag)
                    {
                        StartCoroutine(HidingUI());
                        hideUIFlag = true;
                    }
                }
            }
            break;

            case PowerUp.PowerUpType.Flare:
            {
                if (PlayerBrain.Instance.movementController.flareInput)
                {
                    if (!hideUIFlag)
                    {
                        StartCoroutine(HidingUI());
                        hideUIFlag = true;
                    }
                }
            }
            break;

            case PowerUp.PowerUpType.RadialScan:
            {
                if (PlayerBrain.Instance.movementController.radialScanInput)
                {
                    if (!hideUIFlag)
                    {
                        StartCoroutine(HidingUI());
                        hideUIFlag = true;
                    }
                }
            }
            break;
        }
    }

    public static void GetPowerUI(PowerUp.PowerUpType name)
    {
        if (powerUpUIDict.TryGetValue(name, out Sprite sprite))
        {
            instance.powerUpImage.sprite = sprite;
            activeType = name;
        }
    }


    private IEnumerator ShowingUI()
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Pow(elapsedTime / time, 2);
            Color color = new Color(1, 1, 1, alpha);
            powerUpImage.color = color;
            yield return null;
        }

        powerUpImage.color = Color.white;
    }

    private IEnumerator HidingUI()
    {
        float elapsedTime = time;

        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            float alpha = Mathf.Pow(elapsedTime / time, 2);
            Color color = new Color(1, 1, 1, alpha);
            powerUpImage.color = color;
            yield return null;
        }

        hideUIFlag = false;
        powerUpImage.color = Color.clear;
        activeType = PowerUp.PowerUpType.None;
    }


}
