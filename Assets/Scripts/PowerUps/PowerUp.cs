using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        SonarPing,
        Flare,
        RadialScan

    }

    public PowerUpType powerUpType;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBrain.Instance.audioManager.collectAbilityAudioSource.PlayOneShot(PlayerBrain.Instance.audioManager.collectAbilityAudioSource.clip);
            switch (powerUpType)
            {
                case PowerUpType.Flare:
                {
                    PlayerBrain.Instance.characterStats.canFlare = true;
                }
                break;
                case PowerUpType.SonarPing:
                {
                    PlayerBrain.Instance.characterStats.canSonarPing = true;
                }
                break;
                case PowerUpType.RadialScan:
                {
                    PlayerBrain.Instance.characterStats.canRadialScan = true;
                }
                break;
            }
            gameObject.SetActive(false);
        }
    }
}
