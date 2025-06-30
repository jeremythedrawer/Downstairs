using UnityEngine;

public class Anglerfish : SolitaryFish
{
    private float rotThreshold = 0.9f;

    protected override void OnEnable()
    {
        base.OnEnable();
        GetNewPos();
    }
    private void Update()
    {
        if (distanceFromPlayer > 5f) return;
        UpdateInputs();
        movementController.UpdateRotation();
        movementController.UpdatePos();
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
                float turnStrength = Mathf.Clamp(angleToTarget / 45f, -1f, 1f); 
                movementController.rotationInput = turnStrength;
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
}
