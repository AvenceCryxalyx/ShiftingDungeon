using UnityEngine;
using Sirenix.OdinInspector;

//[CreateAssetMenu(fileName = "ConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/ConditionSO")]
public class ConditionSO : ScriptableObject
{
    [BoxGroup("General Info")]
    public bool shouldInverseResult = false;
    public virtual Condition GetCondition() {  return null; }
}
