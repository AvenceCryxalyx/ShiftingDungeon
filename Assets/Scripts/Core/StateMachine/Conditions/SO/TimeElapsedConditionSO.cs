using UnityEngine;


[CreateAssetMenu(fileName = "TimeElapsedConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/TimeElapsedConditionSO")]
public class TimeElapsedConditionSO :  ConditionSO
{
    public float WaitSeconds;

    public override Condition GetCondition()
    {
        return new TimeElapsedCondition();
    }
}
