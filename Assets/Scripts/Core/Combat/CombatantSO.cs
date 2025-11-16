using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CombatantSO", menuName = "Scriptable Objects/Combat/CombatantSO")]
public class CombatantSO : ScriptableObject
{

    [BoxGroup("Default Base Values")] public float DefaultBaseDamage = 20f;
    [BoxGroup("Default Base Values")] public float DefaultBaseDefense = 10f;
    [BoxGroup("Default Base Values")] public float DefaultBaseCritChance = 20f;
    [BoxGroup("Default Base Values")] public float DefaultBaseEvasionChance = 10f;
    [BoxGroup("Default Base Values")] public float DefaultBaseCritMult = 2.0f;
    [BoxGroup("Value Offsets")] public int DamageOffSetRange = 2;
}
