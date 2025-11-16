using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatherResultView : UIVIew
{
    struct GatherResultData
    {
        public Sprite Sprite;
        public int Amount;
    }

    [SerializeField]
    private GatherResultItem Prefab;

    [SerializeField]
    private Transform resultItemsParentTransform;


    private Dictionary<string, GatherResultData> results = new Dictionary<string, GatherResultData>();

    private void Awake()
    {
        GatherPointManager.instance.EvtRegistered += OnGatherPointRegistered;
        GatherPointManager.instance.EvtDeregistered += OnGatherPointDeregistered;
    }

    private void OnGatherPointRegistered(GatherPoint point)
    {
        point.EvtOnGathered += OnGathered;
    }

    private void OnGatherPointDeregistered(GatherPoint point)
    {
        point.EvtOnGathered -= OnGathered;
    }

    private void OnGathered(IEnumerable<Item> items)
    {
        foreach (Item item in items)
        {
            GatherResultData gatherResult;
            if (results.ContainsKey(item.Name))
            {
                gatherResult = results[item.Name];
                gatherResult.Amount++;
            }
            else
            {
                gatherResult.Sprite = item.Icon;
                gatherResult.Amount = 1;
            }
            results.Add(item.Name, gatherResult);
        }
        Show();
    }

    protected override void OnShow()
    {
        base.OnShow();

        foreach (GatherResultData result in results.Values)
        {
            GatherResultItem item = Instantiate(Prefab, resultItemsParentTransform);
            item.Initialize(result.Sprite, result.Amount);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();

        int childCount = resultItemsParentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(resultItemsParentTransform.GetChild(0).gameObject);
        }
    }
}
