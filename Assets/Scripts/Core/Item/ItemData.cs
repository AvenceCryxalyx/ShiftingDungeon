using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string Name;
    [TextArea]
    public string Description;
    public int BaseValue;
    public Sprite Sprite;
    public Item Prefab;
}
