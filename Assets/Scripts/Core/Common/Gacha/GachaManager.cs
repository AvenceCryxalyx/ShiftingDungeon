using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;

public class Gacha
{
    private NativeArray<float> cumulativeWeights;
    private NativeArray<int> itemIds;
    private Dictionary<int, string> itemIdToResultId;
    private bool isInitialized = false;

    public Gacha(WeightedInfo[] list)
    {
        InitializeGachaSystem(list);
    }

    private void InitializeGachaSystem(WeightedInfo[] gachaList)
    {
        if (gachaList == null || gachaList.Length == 0)
        {
            Debug.LogError("No gacha items configured!");
            return;
        }

        // Create native arrays
        cumulativeWeights = new NativeArray<float>(gachaList.Length, Allocator.Persistent);
        itemIds = new NativeArray<int>(gachaList.Length, Allocator.Persistent);
        itemIdToResultId = new Dictionary<int, string>();

        // Calculate cumulative weights
        float totalWeight = 0f;
        for (int i = 0; i < gachaList.Length; i++)
        {
            totalWeight += gachaList[i].weight;
            cumulativeWeights[i] = totalWeight;
            itemIds[i] = i;
            itemIdToResultId[i] = gachaList[i].id;
        }

        isInitialized = true;
        Debug.Log($"Gacha system initialized with {gachaList.Length} items, total weight: {totalWeight}");
    }

    // Single pull method
    public string PullSingle()
    {
        if (!isInitialized) return null;

        var randomValues = new NativeArray<float>(1, Allocator.TempJob);
        var selectedIds = new NativeArray<int>(1, Allocator.TempJob);

        randomValues[0] = UnityEngine.Random.Range(0f, cumulativeWeights[cumulativeWeights.Length - 1]);

        var job = new WeightedGachaSingle
        {
            cumulativeWeights = cumulativeWeights,
            itemIds = itemIds,
            randomValues = randomValues,
            selectedItemIds = selectedIds
        };

        var handle = job.Schedule();
        handle.Complete();

        int selectedId = selectedIds[0];
        string result = itemIdToResultId[selectedId];

        randomValues.Dispose();
        selectedIds.Dispose();

        return result;
    }

    // Multiple pulls method (more efficient for batch operations)
    public List<string> PullMultiple(int count)
    {
        if (!isInitialized) return new List<string>();

        var selectedIds = new NativeArray<int>(count, Allocator.TempJob);
        uint seed = (uint)UnityEngine.Random.Range(1, int.MaxValue);

        var batchJob = new WeightedGachaMultiple
        {
            cumulativeWeights = cumulativeWeights,
            itemIds = itemIds,
            randomSeed = seed,
            selectedItemIds = selectedIds
        };

        var handle = batchJob.Schedule(count, 64); // Process in batches of 64
        handle.Complete();

        var results = new List<string>(count);
        for (int i = 0; i < count; i++)
        {
            results.Add(itemIdToResultId[selectedIds[i]]);
        }

        selectedIds.Dispose();
        return results;
    }
}