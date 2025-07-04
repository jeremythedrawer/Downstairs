using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public MenuCanvasController[] canvasList;
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

    private static AudioSource audioSource;
    private static int currentIndex = 0;
    private static Dictionary<SoundEffectType, AudioClip> sfxDict;

    private void Awake()
    {
        sfxDict = new Dictionary<SoundEffectType, AudioClip>();

        foreach (SoundEffects sound in sfxList)
        {
            if (!sfxDict.ContainsKey(sound.type))
            {
                sfxDict.Add(sound.type, sound.clip);
            }
        }

        audioSource = GetComponent<AudioSource>();
    }

    public static void MenuInputs(List<MenuButton> menuButtons)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = (currentIndex - 1 + menuButtons.Count) % menuButtons.Count;
            UpdateSelection(menuButtons);
            PlayMenuSFX(SoundEffectType.Hover, 1.1f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = (currentIndex + 1) % menuButtons.Count;
            UpdateSelection(menuButtons);
            PlayMenuSFX(SoundEffectType.Hover, 0.9f);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayMenuSFX(SoundEffectType.Select);
            menuButtons[currentIndex].HitButton();
        }
    }
    public static void UpdateSelection(List<MenuButton> menuButtons)
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            menuButtons[i].SetGlow(i == currentIndex);
        }
    }

    private static void PlayMenuSFX(SoundEffectType type, float pitch = 1)
    {
        if(sfxDict.TryGetValue(type, out AudioClip clip))
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }
}
