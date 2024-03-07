using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMove : State<PoliceAIBehaviour>
{
    private State<PoliceAIBehaviour> stateAttack;
    private State<PoliceAIBehaviour> stateIdle;

    private Vector3 targetPosition;
    private int targetIndex = 0;

    private float searchTime = 1f;

    public PoliceMove(PoliceAIBehaviour owner) : base(owner)
    {

    }

    public void InitializeState(StateMachine<PoliceAIBehaviour> stateMachine, State<PoliceAIBehaviour> stateAttack, State<PoliceAIBehaviour> stateIdle)
    {
        base.InitializeState(stateMachine);
        this.stateAttack = stateAttack;
        this.stateIdle = stateIdle;
    }

    public override void OnEnter()
    {
        owner.Movement.EnableForce(SteeringForces.Seek);
        owner.StartCoroutine(owner.Search(searchTime));
        SetDestination();
    }

    public override void OnUpdate()
    {
        CheckArrived();

        if (owner.AiStats.GetTarget() != null)
            stateMachine.ChangeState(stateAttack);

        owner.UpdateVelocity();
        owner.MoveEntity();
    }

    public override void OnExit()
    {
        owner.Movement.DisableForce(SteeringForces.Seek);
        owner.StopCoroutine(owner.Search(searchTime));
    }

    private void SetDestination()
    {
        //targetPosition = owner.transform.position + owner.transform.forward * 300;      //Just for testing
        targetPosition = owner.AiStats.GetWayPoints()[targetIndex];
        owner.SetTargetPosition(targetPosition);
    }

    private void CheckArrived()
    {
        Vector3 dir = targetPosition - owner.transform.position;

        if (dir.sqrMagnitude < 20f)
        {
            targetIndex++;
            if (targetIndex >= owner.AiStats.GetWayPoints().Count)
            {
                targetIndex = 0;
            }
            stateMachine.ChangeState(stateIdle);
        }
    }
}
