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
        Destroy(Player.instance.Inventory.RemoveItem(keyItem, false).gameObject);
        Destroy(Player.instance.Inventory.RemoveItem(this, false).gameObject);
        Player.instance.Inventory.AddItem(ItemManager.instance.GetItem(openedObjectName));
        return true;
    }
}