using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemDataSO : ScriptableObject
{
    [BoxGroup("General Info")] public string Name;
    [BoxGroup("General Info"), TextArea] public string Description;
    [BoxGroup("General Info")]  public int BaseValue;
    [BoxGroup("General Info")]  public Sprite Sprite;
    [BoxGroup("General Info")]  public Item.ItemType Type;
    [BoxGroup("General Info")]  public Item.ItemRarity Rarity;
    [BoxGroup("General Info")]  public Item OverridePrefab;
    [BoxGroup("General Info")] public bool IsStackable;
    [BoxGroup("General Info"), ShowIf("IsStackable")] public int MaxStack;
}
