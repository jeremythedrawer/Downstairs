using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        Canon,
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
                case PowerUpType.Canon:
                {
                    PlayerBrain.Instance.characterStats.canon = true;
                    PlayerBrain.Instance.canonMesh.enabled = true;
                }
                break;
                case PowerUpType.TurboBoost:
                {
                    PlayerBrain.Instance.characterStats.burst = true;
                    PlayerBrain.Instance.burstMesh.enabled = true;
                }
                break;
                case PowerUpType.SonarPing:
                {
                    PlayerBrain.Instance.characterStats.sonarPing = true;
                    PlayerBrain.Instance.sonarPingMesh.enabled = true;
                }
                break;
            }
            gameObject.SetActive(false);
        }
    }


}
