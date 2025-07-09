using System;
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

            onAquirePowerUp?.Invoke();
            PlayerBrain.lastPowerUp = this;
            switch (powerUpType)
            {
                case PowerUpType.Flare:
                {
                    PopUpCanvas.instance.ShowFlare();
                }
                break;
                case PowerUpType.SonarPing:
                {
                    PopUpCanvas.instance.ShowSonarPing();
                }
                break;
                case PowerUpType.RadialScan:
                {
                    PopUpCanvas.instance.ShowRadialScan();
                }
                break;
            }

            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
