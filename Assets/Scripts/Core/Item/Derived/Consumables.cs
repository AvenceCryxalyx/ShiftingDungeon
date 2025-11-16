using System.Collections.Generic;
using UnityEngine;

public class Consumables : Item
{
    List<Effect> effects = new List<Effect>(); 
    protected override void OnUse(Player targetPlayer)
    {
        foreach (Effect effect in effects)
        {
            effect.ApplyEffect();
        }
    }
}
