using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MapAreaRates
{
    public MapAreaSO Area;
    public int Weight;
}

[CreateAssetMenu(fileName = "MapSO", menuName = "Scriptable Objects/Maps/MapSO")]
public class MapSO : ScriptableObject
{
    public int Rows;
    public int Columns;
    public List<MapAreaSO> AssuredAreas;
    public List<MapAreaRates> RandomizedMapAreas;
    public List<ItemDropRates> RandomizedItems;
    public int RoomNumbers { get { return Rows * Columns; } }
}
