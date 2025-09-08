using UnityEngine;

public abstract class Condition
{
    protected bool shouldInverseResult = false;

    public abstract void Activate(StateController unit);
    public abstract void Deactivate(StateController unit);
    protected abstract bool IsMet(StateController unit);

    public virtual void Initialize(ConditionSO so)
    {
        shouldInverseResult = so.shouldInverseResult;
    }

    public bool ConditionTriggered(StateController unit)
    {
        return (shouldInverseResult) ? !IsMet(unit) : IsMet(unit);
    }
}
