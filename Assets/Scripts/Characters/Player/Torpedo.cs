using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Torpedo : MonoBehaviour
{
    public TorpedoShaderController shaderController;
    public float sprayFactor = 5f;
    public float hitPoints = 5f;
    private float posSpeed = 10f;
    private float shaderSpeed = 1f;
    private float timeElapsed = 0f;

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

        while (timeElapsed < Mathf.PI * 2)
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