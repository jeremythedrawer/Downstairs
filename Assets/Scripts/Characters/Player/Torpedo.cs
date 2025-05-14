using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Torpedo : MonoBehaviour
{
    public TorpedoShaderController shaderController;
    public LayerMask hitMask;
    public BoxCollider boxCollilder;

    public float sprayFactor = 5f;
    public float hitPoints = 5f;
    private float posSpeed = 10f;
    private float shaderSpeed = 1f;
    private float timeElapsed = 0f;

    private bool hasHit;

    Vector3 halfExtents;
    Vector3 center => transform.position + boxCollilder.center;
    private void Start()
    {
        halfExtents = Vector3.Scale(boxCollilder.size, transform.localScale) * 0.5f;
    }

    private void OnDisable()
    {
        hasHit = false;
    }
    private void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation, hitMask);
        if (hits.Length > 0 ) hasHit = true;
    }
    public void UpdateTorpedo(ObjectPool<Torpedo> pool)
    {
        StartCoroutine(UpdatingTorpedo(pool));
    }
    private IEnumerator UpdatingTorpedo(ObjectPool<Torpedo> pool)
    {
        shaderController.time = 0f;
        float angleOffset = Random.Range(-sprayFactor, sprayFactor);
        Vector2 baseDir = PlayerBrain.Instance.currentDir.normalized;
        Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * baseDir;

        while (timeElapsed < Mathf.PI * 2 && !hasHit)
        {
            transform.position += (Vector3)(direction * posSpeed * Time.deltaTime);


            shaderController.time = 0.5f * (Mathf.Sin(timeElapsed * shaderSpeed) + 1f);

            timeElapsed += Time.deltaTime * 2f;

            yield return null;
        }

        pool.Release(this);

        timeElapsed = 0f;
        shaderController.time = 0f;
    }
}