using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public enum SelectionType
    { 
        Play,
        Quit,
        Resume,
        Continue
    }
    public SelectionType selectionType;
    public Image image;

    private readonly int isGlowingID = Shader.PropertyToID("_isGlowing");

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
            case SelectionType.Continue:
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
        SceneManager.LoadSceneAsync(1);
        MenuController.instance.canvasList[0].gameObject.SetActive(false);
        UIManager.instance.TurnOffMenu();
    }
}
