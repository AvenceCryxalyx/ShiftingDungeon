using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Linq;

public class Poolable : MonoBehaviour
{
    public PoolSource Source { get; private set; }
    public bool Bound { get { return Source != null; } }

    public bool Pooled { get; set; }
    public void BindSource(PoolSource source)
    {
        Assert.IsFalse(Bound, "Poolable can only be bound once. You need to instantiate a new object and rebind it.");
        Source = source;
    }

    public void Pool()
    {
        Assert.IsTrue(Bound, "Poolable object not bound to a PoolSource");
        OnPooled();
        Source.Pool(this);
    }

    protected virtual void OnPooled() { }
}

public static class PoolableExtension
{
    public static bool Pool(this GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable && poolable.Bound)
        {
            poolable.Pool();
            return true;
        }

        return false;
    }

    public static bool Pool(this Component comp)
    {
        return Pool(comp.gameObject);
    }

    public static void PoolOrDestroy(this GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable && poolable.Bound)
        {
            poolable.Pool();
        }
        else
        {
            Debug.Log("Destroyed");
            Object.Destroy(obj);
        }
    }

    public static void PoolOrDestroy(this Component comp)
    {
        PoolOrDestroy(comp.gameObject);
    }

    public static bool IsPoolable(this GameObject obj)
    {
        Poolable poolable = obj.GetComponent<Poolable>();
        if (!poolable) return false;

        return poolable.Bound;
    }
}