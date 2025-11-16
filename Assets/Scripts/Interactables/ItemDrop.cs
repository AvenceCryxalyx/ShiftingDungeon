using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;

public class ItemDrop : Interactable
{
    [SerializeField]
    private Item item;

    public Action<ItemDrop> EvtOnTaken;

    private void Awake()
    {
        InteractText = "Pick up";
    }

    public void SetItem(Item item)
    {
        if(this.item == null)
        {
            this.item = item;
            this.item.transform.SetParent(this.transform);
        }
        else
        {
            Debug.LogWarning("Trying to overwrite the item assigned to drop object");
        }
    }

    private void OnDisable()
    {
        item = null;
    }

    public int Interact(PlayerUnitController controller)
    {
        if (item == null)
        {
            return 1;
        }
        int res = Player.instance.GetComponentInChildren<Inventory>().AddItem(item);
        if (res == 0)
        {
            gameObject.PoolOrDestroy();
            if (EvtOnTaken != null)
            {
                EvtOnTaken.Invoke(this);
            }
        }
        return 0;
    }
}
