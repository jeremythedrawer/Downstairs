using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicAudioSource;
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
        float startVolume = musicAudioSource.volume;
        float targetVolume = turnOn ? 1f : 0f;

        while (elapsedTime < turnDownTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / turnDownTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
    }
}
