using UnityEngine;

[CreateAssetMenu(fileName = "GroundedStatusCondition", menuName = "Scriptable Objects/StateMachine/Conditions/GroundedStatusCondition")]
public class GroundedStatusConditionSO : ConditionSO
{
    public override Condition GetCondition()
    {
        return new GroundStatusCondition();
    }
}
