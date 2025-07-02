using UnityEngine;

public class Anglerfish : SolitaryFish
{
    protected override void OnEnable()
    {
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            GetNewPos();
        }
    }
}
