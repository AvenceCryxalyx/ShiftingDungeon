using System;
using UnityEngine;

public sealed class Currency : MonoBehaviour
{
    public enum Type
    {
        Gold
    }

    public Action<int, int> OnGained;
    public Action<int, int> OnLost;

    public int CurrentAmount { get; private set; }

    public void AddAmount(int amount)
    {
        CurrentAmount += amount;

        if(OnGained != null)
        {
            OnGained.Invoke(amount, CurrentAmount);
        }
    }

    public void ReduceAmount(int amount)
    {
        CurrentAmount -= amount;

        if (OnLost != null)
        {
            OnLost.Invoke(amount, CurrentAmount);
        }
    }
}
