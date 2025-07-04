using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public MenuController menuController;
    public PowerUpCanvasController powerUpCanvasController;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void OnEnable()
    {
        MenuButton.onPlay += TurnOffMenuController;
        PowerUp.onAquirePowerUp += TurnOnPowerUpCanvasController;
    }

    private void OnDisable()
    {
        MenuButton.onPlay -= TurnOffMenuController;
        PowerUp.onAquirePowerUp -= TurnOnPowerUpCanvasController;
    }
    public void TurnOffMenuController()
    {
        menuController.gameObject.SetActive(false);
    }

    public void TurnOnPowerUpCanvasController()
    {
        powerUpCanvasController.gameObject.SetActive(true);
    }

    public void TurnOffPowerUpCanvasController()
    {
        powerUpCanvasController.gameObject.SetActive(false);
    }
}
