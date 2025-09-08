using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitDetector : MonoBehaviour
{
    public Action<UnitController> EvtUnitEntered;
    public Action<UnitController> EvtUnitExited;
    private List<UnitController> detectedUnits = new List<UnitController>();

    public IEnumerable<UnitController> DetectedUnits { get { return detectedUnits; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UnitController controller = collision.GetComponent<UnitController>();
        if (controller != null)
        {
            detectedUnits.Add(controller);
        }
        if (EvtUnitEntered != null)
        {
            EvtUnitEntered.Invoke(controller);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UnitController controller = collision.GetComponent<UnitController>();
        if (controller != null)
        {
            detectedUnits.Remove(controller);
        }
        if (EvtUnitExited != null)
        {
            EvtUnitExited.Invoke(controller);
        }
    }

    private void OnDisable()
    {
        detectedUnits.Clear();
    }
}
