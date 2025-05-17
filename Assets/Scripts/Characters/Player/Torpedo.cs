using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Torpedo : Projectile
{
    public BasicTimeShaderController shaderController;
    public BasicTimeShaderController explosionShaderController;
    public MeshRenderer torpedoRenderer;
    public BoxCollider explosionCollider;

    private float shaderSpeed = 1f;

    private void Update()
    {
        
    }
    public void UpdateTorpedo(ObjectPool<Torpedo> pool)
    {
        StartCoroutine(UpdatingTorpedo(pool));
    }
    private IEnumerator UpdatingTorpedo(ObjectPool<Torpedo> pool)
    {
        float elapsedTime = 0f;
        shaderController.time = 0f;
        Vector2 baseDir = PlayerBrain.Instance.currentDir.normalized;

        while (elapsedTime < Mathf.PI * 2 && !hasHit)
        {
            transform.position += (Vector3)(baseDir * posSpeed * Time.deltaTime);


            shaderController.time = 0.5f * (Mathf.Sin(elapsedTime * shaderSpeed) + 1f);

            elapsedTime += Time.deltaTime * 2f;

            yield return null;
        }

        if (hasHit)
        {
            torpedoRenderer.enabled = false;
            explosionCollider.enabled = true;
            elapsedTime = 0f;
            float explosionTime = 0.5f;

            while (elapsedTime < explosionTime)
            {
                elapsedTime += Time.deltaTime;
                explosionShaderController.time = elapsedTime / explosionTime;
                yield return null;
            }
            explosionShaderController.time = 0f;
        }
        yield return new WaitForEndOfFrame();
        explosionCollider.enabled = false;
        torpedoRenderer.enabled = true;
        shaderController.time = 0f;
        pool.Release(this);

    }
}