using System.Collections;
using UnityEngine;

public class WalkState : State
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
    }

    public override IEnumerator OnExit()
    {
        yield return base.OnExit();
    }
}
