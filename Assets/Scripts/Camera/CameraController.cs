using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }
}
