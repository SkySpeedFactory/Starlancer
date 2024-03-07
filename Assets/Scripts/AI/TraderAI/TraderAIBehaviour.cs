using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderAIBehaviour : MovingEntity
{
    public SteeringForces steeringForcesAtStart;
    public SteeringForces currentSteeringForce;     //For Testing


    private StateMachine<TraderAIBehaviour> stateMachine;
    private TraderIdle stateIdle;
    private TraderMove stateMove;


    private void OnEnable()
    {
        base.Start();

        SetUpStateMachine();
        currentSteeringForce = steeringForcesAtStart;
        Movement.EnableForce(SteeringForces.ObstacleAvoidance);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.OnUpdate();
    }

    private void SetUpStateMachine()
    {
        stateIdle = new TraderIdle(this);
        stateMove = new TraderMove(this);

        stateMachine = new StateMachine<TraderAIBehaviour>(this, stateIdle);

        stateIdle.InitializeState(stateMachine, stateMove);
        stateMove.InitializeState(stateMachine, stateIdle);

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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 600f);
        List<Transform> potentialTargets = new List<Transform>();
        //if (GetTarget() == null)
        //{
        //    colliders = Physics.OverlapSphere(transform.position, 500f);
        //}
        //else
        //{
        //    colliders = Physics.OverlapCapsule(transform.forward, transform.forward * 150f, DetectionWidth);
        //}

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

                //SetTarget(c.transform);
                //return;
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
    }

    private bool IsHostile(Transform potTarget)
    {
        if (potTarget.GetComponent<AIStats>() != null)
        {
            if (potTarget.GetComponent<AIStats>().GetAIFaction() == Factions.PIRATES)
            {
                return true;
            }
        }

        return false;
    }

    private void SetClosestTarget(List<Transform> potTargets)
    {
        float distance = 600f;

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
            case MoralityLevel.PUBLICENEMY:
                AiStats.SetTarget(player);
                break;
            case MoralityLevel.CRIMINAL:
                AiStats.SetTarget(player);
                break;
            case MoralityLevel.PETTYCRIMINAL:
                return true;
            case MoralityLevel.ROWDY:
                return true;
            default:
                break;
        }

        return AiStats.CheckPlayerAggro();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 600f);
    }
}
