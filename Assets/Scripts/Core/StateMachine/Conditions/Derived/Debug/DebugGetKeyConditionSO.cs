using UnityEngine;

[CreateAssetMenu(fileName = "DebugGetKeyConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/DebugGetKeyConditionSO")]
public class DebugGetKeyConditionSO : ConditionSO
{
    public KeyCode KeyCode;

    public override Condition GetCondition()
    {
        return new DebugKeyCondition();
    }
}
