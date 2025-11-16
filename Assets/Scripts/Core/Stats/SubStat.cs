using System;
using UnityEngine;

public class SubStat
{
    public Action<Type, float, float> OnSubStatUpdated;

    public enum Type
    {
        PhysicalAttack,
        PhysicalDefense,
        MagicalAttack,
        MagicalDefense,
        EffectResistance,
        AttackSpeed,
        Evasion,
        CriticalRate,
        CriticalDamage,
        SpellCooldown,
    }

    public float BaseValue { get; private set; }
    public ModValue Modifier {  get; private set; }
    public float CurrentValue { get; private set; }
    public Type SubStatType { get; private set; }

    private SubStatData data;
    private Stat mainStat;
    
    public SubStat(SubStatData sub)
    {
        data = sub;
        BaseValue = data.BaseValue;
    }

    public void UpdateValue(Stat stat)
    {
        float oldValue = CurrentValue;
        CalculateValue();
        OnSubStatUpdated.Invoke(SubStatType, oldValue, CurrentValue - oldValue);
    }

    protected void CalculateValue()
    {
        CurrentValue = BaseValue + ((mainStat.CurrentValue/ data.DividerForMainStat) * data.MultiplierPerMainStatThreshold);
    }
}
