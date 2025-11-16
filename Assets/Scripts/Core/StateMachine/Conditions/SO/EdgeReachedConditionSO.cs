using UnityEngine;

[CreateAssetMenu(fileName = "EdgeReachedCondtionSO", menuName = "Scriptable Objects/StateMachine/Conditions/EdgeReachedCondtionSO")]
public class EdgeReachedConditionSO : ConditionSO
{
    public float OffsetX;
    public float OffsetY;
    public LayerMask LayerMask;

    public override Condition GetCondition()
    {
        return new EdgeReachedCondition();
    }
}
