using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        None,
        SonarPing,
        Flare,
        RadialScan
    }

    public PowerUpType powerUpType;
    public AudioSource powerUpAudioSource;
    public static event Action onAquirePowerUp;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            powerUpAudioSource.PlayOneShot(powerUpAudioSource.clip);
            AquirePowerUpUI.GetPowerUI(powerUpType);
            onAquirePowerUp?.Invoke();

            switch (powerUpType)
            {
                case PowerUpType.Flare:
                PlayerBrain.Instance.characterStats.canFlare = true;
                break;
                case PowerUpType.SonarPing:
                PlayerBrain.Instance.characterStats.canSonarPing = true;
                break;
                case PowerUpType.RadialScan:
                PlayerBrain.Instance.characterStats.canRadialScan = true;
                break;
            }

            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
