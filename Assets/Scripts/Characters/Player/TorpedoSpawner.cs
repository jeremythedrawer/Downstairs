using System.Collections;
using UnityEngine;

public class TorpedoSpawner : ObjectPoolSpawner<Torpedo>
{
    public TorpedoShaderController shockwaveShaderController;
    protected override void Start()
    {
       base.Start();
    }


    public void FireTorpedo()
    {
        if (pool == null) return;

        Torpedo torpedo = pool.Get();
        StartCoroutine(UpdatingShockwave());
        torpedo.transform.position = transform.position;
        torpedo.transform.rotation = transform.rotation;
        torpedo.gameObject.SetActive(true);
        torpedo.gameObject.transform.SetParent(null);
        torpedo.UpdateTorpedo(pool);
    }

    private IEnumerator UpdatingShockwave()
    {
        float shockwaveTime = 0.25f;
        float elapsedTime = 0f;

        while (elapsedTime < shockwaveTime)
        {
            elapsedTime += Time.deltaTime;
            shockwaveShaderController.time = elapsedTime / shockwaveTime;
            yield return null;
        }
        shockwaveShaderController.time = 0;
    }
}
