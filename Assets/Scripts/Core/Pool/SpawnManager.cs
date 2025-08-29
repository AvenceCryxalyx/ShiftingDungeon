using UnityEngine;

public class SpawnManager : SimpleSingleton<SpawnManager>
{
    public PoolSourceController PoolSource { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        PoolSource = GetComponentInChildren<PoolSourceController>();
    }

    public T GetSpawn<T>(GameObject prefab) where T : Component
    {
        if (prefab.GetComponent<Poolable>() == null)
        {
            Debug.LogWarning(prefab);
        }
        Poolable poolable = prefab.GetComponent<Poolable>();
        if (!poolable) throw new UnityException("Prefab doesn't contain Poolable component");

        return PoolSource.SourceCollection.GetObject(poolable).GetComponent<T>();
    }
}
