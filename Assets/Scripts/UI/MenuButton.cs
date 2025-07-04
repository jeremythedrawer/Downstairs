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

            }
            break;
            case SelectionType.Resume:
            {

            }
            break;
        }
    }

    private void PlayButton()
    {
        onPlay?.Invoke();
        SceneManager.LoadSceneAsync(1);
    }
}
