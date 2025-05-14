using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        SingleTorpedo,
        TurboBoost,
        LightRadar,
        DoubleTorpedo,
        Bomb,
    }

    public PowerUpType powerUpType;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch(powerUpType)
            {
                case PowerUpType.SingleTorpedo:
                {
                    PlayerBrain.Instance.characterStats.singleTorpedo = true;
                }
                break;
                case PowerUpType.TurboBoost:
                {
                    PlayerBrain.Instance.characterStats.turboBoost = true;
                }
                break;
                case PowerUpType.LightRadar:
                {
                    PlayerBrain.Instance.characterStats.lightRadar = true;
                }
                break;
                case PowerUpType.DoubleTorpedo:
                {
                    PlayerBrain.Instance.characterStats.doubleTorpedo = true;
                }
                break;
                case PowerUpType.Bomb:
                {
                    PlayerBrain.Instance.characterStats.bomb = true;
                }
                break;
            }
            gameObject.SetActive(false);
        }

    }


}
