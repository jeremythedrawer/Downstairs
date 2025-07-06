using System.Linq;
using UnityEngine;

public class SmallFish : SchoolFish<SmallFish>
{
    public float panicSpeedMultiplier = 25f;
    public float triggerRadius = 1f;
    public SmallFishSpawner smallFishSpawner { get; set; }
    public bool panic {  get; private set; } 

    private Quaternion targetRotation;
    private float rotationSpeed = 5f;
    private bool isMoving;
    private float curSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        if (distanceFromPlayer > 20f) return;
        UpdatePos();
        UpdateRotation();
        SetSpeedMaterial(curSpeed * 10f);
        ReleaseToPool();
    }

    private void UpdatePos()
    {
        
        if (PlayerBrain.instance != null && (Vector3.Distance(transform.position, PlayerBrain.instance.transform.position) < triggerRadius && !PlayerBrain.instance.lightController.canPing)) panic = true;

        if (isMoving)
        {
            if (!panic)
            {
                curSpeed = stats.linearSpeed;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, curSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, targetPos) < 0.1f)
                {
                    isMoving = false;
                }

            }
            else
            {
                curSpeed = stats.linearSpeed * panicSpeedMultiplier;
                transform.position += transform.up * curSpeed * Time.deltaTime;
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

    private void SetSpeedMaterial(float newSpeed)
    {
        materialController.SetNewSpeed(newSpeed);
    }
}
