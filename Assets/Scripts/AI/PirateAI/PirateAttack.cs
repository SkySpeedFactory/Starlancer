using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAttack : State<PirateAIBehaviour>
{
    private State<PirateAIBehaviour> stateMove;
    private AICombatBehaviour currentBehavior = AICombatBehaviour.TARGETLOST;

    private bool hasTarget;
    private bool stopEscape = true;

    private float searchTime = 0.2f;
    private float brakeDis = 80f;

    public PirateAttack(PirateAIBehaviour owner) : base(owner)
    {

    }

    public void InitializeState(StateMachine<PirateAIBehaviour> stateMachine, State<PirateAIBehaviour> stateMove)
    {
        base.InitializeState(stateMachine);
        this.stateMove = stateMove;
    }

    public override void OnEnter()
    {
        hasTarget = false;
        
        owner.StartCoroutine(owner.Search(searchTime));
        owner.StartCoroutine(CombatSituation(2f));
    }

    public override void OnUpdate()
    {
        if (owner.AiStats.GetTarget() != null)
        {
            owner.SetTargetPosition(owner.AiStats.GetTarget().position);
        }

        owner.UpdateVelocity();
        owner.MoveEntity();
        owner.PrimaryAttack();
    }

    public override void OnExit()
    {
        owner.AiStats.SetPlayerAggro(false);        //Needs more testing
        owner.Movement.DisableForce(SteeringForces.Seek);
        owner.StopCoroutine(owner.Search(searchTime));
        owner.StopCoroutine(CombatSituation(1f));
    }

    private void CheckCombatSituation()
    {
        if (owner.AiStats.GetTarget() == null)
        {
            UpdateSteeringForces(AICombatBehaviour.TARGETLOST);
            return;
        }
        else if (currentBehavior == AICombatBehaviour.TARGETLOST)
        {
            UpdateSteeringForces(AICombatBehaviour.TARGETFOUND);
        }

        if (ChaseTarget())
        {
            UpdateSteeringForces(AICombatBehaviour.CHASETARGET);
            
        }
        else if(!stopEscape)
        {
            UpdateSteeringForces(AICombatBehaviour.EVADETARGET);
            
        }
    }

    private bool ChaseTarget()
    {
        if (Vector3.Dot(owner.transform.forward, (owner.AiStats.GetTarget().position - owner.transform.position).normalized) > 0.5)
        {
            
            return true;
        }

        if (!owner.SetStoppingDistance(40f))
        {
            return true;
        }

        stopEscape = false;

        return false;
    }

    private void UpdateSteeringForces(AICombatBehaviour newBehaviour)
    {
        if (newBehaviour == currentBehavior)
            return;

        switch (newBehaviour)
        {
            case AICombatBehaviour.CHASETARGET:
                owner.Movement.EnableForce(SteeringForces.Pursuit);
                owner.Movement.EnableForce(SteeringForces.Arrive);
                owner.Movement.DisableForce(SteeringForces.Evade);

                if (!hasTarget)
                {
                    owner.StopCoroutine(TargetLost(20f));
                    owner.Movement.DisableForce(SteeringForces.Seek);
                    hasTarget = true;
                }
                break;

            case AICombatBehaviour.EVADETARGET:
                stopEscape = false;
                owner.StartCoroutine(GetBackInCombat(3f));
                owner.Movement.EnableForce(SteeringForces.Evade);
                owner.Movement.DisableForce(SteeringForces.Arrive);
                owner.Movement.DisableForce(SteeringForces.Pursuit);

                if (!hasTarget)
                {
                    owner.StopCoroutine(TargetLost(20f));
                    owner.Movement.DisableForce(SteeringForces.Seek);
                    hasTarget = true;
                }
                break;

            case AICombatBehaviour.TARGETLOST:
                owner.Movement.EnableForce(SteeringForces.Seek);
                if (currentBehavior == AICombatBehaviour.CHASETARGET)
                {
                    owner.Movement.DisableForce(SteeringForces.Pursuit);
                    owner.Movement.DisableForce(SteeringForces.Arrive);
                }
                else
                {
                    owner.Movement.DisableForce(SteeringForces.Evade);
                }
                owner.StartCoroutine(TargetLost(20f));
                hasTarget = false;
                break;

            case AICombatBehaviour.TARGETFOUND:
                owner.Movement.EnableForce(SteeringForces.Seek);
                owner.Movement.EnableForce(SteeringForces.Arrive);
                owner.Movement.EnableForce(SteeringForces.Evade);
                owner.Movement.EnableForce(SteeringForces.Pursuit);
                break;

            default:
                break;
        }

        currentBehavior = newBehaviour;
    }

    private IEnumerator TargetLost(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stateMachine.ChangeState(stateMove);
    }

    private IEnumerator CombatSituation(float waitTime)
    {
        while (true)
        {
            CheckCombatSituation();
            yield return new WaitForSeconds(waitTime);
            
        }
        
    }
    private IEnumerator GetBackInCombat(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        stopEscape = true;
    }
}
