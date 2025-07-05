using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private float turnDownTime = 2;

    private void OnEnable()
    {
        MenuButton.onPlay += TurnOffMusic;
    }

    private void OnDisable()
    {
        MenuButton.onPlay -= TurnOffMusic;
    }

    private void TurnOffMusic()
    {
        StartCoroutine(TogglingMusic(turnOn: false));
    }

    private IEnumerator TogglingMusic(bool turnOn)
    {
        float elapsedTime = 0f;
        float startVolume = audioSource.volume;
        float targetVolume = turnOn ? 1f : 0f;

        while (elapsedTime < turnDownTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / turnDownTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
