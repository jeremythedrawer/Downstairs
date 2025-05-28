using System.Collections;
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

            int materialCount = PlayerBrain.Instance.playerMaterialController.GetMaterialCount();

            for (int i = 0; i < materialCount; i++)
            {
                StartCoroutine(PoweringUpMaterial(i));
            }

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

    private IEnumerator PoweringUpMaterial(int materialIndex)
    {
        float powerUpTime = 0.5f;
        float elapsedTime = 0f;
        var matController = PlayerBrain.Instance.playerMaterialController;
        Color originalColor = matController.GetOriginalColor(materialIndex);

        while (elapsedTime < powerUpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / powerUpTime;

            float intensity = Mathf.Lerp(40, 1, t);

            Color pulsedColor = originalColor * intensity;

            matController.SetColor(materialIndex, pulsedColor);

            yield return null;
        }
        // Restore original color after pulse
        matController.SetColor(materialIndex, originalColor);
    }
}
