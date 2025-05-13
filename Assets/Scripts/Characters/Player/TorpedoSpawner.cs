public class TorpedoSpawner : ObjectPoolSpawner<Torpedo>
{
    protected override void Start()
    {
       base.Start();
    }


    public void FireTorpedo()
    {
        if (pool == null) return;

        Torpedo torpedo = pool.Get();
        torpedo.transform.position = transform.position;
        torpedo.transform.rotation = transform.rotation;
        torpedo.gameObject.SetActive(true);
        torpedo.gameObject.transform.SetParent(null);
        torpedo.UpdateTorpedo(pool);
    }
}
