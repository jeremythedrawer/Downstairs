public abstract class Enemy<T> : EnemyBase where T : Enemy<T>
{
    public ObjectPoolSpawner<T> spawner;

    public delegate void OnDeath();
    protected void Death(T enemy, OnDeath onDeath = null)
    {
        if (currentHealth <= 0)
        {
            spawner.pool.Release(enemy);
            currentHealth = health;
            onDeath?.Invoke();
        }
    }
}
