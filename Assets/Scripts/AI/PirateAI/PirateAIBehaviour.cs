using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateAIBehaviour : MovingEntity
{
    private SteeringForces steeringForcesAtStart;
    public SteeringForces currentSteeringForce;     //For Testing


    private StateMachine<PirateAIBehaviour> stateMachine;
    private PirateIdle stateIdle;
    private PirateMove stateMove;
    private PirateAttack stateAttack;

    private void OnEnable()
    {
        base.Start();

        SetUpStateMachine();
        currentSteeringForce = steeringForcesAtStart;
        Movement.EnableForce(SteeringForces.ObstacleAvoidance);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.OnUpdate();
    }

    private void SetUpStateMachine()
    {
        stateIdle = new PirateIdle(this);
        stateMove = new PirateMove(this);
        stateAttack = new PirateAttack(this);

        stateMachine = new StateMachine<PirateAIBehaviour>(this, stateIdle);

        stateIdle.InitializeState(stateMachine, stateMove, stateAttack);
        stateMove.InitializeState(stateMachine, stateAttack, stateIdle);
        stateAttack.InitializeState(stateMachine, stateMove);

        stateMachine.StartStateMachine();
    }

    public IEnumerator Search(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            SearchForTarget();
        }
    }

    private void SearchForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 500f);
        List<Transform> potentialTargets = new List<Transform>();
        List<Transform> nearbyAllies = new List<Transform>();

        if (colliders.Length == 0)
        {
            return;
        }

        foreach (var c in colliders)
        {
            if (c.transform.GetComponent<AIStats>() != null)
            {
                if (IsHostile(c.transform))
                {
                    potentialTargets.Add(c.transform);
                }

                if (c.transform.GetComponent<AIStats>().GetAIFaction() == AiStats.GetAIFaction())
                {
                    nearbyAllies.Add(c.transform);
                }
            }
            else if (c.transform.GetComponentInParent<PlayerController>() != null)
            {
                if (CheckPlayerHostility(c.transform))
                {
                    potentialTargets.Add(c.transform);
                }
            }
        }

        if (potentialTargets.Count == 0)
        {
            AiStats.SetTarget(null);
            return;
        }

        SetClosestTarget(potentialTargets);
        AlarmAllies(nearbyAllies);
    }

    private bool IsHostile(Transform potTarget)
    {
        if (potTarget.GetComponent<AIStats>() != null)
        {
            if (potTarget.GetComponent<AIStats>().GetAIFaction() != Factions.PIRATES)
            {
                return true;
            }
        }

        return false;
    }

    private void SetClosestTarget(List<Transform> potTargets)
    {
        float distance = 500f;

        foreach (var potTarget in potTargets)
        {
            if (Vector3.Distance(potTarget.position, transform.position) < distance)
            {
                AiStats.SetTarget(potTarget);
                distance = Vector3.Distance(potTarget.position, transform.position);
            }
        }
    }

    private bool CheckPlayerHostility(Transform player)
    {
        switch (MorialityManager.Instance.GetMoralityLevel())
        {
            case MoralityLevel.MARSHAL:
                //AiStats.SetTarget(player);
                //break;
                return true;
            case MoralityLevel.SHERIFF:
                //AiStats.SetTarget(player);
                //break;
                return true;
            case MoralityLevel.DEPUTY:
                return true;
            case MoralityLevel.CITIZEN:
                return true;
            case MoralityLevel.NEUTRAL:
                return true;
            case MoralityLevel.ROWDY:
                return true;
            case MoralityLevel.PETTYCRIMINAL:
                return true;
            case MoralityLevel.CRIMINAL:
                break;
            case MoralityLevel.PUBLICENEMY:
                break;
            default:
                break;
        }

        return AiStats.CheckPlayerAggro();
    }

    private void AlarmAllies(List<Transform> allies)
    {
        if (allies.Count == 0)
            return;

        foreach (var a in allies)
        {
            if (a.transform.GetComponent<AIStats>().GetTarget() == null)
            {
                a.transform.GetComponent<AIStats>().SetTarget(AiStats.GetTarget());
                if (AiStats.CheckPlayerAggro())
                {
                    a.transform.GetComponent<AIStats>().SetPlayerAggro(true);
                }
            }
        }
    }

    public StateMachine<PirateAIBehaviour> GetStateMachine()
    {
        return stateMachine;
    }

    public void SetStateMachine(StateMachine<PirateAIBehaviour> stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 500f);
    }
}
