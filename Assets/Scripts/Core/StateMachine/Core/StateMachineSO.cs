using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateMachineSO", menuName = "Scriptable Objects/StateMachine/StateMachineSO")]
public class StateMachineSO : ScriptableObject
{
    public StateSO EntryState;
    public List<StateSO> AllPossibleStates;
}
