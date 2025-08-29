using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultScreenInventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    private Item item;
    public Action<Item> EvtClicked;
    public Action<Item> OnSold;
    public void Initialize(Item item)
    {
        this.item = item;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(EvtClicked != null)
        {
            EvtClicked.Invoke(item);
        }
    }

    public void Sell()
    {
        Player.instance.Inventory.RemoveItem(item);
        if (OnSold != null)
        {
            OnSold.Invoke(item);
        }
    }
}
