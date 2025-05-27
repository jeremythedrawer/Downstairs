using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask hitMask;
    public BoxCollider boxCollilder;

    protected float posSpeed = 10f;

    protected bool hasHit;

    protected Vector3 halfExtents => Vector3.Scale(boxCollilder.size, transform.localScale) * 0.5f;
    protected Vector3 center => transform.position + boxCollilder.center;

    protected virtual void OnDisable()
    {
        hasHit = false;
    }

    protected virtual void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation, hitMask);
        if (hits.Length > 0) hasHit = true;
    }

}
