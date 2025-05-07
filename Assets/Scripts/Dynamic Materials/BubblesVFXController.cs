using UnityEngine;
using UnityEngine.VFX;
public class BubblesVFXController : MonoBehaviour
{
    public PlayerBrain player;
    private float normPlayerSpeed;
    private float scale;

    private readonly int scaleID = Shader.PropertyToID("scale");
    private float speed;

    private readonly int speedID = Shader.PropertyToID("speed");

    public VisualEffect bubblesVFX;

    private void Update()
    {
        normPlayerSpeed = player.body.linearVelocity.magnitude / player.characterStats.runSpeed;
        if (normPlayerSpeed > 0.1f)
        {            
            scale = Mathf.Lerp(0f, 1f, normPlayerSpeed);
            speed = Mathf.Lerp(0f, 2f, normPlayerSpeed);
        }
        else
        {
            scale = 0f;
            speed = 0f;
        }

        bubblesVFX.SetFloat(scaleID, scale);
        bubblesVFX.SetFloat(speedID, speed);
    }
}
