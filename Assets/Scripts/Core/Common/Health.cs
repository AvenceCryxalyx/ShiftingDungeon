using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Health
{
    #region Properties
    public int CurrentValue { get; private set; }
    public int MaxValue {  get; private set; }
    public bool IsAlive { get { return CurrentValue != 0; } }
    public bool IsInvulnerable { get; private set; }
    #endregion

    #region Events
    public Action<Health, int> OnHeal;
    public Action<Health, int> OnDamageTaken;
    public Action<Health> OnDeath;
    #endregion

    #region Public Methods
    public void FullRestore()
    {
        CurrentValue = MaxValue;
    }

    public void Heal(int value)
    {
        if(value > MaxValue - CurrentValue)
        {
            value = MaxValue - CurrentValue;
        }

        if (OnHeal != null)
        {
            OnHeal.Invoke(this, value);
        }

        CurrentValue += value;
        CheckStatus();
    }

    public void Damage(int value)
    {
        if(value > CurrentValue)
        {
            value = CurrentValue;
        }
        if (OnDamageTaken != null)
        {
            OnDamageTaken.Invoke(this, value);
        }
        CurrentValue -= value;
        CheckStatus();
    }
    #endregion

    #region Private Methods
    private void CheckStatus()
    {
        if(CurrentValue <= 0 && OnDeath != null)
        {
            OnDeath.Invoke(this);
        }
    }
    #endregion
}
