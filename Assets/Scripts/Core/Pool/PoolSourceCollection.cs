using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class PoolSourceCollection
{
    private HashSet<PoolSource> sources = new HashSet<PoolSource>();
    public IEnumerable<PoolSource> Sources { get { return sources; } }
    public Transform Container { get; private set; }

    public PoolSourceCollection(Transform container = null)
    {
        Container = container;
    }

    public PoolSource RegisterPrefab(Poolable prefab, int buffer = 0, Transform overrideContainer = null)
    {
        Assert.IsNotNull(prefab, "Prefab cannot be null");

        // Return cached pool source if it already exists
        PoolSource source = sources.Where(p => p.Prefab == prefab).SingleOrDefault();
        if (source != null) return source;

        Transform container = (overrideContainer == null) ? Container : overrideContainer;

        source = new PoolSource(prefab, buffer, container);
        sources.Add(source);
        return source;
    }

    public void RegisterSource(PoolSource poolSource)
    {
        Assert.IsNotNull(poolSource, "poolSource cannot be null");

        sources.Add(poolSource);
    }

    public void Pool(Poolable obj)
    {
        if (!sources.Where(p => p == obj.Source).Any()) throw new UnityException("Cannot pool object which is not registered in this controller");
        obj.Pool();
    }

    public Poolable GetObject(Poolable prefab)
    {
        // Look for the pool source with the prefab
        PoolSource source = sources.Where(p => p.Prefab == prefab).SingleOrDefault();
        if (source == null) throw new UnityException("Prefab is not registered in this controller");
        return source.GetObject();
    }

    public bool IsRegistered(Poolable prefab)
    {
        return sources.Where(p => p.Prefab == prefab).Any();
    }

    public T GetObject<T>(Poolable prefab) where T : Component
    {
        Poolable pooled = GetObject(prefab);
        T comp = pooled.GetComponent<T>();
        Assert.IsNotNull(comp, "Trying to get missing component from pooled object");

        return comp;
    }

    public T GetObject<T>(T prefab) where T : Component
    {
        if (prefab.GetComponent<Poolable>() == null)
        {
            Debug.LogWarning(prefab);
        }
        Poolable poolable = prefab.GetComponent<Poolable>();
        if (!poolable) throw new UnityException("Prefab doesn't contain Poolable component");

        return GetObject<T>(poolable);
    }

    public T GetObject<T>(GameObject prefab) where T : Component
    {
        return GetObject<T>(prefab.GetComponent<T>());
    }

    public void Clear()
    {
        foreach (PoolSource source in sources)
        {
            source.Clear();
        }
    }
}
