using UnityEngine;

[CreateAssetMenu(fileName = "XInputCheckConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/XInputCheckConditionSO")]
public class XVelocityCheckConditionSO : ConditionSO
{
    public ValueCheckType Type;
    public float ValueToCompare;

    public override Condition GetCondition()
    {
        return new XVelocityCheckCondition();
    }
}
