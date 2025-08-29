using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;

public class DebugMenu : MonoBehaviour
{
    #region Debug
    [SerializeField] private GameObject menuUI;
    #endregion

    #region Inventory Debug
    [SerializeField, ValueDropdown("GetListOfItemsSO")] private ItemDataSO ItemToAdd;
    [SerializeField, ValueDropdown("GetListOfItemsSO")] private ItemDataSO ItemToRemove;
    [SerializeField, ValueDropdown("GetListOfItemsSO")] private ItemDataSO ItemToReplace;
    [SerializeField, ValueDropdown("GetListOfKeys")] private ItemDataSO KeyItemToUse;

    public void AddItem()
    {
        Player.instance.Inventory.AddItem(ItemManager.instance.GetItem(ItemToAdd.name));
    }

    public void RemoveItem()
    {
        Player.instance.Inventory.RemoveItem(ItemManager.instance.GetItem(ItemToRemove.name));
    }

    public void TryOpenChestsInInventory()
    {
        Item key = ItemManager.instance.GetItem(KeyItemToUse.name);
        Player.instance.Inventory.AddItem(key);
        foreach (Item item in Player.instance.Inventory.Items)
        {
            if(item is LockedItem)
            {
                LockedItem locked = (LockedItem)item;
                locked.Open(key);
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if(!menuUI.activeSelf)
            {
                menuUI.SetActive(true);
            }
            else
            {
                menuUI.SetActive(false);
            }
        }
    }

    public static IEnumerable GetListOfItemsSO()
    {
        return Resources.LoadAll<ItemDataSO>("Items");
    }

    public static IEnumerable GetListOfKeys()
    {
        return Resources.LoadAll<ItemDataSO>("Items").Where(x => x.Type == Item.ItemType.Key);
    }

    #endregion
}