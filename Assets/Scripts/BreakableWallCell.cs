using UnityEngine;

public class BreakableWallPart : MonoBehaviour
{
    private BreakableWall parentWall;

    private void Start()
    {
        parentWall = GetComponentInParent<BreakableWall>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Vector3 forceDirection = (transform.position - other.transform.position).normalized;
            parentWall?.BreakWall(forceDirection);
        }
    }
}