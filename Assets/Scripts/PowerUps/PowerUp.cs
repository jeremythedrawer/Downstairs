using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        Arm,
        TurboBoost,
        SonarPing,
    }

    public PowerUpType powerUpType;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (powerUpType)
            {
                case PowerUpType.Arm:
                {
                    PlayerBrain.Instance.characterStats.arm = true;
                    PlayerBrain.Instance.armObject.SetActive(true);
                }
                break;
                case PowerUpType.TurboBoost:
                {
                    PlayerBrain.Instance.characterStats.burst = true;
                    PlayerBrain.Instance.burstObject.SetActive(true);
                }
                break;
                case PowerUpType.SonarPing:
                {
                    PlayerBrain.Instance.characterStats.sonarPing = true;
                    PlayerBrain.Instance.sonarPingObject.SetActive(true);
                }
                break;
            }
            gameObject.SetActive(false);
        }
    }


}
