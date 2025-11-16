using UnityEngine;

public class XVelocityCheckCondition : Condition
{
    private ValueCheckType _type;
    private float valueToCompare;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        XVelocityCheckConditionSO con = (XVelocityCheckConditionSO)so;
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
                return unit.Unit.MoveX < valueToCompare;
            case ValueCheckType.LessThanOrEqual:
                return unit.Unit.MoveX <= valueToCompare;
            case ValueCheckType.Equals:
                return unit.Unit.MoveX == valueToCompare;
            case ValueCheckType.NotEquals:
                return unit.Unit.MoveX != valueToCompare;
            case ValueCheckType.GreaterThan:
                return unit.Unit.MoveX > valueToCompare;
            case ValueCheckType.GreaterThanOrEqual:
                return unit.Unit.MoveX >= valueToCompare;
            default:
                return unit.Unit.MoveX < valueToCompare;
        }
    }
}
