using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody body;
    public CharacterStats stats;
    public float moveInput { get; set; }
    public float rotationInput { get; set; }
    public bool canMove { get; set; } = true;

    private float desiredAngle;

    public void UpdatePos()
    {
        if (!canMove)
        {
            return;
        }
        Vector2 moveDIr = transform.right * moveInput;
        Vector2 desiredVelocity = moveDIr * stats.linearSpeed;

        if (Mathf.Abs(moveInput) <= 0.01f)
        {
            body.linearVelocity = Vector2.Lerp(body.linearVelocity, Vector2.zero, stats.linearDamp * Time.deltaTime);
        }
        else
        {

            float dampFactor = 1f - Mathf.Exp(-stats.linearDamp * Time.fixedDeltaTime);
            body.linearVelocity = Vector2.Lerp(body.linearVelocity, desiredVelocity, dampFactor);
        }
    }
    public void UpdateRotation()
    {
        if (!canMove)
        {
            return;
        }
        if (rotationInput != 0)
        {
            desiredAngle += rotationInput * stats.rotationSpeed * Time.deltaTime;
        }

        float dampFactor = 1f - Mathf.Exp(-stats.rotationDamp * Time.deltaTime);

        float currentY = body.rotation.eulerAngles.y;
        float smoothedY = Mathf.LerpAngle(currentY, -desiredAngle, dampFactor);

        float smoothedZ = Mathf.Sin(-smoothedY * Mathf.Deg2Rad) * 90f;

        Quaternion finalRotation = Quaternion.Euler(0, smoothedY, smoothedZ);
        body.MoveRotation(finalRotation);
    }
}
