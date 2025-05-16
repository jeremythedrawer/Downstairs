using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaUrchinSpawner : ObjectPoolSpawner<SeaUrchin>
{
    public static SeaUrchinSpawner Instance { get; private set; }

    public List<SeaUrchin> seaUrchins = new List<SeaUrchin>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        SpawnJellyFishes();
        CachePolygonData();
    }

    private void SpawnJellyFishes()
    {
        spawnActive = true;
        StartCoroutine(SpawningJellyFishes());
    }
    private IEnumerator SpawningJellyFishes()
    {
        yield return new WaitForEndOfFrame();
        while (spawnActive)
        {
            if (seaUrchins.Count < maxSize)
            {
                SpawnSeaUrchin();
            }
            else
            {
                Debug.Log("Sea Urchin count has reached maximum size");
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnSeaUrchin()
    {
        if (pool == null) return;

        SeaUrchin seaUrchin = pool.Get();
        if (seaUrchin.spawner == null || seaUrchin.seaUrchinSpawner == null)
        {
            seaUrchin.spawner = this;
            seaUrchin.seaUrchinSpawner = this;
        }
        seaUrchins.Add(seaUrchin);
        seaUrchin.transform.position = GetRandomPosition();
        seaUrchin.gameObject.SetActive(true);
        seaUrchin.gameObject.transform.SetParent(this.transform);
    }
    private void OnDrawGizmos()
    {
        DrawArea(Color.magenta);
    }
}
