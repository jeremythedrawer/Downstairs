using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        SonarPing,
        Flare

    }

    public PowerUpType powerUpType;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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
            }
            gameObject.SetActive(false);
        }
    }


}
