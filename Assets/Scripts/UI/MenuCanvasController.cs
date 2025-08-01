using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasController : MonoBehaviour
{
    public static MenuCanvasController instance;

    public MenuCanvas mainMenuCanvas;
    public MenuCanvas pauseMenuCanvas;
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
    private static bool canInput = true;

    private static Dictionary<SoundEffectType, AudioClip> sfxDict;
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

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        TurnOnMainMenu();
    }
    private void OnEnable()
    {
        MenuButton.onPlay += TurnOffMainMenu;

        MenuButton.onMainMenu += TurnOnMainMenu;
        MenuButton.onMainMenu += TurnOffPauseMenu;
    }
    private void OnDisable()
    {
        MenuButton.onPlay -= TurnOffMainMenu;

        MenuButton.onMainMenu -= TurnOnMainMenu;
        MenuButton.onMainMenu -= TurnOffPauseMenu;
    }
    public static void MenuInputs(List<MenuButton> menuButtons)
    {
        if (canInput)
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
                canInput = false;
            }
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

    public void TurnOnMainMenu()
    {
        canInput = true;
        ToggleCanvas(turnOn: true, mainMenuCanvas);
    }
    public void TurnOffMainMenu()
    {
        ToggleCanvas(turnOn: false, mainMenuCanvas);
    }

    public void TurnOnPauseMenu()
    {
        canInput = true;
        ToggleCanvas(turnOn: true, pauseMenuCanvas);
    }

    public void TurnOffPauseMenu()
    {
        ToggleCanvas(turnOn: false, pauseMenuCanvas);
    }
    private void ToggleCanvas(bool turnOn, MenuCanvas menuCanvas)
    {
        menuCanvas.gameObject.SetActive(turnOn);
    }
}
