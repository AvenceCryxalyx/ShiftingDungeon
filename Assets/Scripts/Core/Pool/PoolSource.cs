using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class PoolSource
{
    public Poolable Prefab { get; private set; }
    public Transform Container { get; private set; }

    public string Guid { get; private set; }

    public IEnumerable<Poolable> PooledObjects { get { return pooledObjects; } }
    private HashSet<Poolable> pooledObjects = new HashSet<Poolable>();

    public PoolSource(Poolable prefab, int buffer, Transform container = null)
    {
        Assert.IsNotNull(prefab);

        Prefab = prefab;
        Container = container;
        Guid = System.Guid.NewGuid().ToString();

        // Buffer
        for (int i = 0; i < buffer; i++)
        {
            Poolable obj = GameObject.Instantiate<Poolable>(Prefab, container);
            obj.BindSource(this);

            // Add to pool
            obj.Pooled = true;
            pooledObjects.Add(obj);
            obj.SendMessage("OnPoolSleep", SendMessageOptions.DontRequireReceiver);
            obj.gameObject.SetActive(false);
            if (Container) obj.transform.SetParent(Container, false);
            obj.gameObject.name += " (Pooled)";
        }
    }

    ~PoolSource()
    {
        Clear();
    }

    public Poolable GetObject()
    {
        Poolable pooled = pooledObjects.FirstOrDefault();

        // Create new object if pool is empty
        if (pooled == null)
        {
            pooled = GameObject.Instantiate<Poolable>(Prefab);
            pooled.BindSource(this);
        }
        // Remove object from pool if found
        else pooledObjects.Remove(pooled);

        pooled.gameObject.name = Prefab.gameObject.name;
        pooled.gameObject.SetActive(true);
        pooled.Pooled = false;

        pooled.SendMessage("OnPoolAwake", SendMessageOptions.DontRequireReceiver);

        return pooled;
    }

    public T GetObject<T>() where T : Component
    {
        Poolable pooled = GetObject();

        T comp = pooled.GetComponent<T>();
        Assert.IsNotNull(comp, "Trying to get missing component from pooled object");

        return comp;
    }

    public void Pool(Poolable obj)
    {
        Assert.IsTrue(obj.Source == this, "Cannot pool object that has different PoolSource");
        Assert.IsFalse(pooledObjects.Contains(obj), "Object is already pooled!");

        // Add to pool
        obj.Pooled = true;
        pooledObjects.Add(obj);
        obj.SendMessage("OnPoolSleep", SendMessageOptions.DontRequireReceiver);
        obj.gameObject.SetActive(false);
        if (Container) obj.transform.SetParent(Container, false);
        obj.gameObject.name += " (Pooled)";
    }

    /// <summary>
    /// Destroys all pooled objects. Must be explicitly called from Unity's API 
    /// </summary>
    public void Clear()
    {
        foreach (Poolable pool in pooledObjects)
        {
            UnityEngine.Object.Destroy(pool.gameObject);
        }
        pooledObjects.Clear();
    }
}
