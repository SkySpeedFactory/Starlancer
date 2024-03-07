using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderIdle : State<TraderAIBehaviour>
{
    private State<TraderAIBehaviour> stateMove;

    private float searchTime = 2f;

    public TraderIdle(TraderAIBehaviour owner) : base(owner)
    {

    }

    public void InitializeState(StateMachine<TraderAIBehaviour> stateMachine, State<TraderAIBehaviour> stateMove)
    {
        base.InitializeState(stateMachine);
        this.stateMove = stateMove;
    }

    public override void OnEnter()
    {
        owner.StartCoroutine(owner.Search(searchTime));
        owner.StartCoroutine(IdleTime(2f));
        owner.Movement.EnableForce(SteeringForces.Inactive);
    }

    public override void OnUpdate()
    {
        if (owner.AiStats.GetTarget() != null)
        {
            stateMachine.ChangeState(stateMove);
        }
    }

    public override void OnExit()
    {
        owner.StopCoroutine(owner.Search(searchTime));
        owner.StopCoroutine(IdleTime(2f));
        owner.Movement.DisableForce(SteeringForces.Inactive);
    }

    private IEnumerator IdleTime(float idleTime)
    {
        yield return new WaitForSeconds(idleTime);
        stateMachine.ChangeState(stateMove);
    }
}
