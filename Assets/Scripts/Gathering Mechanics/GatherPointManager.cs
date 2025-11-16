using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GatherPointManager : SimpleSingleton<GatherPointManager>
{
    public Action<GatherPoint> EvtRegistered;
    public Action<GatherPoint> EvtDeregistered;

    private List<GatherPoint> _gatherPoints = new List<GatherPoint>();
    public static void Register(GatherPoint gatherPoint)
    {
        if(gatherPoint == null && instance._gatherPoints.Contains(gatherPoint))
        {
            return;
        }

        instance._gatherPoints.Add(gatherPoint);

        if(instance.EvtRegistered != null)
        {
            instance.EvtRegistered.Invoke(gatherPoint);
        }
    }

    public static void Deregister(GatherPoint gatherPoint)
    {
        if (gatherPoint == null && !instance._gatherPoints.Contains(gatherPoint))
        {
            return;
        }

        instance._gatherPoints.Remove(gatherPoint);

        if (instance.EvtDeregistered != null)
        {
            instance.EvtDeregistered.Invoke(gatherPoint);
        }
    }
}
