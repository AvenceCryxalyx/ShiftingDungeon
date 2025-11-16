using System.Collections.Generic;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;


public sealed class Combat
{
    public static List<Combat> pooledCombats = new List<Combat>();

    public const float DefToDmgMitigationPercent = 0.7f;
    public const float DefaultCritDamageMult = 2.0f;
    public enum DamageType
    {
        None = 0,
        Physical,
        Magical,
        Pure,
    }

    public enum DamageSource
    {
        None,
        Attack,
        Skill,
        Projectile,
        Environment,
    }

    public ModValue DamageMod;
    public ModValue DefenseMod;

    public float BaseDamage;
    public float BaseDefense;
    public float ProcessedDamage;
    public float MitigatedDamage;
    public int FinalDamage;
    public bool DidHit;
    public bool DidCrit;
    public bool TrueStrike = false;
    public bool PerfectDodge = false;
    public DamageType Type;

    public Combatant Attacker { get; private set; }
    public Combatant Defender { get; private set; }

    public Combat(Combatant attacker, Combatant defender, DamageType type)
    {
        Attacker = attacker;
        Defender = defender;
        Type = type;
    }

    public static Combat Create(Combatant attacker, Combatant defender, DamageType damageType = DamageType.Physical)
    {
        if (pooledCombats.Count > 0)
        {
            Combat pooled = pooledCombats[0];
            pooledCombats.RemoveAt(0);
            return pooled;
        }
        
        return new Combat(attacker, defender, damageType);
    }

    public void Engage()
    {
        if (!CheckHit())
        {
            Attacker.EvtOnCombatMissed.Invoke(this);
            Defender.EvtOnCombatDodged.Invoke(this);
            return;
        }

        CombatInitiated();

        CombatEngaging();

        CombatEngaged();

        Reset();
    }

    private void CombatInitiated()
    {
        if(Attacker.EvtCombatInitiated != null)
        {
            Attacker.EvtCombatInitiated.Invoke(this);
        }
        if(Defender.EvtCombatInitiated!= null)
        {
            Defender.EvtCombatInitiated.Invoke(this);
        }
        BaseDamage = Attacker.GetDamage(Type);
        BaseDefense = Defender.GetDefense(Type);
        DamageMod = new ModValue(1, 0f, 0f);
        DefenseMod = new ModValue(1, 0f, 0f);
    }

    private void CombatEngaging()
    {
        ProcessedDamage = CalculateProcessedDamage();
        MitigatedDamage = CalculateMitigatedDamage();

        if (CheckCrit())
        {
            ProcessedDamage = Mathf.CeilToInt(ProcessedDamage * Attacker.GetCritDamageMult());
        }

        if (Attacker.EvtOnDealingDamage != null)
        {
            Attacker.EvtOnDealingDamage.Invoke(this);
        }
        if (Defender.EvtOnReceivingDamage != null)
        {
            Defender.EvtOnReceivingDamage.Invoke(this);
        }
    }

    private void CombatEngaged()
    {
        FinalDamage = Mathf.CeilToInt(ProcessedDamage - MitigatedDamage);

        Attacker.DealDamage(FinalDamage);
        Defender.TakeDamage(FinalDamage);

        if (Attacker.EvtOnDealtDamage != null)
        {
            Attacker.EvtOnDealtDamage.Invoke(this);
        }
        if (Defender.EvtOnReceivedDamage != null)
        {
            Defender.EvtOnReceivedDamage.Invoke(this);
        }
    }

    private bool CheckHit()
    {
        if(TrueStrike && !PerfectDodge)
        {
            return true;
        }
        if(PerfectDodge && !TrueStrike)
        {
            return false;
        }
        DidHit = Defender.GetEvasion() > UnityEngine.Random.Range(0, 100f);
        return DidHit;
    }

    private bool CheckCrit()
    {
        DidCrit = Attacker.GetCritChance() > UnityEngine.Random.Range(0, 100f);
        return DidCrit;
    }

    private float CalculateProcessedDamage()
    {
        return BaseDamage * DamageMod.Current;
    }

    private float CalculateMitigatedDamage()
    {
        return ProcessedDamage - ((BaseDefense * DefToDmgMitigationPercent) * DefenseMod.Current);
    }

    private void Reset()
    {
        BaseDamage = 0;
        ProcessedDamage = 0;
        BaseDefense = 0;
        MitigatedDamage = 0;
        FinalDamage = 0;
        DamageMod = null;
        DefenseMod = null;
        Type = DamageType.None;

        pooledCombats.Add(this);
    }
}

