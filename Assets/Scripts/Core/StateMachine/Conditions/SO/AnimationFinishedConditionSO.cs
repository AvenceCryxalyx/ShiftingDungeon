using UnityEngine;

[CreateAssetMenu(fileName = "AnimationFinishedConditionSO", menuName = "Scriptable Objects/StateMachine/Conditions/AnimationFinishedConditionSO")]
public class AnimationFinishedConditionSO : ConditionSO
{
    public override Condition GetCondition()
    {
        return new AnimationFinishedCondition();
    }
}
