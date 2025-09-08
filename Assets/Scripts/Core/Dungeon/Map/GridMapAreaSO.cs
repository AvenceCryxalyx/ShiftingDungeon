using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GridMapSO", menuName = "Scriptable Objects/Maps/GridMapSO")]
public class GridMapAreaSO : MapAreaSO
{
    [BoxGroup("Specifics")] public bool hasSpecifics = false;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int CoordinateX = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int CoordinateY = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool PerRowRequirement;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool PerColumnRequirement;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int SpecificRow = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public int SpecificColumn = -1;
    [BoxGroup("Specifics"), ShowIf("hasSpecifics")] public bool shouldBeMiddle = false;

    [BoxGroup("Restrictions")] public bool hasSwapRestrictions = false;
    [BoxGroup("Restrictions")] public bool hasSelectionRestrictions = false;
    [BoxGroup("Restrictions"), ShowIf("hasSelectionRestrictions")] public bool IsRowLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasSelectionRestrictions")] public bool IsColumnLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasSelectionRestrictions")] public bool IsPositionLocked = false;
    [BoxGroup("Restrictions"), ShowIf("hasSwapRestrictions")] public bool IsTopRowRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasSwapRestrictions")] public bool IsBottomRowRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasSwapRestrictions")] public bool IsLeftColRestricted = false;
    [BoxGroup("Restrictions"), ShowIf("hasSwapRestrictions")] public bool IsRightColRestricted = false;
}
