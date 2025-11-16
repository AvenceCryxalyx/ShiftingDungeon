using UnityEngine;
using System.Collections;

public class AttackState : State
{
    private float timeElapsed = 0;
    private AttackStateSO atkSO;
    public override void Do()
    {
        base.Do();
    }

    public override void Initialize(StateSO stateSO, StateController controller)
    {
        base.Initialize(stateSO, controller);
        atkSO = (AttackStateSO)stateSO;
    }

    public override IEnumerator OnEnter()
    {
        yield return base.OnEnter();
        yield return null;
        Bound.Unit.Avatar.SetTrigger(AnimationName);
    }

    public override IEnumerator OnExit()
    {
        return base.OnExit();
    }
}
