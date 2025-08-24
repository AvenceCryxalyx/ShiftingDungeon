using UnityEngine;
using UnityEngine.InputSystem;

public abstract class CustomAddedPlayerAction
{
    protected InputAction action;
    protected GameObject source;
    public bool IsEnabled { get; protected set; }
    public abstract void Initialize(PlayerUnitController controller);
    public abstract void Do();
    public void SetEnabled(bool enabled)
    {
        IsEnabled = enabled;
    }
}
