using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T>
{
    private State<T> currentState;

    private T owner;

    private bool isRunning = false;

    public State<T> GetCurrentState()
    {
        return currentState;
    }

    public StateMachine(T owner, State<T> startingState)
    {
        this.owner = owner;
        currentState = startingState;
    }

    public void StartStateMachine()
    {
        currentState.OnEnter();

        isRunning = true;
    }

    public void OnUpdate()
    {
        if (isRunning)
        {
            currentState.OnUpdate();
        }
    }

    public void ForceExit()
    {
        currentState.OnExit();
    }

    public void ChangeState(State<T> newState)
    {
        currentState.OnExit();

        currentState = newState;

        currentState.OnEnter();
    }
}
