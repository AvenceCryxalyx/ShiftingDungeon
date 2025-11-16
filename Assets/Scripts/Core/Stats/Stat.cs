using UnityEngine;

public class Stat
{
    public enum Type
    {
        Strength,
        Endurance,
        Insight,
        Agility,
        Resilience,
        Luck,
    }
    public float BaseValue { get; private set; }
    public float CurrentValue { get; private set; }
    public ModValue Modifier { get; private set; }
    public int Points {  get; private set; }
    public Type StatType { get; private set; }

    private StatData data;

    public Stat(StatData stat)
    {
        data = stat;
        BaseValue = data.BaseValue;
        Modifier = new ModValue(data.ModBase);
        StatType = data.Type;
        Points = 0;

        UpdateBaseValue();
    }

    public void AddPoint(int Add = 1) 
    {
        Points += Add;
        UpdateBaseValue();
    }
    public void RemovePoint(int Remove = 1) 
    { 
        Points -= Remove;
        UpdateBaseValue();
    }

    private void UpdateBaseValue()
    {
        CurrentValue = BaseValue + (BaseValue * Modifier.Current) + (Points * data.StatIncreasePerPoint);
    }

    public void AddBaseValue(float additionalValue)
    {
        BaseValue += additionalValue;
    }
}
