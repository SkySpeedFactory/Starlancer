using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStats : MonoBehaviour
{
    [SerializeField] private ScriptableSmallShip shipData;
    [SerializeField] private ScriptableShield shieldData;

    [SerializeField] private GameObject[] visualDamageEffects;
    [SerializeField] private ParticleSystem explosionParticleSystem;

    [SerializeField] private Factions aiFaction;
    private Transform target;
    private List<Vector3> wayPoints = new List<Vector3>();

    private EnumSmallShipTypes shipType;

    private float maxHealth;
    private float currentHealth;

    private float shieldAbsorbCapacity;
    private float shieldRechargeRate;

    private float yawPitchTorque;
    private float rollTorque;

    private float maxThrusterSpeed;
    private float maxSideThrusterSpeed;
    private float maxUpDownThrusterSpeed;

    private float mainThrusterAcceleration;
    private float sideThrusterAcceleration;
    private float upDownThrusterAcceleration;
    private float rollAcceleration;

    private ForceFieldController forceFieldController;

    private bool isPlayerAggresive = false;
    private bool canCheckPlayerInventory;
    //private float maxBoosterSpeed;


    private float lastHitMaxTimer = 5f;
    private float lastHitCountDown;
    private void Awake()
    {
        InitializeShipData();

    }
    // Start is called before the first frame update
    void Start()
    {
        //forceFieldController = transform.GetChild(0).GetComponent<ForceFieldController>();
        //forceFieldController.SetOpenCloseValue(2);

        lastHitCountDown = lastHitMaxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        RechargeShield();
    }

    private void InitializeShipData()
    {
        shipType = shipData.ShipType;

        maxHealth = shipData.MaxHealth;
        currentHealth = maxHealth;

        yawPitchTorque = shipData.YawPitchTorque;
        rollTorque = shipData.RollTorque;

        maxThrusterSpeed = shipData.Thruster;
        maxSideThrusterSpeed = shipData.SideThruster;
        maxUpDownThrusterSpeed = shipData.UpDownThruster;

        mainThrusterAcceleration = shipData.MainThrusterAcceleration;
        sideThrusterAcceleration = shipData.SideThrusterAcceleration;
        upDownThrusterAcceleration = shipData.UpdDownAcceleration;
        rollAcceleration = shipData.RollAcceleration;
        //maxBoosterSpeed = shipData.MaxBoosterSpeed;

        InitializeShieldData();
    }

    private void InitializeShieldData()
    {
        shieldAbsorbCapacity = shieldData.AbsorbCapacity;
        shieldRechargeRate = shieldData.RechargeRate;
    }

    private void RechargeShield()
    {
        if (shieldAbsorbCapacity != shieldData.AbsorbCapacity)
        {
            lastHitCountDown -= Time.deltaTime;
            if (lastHitCountDown <= 0 && shieldAbsorbCapacity < shieldData.AbsorbCapacity)
            {
                shieldAbsorbCapacity += Time.deltaTime * shieldRechargeRate;
                if (shieldAbsorbCapacity > shieldData.AbsorbCapacity)
                {
                    shieldAbsorbCapacity = shieldData.AbsorbCapacity;
                }
            }
            
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetYawPitchTorque()
    {
        return yawPitchTorque;
    }


    public float GetRollTorque()
    {
        return rollTorque;
    }

    public float GetMainThrusterSpeed()
    {
        return maxThrusterSpeed;
    }

    public float GetSideThrusterSpeed()
    {
        return maxSideThrusterSpeed;
    }

    public float GetUpDownThrusterSpeed()
    {
        return maxUpDownThrusterSpeed;
    }

    public float GetMainThrusterAcceleration()
    {
        return mainThrusterAcceleration;
    }

    public float GetSideThrusterAcceleration()
    {
        return sideThrusterAcceleration;
    }

    public float GetUpDownThrusterAcceleration()
    {
        return upDownThrusterAcceleration;
    }

    public float GetRollAcceleration()
    {
        return rollAcceleration;
    }

    public Factions GetAIFaction()
    {
        return aiFaction;
    }

    public bool GetCanCheckPlayerInventory()
    {
        return canCheckPlayerInventory;
    }

    public Transform GetTarget()
    {
        return target;
    }

    public List<Vector3> GetWayPoints()
    {
        return wayPoints;
    }

    public bool CheckPlayerAggro()
    {
        return isPlayerAggresive;
    }

    public void SetWayPoints(List<Transform> points, Vector3 offset)
    {
        wayPoints.Clear();
        foreach (var p in points)
        {
            wayPoints.Add(p.position + offset);
        }
        
    }

    public void SetAIFaction(Factions faction)
    {
        aiFaction = faction;
    }

    public void SetCanCheckPlayerInventory(bool canCheckInventory)
    {
        canCheckPlayerInventory = canCheckInventory;
    }

    public void SetPlayerAggro(bool isAgressive)
    {
        isPlayerAggresive = isAgressive;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetDamage(float incomingDamage, bool isPlayer)
    {
        lastHitCountDown = lastHitMaxTimer;
        if (shieldAbsorbCapacity > 0)
        {
            shieldAbsorbCapacity -= incomingDamage;
            //isHit = false;
            if (shieldAbsorbCapacity < 0)
            {
                currentHealth -= shieldAbsorbCapacity * -1;
                shieldAbsorbCapacity = 0;
            }
        }
        else
        {
            currentHealth -= incomingDamage;

            if (isPlayer)
            {
                SetPlayerAggro(true);
            }
            //isHit = true;
        }

        if (currentHealth <= 0)
        {
            MorialityManager.Instance.SetMoriality(aiFaction, 200);
            TriggerExplosion();
            AIManager.Instance.RemoveAI(this.gameObject);
            isPlayerAggresive = false;
            gameObject.GetComponent<MapObject>().DeactivateIcon();
            gameObject.SetActive(false);
            // Respawn if needed
        }
        else
        {
            //VisualiceDamage();
        }
    }

    private void TriggerExplosion()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        //gameObject.GetComponent<PlayerController>().enabled = false;

        ParticleSystem explosionParticles = Instantiate(explosionParticleSystem, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosionParticles.gameObject, 10);
        // Show death text on ui

    }
}
