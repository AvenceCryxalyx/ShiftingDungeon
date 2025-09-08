using System.Collections;
using UnityEngine;

public class LandingState : State
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
        yield return base.OnEnter();

        PlayerUnitController player = Bound.Unit.GetComponent<PlayerUnitController>();
        if (player != null)
        {
            player.DisableInputs();
        }
    }

    public override IEnumerator OnExit()
    {
        yield return base.OnExit();
        PlayerUnitController player = Bound.Unit.GetComponent<PlayerUnitController>();
        if (player != null)
        {
            player.EnableInputs();
        }
        Bound.Unit.ChangeState(UnitController.UnitState.OnGround);
    }
}
