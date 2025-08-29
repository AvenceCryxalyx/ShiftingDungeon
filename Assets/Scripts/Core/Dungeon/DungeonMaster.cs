using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonMaster : SimpleSingleton<DungeonMaster>
{
    protected List<UnitController> monsters = new List<UnitController>();

    public static int MapAreaSizeX = 42;
    public static int MapAreaSizeY = 18;
    public static float MapAreaMoveSpeed = 20f;

    public GridMap Map {  get; set; }
    public Gacha ItemSpawnGacha { get; private set; }
    public Gacha MonstersGacha { get; private set; }
    public Gacha RoomGacha { get; private set; }

    private List<MapSO> dungeonMapLists;
    private MapSO currentMapSO;
    private Dictionary<string, MapAreaSO> areaList = new Dictionary<string, MapAreaSO>();
    private Dictionary<string, ItemDataSO> itemsList = new Dictionary<string, ItemDataSO>();
    protected override void Awake()
    {
        base.Awake();
        dungeonMapLists = Resources.LoadAll<MapSO>("Maps").ToList();
    }

    public IEnumerator InitializeDungeon(bool randomizedMap = true, MapSO selectedMap = null)
    {
        if(randomizedMap || selectedMap == null)
        {
            MapSO random = dungeonMapLists[UnityEngine.Random.Range(0, dungeonMapLists.Count)];
            currentMapSO = random;
            Map.Initialize(random);
            if (random.RandomizedMapAreas != null)
            {
                List<WeightedInfo> weightedInfos = new List<WeightedInfo>();
                foreach (var item in random.RandomizedMapAreas)
                {
                    areaList.Add(item.Area.Id, item.Area);
                    WeightedInfo newInfo;
                    newInfo.id = item.Area.Id;
                    newInfo.weight = item.Weight;
                    weightedInfos.Add(newInfo);
                }
                RoomGacha = new Gacha(weightedInfos.ToArray());
            }
            if (random.RandomizedItems != null)
            {
                List<WeightedInfo> weightedInfos = new List<WeightedInfo>();
                foreach (var item in random.RandomizedItems)
                {
                    itemsList.Add(item.Item.name, item.Item);
                    WeightedInfo newInfo;
                    newInfo.id = item.Item.name;
                    newInfo.weight = item.Weight;
                    weightedInfos.Add(newInfo);
                }
                ItemSpawnGacha = new Gacha(weightedInfos.ToArray());
            }
            yield return StartCoroutine(InstantiateMapAreas());
            yield return StartCoroutine(Map.PositionMapAreas());
            yield return new WaitUntil(() => Map.IsInitialized);
        }
        yield return null;
    }

    public IEnumerator InstantiateMapAreas()
    {
        List<MapArea> mapAreas = new List<MapArea>();
        foreach(MapAreaSO area in currentMapSO.AssuredAreas)
        {
            mapAreas.Add(Instantiate(area.Prefab, Map.transform));
        }

        foreach (string id in RoomGacha.PullMultiple((Map.CurrentRows * Map.CurrentColumns) - mapAreas.Count))
        {
            mapAreas.Add(Instantiate(areaList[id].Prefab, Map.transform));
        }
        yield return null;
    }

    public string GetDropItemID()
    {
        return ItemSpawnGacha.PullSingle();
    }

    public List<string> GetDropItemIDs(int amount)
    {
        return ItemSpawnGacha.PullMultiple(amount);        
    }

    public void UnloadDungeon()
    {
        Map.OnUnloadMap();
        Map = null;
    }
}
