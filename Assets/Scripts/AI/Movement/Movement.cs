using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum SteeringForces
{
    Inactive = 0,
    Seek = 1,
    Flee = 2,
    Arrive = 4,
    Pursuit = 8,
    Evade = 16,
    Interpose = 32,
    OffsetPursuit = 64,
    ObstacleAvoidance = 128
}

public class Movement
{
    private SteeringForces steeringForces;

    private MovingEntity owner;

    private Vector3 steeringForce;

    private float minDetectionLength = 0.5f;
    private float maxDetectionLength = 100f;
    private float sideDetectionLength = 1f;


    public Movement(MovingEntity owner)
    {
        this.owner = owner;
    }

    public Vector3 Calculate(Vector3 targetPos)
    {
        return CalculateSimple(targetPos);
    }

    #region CALCULATION
    private Vector3 CalculateSimple(Vector3 targetPos)
    {
        steeringForce = Vector3.zero;

        if (IsOn(SteeringForces.Seek))
            steeringForce += Seek(targetPos);
        if (IsOn(SteeringForces.Flee))
            steeringForce += Flee(targetPos);
        if (IsOn(SteeringForces.ObstacleAvoidance))
            steeringForce += ObstacleAvoidance(targetPos);
        if (IsOn(SteeringForces.Arrive))
            steeringForce += Arrive(targetPos, 1 / owner.AiStats.GetMainThrusterAcceleration());
        if (IsOn(SteeringForces.Pursuit) && owner.AiStats.GetTarget() != null)
            steeringForce += Pursuit(owner.AiStats.GetTarget(), targetPos);
        if (IsOn(SteeringForces.Evade) && owner.AiStats.GetTarget() != null)
            steeringForce += Evade(owner.AiStats.GetTarget(), targetPos);

        return steeringForce;
    }

    private Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = (targetPos - owner.transform.position).normalized * owner.GetMaxSpeed();

        return desiredVelocity - owner.GetVelocity();
    }

    private Vector3 Flee(Vector3 fleeTargetPos, float fleeDistance)
    {
        fleeDistance *= fleeDistance;

        if (Vector3.SqrMagnitude(fleeTargetPos - owner.transform.position) <= fleeDistance)
        {
            return Flee(fleeTargetPos);
        }
        else
        {
            return Vector3.zero;
        }
    }

    private Vector3 Flee(Vector3 fleeTargetPos)
    {
        Vector3 desiredVelocity = (owner.transform.position - fleeTargetPos).normalized * owner.GetMaxSpeed();

        return desiredVelocity - owner.GetVelocity();
    }


    private Vector3 Arrive(Vector3 targetPos, float deceleration)
    {
        deceleration = Mathf.Clamp(deceleration, 0, 0.2f);
        deceleration = 1 - deceleration;

        Vector3 toTarget = targetPos - owner.transform.position;
        float distSqr = Vector3.SqrMagnitude(toTarget);

        if (distSqr <= 0)
            return Vector3.zero;

        float distance = Mathf.Sqrt(distSqr);

        float speed = distance * deceleration;
        speed = Mathf.Min(speed, owner.GetMaxSpeed());

        Vector3 desiredVelocity = (toTarget / distance) * speed;

        return desiredVelocity - owner.GetVelocity();
    }


    private Vector3 Pursuit(Transform target, Vector3 targetPos)
    {
        if (Mathf.Abs(Vector3.Dot(target.transform.forward, owner.transform.forward)) > 0.95f)
            return Seek(targetPos);

        Vector3 toTarget = targetPos - owner.transform.position;

        float lookAheadTime;

        if (target.GetComponent<MovingEntity>() != null)
        {
            MovingEntity aiTarget = target.GetComponent<MovingEntity>();
            lookAheadTime = toTarget.magnitude / (owner.GetMaxSpeed() + aiTarget.GetVelocity().magnitude);

            return Seek(targetPos + aiTarget.GetVelocity() * lookAheadTime);
        }

        if (target.GetComponent<PlayerController>() != null)
        {
            PlayerController playerTarget = target.GetComponent<PlayerController>();
            lookAheadTime = toTarget.magnitude / (owner.GetMaxSpeed() + playerTarget.GetPlayerVelocity().magnitude);

            return Seek(targetPos + playerTarget.GetPlayerVelocity() * lookAheadTime);
        }
        //PlayerController playerTarget = target.GetComponent<PlayerController>();
        return Vector3.zero;
    }

    private Vector3 Evade(Transform pursuer, Vector3 pursuerPos)
    {
        Vector3 toPursuer = pursuerPos - owner.transform.position;

        float lookAheadTime;

        if (pursuer.GetComponent<MovingEntity>() != null)
        {
            MovingEntity aiPursuer = pursuer.GetComponent<MovingEntity>();
            lookAheadTime = toPursuer.magnitude / (owner.GetMaxSpeed() + aiPursuer.GetVelocity().magnitude);
            return Flee(pursuerPos + aiPursuer.GetVelocity() * lookAheadTime);
        }
        
        if (pursuer.GetComponent<PlayerController>() != null)
        {
            PlayerController playerPursuer = pursuer.GetComponent<PlayerController>();
            lookAheadTime = toPursuer.magnitude / (owner.GetMaxSpeed() + playerPursuer.GetPlayerVelocity().magnitude);
            return Flee(pursuerPos + playerPursuer.GetPlayerVelocity() * lookAheadTime);
        }
        return Vector3.zero;
    }

    private Vector3 ObstacleAvoidance(Vector3 targetPos)
    {
        float detectionLength = Mathf.Max(minDetectionLength, owner.GetVelocity().magnitude / owner.GetMaxSpeed() * maxDetectionLength);
        

        RaycastHit[][] hits = new RaycastHit[][]
        {
            Physics.SphereCastAll(owner.transform.position, owner.GetDetectionWidth(), owner.transform.forward, detectionLength, owner.ObstacleLayerMask),
            Physics.SphereCastAll(owner.transform.position + owner.transform.up, owner.GetDetectionWidth(), owner.transform.up, sideDetectionLength, owner.ObstacleLayerMask),
            Physics.SphereCastAll(owner.transform.position - owner.transform.up, owner.GetDetectionWidth(), -owner.transform.up, sideDetectionLength, owner.ObstacleLayerMask),
            Physics.SphereCastAll(owner.transform.position + owner.transform.right *10f, owner.GetDetectionWidth(), owner.transform.right, sideDetectionLength, owner.ObstacleLayerMask),
            Physics.SphereCastAll(owner.transform.position - owner.transform.right *10f, owner.GetDetectionWidth(), -owner.transform.right, sideDetectionLength, owner.ObstacleLayerMask),
        };

        Debug.DrawRay(owner.transform.position, owner.transform.forward * detectionLength, Color.blue);
        Debug.DrawRay(owner.transform.position + owner.transform.up, owner.transform.up * sideDetectionLength, Color.blue);
        Debug.DrawRay(owner.transform.position - owner.transform.up, -owner.transform.up * sideDetectionLength, Color.blue);
        Debug.DrawRay(owner.transform.position + owner.transform.right * 10f, owner.transform.right * sideDetectionLength, Color.blue);
        Debug.DrawRay(owner.transform.position - owner.transform.right * 10f, -owner.transform.right * sideDetectionLength, Color.blue);


        GameObject ClosestHit = null;
        float closestDistanceSqr = (targetPos - owner.transform.position).sqrMagnitude;
        Vector3 hitPoint = Vector3.zero;    //For Debug

        foreach (var h in hits)
        {
            
            if (h.Length == 0)
                continue;
            
            for (int i = 0; i < h.Length; i++)
            {
                if (h[i].collider.gameObject == owner.gameObject)
                {
                    continue;
                }

                float sqrDistance = Vector3.SqrMagnitude(h[i].point - owner.transform.position);

                if (closestDistanceSqr > sqrDistance)
                {
                    ClosestHit = h[i].collider.gameObject;
                    closestDistanceSqr = sqrDistance;
                    hitPoint = h[i].point;
                }

            }
        }

        if (ClosestHit == null)
            return Vector3.zero;

        Debug.DrawLine(owner.transform.position, hitPoint, Color.magenta);


        Vector3 localPos = owner.transform.InverseTransformVector(hitPoint);
        float localForwardDifference = localPos.z;
        float localRightDifference = localPos.x;
        float localUpDifference = localPos.y;

        float proximityMultiplier = 1 + (detectionLength - localForwardDifference) / detectionLength;

        float perpendicularHorizontalForce = (hitPoint.x - localRightDifference) * proximityMultiplier * 0.1f;

        float perpendicularVerticalForce = (hitPoint.y - localUpDifference) * proximityMultiplier * 0.1f;

        float brakingWeight = 0.2f;

        float brakeForce = (hitPoint.x - localForwardDifference + hitPoint.y - localForwardDifference) * brakingWeight;

        float perpHorizontalFactor = Vector3.Cross(owner.transform.forward, hitPoint - owner.transform.position).y > 0 ? -1 : 1;
        float perpVerticalFactor = Vector3.Cross(owner.transform.forward, hitPoint - owner.transform.position).x > 0 ? 1 : -1;

        return owner.transform.right * perpendicularHorizontalForce * perpHorizontalFactor + owner.transform.up * perpendicularVerticalForce * perpVerticalFactor - owner.transform.forward * brakeForce;


    }
    #endregion

    #region ENABLE/DISABLE

    public void EnableForce(SteeringForces force)
    {
        steeringForces |= force;
    }

    public void DisableForce(SteeringForces force)
    {
        steeringForces ^= force;
    }

    private bool IsOn(SteeringForces force)
    {
        return (steeringForces & force) == force;
    }

    #endregion

    
}
