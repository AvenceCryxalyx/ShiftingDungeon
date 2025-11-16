using System.Collections.Generic;
using System;
using UnityEngine;

public class GatherPoint : MonoBehaviour
{
    [SerializeField]
    private int minAmountGatherable = 1;
    [SerializeField]
    private int maxAmountGatherable = 3;

    public Action<IEnumerable<Item>> EvtOnGathered;
    public Action<GatherPoint> EvtGatherCheck;

    private ItemGachaHandler m_ItemGachaHandler;

    public IEnumerable<Item> PossibleItems { get { return m_ItemGachaHandler.PossibleItems; } }

    private void Awake()
    {
        m_ItemGachaHandler = GetComponentInChildren<ItemGachaHandler>();
    }

    public void DoCheck()
    {
        if(EvtGatherCheck != null)
        {
            EvtGatherCheck.Invoke(this);
        }
        else
        {
            GetItemsFromPoint();
        }
    }

    public List<Item> GetItemsFromPoint()
    {
        Inventory playerInventory = Player.instance.GetComponentInChildren<Inventory>();

        int amount = UnityEngine.Random.Range(minAmountGatherable, maxAmountGatherable);
        List<Item> items = m_ItemGachaHandler.GetItems(amount);

        if(EvtOnGathered != null)
        {
            EvtOnGathered.Invoke(items);
        }

        foreach (Item item in items)
        {
            playerInventory.AddItem(item);
        }
        return items;
    }

}
