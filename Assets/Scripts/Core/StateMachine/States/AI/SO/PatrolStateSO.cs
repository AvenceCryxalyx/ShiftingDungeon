using UnityEngine;

[CreateAssetMenu(fileName = "PatrolStateSO", menuName = "Scriptable Objects/StateMachine/States/PatrolStateSO")]
public class PatrolStateSO : StateSO
{
    public int InputX;
    public int InputY;
    public Vector2[] TargetPositions;

    public Vector2 GetTargetPosition(int index)
    {
        return TargetPositions[index];
    }
}
