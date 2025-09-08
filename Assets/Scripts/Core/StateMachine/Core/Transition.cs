using System.Linq;
using UnityEngine;

public class Transition
{
    protected State FromState;
    protected Condition[] conditions;
    protected State ToState;
    protected bool IsActivelyChecking;

    public Transition(State from, State to, TransitionSO so)
    {
        this.FromState = from;
        this.ToState = to;
        conditions = null;
        conditions = new Condition[so.conditions.Count];

        for(int i = 0; i < conditions.Length; i++)
        {
            Condition con = so.conditions[i].GetCondition();
            if (con  != null)
            {
                conditions[i] = con;
                con.Initialize(so.conditions[i]);
            }
        }
    }

    public void DoTranstion(StateController stateCon)
    {
        if(!IsActivelyChecking)
        {
            return;
        }
        stateCon.ChangeState(ToState);
    }

    public void Activate(StateController stateCon)
    {
        foreach (Condition condition in conditions)
        {
            condition.Activate(stateCon);
        }
        IsActivelyChecking = true;
    }

    public void Deactivate(StateController stateCon)
    {
        foreach (Condition condition in conditions)
        {
            condition.Deactivate(stateCon);
        }
        IsActivelyChecking = false;
    }

    public virtual bool CheckTransitionCondition(StateController stateCon)
    {
        if (!IsActivelyChecking)
        {
            return false;
        }
        if (conditions == null)
        {
            return conditions.All(x => x.ConditionTriggered(stateCon));
        }
        return false;
    }
}
