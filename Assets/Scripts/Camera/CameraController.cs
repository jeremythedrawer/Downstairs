using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float damping = 5f;
    public float forwardOffset = 0f;
    public static Camera mainCam;
    public static float camHeight;
    public static float camWidth;
    public static float minX;
    public static float maxX;
    public static float minY;
    public static float maxY;

    private Vector3 camPos;

    private Vector3 target;

    private Vector3 offset;
    private void Start()
    {
        mainCam = Camera.main;
        camHeight = 2f * mainCam.orthographicSize;
        camWidth = camHeight * mainCam.aspect;


    }

    private void Update()
    {
        GetTargetPos();
        FollowTarget();
        GetCurrentCamBounds();
    }

    private void GetTargetPos()
    {
        Transform playerTransform = PlayerBrain.Instance.transform;
        Vector3 playerPos = playerTransform.position;
        offset = playerTransform.up * forwardOffset;
        target = new Vector3(playerPos.x + offset.x, playerPos.y + offset.y, transform.position.z);
    }

    private void FollowTarget()
    {
        float camWoldPosX = Mathf.Lerp(transform.position.x, target.x, Time.deltaTime * damping);
        float camWoldPosY = Mathf.Lerp(transform.position.y, target.y, Time.deltaTime * damping);

        transform.position = new Vector3(camWoldPosX, camWoldPosY, transform.position.z);
    }

    private void GetCurrentCamBounds()
    {
        camPos = mainCam.transform.position;

        minX = camPos.x - camWidth / 2f;
        maxX = camPos.x + camWidth / 2f;
        minY = camPos.y - camHeight / 2f;
        maxY = camPos.y + camHeight / 2f;
    }
}
