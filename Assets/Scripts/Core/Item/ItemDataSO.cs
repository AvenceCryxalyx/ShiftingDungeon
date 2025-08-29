using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemDataSO : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public int BaseValue;
    public Sprite Sprite;
    public Item.ItemType Type;
    public Item.ItemRarity Rarity;
    public Item OverridePrefab;
}
