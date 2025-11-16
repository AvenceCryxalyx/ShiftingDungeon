using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct ItemDropRates
{
    public ItemDataSO Item;
    public int Weight;
}

public class ItemGachaHandler : MonoBehaviour
{
    [SerializeField]
    private ItemGachaSO gachaSO;

    public IEnumerable<Item> PossibleItems { get { return possibleItems.Values; } }

    private Dictionary<string, Item> possibleItems = new Dictionary<string, Item>();
    private Gacha Gacha;

    #region Unity Mothods
    private void Awake()
    {
        List<WeightedInfo> weightedInfos = new List<WeightedInfo>();
        foreach (ItemDropRates rates in gachaSO.DropRates)
        {
            possibleItems.Add(rates.Item.name, ItemManager.instance.GetItem(rates.Item.Name));
            WeightedInfo newInfo;
            newInfo.id = rates.Item.name;
            newInfo.weight = rates.Weight;
            weightedInfos.Add(newInfo);
        }
        Gacha = new Gacha(weightedInfos.ToArray());
    }
    #endregion

    #region Public Methods
    public List<Item> GetItems(int amount)
    {
        List<Item> list = new List<Item>();
        if(amount > 1)
        {

            foreach(string id in Gacha.PullMultiple(amount))
            {
                Item item = list.First(x => x.SO.name == id);
                if (item == null)
                {
                    item = ItemManager.instance.GetItem(id, true);
                    item.transform.SetParent(this.transform);
                    list.Add(item);
                    continue;
                }
                else if (item.IsStackable && item.HasStackSpace)
                {
                    item.AddStack(amount);
                }
            }
        }
        else
        {
            Item item = ItemManager.instance.GetItem(Gacha.PullSingle(), true);
            item.transform.SetParent(this.transform);
            list.Add(item);
        }

        return list;
    }
    #endregion
}
