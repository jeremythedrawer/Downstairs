using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishSpawner : ObjectPoolSpawner<Jellyfish>
{

    public List<Jellyfish> jellyfishes = new List<Jellyfish>();

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
            if (jellyfishes.Count < maxSize)
            {
                SpawnJellyFish();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnJellyFish()
    {
        if (pool == null) return;

        Jellyfish jellyfish = pool.Get();
        if (jellyfish.spawner == null || jellyfish.jellyfishSpawner == null)
        {
            jellyfish.spawner = this;
            jellyfish.jellyfishSpawner = this;
        }
        jellyfishes.Add(jellyfish);
        jellyfish.transform.position = GetRandomPosition();
        jellyfish.startPos = jellyfish.transform.position;
        jellyfish.gameObject.SetActive(true);
        jellyfish.gameObject.transform.SetParent(transform);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawArea(Color.red);
    }
#endif
}
