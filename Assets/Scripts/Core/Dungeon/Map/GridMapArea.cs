using System;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GridMapArea : MapArea
{
    GridMapAreaSO SO { get; set; }

    public override void Initialize(Tuple<int, int> coordinates)
    {
        base.Initialize(coordinates);
    }
}
