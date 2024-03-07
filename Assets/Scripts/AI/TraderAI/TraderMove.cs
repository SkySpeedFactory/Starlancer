using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderMove : State<TraderAIBehaviour>
{
    private State<TraderAIBehaviour> stateIdle;

    private Vector3 targetPosition;
    private int targetIndex = 0;
    private bool stopEscape = true;

    private float searchTime = 1f;

    public TraderMove(TraderAIBehaviour owner) : base(owner)
    {

    }

    public void InitializeState(StateMachine<TraderAIBehaviour> stateMachine, State<TraderAIBehaviour> stateIdle)
    {
        base.InitializeState(stateMachine);
        this.stateIdle = stateIdle;
    }

    public override void OnEnter()
    {
        owner.Movement.EnableForce(SteeringForces.Seek);
        owner.StartCoroutine(owner.Search(searchTime));
        owner.StartCoroutine(ChangeManeuver(searchTime));
        SetDestination();
    }

    public override void OnUpdate()
    {
        if (!stopEscape && owner.AiStats.GetTarget() != null)
        {
            owner.SetTargetPosition(owner.AiStats.GetTarget().position);
        }

        CheckArrived();


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
        targetPosition = owner.AiStats.GetWayPoints()[targetIndex];
        owner.SetTargetPosition(targetPosition);
    }

    private void CheckArrived()
    {
        Vector3 dir = targetPosition - owner.transform.position;

        if (dir.sqrMagnitude < 20f && stopEscape)
        {
            targetIndex++;
            if (targetIndex >= owner.AiStats.GetWayPoints().Count)
            {
                targetIndex = 0;
            }
            stateMachine.ChangeState(stateIdle);
        }
    }

    private IEnumerator ChangeManeuver(float waitTime)
    {
        while (true)
        {
            if (stopEscape)
            {
                if (owner.AiStats.GetTarget() != null)
                {
                    owner.Movement.EnableForce(SteeringForces.Flee);
                    owner.Movement.DisableForce(SteeringForces.Seek);
                    owner.StartCoroutine(GetBackOnRoute(3f));
                    stopEscape = false;
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator GetBackOnRoute(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        owner.Movement.EnableForce(SteeringForces.Seek);
        owner.Movement.DisableForce(SteeringForces.Flee);
        owner.SetTargetPosition(targetPosition);
        stopEscape = true;
    }
}
