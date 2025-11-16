using UnityEngine;

public class AnimationFinishedCondition : Condition
{
    public override void Activate(StateController unit)
    {
        
    }

    public override void Deactivate(StateController unit)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        return unit.Unit.Avatar.IsCurrentAnimationFinished(unit.CurrentState.AnimationName);
    }
}
