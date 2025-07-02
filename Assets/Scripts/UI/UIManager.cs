using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public MenuController menuController;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void TurnOffMenu()
    {
        menuController.gameObject.SetActive(false);
    }
}
