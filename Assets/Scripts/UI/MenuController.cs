using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public List<MenuButton> menuButtons;

    public Canvas[] canvasList;
    public enum SoundEffectType
    {
        Hover,
        Select
    }

    [Serializable]
    public class SoundEffects
    {
        public AudioClip clip;
        public SoundEffectType type;
    }

    public List<SoundEffects> sfxList;
    public AudioSource audioSource;

    private int currentIndex = 0;
    private Dictionary<SoundEffectType, AudioClip> sfxDict;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;


        sfxDict = new Dictionary<SoundEffectType, AudioClip>();

        foreach (SoundEffects sound in sfxList)
        {
            if (!sfxDict.ContainsKey(sound.type))
            {
                sfxDict.Add(sound.type, sound.clip);
            }
        }
    }
    private void Start()
    {
        UpdateSelection();
    }

    private void Update()
    {
        MenuInputs();
    }

    private void MenuInputs()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = (currentIndex - 1 + menuButtons.Count) % menuButtons.Count;
            UpdateSelection();
            PlayMenuSFX(SoundEffectType.Hover, 1.1f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Count;
            UpdateSelection();
            PlayMenuSFX(SoundEffectType.Hover, 0.9f);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayMenuSFX(SoundEffectType.Select);
            menuButtons[currentIndex].HitButton();
        }
    }
    private void UpdateSelection()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].SetGlow(i == currentIndex);
        }
    }

    private void PlayMenuSFX(SoundEffectType type, float pitch = 1)
    {
        if(sfxDict.TryGetValue(type, out AudioClip clip))
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }
}
