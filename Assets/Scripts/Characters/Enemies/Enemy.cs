public abstract class Enemy<T> : EnemyBase where T : Enemy<T>
{
    public ObjectPoolSpawner<T> spawner;

    public delegate void OnDeath();
    protected void ReleaseToPool(T enemy, OnDeath onDeath = null)
    {
        if (currentHealth <= 0)
        {
            spawner.pool.Release(enemy);
            currentHealth = health;
            onDeath?.Invoke();
        }
    }

    protected void DestroyForever(T enemy)
    {
        if (currentHealth <= 0)
        {
            Destroy(enemy.gameObject);
        }
    }
}
