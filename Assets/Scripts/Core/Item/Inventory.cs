using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;

public class Inventory : MonoBehaviour
{
    public Action<Item> EvtItemPickedUp;
    public Action<Item> EvtItemDropped;
    public Action<Item> EvtInventoryFull;

    public int CurrentSlotCount { get { return Items.Count; } }
    public int MaxSlotCount { get; private set; }
    public bool IsFull { get { return CurrentSlotCount >= MaxSlotCount; } }

    protected List<Item> Items = new List<Item>();

    public void UpdateMaxSlots(int maxCount)
    {
        MaxSlotCount = maxCount;
    }

    public int AddItem(Item item)
    {
        if(CurrentSlotCount == MaxSlotCount)
        {
            EvtInventoryFull.Invoke(item);
            return 1;
        }
        Items.Add(item);

        if(EvtItemPickedUp != null)
        {
            EvtItemPickedUp.Invoke(item);
        }
        return 0;
    }

    public Item RemoveItem(Item item)
    {
        if(Items.Contains(item))
        {
            Items.Remove(item);
        }
        if(EvtItemDropped != null)
        {
            EvtItemDropped.Invoke(item);
        }
        return item;
    }

    public void SwapItems(int index1, int index2)
    {
        if(Items.Count < index1 || Items.Count < index2 || Items.Count == 0)
        {
            return;
        }

        if (Items[index1] != null && Items[index2] != null)
        {
            Item item = Items[index1];
            Items[index1] = Items[index2];
            Items[index2] = item;
        }
    }

    public void ClearInventory(bool toDestroy = true)
    {
        if (toDestroy)
        {
            foreach (Item item in Items)
            {
                Destroy(item);
            }
        }
        Items.Clear();
    }
}
