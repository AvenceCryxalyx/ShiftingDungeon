using UnityEngine;
using UnityEngine.InputSystem;
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

    #region Fields
    protected int jumpCount;
    protected float horizontalMove;
    protected PlayerInputAction playerInput;
    protected InputAction moveAction;
    protected InputAction jumpAction;
    protected InputAction sprintAction;
    protected InputAction interactAction;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        playerInput = new PlayerInputAction();
        moveAction = playerInput.Avatar.HorizontalMovement;
        jumpAction = playerInput.Avatar.Jump;
        sprintAction = playerInput.Avatar.Sprint;
        //interactAction = 
        jumpAction.performed += OnJumpButtonDown;
        jumpAction.canceled += OnJumpButtonUp;
        sprintAction.performed += OnSprintButtonDown;
        sprintAction.canceled += OnSprintButtonUp;
    }

    private void OnDisable()
    {
        DisableInputs();
    }
    #endregion

    #region Overrides
    protected override void OnEnable()
    {
        base.OnEnable();
        EnableInputs();
    }

    protected override void Update()
    {
        InputX = moveAction.ReadValue<float>();
        base.Update();
    }
    #endregion

    #region Public Methods
    public void EnableInputs()
    {
        moveAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
    }

    public void DisableInputs()
    {
        moveAction.Disable();
        jumpAction.Disable();

    }
    #endregion

    #region Private Methods
    private void OnJumpButtonDown(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            velocity.y = jumpPower;
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
        SpeedModifier = 2;
    }
    private void OnSprintButtonUp(InputAction.CallbackContext context)
    {
        SpeedModifier = 1;
    }
    #endregion
}
