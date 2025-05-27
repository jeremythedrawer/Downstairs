using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerLightController : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    private DitherWorldGridVolumeComponent ditherVolume;

    public float sonarPingTime = 1f;
    public float flareTime = 1f;
    public float radialScanTime = 1f;

    private bool canPing = true;
    private bool canFlare = true;
    private bool canScan = true;

    private Vector2 playerScreenPos;

    private float radialScanPrevAngle = 0;
    private void Start()
    {
        volumeProfile.TryGet<DitherWorldGridVolumeComponent>(out ditherVolume);
    }

    private void Update()
    {
        playerScreenPos = Camera.main.WorldToViewportPoint(PlayerBrain.Instance.transform.position);
        ditherVolume.playerPos.value = playerScreenPos;
    }
    public void SonarPing()
    {
        if (canPing)
        {
            StartCoroutine(SonarPinging());
        }
    }

    public void Flare()
    {
        if (canFlare)
        {
            StartCoroutine(Flaring());
        }
    }

    public void RadialScan()
    {
        if (canScan)
        {
            StartCoroutine(RadialScanning());
        }
    }
    private IEnumerator SonarPinging()
    {
        PlayerBrain.Instance.sonarPingAudioSource.Play();
        float elapsedTime = 0;
        canPing = false;
        while (elapsedTime < sonarPingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / sonarPingTime, 0.5f);

            ditherVolume.sonarPingTime.value = t;
            yield return null;
        }
        ditherVolume.sonarPingTime.value = 0;
        canPing = true;
    }

    private IEnumerator Flaring()
    {
        float elapsedTime = 0f;
        canFlare = false;

        Vector2 playerDir = PlayerBrain.Instance.currentDir.normalized;

        while (elapsedTime < flareTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / flareTime, 0.5f);
            ditherVolume.flareTime.value = t;
            ditherVolume.flarePos.value += (playerDir * t * Time.deltaTime) * 0.75f; 
            yield return null;
        }

        ditherVolume.flareTime.value = 0;
        ditherVolume.flarePos.value = playerScreenPos;
        canFlare = true;
    }

    private IEnumerator RadialScanning()
    {
        float elapsedTime = 0f;
        canScan = false;

        Vector2 playerDir = PlayerBrain.Instance.currentDir;
        while(elapsedTime < radialScanTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime/radialScanTime, 2f);
            ditherVolume.radialScanTime.value = t;

            float angle = Mathf.Atan2(playerDir.y, playerDir.x);
            //if (angle < 0f) angle += Mathf.PI * 2f;
            float normalizedAngle = angle / (Mathf.PI * 2f) - 0.25f;
            float angleDiff = Mathf.DeltaAngle(radialScanPrevAngle * 360f, normalizedAngle * 360f) / 360f;
            float smoothAngle = radialScanPrevAngle + angleDiff;
            smoothAngle = (smoothAngle + 1f) % 1f;
            ditherVolume.radialScanRotation.value = smoothAngle;
            radialScanPrevAngle = smoothAngle;

            yield return null;
        }

        ditherVolume.radialScanTime.value = 0;
        canScan = true;
    }

}
