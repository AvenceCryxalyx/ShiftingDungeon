using System.Collections.Generic;
using UnityEngine;

public class Consumables : Item
{
    List<Effect> effects = new List<Effect>(); 
    public override void Use()
    {
        foreach (Effect effect in effects)
        {
            effect.ApplyEffect();
        }
    }
}
