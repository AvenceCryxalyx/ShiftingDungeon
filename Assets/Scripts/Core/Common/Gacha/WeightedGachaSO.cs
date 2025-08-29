using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public struct WeightedInfo
{
    public string id;
    public int weight;
}

[CreateAssetMenu(fileName = "WeightedGacha", menuName = "Scriptable Objects/WeightedGacha")]
public class WeightedGachaSO : ScriptableObject
{
    
}
