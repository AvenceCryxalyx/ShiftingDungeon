using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;

public class Inventory : MonoBehaviour
{
    public Action<Item> EvtItemPickedUp;
    public Action<Item> EvtItemDropped;
    public Action<Item> EvtItemRemoved;
    public Action<Item> EvtInventoryFull;
    public Action EvtItemsUpdated;
    [SerializeField]
    private InventorySO inventoryData;

    public int CurrentSlotCount { get { return items.Count; } }
    public int MaxSlotCount { get; private set; }
    public bool IsFull { get { return CurrentSlotCount >= MaxSlotCount; } }
    public IEnumerable<Item> Items { get { return items; } }
    protected List<Item> items = new List<Item>();

    private void Awake()
    {
        MaxSlotCount = inventoryData.MaxSlots;
    }

    public void UpdateMaxSlots(int maxCount)
    {
        MaxSlotCount = maxCount;
    }

    public int AddItem(Item item)
    {
        if (CurrentSlotCount == MaxSlotCount)
        {
            if(EvtInventoryFull != null)
            {
                EvtInventoryFull.Invoke(item);
            }
            return 1;
        }

        items.Add(item);
        Debug.Log(item);
        item.transform.parent = this.transform;

        if(EvtItemPickedUp != null)
        {
            EvtItemPickedUp.Invoke(item);
        }
        return 0;
    }

    public Item RemoveItem(Item item, bool wasDropped = true)
    {
        if(items.Contains(item))
        {
            items.Remove(item);
        }
        if(EvtItemDropped != null && wasDropped)
        {
            EvtItemDropped.Invoke(item);
        }
        else if(EvtItemRemoved != null && !wasDropped)
        {
            EvtItemRemoved.Invoke(item);
        }
        ItemsUpdated();
        return item;
    }

    public void ReplaceItem(Item item, Item newItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items[i] = newItem;
                newItem.transform.parent = this.transform;
            }
        }
        RemoveItem(item, false);
        item.PoolOrDestroy();
        ItemsUpdated();
    }

    public void ClearInventory(bool toDestroy = true)
    {
        if (toDestroy)
        {
            foreach (Item item in items)
            {
                item.PoolOrDestroy();
            }
        }
        items.Clear();
        ItemsUpdated();
    }

    private void ItemsUpdated()
    {
        if (EvtItemsUpdated != null)
        {
            EvtItemsUpdated.Invoke();
        }
    }
}
