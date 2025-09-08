using UnityEngine;

public class AIUnitController : UnitController
{
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
