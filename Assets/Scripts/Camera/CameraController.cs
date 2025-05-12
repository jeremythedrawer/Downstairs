using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Camera mainCam;
    public static float camHeight;
    public static float camWidth;
    public static float minX;
    public static float maxX;
    public static float minY;
    public static float maxY;

    private void Start()
    {
        mainCam = Camera.main;
        camHeight = 2f * mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;

        Vector3 camPos = mainCam.transform.position;

        minX = camPos.x - camWidth / 2f;
        maxX = camPos.x + camWidth / 2f;
        minY = camPos.y - camHeight / 2f;
        maxY = camPos.y + camHeight / 2f;
    }
}
