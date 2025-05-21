using System.Collections;
using UnityEngine;

public class Anglerfish : Enemy<Anglerfish>
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float chaseRadius = 5f;
    [SerializeField] private float wallCheckDistance = 0.5f;
    private Vector3 targetPos;

    private bool isMoving;
    private float currentTime;
    private float moveDuration;

    private Quaternion targetRotation;
    private float rotationSpeed = 5f;

    public AnglerfishSpawner anglerfishSpawner {  get; set; }
    protected void Start()
    {
        targetRotation = transform.rotation;
    }
    protected void Update()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if (isMoving)
        {
            if (IsHittingWall())
            {
                isMoving = false;
                return;
            }

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
        else if (Vector3.Distance(transform.position, PlayerBrain.Instance.transform.position) < chaseRadius)
        {
            StartCoroutine(GetNewPosition());
        }
    }

    private IEnumerator GetNewPosition()
    {
        yield return new WaitForSeconds(1f);

        float randomRadius = 5f;
        Vector2 randomOffset = Random.insideUnitCircle * randomRadius;
        Vector3 randomTarget = PlayerBrain.Instance.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        Vector3 direction = randomTarget - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        targetPos = randomTarget;
        targetRotation = Quaternion.Euler(0, 0, angle);

        float dist = Vector3.Distance(transform.position, targetPos);
        moveDuration = dist / moveSpeed;
        currentTime = 0;

        isMoving = true;
    }


    private float SpeedPattern(float time, float duration)
    {
        float t = time / duration;
        return Mathf.Pow(t, 2f);
    }
    private bool IsHittingWall()
    {
        Vector3 direction = (targetPos - transform.position).normalized;
        return Physics.Raycast(transform.position, direction, wallCheckDistance, wallLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Vector3 direction = (targetPos - transform.position).normalized;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * wallCheckDistance);
    }
}
