using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "StateSO", menuName = "Scriptable Objects/StateMachine/States/StateSO")]
public class StateSO : ScriptableObject
{
    [BoxGroup("State")] public State StatePrefab;
    [BoxGroup("Animations")] public bool HasAnimation = true;
    [BoxGroup("Transitions")] public List<TransitionSO> transitions;
    [BoxGroup("Animations"), ShowIf("HasAnimation")] public string AnimationName;
}
