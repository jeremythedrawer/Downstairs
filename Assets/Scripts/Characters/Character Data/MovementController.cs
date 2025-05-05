using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public CharacterBrain brain;
    private CharacterStats stats => brain.characterStats;
    public Vector2 moveInput { get; set; }
    public bool canMove { get; set; }
    private void Update()
    {
        MoveWithInput();
    }

    private void FixedUpdate()
    {
        brain.body.MovePosition(brain.body.position + moveInput.normalized * stats.runSpeed * Time.fixedDeltaTime);
    }
    public void MoveWithInput()
    {

        Vector3 mouseWorldPos = CameraController.mainCam.ScreenToWorldPoint(Input.mousePosition);


        Vector2 lookDir = (mouseWorldPos - transform.position);


        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f ;

        brain.body.rotation = angle;
    }
}
