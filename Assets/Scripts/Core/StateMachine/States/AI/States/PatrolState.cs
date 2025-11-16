using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PatrolState : State
{
    private AIUnitController aiController;
    private PatrolStateSO so;

    private List<Vector2> patrolPoints = new List<Vector2>();

    public override void Do()
    {
        base.Do();
        if(aiController == null)
        {
            return;
        }
        aiController.SetInputX(-1);
    }

    public override void Initialize(StateSO stateSO, StateController bound)
    {
        base.Initialize(stateSO, bound);
        so = Instantiate(stateSO, this.transform) as PatrolStateSO;
        aiController = bound.Unit?.GetComponent<AIUnitController>();
    }

    public override IEnumerator OnEnter()
    {
        return base.OnEnter();
    }

    public override IEnumerator OnExit()
    {
        yield return base.OnExit();
    }
}
