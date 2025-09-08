using UnityEngine;

public class TimeElapsedConditionSO :  ConditionSO
{
    public float WaitSeconds;

    public override Condition GetCondition()
    {
        return new TimeElapsedCondition();
    }
}
