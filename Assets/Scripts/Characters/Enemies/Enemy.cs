public abstract class Enemy<T> : EnemyBase where T : Enemy<T>
{
    public ObjectPoolSpawner<T> spawner;
}
