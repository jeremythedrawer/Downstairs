using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolSpawner<T> : MonoBehaviour where T : Component
{
    protected ObjectPool<T> pool;
    [SerializeField] protected T prefab;
    public int maxSize = 50;
    [SerializeField] private int defaultSize = 5;

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

    public virtual T Spawn(Vector3 position, Quaternion rotation)
    {
        T obj = pool.Get();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public virtual void Release(T obj)
    {
        pool.Release(obj);
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
}
