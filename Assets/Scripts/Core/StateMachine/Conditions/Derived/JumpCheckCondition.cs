using UnityEngine;
using UnityEngine.InputSystem;

public class JumpCheckCondition : Condition
{
    private bool performedJump;

    public override void Activate(StateController unit)
    {
        unit.Unit.GetComponent<PlayerUnitController>().EvtJumped += OnJumpButton;
    }

    public override void Deactivate(StateController unit)
    {
        unit.Unit.GetComponent<PlayerUnitController>().EvtJumped -= OnJumpButton;
        performedJump = false;
    }

    public override void Initialize(ConditionSO so)
    {
        
    }

    protected override bool IsMet(StateController unit)
    {
        return performedJump;
    }

    private void OnJumpButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            performedJump = true;
        }
        if (context.canceled)
        {
            performedJump = false;
        }
    }
}
