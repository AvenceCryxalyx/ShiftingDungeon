using UnityEngine;

[RequireComponent(typeof(UnitController))]
public class StateController : MonoBehaviour
{
    [SerializeField]
    private Transform stateTransformParent;
    [SerializeField]
    private StateMachineSO stateMachineSO;

    public bool IsActive { get; protected set; }
    public State CurrentState { get; protected set; }
    public State DefaultState {  get; protected set; }

    public UnitController Unit { get; private set; }
    public bool CanDoStateUpdate { get; protected set; }

    private void Awake()
    {
        Unit = GetComponent<UnitController>();
        if(stateTransformParent == null )
        {
            GameObject stateParent = new GameObject();
            stateParent.transform.SetParent(this.transform);
            stateTransformParent = stateParent.transform;
        }
    }

    private void Start()
    {
        foreach (StateSO state in stateMachineSO.AllPossibleStates)
        {
            State newState = Instantiate(state.StatePrefab, stateTransformParent);
            newState.transform.position = Vector3.zero;
            newState.Initialize(state, this);
        }
    }

    private void Update()
    {
        if(!IsActive || Unit == null || !CanDoStateUpdate || CurrentState)
        {
            return;
        }
        if (CurrentState)
        {
            CurrentState.CheckTransitions();
        }
        CurrentState.Do();
    }

    public void ChangeState(State to)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = to;
        CurrentState.Enter();
    }

    private void OnStateEnter()
    {
        CanDoStateUpdate = true;
    }

    private void OnStateExit()
    {
        CanDoStateUpdate = false;
    }
}
