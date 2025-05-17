using System.Collections;
using UnityEngine;

public class BreakableWallPart : MonoBehaviour
{
    private Rigidbody body;
    private bool grabbed;
    private Transform parent;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        parent = transform.parent;
    }

    private void Update()
    {
        if (grabbed)
        {
            if (PlayerBrain.Instance.armController.releaseObject)
            {
                transform.parent = parent;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Grab"))
        {
            transform.parent = other.transform;
            grabbed = true;
        }
    }
}