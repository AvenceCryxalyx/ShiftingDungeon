using System.Collections;
using UnityEngine;

public class IdleState : State
{
    public override void Do()
    {
        base.Do();
    }

    public override void Initialize(StateSO stateSO, StateController controller)
    {
        base.Initialize(stateSO, controller);
    }

    public override IEnumerator OnEnter()
    {
        if (Bound.Unit != null)
        {
            Bound.Unit.SetYMovementEnable(false);
            Bound.Unit.SetXMovementEnable(true);
            Bound.Unit.ChangeState(UnitController.UnitState.OnGround);
        }
        yield return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }
}
