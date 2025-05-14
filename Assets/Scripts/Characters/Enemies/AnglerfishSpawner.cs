using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnglerfishSpawner : ObjectPoolSpawner<Anglerfish>
{
    public static AnglerfishSpawner Instance { get; private set; }

    public List<Anglerfish> anglerfishes = new List<Anglerfish>();


    public Vector2[] localPoints = new Vector2[4]
    {
        new Vector2(-1, -1),
        new Vector2(-1, 1),
        new Vector2(1, 1),
        new Vector2(1, -1)
    };

    private Vector2[] worldPoints;
    private float[] cumulativeAreas;
    private float totalArea;
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
            else
            {
                Debug.Log("Anglerfish count has reached maximum size");
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
        Debug.Log(randomPos);
        anglerfish.transform.position = randomPos;
        anglerfish.gameObject.SetActive(true);
        anglerfish.gameObject.transform.SetParent(this.transform);
    }

    public Vector3 GetRandomPosition()
    {
        if (cumulativeAreas == null || cumulativeAreas.Length == 0) return transform.position;

        float r = Random.value * totalArea;
        int triIndex = 0;
        for (int i = 0; i < cumulativeAreas.Length; i++)
        {
            if (r <= cumulativeAreas[i])
            {
                triIndex = i;
                break;
            }
        }

        Vector2 a = worldPoints[0];
        Vector2 b = worldPoints[triIndex + 1];
        Vector2 c = worldPoints[triIndex + 2];

        return RandomPointInTriangle(a, b, c);
    }

    private float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Abs((a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) * 0.5f);
    }

    private Vector2 RandomPointInTriangle(Vector2 a, Vector2 b, Vector2 c)
    {
        float u = Random.value;
        float v = Random.value;

        if (u + v > 1)
        {
            u = 1 - u;
            v = 1 - v;
        }

        return a + u * (b - a) + v * (c - a);
    }

    private void CachePolygonData()
    {
        int len = localPoints.Length;
        if (len < 3) return;

        worldPoints = new Vector2[len];
        for (int i = 0; i < len; i++)
        {
            worldPoints[i] = transform.TransformPoint(localPoints[i]);
        }

        cumulativeAreas = new float[len - 2];
        totalArea = 0;
        for (int i = 0; i < len - 2; i++)
        {
            float area = TriangleArea(worldPoints[0], worldPoints[i + 1], worldPoints[i + 2]);
            totalArea += area;
            cumulativeAreas[i] = totalArea;
        }
    }
    private void OnDrawGizmos()
    {
        if (localPoints == null || localPoints.Length < 3) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < localPoints.Length; i++)
        {
            Vector3 current = transform.TransformPoint(localPoints[i]);
            Vector3 next = transform.TransformPoint(localPoints[(i + 1) % localPoints.Length]);
            Gizmos.DrawLine(current, next);
        }
    }
}
