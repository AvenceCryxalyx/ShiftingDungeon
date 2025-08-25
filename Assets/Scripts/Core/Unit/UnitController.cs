using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : PhysicsObject
{
    public enum UnitState
    {
        None,
        OnGround,
        InAir,
        OnLadder,
        Dead
    }
    #region Events
    public Action<UnitState, UnitState> EvtStateChanged;
    #endregion

    #region Inspector Fields
    [SerializeField]
    protected float defaultMoveSpeed = 4f;
    #endregion

    #region Fields
    protected bool isXMovementEnabled = false;
    protected bool isYMovementEnabled = false;
    #endregion

    #region Properties
    public Avatar Avatar { get; protected set; }
    public UnitState CurrentState { get; protected set; }
    public float CurrentMoveSpeed { get; protected set; }
    public int SpeedModifier { get; protected set; }
    public float InputX { get; protected set; }
    public float InputY { get; protected set; }
    #endregion

    #region Unity Methods
    protected virtual void Awake()
    {
        Avatar = GetComponentInChildren<Avatar>();
    }

    protected override void OnEnable()
    {
        SpeedModifier = 1;
        EvtStateChanged += OnStateChanged;
        base.OnEnable();
    }

    protected virtual void OnDisable()
    {
        EvtStateChanged -= OnStateChanged;
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

    public void SetMovementEnable(bool enable = true)
    {
        SetXMovementEnable(enable);
        SetYMovementEnable(enable);
    }

    public void ChangeState(UnitState state)
    {
        if(CurrentState == state)
        {
            return;
        }
        if(EvtStateChanged != null)
        {
            EvtStateChanged.Invoke(CurrentState, state);
        }
        CurrentState = state;
    }
    #endregion

    #region Virtual Methods
    public virtual void SetXMovementEnable(bool enable = true)
    {
        isXMovementEnabled = enable;
    }

    public virtual void SetYMovementEnable(bool enable = true)
    {
        isYMovementEnabled = enable;
    }
    protected virtual void OnStateChanged(UnitState oldState, UnitState newState)
    {

    }
    #endregion
}
