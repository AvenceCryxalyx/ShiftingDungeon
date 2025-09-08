using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class ItemManager : SimpleSingleton<ItemManager>
{
    [SerializeField]
    private Item DefaultItemPrefab;
    [SerializeField]
    private Consumables ConsumablesPrefab;
    [SerializeField]
    private LockedItem LockedItemPrefab;

    List<ItemDataSO> itemDatas = new List<ItemDataSO> ();
    List<Item> allItems = new List<Item> ();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        itemDatas = Resources.LoadAll<ItemDataSO>("Items/").ToList();
        LoadItems();
    }

    private void LoadItems()
    {
        foreach (var itemSo in itemDatas)
        {
            Item newItem;
            if (itemSo.OverridePrefab == null)
            {
                switch (itemSo.Type)
                {
                    case Item.ItemType.Consumables:
                        newItem = Instantiate(ConsumablesPrefab);
                        break;
                    default:
                        newItem = Instantiate(DefaultItemPrefab);
                        break;
                }
            }
            else
            {
                newItem = Instantiate(itemSo.OverridePrefab);
            }
            if (newItem != null)
            {
                newItem.gameObject.name = itemSo.Name;
                newItem.transform.parent = this.transform;
                newItem.Initialize(itemSo);
                allItems.Add(newItem);
            }
        }

    }

    public List<ItemDataSO> GetItemsOfType(Item.ItemType type)
    {
        return itemDatas.Where(x =>type.Equals(x.Type)).ToList();
    }

    public List<ItemDataSO> GetItemsOfRarity(Item.ItemRarity rarity)
    {
        return itemDatas.Where(x => rarity.Equals(x.Rarity)).ToList();
    }

    public Item GetItem(string name, bool shouldBeInstance = true)
    {
        ItemDataSO so = itemDatas.FirstOrDefault(x => x.name == name);

        if (shouldBeInstance)
        {
            if(so == null)
            {
                Debug.LogError("Can't find item: " + name);
            }

            Item item = Instantiate(allItems.FirstOrDefault(x => x.Name == so.Name), transform);
            item.Initialize(so);
            return item;
        }
        return allItems.FirstOrDefault(x => x.Name == so.Name);
    }
}
