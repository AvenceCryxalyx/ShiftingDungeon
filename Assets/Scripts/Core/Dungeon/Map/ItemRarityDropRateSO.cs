using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemRarityDropRateSO", menuName = "Scriptable Objects/ItemRarityDropRateSO")]
public class ItemRarityDropRateSO : ScriptableObject
{
    [Serializable]
    public struct RarityDropRate
    {
        public InteractableItem Drop;
        public int weight;
    }    
    public List<RarityDropRate> ItemDrops;
}
