using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerfishSpawner : ObjectPoolSpawner<Anglerfish>
{
    public static AnglerfishSpawner Instance { get; private set; }

    public List<Anglerfish> anglerfishes = new List<Anglerfish>();
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        SpawnAnglerfishes();
        CachePolygonData();
    }

    private void SpawnAnglerfishes()
    {
        spawnActive = true;
        StartCoroutine(SpawningAnglerfishes());
    }
    private IEnumerator SpawningAnglerfishes()
    {
        yield return new WaitForEndOfFrame();
        while (spawnActive)
        {
            if (anglerfishes.Count < maxSize)
            {
                SpawnAnglerfish();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnAnglerfish()
    {
        if (pool == null) return;

        Anglerfish anglerfish = pool.Get();
        if (anglerfish.spawner == null || anglerfish.anglerfishSpawner == null)
        {
            anglerfish.spawner = this;
            anglerfish.anglerfishSpawner = this;
        }
        anglerfishes.Add(anglerfish);
        Vector3 randomPos = GetRandomPosition();
        anglerfish.transform.position = randomPos;
        anglerfish.gameObject.SetActive(true);
        anglerfish.gameObject.transform.SetParent(this.transform);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawArea(Color.green);
    }
#endif
}
