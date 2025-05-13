using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerfishSpawner : ObjectPoolSpawner<Anglerfish>
{
    public static AnglerfishSpawner Instance { get; private set; }

    public static List<Anglerfish> anglerfishes = new List<Anglerfish>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        SpawnAnglerfishes();
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
                SpawnJellyFish();
            }
            else
            {
                Debug.Log("Anglerfish count has reached maximum size");
            }
            yield return new WaitForSeconds(5f);
        }
    }
    public void SpawnJellyFish()
    {
        if (pool == null) return;

        Anglerfish anglerfish= pool.Get();
        if (anglerfish.spawner == null) anglerfish.spawner = this;
        anglerfishes.Add(anglerfish);
        anglerfish.transform.position = GetRandomPosition();
        anglerfish.gameObject.SetActive(true);
        anglerfish.gameObject.transform.SetParent(this.transform);
    }

    private Vector3 GetRandomPosition()
    {

        int CameraSide = Random.Range(0, 4);

        float x = 0f;
        float y = 0f;

        switch (CameraSide)
        {
            case 0:
            {
                x = Random.Range(CameraController.minX, CameraController.maxX);
                y= CameraController.maxY;
            }
            break;

            case 1:
            {
                x = Random.Range(CameraController.minX, CameraController.maxX);
                y = CameraController.minY;
            }
            break;

            case 2:
            {
                x =  CameraController.minX;
                y = Random.Range(CameraController.minY, CameraController.maxY);
            }
            break;

            case 3:
            {
                x = CameraController.maxX;
                y = Random.Range(CameraController.minY, CameraController.maxY);
            }
            break;
        }

        return new Vector3(x, y, 0);
    }
}
