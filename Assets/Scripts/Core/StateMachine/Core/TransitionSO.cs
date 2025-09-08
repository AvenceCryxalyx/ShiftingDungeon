using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "TransitionSO", menuName = "Scriptable Objects/StateMachine/TransitionSO")]
public class TransitionSO : ScriptableObject
{
    public StateSO ToTranstionState;
    public List<ConditionSO> conditions;
}
