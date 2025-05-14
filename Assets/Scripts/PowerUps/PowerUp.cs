using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    { 
        SingleTorpedo,
        TurboBoost,
        DoubleTorpedo,
        Bomb,
        LightPing,
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
                    PlayerBrain.Instance.meshFilter.mesh = PlayerBrain.Instance.singleTorpedoMesh;
                }
                break;
                case PowerUpType.TurboBoost:
                {
                    PlayerBrain.Instance.characterStats.burst = true;
                    PlayerBrain.Instance.meshFilter.mesh = PlayerBrain.Instance.burstMesh;
                }
                break;
                case PowerUpType.LightPing:
                {
                    PlayerBrain.Instance.characterStats.lightPing = true;
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
