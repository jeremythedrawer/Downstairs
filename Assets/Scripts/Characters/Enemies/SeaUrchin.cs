using System.Collections;
using System.Linq;
using UnityEngine;

public class SeaUrchin : Enemy<SeaUrchin>
{
    public SeaUrchinShaderController shaderController {  get; private set; }
    private bool hasShot;
    private SeaUrchinSpike[] spikes;
    public LayerMask hitMask;

    public SeaUrchinSpawner seaUrchinSpawner { get; set; }
    private void Start()
    {
        shaderController = enemyShaderController as SeaUrchinShaderController;
        spikes = GetComponentsInChildren<SeaUrchinSpike>();
    }
    protected override void Update()
    {
        base.Update();
        Shoot();
        if (seaUrchinSpawner != null)
        {
            ReleaseToPool(this, () => seaUrchinSpawner.seaUrchins.Remove(this));
        }
        else
        {
            DestroyForever(this);
        }
    }

    private void Shoot()
    {
        if (!hasShot)
        {
            hasShot = true;
            StartCoroutine(Shooting());
        }

    }
    private IEnumerator Shooting()
    {
        foreach (SeaUrchinSpike spike in spikes)
        {
            if (spike.seaUrchin == null) spike.seaUrchin = this;
            spike.ReleaseSpikes();

        }

        while(shaderController.time < 1f)
        {
            shaderController.time += Time.deltaTime * 0.5f;
            yield return null;
        }
        shaderController.time = 0f;
        foreach (SeaUrchinSpike spike in spikes)
        {
            spike.ResizeSpikes();
        }

        yield return new WaitUntil(() => spikes.All(spike => spike.transform.localScale.magnitude >= Vector3.one.magnitude));
        hasShot = false;
    }


    

}
