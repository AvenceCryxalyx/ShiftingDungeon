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

    public string Name { get; protected set; }
    public string Description { get; protected set; }   
    public ModValue Value { get; protected set; }
    public Sprite Icon { get; protected set; }
    public ItemType Type { get; protected set; }
    public ItemRarity Rarity { get; protected set; }
    public virtual void Initialize(ItemDataSO data) 
    { 
        Name = data.Name;
        Description = data.Description;
        Type = data.Type;
        Icon = data.Sprite;
        Rarity = data.Rarity;
        Value = new ModValue(data.BaseValue, 0f, 0f);
    }
    public virtual void Use() { }
}
