using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Action<Item> EvtOnUsed;

    public enum ItemType
    {
        OrdinaryItem,
        Consumables,
        Treasure,
        Key,
        Material,
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Cursed,
        Exquisite,
    }

    public string Name { get { return SO.Name; } }
    public string Description { get { return SO.Description; } }   
    public ModValue Value { get; protected set; }
    public Sprite Icon { get { return SO.Sprite; } }
    public ItemType Type { get { return SO.Type; } }
    public ItemRarity Rarity { get { return SO.Rarity; } }

    public int Stack { get; protected set; }
    public int MaxStack { get { return (SO.IsStackable) ? SO.MaxStack : 0; } }
    public bool IsStackable { get { return SO.IsStackable; } }
    public bool HasStackSpace { get { return Stack < MaxStack; } }

    public ItemDataSO SO { get; protected set; }

    public virtual void Initialize(ItemDataSO data) 
    {
        SO = data;
        Stack = 1;
        Value = new ModValue(data.BaseValue, 0f, 0f);
    }

    /// <summary>
    /// Function to add stacks will return the excess if over stacked amount
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int AddStack(int amount)
    {
        int excess = 0;
        if (!IsStackable || !HasStackSpace) return -1;

        if (Stack + amount > MaxStack)
        {
            excess = amount - (MaxStack - Stack);
            Stack = MaxStack;
        }
        else
        {
            Stack += amount;
        }
        return excess;
    }

    public void RemoveStack(int amount)
    {
        if (!IsStackable) return;

        if (amount > Stack)
        {
            Stack = 0;
        }
        else
        {
            Stack -= amount;
        }
    }

    public void Use(Player targetPlayer = null)
    {
        if(SO.IsStackable)
        {
            Stack--;
        }

        OnUse(targetPlayer);

        if(EvtOnUsed != null)
        {
            EvtOnUsed.Invoke(this);
        }
    }
    protected virtual void OnUse(Player targetPlayer) { }
}
