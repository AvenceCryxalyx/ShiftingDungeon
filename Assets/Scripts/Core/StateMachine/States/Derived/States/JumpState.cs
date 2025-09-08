using UnityEngine;
using System.Collections;

public class JumpState : State
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
        Bound.GetComponent<PlayerUnitController>().Avatar.PlayAnimation(AnimationName);
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }
}
