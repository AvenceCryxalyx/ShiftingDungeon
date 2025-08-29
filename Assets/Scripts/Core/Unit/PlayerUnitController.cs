using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.Rendering.DebugUI;

public class PlayerUnitController : UnitController
{
    #region Inspector Fields
    [SerializeField]
    protected float jumpPower = 6f;
    [SerializeField]
    protected float airBourneMovementModifier = 0.75f;
    [SerializeField]
    protected float velocityLingerTime = 20f;
    #endregion

    #region
    public Action<InputAction.CallbackContext> EvtInteract { get; set;}
    public Action<InputAction.CallbackContext> EvtSwitch { get; set; }
    #endregion

    #region Fields
    protected int jumpCount;
    protected float horizontalMove;
    protected PlayerInputAction playerInput;
    protected InputAction horizontalMoveAction;
    protected InputAction verticalMoveAction;
    protected InputAction jumpAction;
    protected InputAction sprintAction;
    protected InputAction interactAction;
    protected InputAction switchInterAction;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();
        Player.instance.RegisterUnit(this);
    }

    public void Initialize(PlayerInputAction inputs)
    {
        playerInput = inputs;
        horizontalMoveAction = playerInput.Avatar.HorizontalMovement;
        verticalMoveAction = playerInput.Avatar.VerticalMovement;
        jumpAction = playerInput.Avatar.Jump;
        sprintAction = playerInput.Avatar.Sprint;
        interactAction = playerInput.Avatar.Interact;
        switchInterAction = playerInput.Avatar.SwitchInteract;
        jumpAction.performed += OnJumpButtonDown;
        jumpAction.canceled += OnJumpButtonUp;
        sprintAction.performed += OnSprintButtonDown;
        //sprintAction.canceled += OnSprintButtonUp;
        interactAction.performed += OnInteract;
        switchInterAction.performed += OnSwitch;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        if (playerInput != null)
        {
            DisableInputs();
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(playerInput != null)
        {
            EnableInputs();
        }
        
    }

    protected override void Update()
    {
        InputX = horizontalMoveAction.ReadValue<float>();
        InputY = verticalMoveAction.ReadValue<float>();
        base.Update();

        if (CurrentState == UnitState.OnLadder)
        {
            velocity = (Vector2.up * InputY * CurrentMoveSpeed);
        }
    }
    #endregion

    #region Overrides
    public override void SetYMovementEnable(bool enable = true)
    {
        base.SetYMovementEnable(enable);
        if(enable)
        {
            verticalMoveAction.Enable();
        }
        else
        {
            verticalMoveAction.Disable();
        }
    }
    public override void SetXMovementEnable(bool enable = true)
    {
        base.SetXMovementEnable(enable);
        if (enable)
        {
            horizontalMoveAction.Enable();
        }
        else
        {
            horizontalMoveAction.Disable();
        }
    }

    protected override void OnStateChanged(UnitState oldState, UnitState newState)
    {
        base.OnStateChanged(oldState, newState);

        UpdateStateChanges(newState);
    }

    protected override void OnGrounded()
    {
        base.OnGrounded();
        ChangeState(UnitState.OnGround);
    }
    #endregion

    #region Public Methods
    public void EnableInputs()
    {
        horizontalMoveAction.Enable();
        verticalMoveAction.Enable();
        jumpAction.Enable();
        interactAction.Enable();
        sprintAction.Enable();
        switchInterAction.Enable();
    }

    public void DisableInputs()
    {
        horizontalMoveAction.Disable();
        verticalMoveAction.Disable();
        jumpAction.Disable();
        interactAction.Disable();
        sprintAction.Disable();
        switchInterAction.Disable();
    }
    #endregion

    #region Private Methods
    private void OnJumpButtonDown(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            velocity.y = jumpPower;
            Avatar.SetTrigger("Jump");
        }
    }

    private void OnJumpButtonUp(InputAction.CallbackContext context)
    {
        if (velocity.y > 0f)
        {
            velocity.y = velocity.y * 0.5f;
        }
    }

    private void OnSprintButtonDown(InputAction.CallbackContext context)
    {
        if(SpeedModifier > 1)
        {
            SpeedModifier = 1;
        }
        else
        {
            SpeedModifier = 2;
        }
    }
    //private void OnSprintButtonUp(InputAction.CallbackContext context)
    //{
    //    SpeedModifier = 1;
    //}

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(EvtInteract != null)
        {
            EvtInteract.Invoke(context);
        }
    }

    private void OnSwitch(InputAction.CallbackContext context)
    {
        if (EvtSwitch != null)
        {
            EvtSwitch.Invoke(context);
        }
    }

    private void UpdateStateChanges(UnitState newState)
    {
        switch (newState)
        {
            case UnitState.OnGround:
                horizontalMoveAction.Enable();
                verticalMoveAction.Disable();
                jumpAction.Enable();
                interactAction.Enable();
                break;
            case UnitState.InAir:
                break;
            case UnitState.OnLadder:
                horizontalMoveAction.Disable();
                verticalMoveAction.Enable();
                jumpAction.Disable();
                SetGravityEnable(false);
                velocity = Vector2.zero;
                break;
            case UnitState.Dead:
                DisableInputs();
                break;
            default:
                DisableInputs();
                break;
        }
    }
    #endregion
}
