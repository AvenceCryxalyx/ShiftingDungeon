using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public struct WeightedGachaMultiple : IJobParallelFor
{
    [ReadOnly] public NativeArray<float> cumulativeWeights;
    [ReadOnly] public NativeArray<int> itemIds;
    [ReadOnly] public uint randomSeed;
    [WriteOnly] public NativeArray<int> selectedItemIds;

    public void Execute(int index)
    {
        // Create a unique random generator for each thread
        var random = Unity.Mathematics.Random.CreateFromIndex((uint)(randomSeed + index));
        float randomValue = random.NextFloat(0f, cumulativeWeights[cumulativeWeights.Length - 1]);

        int selectedIndex = BinarySearchCumulativeWeights(randomValue);
        selectedItemIds[index] = itemIds[selectedIndex];
    }

    private int BinarySearchCumulativeWeights(float randomValue)
    {
        int left = 0;
        int right = cumulativeWeights.Length - 1;

        while (left < right)
        {
            int mid = (left + right) / 2;
            if (cumulativeWeights[mid] < randomValue)
                left = mid + 1;
            else
                right = mid;
        }

        return left;
    }
}