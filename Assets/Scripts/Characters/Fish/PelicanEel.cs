using UnityEngine;

public class PelicanEel : SolitaryFish
{
    public float speedMultiplier = 2f;
    private float normSpeed;
    private Vector2 prevTargetPos;
    protected override void OnEnable()
    {
        normSpeed = stats.linearSpeed;
        prevTargetPos = transform.position;

        base.OnEnable();
        GetNewPos();
    }
    private void Update()
    {
        if (distanceFromPlayer > 20f)
        {
            movementController.rotationInput = 0;
            movementController.moveInput = 0;
            return;
        }
        UpdateInputs();
        movementController.UpdateRotation();
        movementController.UpdatePos();
        UpdateSpeed();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GetNewPos();
        }
    }

    private void UpdateSpeed()
    {
        float pathToTargetDist = Vector2.Distance(prevTargetPos, targetPos);
        float curDistToTarget = Vector2.Distance(transform.position, targetPos);

        float normDist = 1 - (curDistToTarget / pathToTargetDist); // 1 to start, 0 to end
        float maxSpeed = speedMultiplier * normSpeed;

        float curSpeed = Mathf.Lerp(maxSpeed, normSpeed, normDist);

        stats.linearSpeed = curSpeed;
    }
}
