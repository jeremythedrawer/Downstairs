using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float explosionForce = 500f;
    private bool hasBroken = false;

    Rigidbody[] childRigidBodies;
    Collider[] childColliders;

    private void Start()
    {
        childRigidBodies = GetComponentsInChildren<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();
    }

    public void BreakWall(Vector3 forceDirection)
    {
        if (hasBroken) return;
        hasBroken = true;

        Debug.Log("torpedo hit");

        Vector3 explosionOrigin = transform.position;  // center of explosion
        float explosionRadius = 5f;                    // tweak this radius as needed
        float upwardsModifier = 1f;

        foreach (Rigidbody childRB in childRigidBodies)
        {
            childRB.isKinematic = false;
            childRB.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }

        foreach (Collider childColl in childColliders)
        {
            childColl.enabled = false;
        }
    }
}