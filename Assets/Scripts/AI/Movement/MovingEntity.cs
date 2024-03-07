using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingEntity : MonoBehaviour
{
    
    private int shipID;
    public AIStats AiStats;
    public Movement Movement;

    [SerializeField] private GameObject bulletPrefab;
    private Weapon[] weaponSlots;

    private Vector3 velocity;
    private float maxSpeed;
    private float activeMainThrusterSpeed;

    private float detectionWidth;
    public LayerMask ObstacleLayerMask;

    private List<Vector3> wayPoints;
    private Vector3 targetPos;

    
    private float maxSpeedSqr;

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetDetectionWidth()
    {
        return detectionWidth;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPos;
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Movement = new Movement(this);
        InitializeShipComponents();
        InitializeWeapons();
        maxSpeed = AiStats.GetMainThrusterSpeed();

        maxSpeedSqr = maxSpeed*maxSpeed;

        detectionWidth = 10f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void UpdateVelocity()
    {
        Vector3 steeringForce = Movement.Calculate(targetPos);

        //Vector3 accel = Vector3.Slerp(Velocity, steeringForce, aiStats.GetMainThrusterAcceleration());
        //Vector3 accel = steeringForce / aiStats.GetMainThrusterAcceleration();

        //Velocity += accel * Time.deltaTime;

        velocity += Vector3.Slerp(velocity, steeringForce, AiStats.GetMainThrusterAcceleration()) * Time.deltaTime;

        if (velocity.sqrMagnitude > maxSpeedSqr)
        {
            
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        activeMainThrusterSpeed = Mathf.Lerp(activeMainThrusterSpeed, velocity.magnitude, AiStats.GetMainThrusterAcceleration() * Time.deltaTime);
        Debug.DrawRay(transform.position, steeringForce,Color.red);
        Debug.DrawLine(transform.position, targetPos);
        Debug.DrawRay(transform.position, velocity, Color.green);

    }

    public virtual void MoveEntity()
    {
        Quaternion rotation = Quaternion.LookRotation(velocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, AiStats.GetRollAcceleration() * Time.deltaTime);

        //transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.position += transform.forward * activeMainThrusterSpeed * Time.deltaTime;
    }

    public virtual void InitializeShipComponents()
    {
        shipID = gameObject.transform.gameObject.GetInstanceID();
        AiStats = gameObject.GetComponent<AIStats>();
    }

    private void InitializeWeapons()
    {
        weaponSlots = new Weapon[transform.GetChild(0).childCount];
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            //Ship-Container/ShipGraphics/ShipModel/WeaponSlots/WeaponSlot/Weapon
            //print(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i));
            if (transform.GetChild(0).GetChild(i).childCount == 1)
            {
                weaponSlots[i] = transform.GetChild(0).GetChild(i).GetChild(0).GetComponent<Weapon>();
            }
            else
            {
                Debug.LogError($"More then 1 or 0 weapon found in slot {i}. Count weapons in slot: {transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).childCount}");
            }
        }
    }

    public void PrimaryAttack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionWidth, transform.forward, 250);

        if (hits.Length == 0)
            return;

        foreach (var h in hits)
        {
            if(h.collider.transform == AiStats.GetTarget())
            {
                foreach (var weapon in weaponSlots)
                {
                    weapon.Shoot(shipID, false);
                }
                break;
            }
        }
    }

    public bool SetStoppingDistance(float stoppingDis)
    {
        if(AiStats.GetTarget() != null)
        {
            
            Vector3 dir = transform.position - AiStats.GetTarget().position;
            float sqrDis = stoppingDis * stoppingDis;
            targetPos = AiStats.GetTarget().position;
            if (dir.sqrMagnitude > sqrDis)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(-100, 100);
        float y = Random.Range(-100, 100);
        float z = Random.Range(-100, 100);

        return transform.position + new Vector3(x, y, z);
    }
}
