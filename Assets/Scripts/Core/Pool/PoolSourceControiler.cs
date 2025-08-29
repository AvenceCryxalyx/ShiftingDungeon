using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;

/// <summary>
/// PoolSourceController serves as an abstraction to a collection of PoolSource objects.
/// By any means, you can still pool an object without the presence of this script.
/// </summary>
public class PoolSourceController : MonoBehaviour
{
    [System.Serializable]
    public class PoolSourceData
    {
        public Poolable Prefab;
        public int Buffer = 0;
    }

    [SerializeField] private List<PoolSourceData> PreconfiguredPoolList = new List<PoolSourceData>();
    [SerializeField] private bool reparentOnPool;

    public PoolSourceCollection SourceCollection { get; private set; }

    void Awake()
    {
        Transform parent = null;
        if (reparentOnPool) parent = transform;

        SourceCollection = new PoolSourceCollection(parent);

        // Initialize pool data
        foreach (PoolSourceData data in PreconfiguredPoolList)
        {
            SourceCollection.RegisterPrefab(data.Prefab, data.Buffer);
        }
    }
}