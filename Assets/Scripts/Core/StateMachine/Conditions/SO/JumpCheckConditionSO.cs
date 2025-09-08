using UnityEngine;

[CreateAssetMenu(fileName = "GroundedStatusCondition", menuName = "Scriptable Objects/StateMachine/Conditions/JumpCheckCondition")]
public class JumpCheckConditionSO : ConditionSO
{
    public override Condition GetCondition()
    {
        return new JumpCheckCondition();
    }
}
