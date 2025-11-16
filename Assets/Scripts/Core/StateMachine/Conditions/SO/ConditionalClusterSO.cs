using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConditionalClusterSO", menuName = "Scriptable Objects/StateMachine/Conditions/ConditionalClusterSO")]
public class ConditionalClusterSO : ConditionSO
{
    public List<ConditionSO> Conditions;

    public override Condition GetCondition()
    {
        return new ConditionalCluster();
    }
}
