using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class GridMap : Map
{
    protected MapArea[,] map;

    public int CurrentRows { get; private set; }
    public int CurrentColumns { get; private set; }
    public bool IsInitialized 
    { 
        get 
        {
            return allAreas.Count == (CurrentRows * CurrentColumns) &&
                allAreas.Count == allAreas.Where(x => x.Coordinates != null).Count();
        } 
    }

    #region Overrides
    public override void Initialize(MapSO so)
    {
        base.Initialize(so);
        CurrentRows = so.Rows;
        CurrentColumns = so.Columns;
        map = new MapArea[CurrentColumns, CurrentRows];
    }

    private void Awake()
    {
        DungeonMaster.instance.Map = this;
    }
    #endregion

    #region Public Methods
    public MapArea GetAreaType(MapAreaSO.MapAreaType type)
    {
        return allAreas.FirstOrDefault(x => x.AreaInfo.Type == type);
    }
    public IEnumerator PositionMapAreas()
    {
        List<Tuple<int, int>> availableCoordinates = new List<Tuple<int, int>>();
        List<MapArea> availableRooms = allAreas;
        IEnumerable<MapArea> hasSpecifics = availableRooms.Where(x => x.AreaInfo.hasSpecifics);
        IEnumerable<MapArea> hasResrictions = availableRooms.Where(x => x.AreaInfo.hasRestrictions && !x.AreaInfo.hasSpecifics);
        for(int x = 0; x < CurrentColumns; x++)
        {
            for(int y = 0; y < CurrentRows; y++)
            {
                availableCoordinates.Add(new Tuple<int, int>(x, y));
            }
        }

        //Specifics
        foreach(MapArea area in hasSpecifics)
        {
            Tuple<int, int> getSpecific;
            if (area.AreaInfo.CoordinateX != -1 && area.AreaInfo.CoordinateY != -1)
            {
                getSpecific = availableCoordinates[(area.AreaInfo.CoordinateX * CurrentColumns) + area.AreaInfo.CoordinateY];
            }
            else
            {
                getSpecific = CheckOtherSpecifics(area, availableCoordinates);
            }
            area.Initialize(getSpecific);
            map[getSpecific.Item1, getSpecific.Item2] = area;
            availableCoordinates.Remove(getSpecific);
        }

        //Restricted
        foreach(MapArea area in hasResrictions)
        {
            Tuple<int, int> getValid = FilterSpawnRestrictedAreas(area, availableCoordinates);
            area.Initialize(getValid);
            map[getValid.Item1, getValid.Item2] = area;
            availableCoordinates.Remove(getValid);
        }

        List<MapArea> rest = allAreas.Where(x => x.Coordinates == null).ToList();
        //Everything else
        foreach (MapArea area in rest)
        {
            Tuple<int, int> random = GetRandomCoord(area, availableCoordinates);
            area.Initialize(random);
            map[random.Item1, random.Item2] = area;
            availableCoordinates.Remove(random);
        }
        yield return null;
    }

    public void DoMapSetup(int rows, int columns)
    {
        CurrentRows = rows;
        CurrentColumns = columns;
    }

    public void SwapAreas(MapArea area1, MapArea area2)
    {
        Vector3[] positions = new Vector3[2] { area1.transform.localPosition, area2.transform.localPosition };

        area1.MoveToNewPosition(positions[1]);
        area2.MoveToNewPosition(positions[0]);
        var coord = area1.Coordinates;
        area1.ChangeCoordinates(area2.Coordinates);
        area2.ChangeCoordinates(coord);
        
    }

    public void DoRoomSwaps(int numberOfRooms)
    {
        List<MapArea> validRooms = allAreas.Where(x => CheckSelectionRestrictions(x)).ToList();
        for (int i = 0; i < numberOfRooms; i++)
        {
            int index = UnityEngine.Random.Range(0, validRooms.Count());
            MapArea selected = validRooms.ElementAt(index);
            MapArea swapArea = GetRoomToSwap(selected);
            if(swapArea == null)
            {
                continue;
            }
            validRooms.Remove(selected);
            validRooms.Remove(swapArea);
            map[selected.Coordinates.Item1, selected.Coordinates.Item2] = swapArea;
            map[swapArea.Coordinates.Item1, swapArea.Coordinates.Item2] = selected;
            if (swapArea && selected)
            {
                SwapAreas(selected, swapArea);
            }
        }
    }

    public void OnUnloadMap()
    {
        foreach(MapArea area in allAreas)
        {
            area.PoolOrDestroy();
        }
        allAreas.Clear();
        map = null;
    }
    #endregion

    #region Private Methods
    private MapArea GetRoomToSwap(MapArea area)
    {
        Tuple<int, int>[] nearbyCoordinates = new Tuple<int, int>[]
{
            new Tuple<int, int>(area.Coordinates.Item1 + 1, area.Coordinates.Item2),
            new Tuple<int, int>(area.Coordinates.Item1 - 1, area.Coordinates.Item2),
            new Tuple<int, int>(area.Coordinates.Item1, area.Coordinates.Item2 - 1),
            new Tuple<int, int>(area.Coordinates.Item1, area.Coordinates.Item2 + 1),
        };
        Debug.Log(string.Format("Area: {0}", area.Coordinates));
        List<MapArea> validAreas = new List<MapArea>();
        foreach (Tuple<int, int> coord in nearbyCoordinates)
        {
            Debug.Log(string.Format("Nearby: {0}", coord));
            if(coord.Item1 < 0 || coord.Item1 > CurrentColumns - 1  || coord.Item2 < 0 || coord.Item2 > CurrentRows - 1)
            {
                continue;
            }
            if (CheckSwapRestrictions(area, map[coord.Item1, coord.Item2]))
            {
                validAreas.Add(map[coord.Item1, coord.Item2]);
            }
        }
        if (validAreas.Count <= 0)
        {
            return null;
        }
        return validAreas[UnityEngine.Random.Range(0, validAreas.Count)];
    }

    private Tuple<int, int> CheckOtherSpecifics(MapArea area, List<Tuple<int, int>> availableCoords)
    {
        if(area.AreaInfo.shouldBeMiddle)
        {
            int column = ((CurrentColumns - 1) / 2);
            int row = ((CurrentRows - 1) / 2);
            return availableCoords.FirstOrDefault(x => (x.Item1 == column && x.Item2 == row));
        }
        //else if(area.AreaInfo.specificRow != -1)
        //{
        //    int randomColumn = UnityEngine.Random.Range(0, CurrentColumns);
        //    return availableCoords[randomColumn + (CurrentColumns * area.AreaInfo.specificRow)];
        //}
        //else if(area.AreaInfo.specificColumn != -1)
        //{
        //    int randomRow = UnityEngine.Random.Range(0, CurrentRows);
        //    return availableCoords[randomRow + (CurrentRows * area.AreaInfo.specificColumn)];
        //}
        else
            return availableCoords[0];
    }

    private Tuple<int, int> FilterSpawnRestrictedAreas(MapArea area, List<Tuple<int, int>> availableCoords)
    {
        int minRowValid = 0;
        int maxRowValid = CurrentRows - 1;
        int minColumnValid = 0;
        int maxColumnValid = CurrentColumns - 1;

        if(area.AreaInfo.IsBottomRowRestricted)
        {
            minRowValid += 1;

        }
        if(area.AreaInfo.IsTopRowRestricted)
        {
             maxRowValid -= 1;
        }
        if(area.AreaInfo.IsRightColRestricted)
        {
            maxColumnValid -= 1;
        }
        if(area.AreaInfo.IsLeftColRestricted)
        {
            minColumnValid += 1;
        }
        Tuple<int, int>[] valid = availableCoords.Where(x => IsConditionRestricitonValid(x, minColumnValid, minRowValid, maxColumnValid, maxRowValid)).ToArray();
        int index = UnityEngine.Random.Range(0, valid.Length);
        return valid[index];
    }

    private Tuple<int, int> GetRandomCoord(MapArea area, List<Tuple<int, int>> availableCoords)
    {
        int randomIndex = UnityEngine.Random.Range(0, availableCoords.Count);
        return availableCoords[randomIndex];
    }

    private bool IsConditionRestricitonValid(Tuple<int, int> coord, int minX, int minY, int maxX, int maxY)
    {
        if (coord.Item1 < minX || coord.Item1 > maxX || coord.Item2 < minY || coord.Item2 > maxY)
            return false;
        return true;
    }

    private bool CheckSelectionRestrictions(MapArea area)
    {
        if (!area.AreaInfo.hasRestrictions)
        {
            return true;
        }

        if (area.AreaInfo.IsPositionLocked || area.IsMovementLocked)
        {
            return false;
        }
        return true;
    }

    private bool CheckSwapRestrictions(MapArea area1, MapArea area2)
    {
        if (!area1.AreaInfo.hasRestrictions && !area2.AreaInfo.hasRestrictions)
        {
            return true;
        }

        if (area1.AreaInfo.IsPositionLocked || area1.IsMovementLocked || area2.AreaInfo.IsPositionLocked || area2.IsMovementLocked)
        {
            return false;
        }

        if (area1.AreaInfo.IsColumnLocked || area2.AreaInfo.IsColumnLocked)
        {
            return area1.Coordinates.Item1 == area2.Coordinates.Item1;
        }

        if (area1.AreaInfo.IsRowLocked || area2.AreaInfo.IsRowLocked)
        {
            return area1.Coordinates.Item2 == area2.Coordinates.Item2;
        }

        if (area1.AreaInfo.IsTopRowRestricted || area2.AreaInfo.IsTopRowRestricted)
        {
            return (area1.Coordinates.Item2 != 0 && area2.Coordinates.Item2 != 0);
        }

        if (area1.AreaInfo.IsBottomRowRestricted || area2.AreaInfo.IsBottomRowRestricted)
        {
            return (area1.Coordinates.Item2 != CurrentRows - 1 && area2.Coordinates.Item2 != CurrentRows - 1);
        }

        if (area1.AreaInfo.IsLeftColRestricted || area2.AreaInfo.IsLeftColRestricted)
        {
            return (area1.Coordinates.Item1 != 0 && area2.Coordinates.Item1 != 0);
        }

        if (area1.AreaInfo.IsRightColRestricted || area2.AreaInfo.IsRightColRestricted)
        {
            return (area1.Coordinates.Item2 != CurrentColumns - 1 && area2.Coordinates.Item2 != CurrentColumns - 1);
        }
        return true;
    }
    #endregion
}
