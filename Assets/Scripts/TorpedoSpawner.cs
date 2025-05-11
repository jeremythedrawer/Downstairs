using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TorpedoSpawner : MonoBehaviour
{
    [SerializeField] private Torpedo torpedoObject;

    public ObjectPool<Torpedo> torpedoPool;

    public List<Torpedo> torpedoes { get; private set; } = new List<Torpedo>();

    public int maxTorpedoSpawns { get; private set; } = 50;
    private int defaultTorpedoSpawns = 5;

    private void Start()
    {
        CreatePool(torpedoObject, this.transform, maxTorpedoSpawns, defaultTorpedoSpawns);
    }

    public void FireTorpedo()
    {

        if (torpedoPool == null) return;

        Torpedo torpedo = torpedoPool.Get();
        torpedo.transform.position = transform.position;
        torpedo.transform.rotation = transform.rotation;
        torpedo.gameObject.SetActive(true);
        torpedo.gameObject.transform.SetParent(null);
        torpedo.UpdateTorpedo(torpedoPool);
    }
    private void CreatePool(Torpedo torpedo, Transform spawnPos, int maxSpawns, int defaultSpawns)
    {
        torpedoPool = new ObjectPool<Torpedo>
            (
                () => CreatedTorpedo(torpedo, spawnPos),
                ActivateTorpedo,
                DeactivateTorpedo,
                DestroyTorpedo,
                false,
                defaultSpawns,
                maxSpawns
            );
    }
    private Torpedo CreatedTorpedo(Torpedo torpedo, Transform spawnPos)
    {
        return Instantiate(torpedo, spawnPos);
    }
    public void ActivateTorpedo(Torpedo torpedo)
    {
        torpedo.gameObject.SetActive(true);
    }
    private void DeactivateTorpedo(Torpedo torpedo)
    {
        torpedo.gameObject.SetActive(false);
    }
    private void DestroyTorpedo(Torpedo torpedo)
    {
        Destroy(torpedo.gameObject);
    }
}
