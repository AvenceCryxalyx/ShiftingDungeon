using Unity.VisualScripting;
using UnityEngine;

public class EdgeReachedCondition : Condition
{
    private Vector2 offset; 
    private LayerMask layerMask;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        EdgeReachedConditionSO con = so as EdgeReachedConditionSO;
        if (con != null)
        {
            layerMask = con.LayerMask;
            offset.x = con.OffsetX;
            offset.y = con.OffsetY;
        }

    }

    public override void Activate(StateController unit)
    {
        
    }

    public override void Deactivate(StateController unit)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)(unit.transform.position) + (offset * unit.Unit.Direction), -unit.transform.up, 1f, layerMask);
        return hit.collider != null ;
    }
}
