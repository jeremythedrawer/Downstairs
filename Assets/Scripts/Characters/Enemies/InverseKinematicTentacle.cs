using UnityEditor;
using UnityEngine;

public class InverseKinematicTentacle : MonoBehaviour
{
    public Transform target;
    public int chainLength = 2;
    public int iterations = 10;
    public float delta = 0.01f;
    public float snapBackStrength = 10f;
    public bool seeBones;

    private float[] bonesLength;
    private float completeLength;
    private Transform[] bones;
    private Vector3[] positions;
    private Vector3[] startDir;
    private Quaternion[] startRotBone;
    private Quaternion startRotTarget;
    private Quaternion startRotRoot;

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        ResolveIK();
    }
    private void Init()
    {
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDir = new Vector3[chainLength + 1];
        startRotBone = new Quaternion[chainLength + 1];

        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            target.position = transform.position;
        }
        startRotTarget = target.rotation;

        completeLength = 0;

        Transform current = transform;
        for (int i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotBone[i] = current.rotation;

            if (i == bones.Length - 1)
            {
                startDir[i] = target.position - current.position;
            }
            else
            {
                startDir[i] = bones[i + 1].position - current.position;
                bonesLength[i] = startDir[i].magnitude;
                completeLength += bonesLength[i];
            }

            startRotBone[i] = current.rotation;

            current = current.parent;
        }

        startRotRoot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
    }


    private void ResolveIK()
    {
        if (target == null) return;

        if (bonesLength.Length != chainLength)
        {
            Init();
        }

        //get position
        for (int i = 0; i < bones.Length; i++)
        {
            positions[i] = bones[i].position;
        }

        Quaternion rootRot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        Quaternion rootRotDiff = rootRot * Quaternion.Inverse(startRotRoot);

        if ((target.position - bones[0].position).sqrMagnitude >= completeLength * completeLength)
        {
            Vector3 direction = (target.position - positions[0]).normalized;

            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
            }
        }
        else
        {

            for (int i = 0; i < positions.Length - 1; i++)
            {
                positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + rootRotDiff * startDir[i], snapBackStrength);
            }
            for (int i = 0; i < iterations; i++)
            {
                //backwards
                for (int j = positions.Length - 1; j > 0; j--)
                {
                    if (j == positions.Length - 1)
                    {
                        positions[j] = target.position;
                    }
                    else
                    {
                        positions[j] = positions[j + 1] + (positions[j] - positions[j + 1]).normalized * bonesLength[j];
                    }
                }

                //fowards
                for (int j = 1; j < positions.Length; j++)
                {
                    positions[j] = positions[j - 1] + (positions[j] - positions[j - 1]).normalized * bonesLength[j - 1];
                }

                //close enough
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < delta * delta) break;
            }
        }


        //set position & rotation
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == positions.Length - 1)
            {
                bones[i].rotation = target.rotation * Quaternion.Inverse(startRotTarget) * startRotBone[i];
            }
            else
            {
                bones[i].rotation = Quaternion.FromToRotation(startDir[i], positions[i + 1] - positions[i]) * startRotBone[i];
            }
            bones[i].position = positions[i];
        }
    }
    private void OnDrawGizmos()
    {
        if (seeBones)
        {
            Transform current = this.transform;
            for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
            {
                float scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
                Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
                Handles.color = Color.magenta;
                Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
                current = current.parent;
            }
        }
    }
}
