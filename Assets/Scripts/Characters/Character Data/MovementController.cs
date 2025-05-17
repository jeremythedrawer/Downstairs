using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterBrain brain;
    private CharacterStats stats => brain.characterStats;
    public float moveInput { get; set; }
    public float rotationInput { get; set; }
    public bool grabInput { get; set; }
    public bool burstInput { get; set; }
    public bool canMove { get; set; } = true;

    private float desiredAngle;
    private float currentAngle;
    private void Update()
    {
        UpdateRotation();
    }

    private void FixedUpdate()
    {
        UpdatePos();
    }

    private void UpdatePos()
    {
        if (!canMove)
        {
            brain.body.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 moveDIr = transform.up * moveInput;
        Vector2 desiredVelocity = moveDIr * stats.linearSpeed;

        if (Mathf.Abs(moveInput) <= 0.01f)
        {
            brain.body.linearVelocity = Vector2.Lerp(brain.body.linearVelocity, Vector2.zero, stats.linearDamp * Time.fixedDeltaTime);
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

        currentAngle = Mathf.LerpAngle(brain.body.rotation.eulerAngles.z, desiredAngle, dampFactor);

        brain.body.MoveRotation(Quaternion.Euler(0f, 0f, currentAngle));
    }
}
