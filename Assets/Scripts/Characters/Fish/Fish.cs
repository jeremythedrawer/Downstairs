using UnityEngine;

public class Fish : MonoBehaviour
{
    public float moveBoundRadius;

    public FishShaderController shaderController;
    public CharacterStats stats;
    public Vector2 startPos {  get; set; }
    protected Vector2 targetPos { get; set; }

    protected float distanceFromPlayer => (PlayerBrain.instance.transform.position - transform.position).sqrMagnitude;
    protected virtual void OnEnable()
    {
        startPos = transform.position;
        GetNewPos();
    }

    public void GetNewPos()
    {
        Vector2 currentDir = (targetPos - (Vector2)transform.position).normalized;
        Vector2 perp = new Vector2(-currentDir.y, currentDir.x);
        float angle = Random.Range(90f, 0f) * Mathf.Deg2Rad;
        Vector2 rotatedDir = Mathf.Cos(angle) * currentDir + Mathf.Sin(angle) * perp;
        targetPos = startPos + rotatedDir.normalized * moveBoundRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector2 pos = Application.isPlaying ? startPos : transform.position;
        Gizmos.DrawWireSphere(pos, moveBoundRadius);
        DrawPath();
    }

    private void DrawPath()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, targetPos);
        }
    }

}
