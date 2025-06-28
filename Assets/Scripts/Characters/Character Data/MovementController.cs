using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementController : MonoBehaviour
{
    public CharacterBrain brain;
    private CharacterStats stats => brain.characterStats;
    public float moveInput { get; set; }
    public float rotationInput { get; set; }
    public bool sonarPingInput { get; set; }
    public bool flareInput { get; set; }
    public bool radialScanInput { get; set; }
    public bool canMove { get; set; } = true;

    private float desiredAngle;
    private float currentYAngle;

    private void Update()
    {
        UpdateRotation();
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (!canMove)
        {
            brain.body.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 moveDIr = transform.right * moveInput;
        Vector2 desiredVelocity = moveDIr * stats.linearSpeed;

        if (Mathf.Abs(moveInput) <= 0.01f)
        {
            brain.body.linearVelocity = Vector2.Lerp(brain.body.linearVelocity, Vector2.zero, stats.linearDamp * Time.deltaTime);
        }
        else
        {

            float dampFactor = 1f - Mathf.Exp(-stats.linearDamp * Time.fixedDeltaTime);
            brain.body.linearVelocity = Vector2.Lerp(brain.body.linearVelocity, desiredVelocity, dampFactor);
        }
    }
    public void UpdateRotation()
    {
        if (rotationInput != 0)
        {
            desiredAngle += rotationInput * stats.rotationSpeed * Time.deltaTime;
        }

        float dampFactor = 1f - Mathf.Exp(-stats.rotationDamp * Time.deltaTime);

        float currentY = brain.body.rotation.eulerAngles.y;
        float smoothedY = Mathf.LerpAngle(currentY, -desiredAngle, dampFactor);

        float smoothedZ = Mathf.Sin(-smoothedY * Mathf.Deg2Rad) * 90f;

        Quaternion finalRotation = Quaternion.Euler(0, smoothedY, smoothedZ);
        brain.body.MoveRotation(finalRotation);
    }
}
