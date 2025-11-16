using UnityEngine;

[CreateAssetMenu(fileName = "YVelocityCheckConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/YVelocityCheckConditionSO")]
public class YVelocityCheckConditionSO : ConditionSO
{
    public ValueCheckType Type;
    public float ValueToCompare;

    public override Condition GetCondition()
    {
        return new YVelocityConditionCheck();
    }
}
