using UnityEngine;

public class LockedItem : Item
{
    public string KeyName { get; protected set; }

    private string openedObjectName;

    public override void Initialize(ItemDataSO data)
    {
        base.Initialize(data);
        LockedItemSO so = data as LockedItemSO;
        KeyName = so.Key.Name;
        openedObjectName = so.OpenedItem.name;
    }

    public bool Open(Item keyItem)
    {
        if (keyItem.Name != KeyName)
        {
            return false;
        }

        Inventory inventory = Player.instance.GetComponentInChildren<Inventory>();

        if (inventory != null) {
            Destroy(inventory.RemoveItem(keyItem, false).gameObject);
            Destroy(inventory.RemoveItem(this, false).gameObject);
            inventory.AddItem(ItemManager.instance.GetItem(openedObjectName));
        }
        return true;
    }
}