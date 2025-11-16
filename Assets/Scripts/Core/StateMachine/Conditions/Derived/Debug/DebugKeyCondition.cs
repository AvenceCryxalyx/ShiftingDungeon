using UnityEngine;

public class DebugKeyCondition : Condition
{
    [SerializeField]
    KeyCode keyCode;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        DebugGetKeyConditionSO con = (DebugGetKeyConditionSO) so;
        keyCode = con.KeyCode;
    }

    public override void Activate(StateController unit)
    {
        
    }

    public override void Deactivate(StateController unit)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        return Input.GetKey(keyCode);
    }
}
