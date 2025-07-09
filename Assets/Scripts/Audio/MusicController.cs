using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicAudioSource;
    private float turnDownTime = 2;

    private void OnEnable()
    {
        MenuButton.onPlay += TurnOffMusic;
        MenuButton.onMainMenu += TurnOnMusic;
    }

    private void OnDisable()
    {
        MenuButton.onPlay -= TurnOffMusic;
        MenuButton.onMainMenu -= TurnOnMusic;
    }

    private void TurnOffMusic()
    {
        StartCoroutine(TogglingMusic(turnOn: false));
    }

    private void TurnOnMusic()
    {
        StartCoroutine(TogglingMusic(turnOn: true));
    }


    private IEnumerator TogglingMusic(bool turnOn)
    {
        if (turnOn)
        {
            musicAudioSource.PlayOneShot(musicAudioSource.clip);
        }
        float elapsedTime = 0f;
        float startVolume = musicAudioSource.volume;
        float targetVolume = turnOn ? 1f : 0f;

        while (elapsedTime < turnDownTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / turnDownTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        musicAudioSource.volume = targetVolume;
        if (!turnOn)
        {
            musicAudioSource.Stop();
        }
    }
}
