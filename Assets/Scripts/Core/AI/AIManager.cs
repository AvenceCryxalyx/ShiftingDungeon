using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

public class AIManager : SimpleSingleton<AIManager>
{
    public struct UnitPatrolInfo
    {
        public Transform unitTransform;
        public PatrolStateSO so;
        public Vector2 patrolPointEnd;
    }

    private List<AIUnitController> allAIUnits = new List<AIUnitController>();

    private bool allowGroundedAIsToMove;
    private bool allowAirborneAIsMove;

    private Dictionary<StateController, UnitPatrolInfo> patrolUnits = new Dictionary<StateController, UnitPatrolInfo>();

    private void Update()
    {
        HandlePatrolUnits();
    }

    public void SetAsOnPatrol(StateController unit, PatrolStateSO unitPatrolSO, Vector2 patrolPointEnd)
    {
        if(patrolUnits.ContainsKey(unit))
        {
#if UNITY_EDITOR
            Debug.LogError(string.Format("AIManager already has unit {0} registered as patrol unit", unit.gameObject));
#endif
            return;
        }

        UnitPatrolInfo newInfo = new UnitPatrolInfo();
        newInfo.so = unitPatrolSO;
        newInfo.patrolPointEnd = patrolPointEnd;
        newInfo.unitTransform = unit.transform;

        patrolUnits.Add(unit, newInfo);
    }

    public void RemovePatrolUnit(StateController unit)
    {
        if(!patrolUnits.ContainsKey(unit))
        {
#if UNITY_EDITOR
            Debug.LogError(string.Format("AIManager does not have unit {0} registered as patrol unit", unit.gameObject));
#endif
            return;
        }
        patrolUnits.Remove(unit);
    }

    public static void RegisterAIUnit(AIUnitController aiUnitController)
    {
        if(instance == null)
        {
            Debug.LogError("AIManager instance is null");
        }

        if(!instance.allAIUnits.Contains(aiUnitController))
        {
            instance.allAIUnits.Add(aiUnitController);
        }
    }

    public static void RemoveRegisteredAIUnit(AIUnitController aiUnitController)
    {
        if (instance == null)
        {
            Debug.LogError("AIManager instance is null");
        }

        if (instance.allAIUnits.Contains(aiUnitController))
        {
            instance.allAIUnits.Remove(aiUnitController);
        }
    }

    private void HandlePatrolUnits()
    {
        if(patrolUnits.Count == 0)
        {
            return;
        }

        NativeArray<float2> patrolInputs = new NativeArray<float2>(patrolUnits.Count, Allocator.TempJob);
        NativeArray<float2> patrolPositions = new NativeArray<float2>(patrolUnits.Count, Allocator.TempJob);
        NativeArray<float2> patrolUnitTargetPositions = new NativeArray<float2>(patrolUnits.Count, Allocator.TempJob);

        List<UnitPatrolInfo> infos = patrolUnits.Values.ToList();
        for (int i = 0; i < patrolUnits.Count; i ++)
        {
            UnitPatrolInfo info = infos[i];
            patrolInputs[i] = new float2(info.so.InputX, info.so.InputY);
            patrolPositions[i] = new float2(info.unitTransform.position.x, info.unitTransform.position.y);
            patrolUnitTargetPositions[i] = new float2(info.patrolPointEnd.x, info.patrolPointEnd.y);
        }

        PatrolMoveTasks patrol = new PatrolMoveTasks()
        {
            patrolInputs = patrolInputs,
            patrolPosition = patrolPositions,
            targetPoints = patrolUnitTargetPositions
        };

        JobHandle patrolHandle = patrol.Schedule(patrolUnits.Count, 100);
        
        patrolHandle.Complete();

        patrolInputs.Dispose();
        patrolPositions.Dispose(); 
        patrolUnitTargetPositions.Dispose();
    }

    [BurstCompile]
    public struct PatrolMoveTasks : IJobParallelFor
    {
        public NativeArray<float2> patrolInputs;
        public NativeArray<float2> patrolPosition;
        public NativeArray<float2> targetPoints;
        public void Execute(int index)
        {
            float direciton = patrolPosition[index].x > targetPoints[index].x ? -1 : 1;
            patrolInputs[index] += new float2(direciton, 0f);
        }
    }
}
