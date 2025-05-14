using System.Collections;
using UnityEngine;

public class Anglerfish : Enemy<Anglerfish>
{
    private Vector3 targetPos;

    private bool isMoving;
    private float currentTime;
    private float moveDuration;

    private Quaternion targetRotation;
    private float rotationSpeed = 5f;


    protected void Start()
    {
        targetRotation = transform.rotation;
    }
    protected override void Update()
    {
        base.Update();
        UpdatePos();
        ReleaseToPool(this, () => AnglerfishSpawner.anglerfishes.Remove(this));
    }

    private void UpdatePos()
    {

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (isMoving)
        {

            currentTime += Time.deltaTime;
            float speedPattern = SpeedPattern(currentTime, moveDuration);
            float speed = moveSpeed * speedPattern;

            float distanceToTarget = Vector3.Distance(transform.position, targetPos);
            if (distanceToTarget < 0.5f) 
            {
                float dampingFactor = Mathf.Clamp01(distanceToTarget / 0.5f);
                speed *= dampingFactor;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
        else
        {
            StartCoroutine(GetNewPosition());
            isMoving = true;
        }
    }

    private IEnumerator GetNewPosition()
    {
        yield return new WaitForSeconds(1f);
        Vector3 direction = PlayerBrain.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        targetPos = PlayerBrain.Instance.transform.position;
        targetRotation = Quaternion.Euler(0, 0, angle);

        float dist = Vector3.Distance(transform.position, targetPos);
        moveDuration = dist / moveSpeed;
        currentTime = 0;
    }


    private float SpeedPattern(float time, float duration)
    {
        float t = time / duration;
        return Mathf.Pow(t, 2f);
    }
}
