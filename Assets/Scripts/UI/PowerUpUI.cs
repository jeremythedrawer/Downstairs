public class PowerUpUI : ImageController
{
    private void OnEnable()
    {
        ShowUI(time: 3);
    }
    public PowerUp.PowerUpType powerUpType;
}
