using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "MapAreaSO", menuName = "Scriptable Objects/MapAreaSO")]

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
    
    [BoxGroup("Specifics")] public bool hasSpecifics = false;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int CoordinateX = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int CoordinateY = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool PerRowRequirement;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool PerColumnRequirement;
    //[BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int specificRow = -1;
    //[BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int specificColumn = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool shouldBeMiddle = false;
 
    [BoxGroup("Restrictions")] public bool hasRestrictions = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsRowLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsColumnLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsPositionLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsTopRowRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsBottomRowRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsLeftColRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasRestrictions")] public bool IsRightColRestricted = false;
}
