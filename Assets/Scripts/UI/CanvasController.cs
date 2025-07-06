using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;

    public enum CanvasType
    { 
        PowerUp,
        Instruction,
        MainMenu,
        InGame
    }

    [Serializable]
    public class CanvasObject
    {
        public CanvasType type;
        public Canvas canvas;
        public int priority = 0;
    }

    public List<CanvasObject> canvases;
    private static CanvasObject curCanvasObject;
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
    private Dictionary<CanvasType, CanvasObject> canvasDict;

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

        canvasDict = new Dictionary<CanvasType, CanvasObject>();

        foreach (CanvasObject canvasObject in canvases)
        {
            if(!canvasDict.ContainsKey(canvasObject.type))
            {
                canvasDict.Add(canvasObject.type, canvasObject);
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
        MenuButton.onPlay += TurnOnInstructionCanvas;

        MenuButton.onMainMenu += TurnOnMainMenu;

        PowerUp.onAquirePowerUp += TurnOnPowerUpCanvas;
    }
    private void OnDisable()
    {
        MenuButton.onPlay -= TurnOffMainMenu;
        MenuButton.onPlay -= TurnOnInstructionCanvas;

        MenuButton.onMainMenu -= TurnOnMainMenu;

        PowerUp.onAquirePowerUp -= TurnOnPowerUpCanvas;

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
        ToggleCanvas(turnOn: true, CanvasType.MainMenu);
    }
    public void TurnOffMainMenu()
    {
        ToggleCanvas(turnOn: false, CanvasType.MainMenu);
    }

    public void TurnOnPowerUpCanvas()
    {
        ToggleCanvas(turnOn: true, CanvasType.PowerUp);
    }
    public void TurnOffPowerUpCanvas()
    {
        ToggleCanvas(turnOn: false, CanvasType.PowerUp);
    }

    private void TurnOnInstructionCanvas()
    {
        ToggleCanvas(turnOn: true, CanvasType.Instruction);
    }
    public void TurnOffInstructionCanvas()
    {
        ToggleCanvas(turnOn: false, CanvasType.Instruction);
    }

    public void TurnOnInGameMenu()
    {
        canInput = true;
        ToggleCanvas(turnOn: true, CanvasType.InGame);
    }

    public void TurnOffInGameMenu()
    {
        ToggleCanvas(turnOn: false, CanvasType.InGame);
    }
    private void ToggleCanvas(bool turnOn, CanvasType type)
    {
        if (canvasDict.TryGetValue(type, out CanvasObject canvasObject))
        {
            if (turnOn && (curCanvasObject == null || canvasObject.priority >= curCanvasObject.priority))
            {
                curCanvasObject = canvasObject;
                canvasObject.canvas.gameObject.SetActive(true);
            }
            else if (!turnOn)
            {
                curCanvasObject = null;
                canvasObject.canvas.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError($"Canvas Dictionary could not find type {type}");
        }
    }
}
