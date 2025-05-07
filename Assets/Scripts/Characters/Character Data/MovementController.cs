using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterBrain brain;
    private CharacterStats stats => brain.characterStats;
    public Vector2 moveInput { get; set; }
    public bool shootInput;
    public bool canMove { get; set; } = true;

    private float desiredAngle;
    private float currentAngle;
    private void Update()
    {
        MoveWithInput();
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

        Vector2 desiredVelocity = moveInput.normalized * stats.runSpeed;

        if (moveInput.sqrMagnitude <= 0.01f)
        {
            brain.body.linearVelocity = Vector2.Lerp(brain.body.linearVelocity, Vector2.zero, stats.damp * Time.fixedDeltaTime);
        }
        else
        {
            float dampFactor = 1f - Mathf.Exp(-stats.damp * Time.fixedDeltaTime);
            brain.body.linearVelocity = Vector2.Lerp(brain.body.linearVelocity, desiredVelocity, dampFactor);
        }
    }
    public void MoveWithInput()
    {

        Vector3 mouseWorldPos = CameraController.mainCam.ScreenToWorldPoint(Input.mousePosition);


        Vector2 lookDir = (mouseWorldPos - transform.position);


        desiredAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        float dampFactor = 1f - Mathf.Exp(-stats.damp * Time.deltaTime);

        currentAngle = Mathf.LerpAngle(brain.body.rotation, desiredAngle, dampFactor);

        brain.body.rotation = currentAngle;
    }
}
