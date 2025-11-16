using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatHolderSO", menuName = "Scriptable Objects/StatHolderSO")]
public class StatHolderSO : ScriptableObject
{
    public List<StatData> Stats;
    public List<SubStatData> SubStats;

    private void Awake()
    {
        Stats = new List<StatData>();
        SubStats = new List<SubStatData>();

        for(int i = 0; i <  Enum.GetNames(typeof(Stat.Type)).Length; i++)
        {
            StatData data = new StatData();
            data.Type = (Stat.Type)i;
            Stats.Add(data);
        }

        for (int i = 0; i < Enum.GetNames(typeof(SubStat.Type)).Length; i++)
        {
            SubStatData data = new SubStatData();
            data.Type = (SubStat.Type)i;
            SubStats.Add(data);
        }
    }
}
