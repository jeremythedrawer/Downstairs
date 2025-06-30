using System.Linq;
using UnityEngine;

public class SmallFish : SchoolFish<SmallFish>
{
    public float calmMoveSpeed = 0.5f;
    public float panicMoveSpeed = 2f;
    public float triggerRadius = 2f;
    public SmallFishSpawner smallFishSpawner { get; set; }

    private Quaternion targetRotation;
    private float rotationSpeed = 5f;

    private bool isMoving;

    public bool panic {  get; private set; } 
    private void Update()
    {
        UpdatePos();
        UpdateRotation();
        UpdateMaterial();
        ReleaseToPool();
    }

    private void UpdatePos()
    {

        if ((Vector3.Distance(transform.position, PlayerBrain.instance.transform.position) < triggerRadius && !PlayerBrain.instance.lightController.canPing) || smallFishSpawner.smallFishes.Any(fish => fish.panic)) panic = true;

        float speedToUse = panic ? panicMoveSpeed : calmMoveSpeed;
        if (isMoving)
        {
            if (!panic)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speedToUse * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                {
                    isMoving = false;
                }

            }
            else
            {
                transform.position += transform.up * panicMoveSpeed * Time.deltaTime;
            }

        }
        else
        {
            GetNewPos();
            isMoving = true;
        }
    }

    private void UpdateRotation()
    {
        if (panic) return;
        Vector3 dir = transform.position - (Vector3)targetPos;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        targetRotation = Quaternion.Euler(0, 0, angle);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void ReleaseToPool()
    {
        bool outOfCamBounds = transform.position.x < CameraController.minX ||
            transform.position.x > CameraController.maxX ||
            transform.position.y < CameraController.minY ||
            transform.position.y > CameraController.maxY;

        if (panic && outOfCamBounds)
        {
            panic = false;
            spawner.pool.Release(this);
        }
    }

    private void UpdateMaterial()
    {
        shaderController.speed = panic ? panicMoveSpeed * 10f : calmMoveSpeed * 10f;
    }
}
