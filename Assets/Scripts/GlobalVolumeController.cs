using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GlobalVolumeController : MonoBehaviour
{
    public static GlobalVolumeController instance {  get; private set; }
    public VolumeProfile globalVolume;
    public DitherWorldGridVolumeComponent ditherWorldGridVolume;

    private float transitionTime = 3f;


    [Header("Menu Parameters")]
    public float menuGridScale = 10;
    public float menuCentreLightSize = 0;

    [Header("Game Parameters")]
    public float inGameGridScale = 6;
    public float inGameCenterLightSize = 5;

    [Header("Grid Thickness Paremeters")]
    public float normalGridThickness = 0.12f;
    public float maxGridThickness = 0.5f;

    public delegate void SceneChangeDelegate();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        globalVolume.TryGet<DitherWorldGridVolumeComponent>(out ditherWorldGridVolume);
        ditherWorldGridVolume.gridScale.value = menuGridScale;
        ditherWorldGridVolume.centreLightSize.value = menuCentreLightSize;
    }

    public Coroutine TransitionToInGame(SceneChangeDelegate onSceneChange = null)
    {
        return StartCoroutine(TransitioningScene(inGameGridScale, inGameCenterLightSize, onSceneChange));
    }

    public Coroutine TransitionToMenu(SceneChangeDelegate onSceneChange = null)
    {
        return StartCoroutine(TransitioningScene(menuGridScale, menuCentreLightSize, onSceneChange));
    }
    private IEnumerator TransitioningScene(float newGridScale, float newCentreLightSize, SceneChangeDelegate onSceneChange = null)
    {
        float elapsedTime = 0;
        float startCentreLightSize = ditherWorldGridVolume.centreLightSize.value;

        float firstHalfTransTime = transitionTime * 0.5f;
        float maxCentreLightSize = 40f;
        while (elapsedTime < firstHalfTransTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / transitionTime, 0.5f);
            float centreLightSize = AlternatingCosineWave(t, startCentreLightSize, newCentreLightSize, maxCentreLightSize);

            ditherWorldGridVolume.gridThickness.value = Mathf.Lerp(normalGridThickness, maxGridThickness, t * 2);
            ditherWorldGridVolume.centreLightSize.value = centreLightSize;
            yield return null;
        }

        ditherWorldGridVolume.gridScale.value = newGridScale;

        onSceneChange?.Invoke();

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;
            float centreLightSize = AlternatingCosineWave(t, startCentreLightSize, newCentreLightSize, maxCentreLightSize);

            ditherWorldGridVolume.gridThickness.value = Mathf.Lerp(normalGridThickness, maxGridThickness, 1 - (t*2));
            ditherWorldGridVolume.centreLightSize.value = centreLightSize;
            yield return null;
        }
        ditherWorldGridVolume.centreLightSize.value = newCentreLightSize;
    }

    float AlternatingCosineWave(float t, float a, float b, float c)
    {
        float cos = Mathf.Cos(Mathf.PI * t);
        float cosSq = cos * cos;
        float peak = 0.5f * (a + b) + 0.5f * (a - b) * cos;
        return c + (peak - c) * cosSq;
    }
}
