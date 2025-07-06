using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolSpawner<T> : MonoBehaviour where T : Component
{
    public ObjectPool<T> pool;
    [SerializeField] protected T prefab;
    public int maxSize = 50;
    [SerializeField] private int defaultSize = 5;
    protected bool spawnActive;

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
    protected virtual void Start()
    {
        CreatePool();
    }

    protected virtual void CreatePool()
    {
        pool = new ObjectPool<T>(
            CreateFunc,
            OnGet,
            OnRelease,
            DestroyPrefab,
            collectionCheck: false,
            defaultCapacity: defaultSize,
            maxSize: maxSize
        );
    }

    protected virtual T CreateFunc()
    {
        return Instantiate(prefab);
    }

    protected virtual void OnGet(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    protected virtual void OnRelease(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    protected virtual void DestroyPrefab(T obj)
    {
        Destroy(obj.gameObject);
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

        Vector2 finalPoint = RandomPointInTriangle(a, b, c);
        return new Vector3(finalPoint.x, finalPoint.y, transform.position.z);
    }

    protected float TriangleArea(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Abs((a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y)) * 0.5f);
    }

    protected Vector2 RandomPointInTriangle(Vector2 a, Vector2 b, Vector2 c)
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

    protected void CachePolygonData()
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

#if UNITY_EDITOR
    protected void DrawArea(Color color)
    {
        if (localPoints == null || localPoints.Length < 3) return;

        Gizmos.color = color;

        for (int i = 0; i < localPoints.Length; i++)
        {
            Vector3 current = transform.TransformPoint(localPoints[i]);
            Vector3 next = transform.TransformPoint(localPoints[(i + 1) % localPoints.Length]);
            Gizmos.DrawLine(current, next);
        }
    }
#endif
}
