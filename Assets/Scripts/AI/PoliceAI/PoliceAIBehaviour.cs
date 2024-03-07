using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAIBehaviour : MovingEntity
{
    
    private SteeringForces steeringForcesAtStart;
    public SteeringForces currentSteeringForce;     //For Testing


    private StateMachine<PoliceAIBehaviour> stateMachine;
    private PoliceIdle stateIdle;
    private PoliceMove stateMove;
    private PoliceAttack stateAttack;

    private bool hasCheckedPlayer = false;


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
        stateIdle = new PoliceIdle(this);
        stateMove = new PoliceMove(this);
        stateAttack = new PoliceAttack(this);

        stateMachine = new StateMachine<PoliceAIBehaviour>(this, stateIdle);

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
                //if (AiStats.GetTarget() != null)
                //{
                //    return;
                //}
            }
            //if(c.transform.GetComponent<AIStats>() != null)
            //{

            //}
        }

        //Debug.Log(potentialTargets.Count);
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
        if(potTarget.GetComponent<AIStats>() != null)
        {
            if(potTarget.GetComponent<AIStats>().GetAIFaction() == Factions.PIRATES)
            {
                return true;
            }
        }

        return false;
    }

    private void SetClosestTarget(List<Transform> potTargets)
    {
        if (potTargets.Count == 0)
            return;

        float distance = 500f;

        foreach (var potTarget in potTargets)
        {
            if(Vector3.Distance(potTarget.position, transform.position) < distance)
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
                //SetTarget(player);
                //break;
                return true;
            case MoralityLevel.CRIMINAL:
                //SetTarget(player);
                //break;
                return true;
            case MoralityLevel.PETTYCRIMINAL:
                if (AiStats.GetCanCheckPlayerInventory() && !hasCheckedPlayer)
                {
                    return CheckPlayerInventory(20, player);
                }
                break;
            case MoralityLevel.ROWDY:
                if (AiStats.GetCanCheckPlayerInventory() && !hasCheckedPlayer)
                {
                    return CheckPlayerInventory(15, player);
                }
                break;
            case MoralityLevel.NEUTRAL:
                if (AiStats.GetCanCheckPlayerInventory() && !hasCheckedPlayer)
                {
                    return CheckPlayerInventory(10, player);
                }
                break;
            case MoralityLevel.CITIZEN:
                if (AiStats.GetCanCheckPlayerInventory() && !hasCheckedPlayer)
                {
                    return CheckPlayerInventory(5, player);
                }
                break;
            case MoralityLevel.DEPUTY:
                if (AiStats.GetCanCheckPlayerInventory() && !hasCheckedPlayer)
                {
                    return CheckPlayerInventory(1, player);
                }
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

    private bool CheckPlayerInventory(int searchChance, Transform player)
    {
        if (hasCheckedPlayer)
            return false;

        AIManager.Instance.PlayerInventoryCheck();
        int randomPercent = Random.Range(0,100);
        if (randomPercent <= searchChance)
        {
            foreach (var item in player.GetComponentInParent<PlayerInventory>().GetPlayerInventory())
            {
                if (item.Item.IsIllegal)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool GetHasCheckedPlayer()
    {
        return hasCheckedPlayer;
    }

    public void SetHasCheckedPlayer(bool hasChecked)
    {
        hasCheckedPlayer = hasChecked;
    }

    public void RestartStateMachine()
    {
        SetUpStateMachine(); 
        currentSteeringForce = steeringForcesAtStart;
        Movement.EnableForce(SteeringForces.ObstacleAvoidance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 500f);
    }
}
