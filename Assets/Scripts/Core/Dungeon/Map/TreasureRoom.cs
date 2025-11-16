using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : SpawnRoom
{
    [SerializeField]
    private bool randomAmountSpawn = true;
    [SerializeField]
    private int specificSpawnCount = 2;
    [SerializeField, ShowIf("randomAmountSpawn")]
    private int minSpawn = 2;
    [SerializeField, ShowIf("randomAmountSpawn")]
    private int maxSpawn = 5;
    [SerializeField]
    private InteractableItemDropsSO dropObjectsList;

    //private Gacha TreasureGacha;
    private Dictionary<string, ItemDrop> possibleDrops = new Dictionary<string, ItemDrop>();
    private List<ItemDrop> drops = new List<ItemDrop>();
    public override void Initialize(Tuple<int, int> coordinates)
    {
        base.Initialize(coordinates);
    }

    public override void Load()
    {
        base.Load();
        SpawnItems();
    }

    public override void Unload()
    {
        base.Unload();
        foreach (ItemDrop item in drops)
        {
            if (item)
            {
                item.PoolOrDestroy();
            }
        }
        drops.Clear();
    }

    private void SpawnItems()
    {
        int spawnAmount = specificSpawnCount;
        if (randomAmountSpawn)
        {
            spawnAmount = UnityEngine.Random.Range(minSpawn, maxSpawn);
        }

        foreach (string id in DungeonMode.Master.ItemSpawnGacha.PullMultiple(spawnAmount))
        {
            Item item = ItemManager.instance.GetItem(id);
            ItemDrop drop = dropObjectsList.GetDropItem(item.Rarity);
            drop.transform.SetParent(this.transform);
            drop.SetItem(item);
            drop.EvtOnTaken += OnInteractableActiveChange;
            Spawn(drop.gameObject);
            drops.Add(drop);
        }
    }

    private void OnInteractableActiveChange(ItemDrop drop)
    {
        if(!drop.gameObject.activeSelf)
        {
            drops.Remove(drop);
            drop.EvtOnTaken -= OnInteractableActiveChange;
        }
    }
}
