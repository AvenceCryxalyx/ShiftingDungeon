using UnityEngine;

public class UnitController : PhysicsObject
{
    #region Inspector Fields
    [SerializeField]
    protected float defaultMoveSpeed = 4f;
    #endregion

    #region Fields
    protected bool isXMovementEnabled = false;
    protected bool isYMovementEnabled = false;
    #endregion

    #region Properties
    public float CurrentMoveSpeed { get; protected set; }
    public int SpeedModifier { get; protected set; }
    public float InputX { get; protected set; }
    public float InputY { get; protected set; }
    #endregion

    #region Unity Methods
    protected override void OnEnable()
    {
        SpeedModifier = 1;
        base.OnEnable();
    }
    #endregion

    #region Overrides
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        CurrentMoveSpeed = defaultMoveSpeed * SpeedModifier;

        move.x = InputX;
        targetVelocity = move * CurrentMoveSpeed;
    }
    #endregion

    #region Public Methods
    public void SetXMovementEnable(bool enable = true)
    {
        isXMovementEnabled = enable;
    }

    public void SetYMovementEnable(bool enable = true)
    {
        isYMovementEnabled = enable;
    }

    public void SetMovementEnable(bool enable = true)
    {
        SetXMovementEnable(enable);
        SetYMovementEnable(enable);
    }
    #endregion
}
