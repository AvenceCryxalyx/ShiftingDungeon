using UnityEngine;

[CreateAssetMenu(fileName = "JumpCheckConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/JumpCheckCondition")]
public class JumpCheckConditionSO : ConditionSO
{
    public override Condition GetCondition()
    {
        return new JumpCheckCondition();
    }
}
