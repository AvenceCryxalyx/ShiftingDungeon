using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        OrdinaryItem,
        Consumables,
        Treasure,
        Key,
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Cursed,
        Exquisite,
    }

    public string Name { get { return so.Name; } }
    public string Description { get { return so.Description; } }   
    public ModValue Value { get; protected set; }
    public Sprite Icon { get { return so.Sprite; } }
    public ItemType Type { get { return so.Type; } }
    public ItemRarity Rarity { get { return so.Rarity; } }
    public int Stack { get; protected set; }
    public int MaxStack { get { return (so.IsStackable) ? so.MaxStack : 0; } }

    private ItemDataSO so;

    public virtual void Initialize(ItemDataSO data) 
    { 
        so = data;
        Value = new ModValue(data.BaseValue, 0f, 0f);
    }
    public virtual void Use() { }
}
