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
        public ItemDrop DropItem;
    }
    public List<InteractableItemDropRarity> RarityDropLists;

    public ItemDrop GetDropItem(Item.ItemRarity rarity)
    {
        return SpawnManager.instance.GetSpawn<ItemDrop>(RarityDropLists[(int)rarity].DropItem.gameObject);
    }
}
