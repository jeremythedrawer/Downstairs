using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyfishSpawner : ObjectPoolSpawner<Jellyfish>
{
    public static JellyfishSpawner Instance { get; private set; }

    public static List<Jellyfish> jellyfishes = new List<Jellyfish>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    protected override void Start()
    {
        base.Start();
        SpawnJellyFishes();
    }

    private void SpawnJellyFishes()
    {
        spawnActive = true;
        StartCoroutine(SpawningJellyFishes());
    }
    private IEnumerator SpawningJellyFishes()
    {
        while (spawnActive)
        {
            if (jellyfishes.Count < maxSize)
            {
                SpawnJellyFish();
            }
            else
            {
                Debug.Log("Jellyfish count has reached maximum size");
            }
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnJellyFish()
    {
        if (pool == null) return;

        Jellyfish jellyfish = pool.Get();
        if (jellyfish.spawner == null) jellyfish.spawner = this;
        jellyfishes.Add(jellyfish);
        jellyfish.transform.position = GetRandomPosition();
        jellyfish.gameObject.SetActive(true);
        jellyfish.gameObject.transform.SetParent(this.transform);
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(CameraController.minX, CameraController.maxX);
        float randomY = Random.Range(CameraController.minY, CameraController.maxY);

        return new Vector3(randomX, randomY, 0);
    }
}
