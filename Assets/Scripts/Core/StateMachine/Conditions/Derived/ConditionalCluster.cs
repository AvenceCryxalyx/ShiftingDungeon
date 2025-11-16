using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class ConditionalCluster : Condition
{
    private Condition[] _conditions;

    public override void Initialize(ConditionSO so)
    {
        base.Initialize(so);
        ConditionalClusterSO con = so as ConditionalClusterSO;
        if (con != null)
        {
            _conditions = new Condition[con.Conditions.Count];
        }
        for(int i = 0; i < _conditions.Length; i++)
        {
            _conditions[i] = con.Conditions[i].GetCondition();
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
        return _conditions.All(x => x.ConditionTriggered(unit));
    }
}
