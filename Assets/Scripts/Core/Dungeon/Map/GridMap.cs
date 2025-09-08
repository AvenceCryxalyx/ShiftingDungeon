using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class GridMap : Map
{
    private struct AreaSwapInfo
    {
        public MapArea SelectedArea;
        public MapArea ToSwapArea;
    }
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

    private List<AreaSwapInfo> toSwapAreas = new List<AreaSwapInfo>();

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
        DungeonMode.Master.Map = this;
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
        List<Tuple<int, int>> setCoordinates = new List<Tuple<int, int>>();
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
            setCoordinates.Add(getSpecific);
        }

        //Restricted
        foreach(MapArea area in hasResrictions)
        {
            Tuple<int, int> getValid = FilterSpawnRestrictedAreas(area, availableCoordinates);
            area.Initialize(getValid);
            map[getValid.Item1, getValid.Item2] = area;
            setCoordinates.Add(getValid);
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

    public void LoadAllAreas()
    {
        foreach(MapArea area in allAreas)
        {
            area.Load();
        }
    }

    public void DoMapSetup(int rows, int columns)
    {
        CurrentRows = rows;
        CurrentColumns = columns;
    }

    /// <summary>
    /// Swaps the Areas that were selected by the function call SelectAreasToSwap. Requires SelectAreasToSwap to be called before this.
    /// </summary>
    public void DoRoomSwaps()
    {
        if (toSwapAreas.Count <= 0)
            return;

        NativeArray<float2> positions = new NativeArray<float2>(toSwapAreas.Count, Allocator.TempJob);
        TransformAccessArray transformAccessArray = new TransformAccessArray(toSwapAreas.Count, 100);

        for(int i = 0; i < toSwapAreas.Count; i++)
        {
            AreaSwapInfo info = toSwapAreas[i];
            transformAccessArray.Add(info.SelectedArea.transform);
            positions[i] = new float2(info.ToSwapArea.transform.position.x, info.ToSwapArea.transform.position.y);
        }

        SwapPositionTasks swapTask = new SwapPositionTasks()
        {
            positions = positions,
            deltaTime = Time.deltaTime
        };

        JobHandle handle = swapTask.Schedule(transformAccessArray);
        handle.Complete();

        foreach (AreaSwapInfo info in toSwapAreas)
        {
            Tuple<int, int> oldCoord = info.SelectedArea.Coordinates;
            info.SelectedArea.ChangeCoordinates(info.ToSwapArea.Coordinates);
            info.ToSwapArea.ChangeCoordinates(oldCoord);
        }

        //Clean up
        transformAccessArray.Dispose();
        positions.Dispose();
        toSwapAreas.Clear();
    }

    public void SelectAreasToSwap(int numberOfRooms)
    {
        List<MapArea> validRooms = allAreas.Where(x => CheckSelectionRestrictions(x)).ToList();

        for (int i = 0; i < numberOfRooms; i++)
        {
            int index = UnityEngine.Random.Range(0, validRooms.Count());
            MapArea selected = validRooms.ElementAt(index);
            MapArea swapArea = GetRoomToSwap(selected);
            if (swapArea == null)
            {
                continue;
            }
            validRooms.Remove(selected);
            validRooms.Remove(swapArea);
            map[selected.Coordinates.Item1, selected.Coordinates.Item2] = swapArea;
            map[swapArea.Coordinates.Item1, swapArea.Coordinates.Item2] = selected;

            if (swapArea && selected)
            {
                AreaSwapInfo areaSwapInfo = new AreaSwapInfo();
                areaSwapInfo.SelectedArea = selected;
                areaSwapInfo.ToSwapArea = swapArea;
                
                toSwapAreas.Add(areaSwapInfo);
            }
        }
    }

    public void OnUnloadMap()
    {
        foreach(MapArea area in allAreas)
        {
            area.Unload();
            area.PoolOrDestroy();
        }
        allAreas.Clear();
        map = null;
    }
    #endregion

    #region Private Methods
    [BurstCompile]
    private struct SwapPositionTasks : IJobParallelForTransform
    {
        public NativeArray<float2> positions;
        [ReadOnly] public float deltaTime;

        public void Execute(int index, TransformAccess transform)
        {
            float2 direction = new float2(transform.position.x - positions[index].x, transform.position.y - positions[index].y);
            transform.position += new Vector3(direction.x, direction.y, 0).normalized * deltaTime;
        }
    }

    private MapArea GetRoomToSwap(MapArea area)
    {
        Tuple<int, int>[] nearbyCoordinates = new Tuple<int, int>[]
{
            new Tuple<int, int>(area.Coordinates.Item1 + 1, area.Coordinates.Item2),
            new Tuple<int, int>(area.Coordinates.Item1 - 1, area.Coordinates.Item2),
            new Tuple<int, int>(area.Coordinates.Item1, area.Coordinates.Item2 - 1),
            new Tuple<int, int>(area.Coordinates.Item1, area.Coordinates.Item2 + 1),
        };
        Debug.Log($"Area: {area.Coordinates}");
        List<MapArea> validAreas = new List<MapArea>();
        foreach (Tuple<int, int> coord in nearbyCoordinates)
        {
            Debug.Log($"Nearby: {coord}");
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

    private bool CheckIfCoordinateIsFilled(int x, int y)
    {
        if(map == null)
        {
            Debug.LogError("Map not initialized");
            return true;
        }
        return map[x, y] != null;
    }

    private Tuple<int, int> CheckOtherSpecifics(MapArea area, List<Tuple<int, int>> availableCoords)
    {
        if(area.AreaInfo.shouldBeMiddle)
        {
            int column = ((CurrentColumns - 1) / 2);
            int row = ((CurrentRows - 1) / 2);
            return availableCoords.FirstOrDefault(x => (x.Item1 == column && x.Item2 == row));
        }
        else if(area.AreaInfo.SpecificRow != -1 && (area.AreaInfo.SpecificColumn != -1))
        {
            if (!CheckIfCoordinateIsFilled(area.AreaInfo.SpecificColumn, area.AreaInfo.SpecificRow))
            {
                Debug.LogError("Specific Coordinates already occupied.");
                return null;
            }
            return availableCoords.First(x => (x.Item1 == area.AreaInfo.SpecificColumn && area.AreaInfo.SpecificRow == x.Item2));
        }
        else if (area.AreaInfo.SpecificRow != -1)
        {
            int randomColumn = UnityEngine.Random.Range(0, CurrentColumns);
            if (CheckIfCoordinateIsFilled(randomColumn, area.AreaInfo.SpecificRow))
            {
                return availableCoords[randomColumn + (CurrentColumns * area.AreaInfo.SpecificRow)];
            }
            return CheckOtherSpecifics(area, availableCoords);
        }
        else if (area.AreaInfo.SpecificColumn != -1)
        {
            int randomRow = UnityEngine.Random.Range(0, CurrentRows);
            if(CheckIfCoordinateIsFilled(area.AreaInfo.SpecificColumn, randomRow))
            {
                return availableCoords[randomRow + (CurrentRows * area.AreaInfo.SpecificColumn)];
            }
            return CheckOtherSpecifics(area, availableCoords);
        }
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
        if(!CheckIfCoordinateIsFilled(coord.Item1, coord.Item2))
        {
            return false;
        }
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
