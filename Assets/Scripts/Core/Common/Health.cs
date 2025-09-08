using NUnit.Framework.Constraints;
using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Health : MonoBehaviour
{
    #region Properties
    public int CurrentValue { get; private set; }
    public int MaxValue {  get; private set; }
    public bool IsAlive { get { return CurrentValue != 0; } }
    public bool IsInvulnerable { get; private set; }
    #endregion

    public void Initilaize(int baseValue, int maxValue)
    {
        MaxValue = maxValue;
        CurrentValue = baseValue;
    }

    public void UpdateMaxHeatlh(int newValue)
    {
        MaxValue = newValue;
    }

    #region Events
    public Action<Health, int> EvtHealed;
    public Action<Health, int> EvtDamageTaken;
    public Action<Health> EvtRevived;
    public Action<Health> EvtDying;
    public Action<Health> EvtDeath;
    #endregion

    #region Fields
    private Coroutine onDyingCor;
    #endregion

    #region Public Methods
    public void FullRestore()
    {
        CurrentValue = MaxValue;
    }
    
    public void SetInvulnerability(bool set)
    {
        IsInvulnerable = set;
    }

    public void Kill(bool trueDeath = false)
    {
        if(!trueDeath)
        {
            Damage(MaxValue);
        }
        else
        {
            OnHealthDepleted();
        }
    }

    public void Heal(int value)
    {
        if(value > MaxValue - CurrentValue)
        {
            value = MaxValue - CurrentValue;
        }

        if (EvtHealed != null)
        {
            EvtHealed.Invoke(this, value);
        }
        CurrentValue += value;
        CheckStatus();
    }

    public void Damage(int value)
    {
        if (IsInvulnerable)
        {
            return;
        }
        if(value > CurrentValue)
        {
            value = CurrentValue;
        }
        if (EvtDamageTaken != null)
        {
            EvtDamageTaken.Invoke(this, value);
        }
        CurrentValue -= value;
        CheckStatus();
    }
    #endregion

    #region Private Methods
    private void CheckStatus()
    {
        if(CurrentValue <= 0)
        {
            OnHealthDepleted();
        }
    }

    protected void OnHealthDepleted()
    {
        if(onDyingCor != null)
        {
            Debug.LogWarning("Dying Coroutine in progress will stop and replace");
            StopCoroutine(onDyingCor);
            onDyingCor = null;
        }
        onDyingCor = StartCoroutine(OnDyingTask());
    }

    private IEnumerator OnDyingTask()
    {
        if(EvtDying != null)
        {
            EvtDying.Invoke(this);
        }
        yield return null;
        yield return new WaitUntil(() => CurrentValue <= 0);
        if(EvtDeath != null)
        {
            EvtDeath.Invoke(this);
        }
        yield return null;
        onDyingCor = null;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        onDyingCor = null;
    }

    #endregion
}
