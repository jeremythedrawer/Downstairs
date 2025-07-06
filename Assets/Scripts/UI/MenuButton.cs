using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuButton : MonoBehaviour
{
    public enum SelectionType
    { 
        Play,
        Quit,
        Resume,
        MainMenu
    }
    public SelectionType selectionType;
    public Image image;

    private readonly int isGlowingID = Shader.PropertyToID("_isGlowing");

    public static event Action onPlay;
    public static event Action onMainMenu;
    public void SetGlow(bool state)
    {
        image.material.SetFloat(isGlowingID, state ? 1f : 0f);
    }

    public void HitButton()
    {
        SetGlow(false);

        switch (selectionType)
        {
            case SelectionType.Play:
            {
                GlobalVolumeController.instance.TransitionToInGame(PlayButton);
            }
            break;
            case SelectionType.Quit:
            {
                Debug.Log("quit");
                Application.Quit();
            }
            break;
            case SelectionType.MainMenu:
            {
                GlobalVolumeController.instance.TransitionToMenu(MainMenuButton);
            }
            break;
            case SelectionType.Resume:
            {
                MenuCanvas.instance.HideUI(time: 1, ResumeButton);
            }
            break;
        }
    }

    private void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
        onPlay?.Invoke();
    }

    private void MainMenuButton()
    {
        SceneManager.LoadSceneAsync(0);
        onMainMenu?.Invoke();
    }

    private void ResumeButton()
    {
    }
}
