using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GlobalVolumeController : MonoBehaviour
{
    public static GlobalVolumeController instance {  get; private set; }
    public VolumeProfile globalVolume;
    private DitherWorldGridVolumeComponent ditherWorldGridVolume;

    private float transitionTime = 2f;


    [Header("Menu Parameters")]
    public float menuGridScale = 10;
    public float menuCentreLightSize = 0;

    [Header("Game Parameters")]
    public float inGameGridScale = 6;
    public float inGameCenterLightSize = 5;

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
    private IEnumerator TransitioningScene(float endGridScale, float centreLightSize, SceneChangeDelegate onSceneChange = null)
    {
        float elapsedTime = 0;
        float startGridScale = ditherWorldGridVolume.gridScale.value;

        float firstHalfTransTime = transitionTime * 0.5f;
        float maxGridScale = 100f;
        while (elapsedTime < firstHalfTransTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Pow(elapsedTime / transitionTime, 0.5f);
            float gridScale = AlternatingCosineWave(t, startGridScale, endGridScale, maxGridScale);
            ditherWorldGridVolume.gridScale.value = gridScale;
            yield return null;
        }

        onSceneChange?.Invoke();
        ditherWorldGridVolume.centreLightSize.value = centreLightSize;

        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionTime;
            float gridScale = AlternatingCosineWave(t, startGridScale, endGridScale, maxGridScale);
            ditherWorldGridVolume.gridScale.value = gridScale;
            yield return null;
        }
        ditherWorldGridVolume.gridScale.value = endGridScale;
    }

    float AlternatingCosineWave(float t, float a, float b, float c)
    {
        float cos = Mathf.Cos(Mathf.PI * t);
        float cosSq = cos * cos;
        float peak = 0.5f * (a + b) + 0.5f * (a - b) * cos;
        return c + (peak - c) * cosSq;
    }
}
