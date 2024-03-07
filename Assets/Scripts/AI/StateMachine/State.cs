using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected T owner;

    protected StateMachine<T> stateMachine;

    public State(T owner)
    {
        this.owner = owner;
    }

    public virtual void InitializeState(StateMachine<T> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnExit()
    {

    }
}
