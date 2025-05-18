using System.Collections;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public LayerMask grabMask;
    public Transform endPos;
    public BoxCollider grabTrigger;
    public Transform leftClaw;
    public Transform rightClaw;


    private Vector3 startPos;
    private float extendTime = 2f;
    private float clawAngle = 30f;
    public bool usingClaw {  get; private set; }
    public bool grabbedSomething { get; set; }
    public bool releaseObject { get; private set; }

    private Collider currentGrabbedCollider;

    private FixedJoint joint;
    private void Update()
    {
        grabTrigger.enabled = usingClaw;
        GrabObject();

    }

    private void GrabObject()
    {
        if (usingClaw)
        {
            Collider[] hits = Physics.OverlapBox(grabTrigger.transform.position, grabTrigger.bounds.extents, Quaternion.identity, grabMask);
            if (hits.Length > 0)
            {
                leftClaw.localRotation = Quaternion.Euler(0, 0, -clawAngle);
                rightClaw.localRotation = Quaternion.Euler(0, 0, clawAngle);

                if (!releaseObject && currentGrabbedCollider == null)
                {
                    currentGrabbedCollider = hits[0];
                    Rigidbody grabbedRB = currentGrabbedCollider.attachedRigidbody;

                    if (grabbedRB != null)
                    {
                        if (joint != null) Destroy(joint);

                        grabbedRB.constraints = RigidbodyConstraints.None;

                        joint = PlayerBrain.Instance.body.gameObject.AddComponent<FixedJoint>();
                        joint.connectedBody = grabbedRB;
                        joint.breakForce = Mathf.Infinity;
                        joint.breakTorque = Mathf.Infinity;

                    }
                    grabbedSomething = true;
                }
            }
            else
            {
                leftClaw.localRotation = Quaternion.identity;
                rightClaw.localRotation = Quaternion.identity;
            }

            if (releaseObject && currentGrabbedCollider != null)
            {
                Debug.Log("releasing object");
                if (joint != null)
                {
                    Destroy(joint);
                    joint = null;
                }

                currentGrabbedCollider = null;
                grabbedSomething = false;
            }
        }
    }
    public void Extend()
    {
        StartCoroutine(Extending());
    }
    public void Retract()
    {
        if (Vector3.Distance(transform.localPosition, endPos.localPosition) < 0.01f)
        {
            StartCoroutine(Retracting());
        }
    }

    private IEnumerator Extending()
    {
        usingClaw = true;
        float elapsedTime = 0;
        startPos = transform.localPosition;

        while (elapsedTime < extendTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / extendTime;

            transform.localPosition = Vector3.Lerp(startPos, endPos.localPosition, t);
            yield return null;
        }
        transform.localPosition = endPos.localPosition;

    }

    private IEnumerator Retracting()
    {
        float elapsedTime = 0;
        releaseObject = true;
        grabbedSomething = false;
        while (elapsedTime < extendTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / extendTime;

            transform.localPosition = Vector3.Lerp(endPos.localPosition, startPos, t);
            yield return null;
        }

        transform.localPosition = startPos;
        usingClaw = false;
        releaseObject = false;

    }
}
