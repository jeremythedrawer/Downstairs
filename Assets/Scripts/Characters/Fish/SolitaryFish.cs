using UnityEngine;

public abstract class SolitaryFish : Fish
{
    public float closeToPlayerRadius;
    public MovementController movementController;
    public AudioSource audioSource;
    public SolitaryFishMaterialController materialController;
    public bool uncovered {  get; set; }

    private float rotThreshold = 0.9f;
    private float distThreshold = 0.3f;

    protected virtual void Update()
    {
        PulsingVolume();
    }
    protected void UpdateInputs()
    {
        Vector2 toTarget = targetPos - (Vector2)transform.position;
        float distToTarget = toTarget.magnitude;

        if (distToTarget > distThreshold)
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

    private void PulsingVolume()
    {
        if (!uncovered)
        {
            audioSource.volume = 1 - (Vector2.Distance(transform.position, PlayerBrain.instance.transform.position) / closeToPlayerRadius);
        }
        else
        {
            audioSource.Stop();
        }
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, closeToPlayerRadius);

    }
}
