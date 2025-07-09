using System.Collections;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Rigidbody body;
    public CharacterStats stats;
    public float moveInput { get; set; }
    public float rotationInput { get; set; }
    public bool canMove { get; set; } = true;

    public Transform rotationPivot;
    private float desiredAngle;
    public void UpdatePos()
    {
        if (!canMove)
        {
            body.linearVelocity = Vector2.zero;
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

        desiredAngle += rotationInput * stats.rotationSpeed * Time.deltaTime;

        float currentZ = body.rotation.eulerAngles.z;

        float dampFactor = 1f - Mathf.Exp(-stats.rotationDamp * Time.deltaTime);
        float smoothedZ = Mathf.LerpAngle(currentZ, desiredAngle, dampFactor);

        Quaternion newBodyRot = Quaternion.AngleAxis(smoothedZ, Vector3.forward);
        body.MoveRotation(newBodyRot);

        float pivotAngle = smoothedZ - currentZ;
        float pivotDirection = Mathf.Sign(Vector3.Dot(transform.right, Vector3.down));
        rotationPivot.Rotate(Vector3.right, pivotAngle * pivotDirection);
    } 
}
