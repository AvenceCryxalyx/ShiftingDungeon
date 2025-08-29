using UnityEngine;
using System;
using Unity.VisualScripting;

public class ModValue
{
    public ModValue(int baseValue, float addMod, float multMod)
    {
        Base = baseValue;
        AddMod = addMod;
        MultMod = 1f + multMod;
        UpdateCurrent();
    }

    #region Events
    public Action<int, int> EvtValueChanged;
    #endregion

    #region Properties
    public int Current { get; protected set; }
    public int Base { get; protected set; }
    public float AddMod { get; protected set; }
    public float MultMod {  get; protected set; }
    #endregion

    public void AddFlatValue(float value)
    {
        AddMod += value;
        UpdateCurrent();
    }

    public void ReduceFlatValue(float value)
    {
        AddMod -= value;
        UpdateCurrent();
    }

    public void AddMultValue(float value)
    {
        MultMod += value;
        UpdateCurrent();
    }

    public void ReduceMultValue(float value)
    {
        MultMod -= value;
        UpdateCurrent();
    }

    protected void UpdateCurrent()
    {
        int newValue = (int)((Base * MultMod) + AddMod);
        if(Current == newValue)
        {
            return;
        }
        if(EvtValueChanged != null)
        {
            EvtValueChanged.Invoke(Current, newValue);
        }
        Current = newValue;
    }
}
