using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveBoundRadius;

    public FishShaderController shaderController;
    public CharacterStats stats;
    public Vector2 startPos {  get; set; }
    protected Vector2 targetPos { get; set; }

    protected float distanceFromPlayer => Vector3.Distance(PlayerBrain.instance.transform.position, transform.position);
    protected virtual void OnEnable()
    {
        startPos = transform.position;
    }

    public void GetNewPos()
    {
        targetPos = (Random.insideUnitCircle * moveBoundRadius) + startPos;
    }

    private void OnDrawGizmosSelected()
    {

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, moveBoundRadius);
        DrawPath();
    }

    private void DrawPath()
    {
        Debug.DrawLine(transform.position, targetPos, Color.magenta);
    }

}
