using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatherConfirmView : ConfirmationView
{
    [SerializeField]
    private GameObject gatheredObjectPrefab;
    [SerializeField]
    private Transform itemGatheredContainer;

    private List<Item> itemsGathered;
    private GatherPoint currentGP;

    protected virtual void OnEnable()
    {
        base.Awake();
        GatherPointManager.instance.EvtRegistered += OnGatherPointRegistered;
        GatherPointManager.instance.EvtDeregistered += OnGatherPointDeregistered;
    }


    protected virtual void OnDisable()
    {
        GatherPointManager.instance.EvtRegistered -= OnGatherPointRegistered;
        GatherPointManager.instance.EvtDeregistered -= OnGatherPointDeregistered;
    }

    private void OnGatherPointRegistered(GatherPoint gatherPoint)
    {
        gatherPoint.EvtGatherCheck += SetUpPossibleResults;
    }

    private void OnGatherPointDeregistered(GatherPoint gatherPoint)
    {
        gatherPoint.EvtGatherCheck -= SetUpPossibleResults;
    }

    private void SetUpPossibleResults(GatherPoint point)
    {
        itemsGathered = point.PossibleItems.ToList();
        currentGP = point;
        Show();
    }

    public void ShowResults()
    {
        currentGP.GetItemsFromPoint();
        Hide();
    }

    protected override void OnShow()
    {
        foreach (var item in itemsGathered)
        {
            Instantiate(gatheredObjectPrefab, itemGatheredContainer);
        }
    }

    protected override void OnHide()
    {
        int count = itemGatheredContainer.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(itemGatheredContainer.GetChild(0).gameObject);
        }
    }
}
