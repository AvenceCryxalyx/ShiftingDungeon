using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractableItemDropsSO", menuName = "Scriptable Objects/InteractableItemDropsSO")]
public class InteractableItemDropsSO : ScriptableObject
{
    [Serializable]
    public struct InteractableItemDropRarity
    {
        public Item.ItemRarity Rarity;
        public InteractableItem DropItem;
    }
    public List<InteractableItemDropRarity> RarityDropLists;

    public InteractableItem GetDropItem(Item.ItemRarity rarity)
    {
        return SpawnManager.instance.GetSpawn<InteractableItem>(RarityDropLists[(int)rarity].DropItem.gameObject);
    }
}
