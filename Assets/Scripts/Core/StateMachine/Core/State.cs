using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    #region Events
    public Action OnEntered;
    public Action OnExited;
    #endregion

    #region Properties
    public string AnimationName { get; protected set; }
    public StateController Bound { get; protected set; }
    #endregion

    #region Fields
    protected Transition[] transitions;
    private Coroutine currentStateTask;
    private Transition transitionToNextState;
    private bool isTransitioning;
    protected bool hasAnimation;
    #endregion

    #region Virtual Methods
    public virtual IEnumerator OnEnter() 
    {
        if (Bound.Unit != null && hasAnimation)
        {
            Bound.Unit.Avatar.PlayAnimation(AnimationName);
        }
        yield return null; 
    }
    public virtual void Do() { }
    public virtual IEnumerator OnExit() { yield return null; }


    public virtual void Initialize(StateSO stateSO, StateController bound)
    {
        Bound = bound;
        hasAnimation = stateSO.HasAnimation;
        AnimationName = stateSO.AnimationName;
        transitions = new Transition[stateSO.transitions.Count];
    }
    #endregion

    #region Public Methods
    public void CheckTransitions()
    {
        if(isTransitioning)
        {
            return;
        }

        foreach (var transition in transitions)
        {
            if(transition.CheckTransitionCondition(Bound))
            {
                transition.DoTranstion(Bound);
                isTransitioning = false;
            }
        }
    }

    public void Enter()
    {
        isTransitioning = true;
        InitializeTransitions();
        currentStateTask = StartCoroutine(DoEnter());
    }

    public void Exit()
    {
        isTransitioning = true;
        DeinitializeTransitions();
        currentStateTask = StartCoroutine(DoExit());
    }
    #endregion

    #region Private Methods
    private IEnumerator DoEnter()
    {
        yield return StartCoroutine(OnEnter());
        if(OnEntered != null)
        {
            OnEntered.Invoke();
        }
        currentStateTask = null;
        yield return null;
        isTransitioning = false;
    }

    private IEnumerator DoExit()
    {
        yield return StartCoroutine(OnExit());
        if (OnExited != null)
        {
            OnExited.Invoke();
        }
        currentStateTask = null;
        yield return null;
        isTransitioning = false;
    }

    private void InitializeTransitions()
    {
        foreach (var transition in transitions)
        {
            transition.Activate(Bound);
        }
    }

    private void DeinitializeTransitions()
    {
        foreach (var transition in transitions)
        {
            transition.Activate(Bound);
        }
    }
    #endregion
}
