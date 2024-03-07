using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateIdle : State<PirateAIBehaviour>
{
    private State<PirateAIBehaviour> stateMove;
    private State<PirateAIBehaviour> stateAttack;

    private float searchTime = 2f;

    public PirateIdle(PirateAIBehaviour owner) : base(owner)
    {

    }

    public void InitializeState(StateMachine<PirateAIBehaviour> stateMachine, State<PirateAIBehaviour> stateMove, State<PirateAIBehaviour> stateAttack)
    {
        base.InitializeState(stateMachine);
        this.stateMove = stateMove;
        this.stateAttack = stateAttack;
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
            stateMachine.ChangeState(stateAttack);
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
