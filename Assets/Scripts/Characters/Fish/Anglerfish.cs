using UnityEngine;

public class Anglerfish : SolitaryFish
{
    protected override void OnEnable()
    {
        base.OnEnable();
        GetNewPos();
    }
    protected override void Update()
    {
        base.Update();
        if (distanceFromPlayer > 30f)
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
