using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map  : MonoBehaviour
{
    protected List<MapArea> allAreas = new List<MapArea>();

    protected MapSO so;

    public virtual void Initialize(MapSO so) 
    {
        this.so = so;
    }

    public virtual void Register(MapArea area)
    {
        if(!allAreas.Contains(area))
        {
            allAreas.Add(area);
        }
    }
}
