using UnityEngine;

public enum ValueCheckType
{
    LessThan,
    LessThanOrEqual,
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
}

public class YVelocityConditionCheck : Condition
{
    private ValueCheckType _type;
    private float valueToCompare;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        YVelocityCheckConditionSO con = (YVelocityCheckConditionSO) so;
        _type = con.Type;
        valueToCompare = con.ValueToCompare;
    }

    public override void Activate(StateController unit)
    {
        
    }

    public override void Deactivate(StateController unit)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        switch (_type)
        {
            case ValueCheckType.LessThan:
                return unit.Unit.MoveY < valueToCompare;
            case ValueCheckType.LessThanOrEqual:
                return unit.Unit.MoveY <= valueToCompare;
            case ValueCheckType.Equals:
                return unit.Unit.MoveY == valueToCompare;
            case ValueCheckType.NotEquals:
                return unit.Unit.MoveY != valueToCompare;
            case ValueCheckType.GreaterThan:
                return unit.Unit.MoveY > valueToCompare;
            case ValueCheckType.GreaterThanOrEqual:
                return unit.Unit.MoveY >= valueToCompare;
            default:
                return unit.Unit.MoveY < valueToCompare;
        }
    }
}
