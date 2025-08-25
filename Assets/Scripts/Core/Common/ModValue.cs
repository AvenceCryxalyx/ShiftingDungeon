using UnityEngine;
using System;
using Unity.VisualScripting;

public class ModValue
{
    public ModValue(int baseValue, float addMod, float multMod)
    {
        BaseValue = baseValue;
        AddMod = addMod;
        MultMod = multMod;
        UpdateCurrent();
    }

    #region Events
    public Action<int, int> EvtValueChanged;
    #endregion

    #region Properties
    public int CurrentValue { get; protected set; }
    public int BaseValue { get; protected set; }
    public float AddMod { get; protected set; }
    public float MultMod {  get; protected set; }
    #endregion

    public void AddFlatValue(float value)
    {
        AddMod += value;
        UpdateCurrent();
    }

    public void RemoveFlatValue(float value)
    {
        AddMod -= value;
        UpdateCurrent();
    }

    public void AddMultValue(float value)
    {
        MultMod += value;
        UpdateCurrent();
    }

    public void RemoveMultValue(float value)
    {
        MultMod -= value;
        UpdateCurrent();
    }

    protected void UpdateCurrent()
    {
        int newValue = (int)((BaseValue * MultMod) + AddMod);
        if(CurrentValue == newValue)
        {
            return;
        }
        if(EvtValueChanged != null)
        {
            EvtValueChanged.Invoke(CurrentValue, newValue);
        }
        CurrentValue = newValue;
    }
}
