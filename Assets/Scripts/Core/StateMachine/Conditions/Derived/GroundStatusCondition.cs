using UnityEngine;

public class GroundStatusCondition : Condition
{
    public override void Initialize(ConditionSO so)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        return unit.Unit.IsGrounded;
    }
    public override void Activate(StateController unit)
    {
        
    }
    public override void Deactivate(StateController unit)
    {
        
    }
}
