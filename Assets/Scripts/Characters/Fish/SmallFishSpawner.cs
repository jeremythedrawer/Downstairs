using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallFishSpawner : ObjectPoolSpawner<SmallFish>
{
    public List<SmallFish> smallFishes = new List<SmallFish>();

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
            if (smallFishes.Count < maxSize)
            {
                SpawnJellyFish();
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnJellyFish()
    {
        if (pool == null) return;

        SmallFish smallFish = pool.Get();

        if (smallFish.spawner == null || smallFish.smallFishSpawner == null)
        {
            smallFish.spawner = this;
            smallFish.smallFishSpawner = this;
        }
        smallFishes.Add(smallFish);
        smallFish.transform.position = GetRandomPosition();
        smallFish.startPos = smallFish.transform.position;
        smallFish.gameObject.SetActive(true);
        smallFish.gameObject.transform.SetParent(this.transform);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawArea(Color.blue);
    }
#endif
}
