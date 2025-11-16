using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemGachaSO", menuName = "Scriptable Objects/Gacha/ItemGachaSO")]
public class ItemGachaSO : ScriptableObject
{
    public List<ItemDropRates> DropRates = new List<ItemDropRates>();
}
