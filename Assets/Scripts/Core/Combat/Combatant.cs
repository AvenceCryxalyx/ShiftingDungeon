using UnityEngine;
using System;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Collections;
using UnityEngine.Events;

[System.Serializable]
public class CombatInitiatedEvent : UnityEvent<Combat> { }
[System.Serializable]
public class CombatEndedEvent : UnityEvent<Combat> { }
[System.Serializable]
public class CombatReceivingDamageEvent : UnityEvent<Combat> { }
[System.Serializable]
public class CombatReceivedDamageEvent : UnityEvent<Combat> { }
[System.Serializable]
public class CombatDealingDamageEvent : UnityEvent<Combat> { }
[System.Serializable]
public class CombatDealtDamageEvent : UnityEvent<Combat> { }
public class CombatMissedEvent : UnityEvent<Combat> { }
public class CombatDodgedEvent : UnityEvent<Combat> { }

public class Combatant : MonoBehaviour
{
    public CombatInitiatedEvent EvtCombatInitiated = new CombatInitiatedEvent();
    public CombatEndedEvent EvtCombatFinished = new CombatEndedEvent();
    public CombatDealingDamageEvent EvtOnDealingDamage = new CombatDealingDamageEvent();
    public CombatDealtDamageEvent EvtOnDealtDamage = new CombatDealtDamageEvent();
    public CombatReceivingDamageEvent EvtOnReceivingDamage = new CombatReceivingDamageEvent();
    public CombatReceivedDamageEvent EvtOnReceivedDamage = new CombatReceivedDamageEvent();
    public CombatDodgedEvent EvtOnCombatDodged = new CombatDodgedEvent();
    public CombatMissedEvent EvtOnCombatMissed = new CombatMissedEvent();

    public CombatantSO sO;
    public bool IsActive { get; protected set; }
    public bool IsDead { get; protected set; }

    public void Initialize(CombatantSO so)
    {
        sO = so;
    }

    public virtual float GetDamage(Combat.DamageType dmgType)
    {
        float damage = UnityEngine.Random.Range(sO.DefaultBaseDamage - sO.DamageOffSetRange, sO.DefaultBaseDamage + sO.DamageOffSetRange);
        return damage;
    }
    public virtual float GetDefense(Combat.DamageType dmgType)
    {
        return sO.DefaultBaseDefense;
    }
    public virtual float GetCritChance()
    {
        return sO.DefaultBaseCritChance;
    }

    public virtual float GetCritDamageMult()
    {
        return sO.DefaultBaseCritMult;
    }

    public virtual float GetEvasion()
    {
        return sO.DefaultBaseEvasionChance;
    }

    public virtual void EngageCombatant(Combatant defender, Combat.DamageType type)
    {
        Combat combat = Combat.Create(this, defender, type);
        combat.BaseDamage = GetDamage(type);
        combat.BaseDefense = defender.GetDefense(type);

        combat.Engage();
    }

    public virtual void TakeDamage(int  damage)
    {
        
    }

    public virtual void DealDamage(int damage)
    {

    }
}
