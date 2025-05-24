using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLightController : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    private DitherWorldGridVolumeComponent ditherVolume;
    public float sonarPingRange = 10f;
    public float sonarPingInsensity = 5f;
    public float sonarPingTime = 1f;

    private float normLightRange;
    private float normLightIntensity;

    private bool canPing = true;
    private void Start()
    {
        volumeProfile.TryGet<DitherWorldGridVolumeComponent>(out ditherVolume);
    }

    private void Update()
    {
        ditherVolume.playerPos.value = Camera.main.WorldToViewportPoint(PlayerBrain.Instance.transform.position);
    }
    public void SonarPing()
    {
        if (canPing)
        {
            StartCoroutine(SonarPinging());
        }
    }

    private IEnumerator SonarPinging()
    {
        PlayerBrain.Instance.audioSource.Play();
        float elapsedTime = 0;
        canPing = false;
        while (elapsedTime < sonarPingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sonarPingTime;
            float pingValue = Mathf.Sin(t * Mathf.PI);

            ditherVolume.sonarPingTime.value = t;
             // submarineLight.range = Mathf.Lerp(normLightRange, sonarPingRange, pingValue);
             // submarineLight.intensity = Mathf.Lerp(normLightIntensity, sonarPingInsensity, pingValue);
             yield return null;
        }
        ditherVolume.sonarPingTime.value = 0;
        canPing = true;
    }
}
