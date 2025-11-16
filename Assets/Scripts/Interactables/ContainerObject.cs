using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ContainerObject : Interactable
{
    [BoxGroup("Info"), SerializeField]
    private string interactionText = "Open";
    [BoxGroup("Items"), SerializeField]
    private bool doOverrideGacha = false;
    [BoxGroup("Items"), SerializeField, ShowIf("doOverrideGacha")]
    private ItemGachaSO overrideGachaRates;
    [BoxGroup("Items"),SerializeField]
    private int amountOfItemsToGacha = 3;
    private Animator _animator;

    private const string interactAnimation = "Interact";

    private bool interactionDone = false;
    private Gacha overrideGacha;
    private Dictionary<string, ItemDataSO> items = new Dictionary<string, ItemDataSO>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InteractText = interactionText;
        if (doOverrideGacha)
        {
            List<WeightedInfo> weightedInfos = new List<WeightedInfo>();
            foreach (var item in overrideGachaRates.DropRates)
            {
                items.Add(item.Item.name, item.Item);
                WeightedInfo newInfo;
                newInfo.id = item.Item.name;
                newInfo.weight = item.Weight;
                weightedInfos.Add(newInfo);
            }
            overrideGacha = new Gacha(weightedInfos.ToArray());
        }
    }

    public override int Interact(InteractionHandler handler)
    {
        if(interactionDone)
        {
            return (int)InteractableStatus.None;
        }
        if (handler == null)
        {
            return base.Interact(handler);
        }
        _animator.Play(interactAnimation);

        List<string> itemIds = new List<string>();

        if (amountOfItemsToGacha > 1)
        {
            if (doOverrideGacha)
            {
                itemIds = overrideGacha.PullMultiple(amountOfItemsToGacha);
            }
            else
            {
                itemIds = DungeonMode.Master.ItemSpawnGacha.PullMultiple(amountOfItemsToGacha);
            }
        }
        else
        {
            if (doOverrideGacha)
            {
                itemIds.Add(overrideGacha.PullSingle());
            }
            else
            {
                itemIds.Add(DungeonMode.Master.ItemSpawnGacha.PullSingle());
            }
        }

        foreach (string id in itemIds)
        {
            Item item = ItemManager.instance.GetItem(id);
            Player.instance.GetComponentInChildren<Inventory>().AddItem(item);
        }
        interactionDone = true;
        return (int)InteractableStatus.Success;
    }


}
