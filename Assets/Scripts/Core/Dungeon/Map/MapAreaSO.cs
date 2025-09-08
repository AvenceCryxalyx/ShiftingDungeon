using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "MapAreaSO", menuName = "Scriptable Objects/Maps/MapAreaSO")]

public class MapAreaSO : ScriptableObject
{
    public enum MapAreaType
    {
        Empty,
        Treasure,
        RowTransfer,
        ColumnBridger,
        Entry,
        Exit,
        Spawn,
    }

    [BoxGroup("Info")] public string Id;
    [BoxGroup("Info")] public bool IsRequired;
    [BoxGroup("Info")] public MapAreaType Type;
    [BoxGroup("Info"), ShowIf("IsRequired")] public int AmountRequired;
    [BoxGroup("Info")] public int MaxSpawnedAmount;
    [BoxGroup("Info")] public MapArea Prefab;
}
