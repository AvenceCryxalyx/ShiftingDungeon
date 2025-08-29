using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
public struct WeightedGachaSingle : IJob
{
    [ReadOnly] public NativeArray<float> cumulativeWeights;
    [ReadOnly] public NativeArray<int> itemIds;
    [ReadOnly] public NativeArray<float> randomValues;
    [WriteOnly] public NativeArray<int> selectedItemIds;

    public void Execute()
    {
        for (int i = 0; i < randomValues.Length; i++)
        {
            float randomValue = randomValues[i];
            int selectedIndex = BinarySearchCumulativeWeights(randomValue);
            selectedItemIds[i] = itemIds[selectedIndex];
        }
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

