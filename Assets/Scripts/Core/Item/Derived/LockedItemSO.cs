using UnityEngine;

[CreateAssetMenu(fileName = "LockedItemSO", menuName = "Scriptable Objects/LockedItemSO")]
public class LockedItemSO : ItemDataSO
{
    public ItemDataSO Key;
    public ItemDataSO OpenedItem;
}
