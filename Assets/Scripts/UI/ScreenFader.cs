using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;

    public static ScreenFader Instance { get; private set; }

    public delegate void FadeDelegate();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void FadeBlackAtEndLevel(FadeDelegate onFadeOut = null)
    {
        StartCoroutine(FadingBlack(onFadeOut, fadeInTime: 0.8f, holdTime: 0.2f, fadeOutTime: 0.8f));
    }

    public void FadeBlackToCheckpoint(FadeDelegate onFadeOut = null)
    {
        StartCoroutine(FadingBlack(onFadeOut, fadeInTime: 0.4f, holdTime: 0.0f, fadeOutTime: 0.5f));
    }
    public IEnumerator FadingBlack(FadeDelegate onFadeOut = null, float fadeInTime = 1f, float holdTime = 1f, float fadeOutTime = 1f)
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeInTime;
            t = Mathf.Pow(t, 2f);
            color.a = Mathf.Lerp(0f, 1f, t);

            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;

        onFadeOut?.Invoke();
        yield return new WaitForSecondsRealtime(holdTime);


        elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / fadeOutTime;
            t = 1 - Mathf.Pow(1 - t, 2f);
            color.a = Mathf.Lerp(1f, 0f, t);

            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }

    public void ResetFade()
    {
        StopAllCoroutines();
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
    }
}
