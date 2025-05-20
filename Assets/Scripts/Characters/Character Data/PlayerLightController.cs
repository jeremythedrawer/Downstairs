using System.Collections;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Light submarineLight;
    public float sonarPingRange = 10f;
    public float sonarPingInsensity = 5f;
    public float sonarPingTime = 1f;

    private float normLightRange;
    private float normLightIntensity;

    private bool canPing = true;
    private void Start()
    {
        normLightRange = submarineLight.range;
        normLightIntensity = submarineLight.intensity;
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
        float elapsedTime = 0;
        canPing = false;
        while (elapsedTime < sonarPingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / sonarPingTime;
            float pingValue = Mathf.Sin(t * Mathf.PI);

            submarineLight.range = Mathf.Lerp(normLightRange, sonarPingRange, pingValue);
            submarineLight.intensity = Mathf.Lerp(normLightIntensity, sonarPingInsensity, pingValue);
            yield return null;
        }

        submarineLight.range = normLightRange;
        submarineLight.intensity = normLightIntensity;
        canPing = true;
    }
}
