public abstract class SchoolFish<T> : Fish where T : SchoolFish<T>
{
    public ObjectPoolSpawner<T> spawner;
}
