using UnityEngine;

public class Anglerfish : SolitaryFish
{
    private float rotThreshold = 0.9f;

    private Vector2 dirToTarget;
    private float dirDot;
    protected override void OnEnable()
    {
        base.OnEnable();
        GetNewPos();
    }
    protected void Update()
    {
        UpdateInputs();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GetNewPos();
        }
    }
    private void UpdateInputs()
    {
        Vector2 toTarget = targetPos - (Vector2)transform.position;
        float distToTarget = toTarget.magnitude;

        if (distToTarget > 0.3f)
        {
            float angleToTarget = Vector2.SignedAngle(transform.right, toTarget.normalized);

            if (Mathf.Abs(angleToTarget) > rotThreshold)
            {
                movementController.rotationInput = angleToTarget > 0 ? 1 : -1;
                movementController.moveInput = 0;
            }
            else
            {
                movementController.rotationInput = 0;
                movementController.moveInput = 1;
            }
        }
        else
        {
            GetNewPos();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, moveBoundRadius);
        }
        DrawPath();
    }

    private void DrawPath()
    {
        Debug.DrawLine(transform.position, targetPos, Color.magenta);
    }

}
