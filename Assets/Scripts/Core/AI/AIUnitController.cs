using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIUnitController : UnitController
{
    [SerializeField]
    private List<Transform> patrolPoints;

    public IEnumerable<Transform> PatrolPoints { get { return patrolPoints; } }

    protected override void Start()
    {
        base.Start();
        AIManager.RegisterAIUnit(this);
        Health = GetComponentInChildren<Health>();
        Avatar = GetComponentInChildren<Avatar>();
    }

    public void SetInputX(float x)
    {
        InputX = x;
    }

    public void SetInputY(float y)
    {
        InputY = y;
    }

    private void OnDestroy()
    {
        AIManager.RemoveRegisteredAIUnit(this);
    }
}
