using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats _instance;
    public static PlayerStats Instance { get { return _instance; } }

    private PlayerHUD playerHUD;

    [SerializeField] private ScriptableSmallShip shipData;
    [SerializeField] private ScriptableShield shieldData;

    [SerializeField] private GameObject[] visualDamageEffects;
    [SerializeField] private ParticleSystem explosionParticleSystem;

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


    //private float maxBoosterSpeed;


    private float lastHitMaxTimer = 5f;
    private float lastHitCountDown;

    private int playerStationID = -1;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            if (DataManager.LoadSaveGame)
            {
                //SaveManager.Instance.LoadData();
            }
        }
        InitializeShipData();
    }
    // Start is called before the first frame update
    void Start()
    {
        InitializePlayerStatsHUD();
         //playerHUD.UpdateHealthTextfield(maxHealth, currentHealth);
        forceFieldController = transform.GetChild(0).GetComponent<ForceFieldController>();
        forceFieldController.SetOpenCloseValue(-2);

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

            if (playerHUD != null)
            {

                playerHUD.UpdateShieldTextField(shieldData.AbsorbCapacity, shieldAbsorbCapacity);
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

    public int GetPlayerStationID()
    {
        return playerStationID;
    }

    public float GetShieldStrength()
    {
        return shieldAbsorbCapacity;
    }

    public void SetPlayerStationID(int StationID)
    {
        playerStationID = StationID;
    }

    public void SetCurrentHealth(float newCurrentHealth)
    {
        currentHealth = newCurrentHealth;
    }

    /*
    public float GetMaxBoosterSpeed()
    {
        return maxBoosterSpeed;
    }
    */

    public void SetDamage(float incomingDamage)
    {
        lastHitCountDown = lastHitMaxTimer;
        if (shieldAbsorbCapacity > 0)
        {
            shieldAbsorbCapacity -= incomingDamage;

            if (shieldAbsorbCapacity < 0)
            {
                currentHealth -= shieldAbsorbCapacity * -1;
                shieldAbsorbCapacity = 0;
            }
        }
        else
        {
            currentHealth -= incomingDamage;
        }
        if (playerHUD == null)
        {
            playerHUD = PlayerHUD.Instance;
        }
        playerHUD.UpdateHealthTextField(maxHealth, currentHealth);
        playerHUD.UpdateShieldTextField(shieldData.AbsorbCapacity, shieldAbsorbCapacity);

        if (currentHealth <= 0)
        {
            TriggerExplosion();
            // WIP Respawn or Reload
        }
        else
        {
            //VisualiceDamage();
        }
    }

    private void VisualiseDamage()
    {
        float percentageValue = visualDamageEffects.Length * 0.1f;
        for (int i = 0; i < visualDamageEffects.Length; i++)
        {   
            if (!visualDamageEffects[i].activeInHierarchy && currentHealth <= maxHealth * Math.Round(1 - (percentageValue * (i + 1)), 2))
            {
                visualDamageEffects[i].SetActive(true);
            }
        }
    }
    private void ResetVisualDamage()
    {
        for (int i = 0; i < visualDamageEffects.Length; i++)
        {
            visualDamageEffects[i].SetActive(false);
        }
    }

    private void TriggerExplosion()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        gameObject.GetComponent<PlayerController>().enabled = false;

        ParticleSystem explosionParticles = Instantiate(explosionParticleSystem, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(explosionParticles.gameObject, 10);
        StartCoroutine(WaitForDeath());
        
    }

    public void PlayDestruction(bool state)
    {
        if (state)
        {
            explosionParticleSystem.Play();
        }
        else
        {
            explosionParticleSystem.Stop();
        }
    }

    public void ResetToBaseValues()
    {
        currentHealth = maxHealth;
    }

    private void InitializePlayerStatsHUD()
    {
        if (PlayerHUD.Instance != null)
        {
            playerHUD = PlayerHUD.Instance;

            playerHUD.UpdateHealthTextField(maxHealth, currentHealth);
            playerHUD.UpdateShieldTextField(shieldAbsorbCapacity, shieldAbsorbCapacity);
        }
    }
    public void SetShipData(ScriptableSmallShip ship)
    {
        shipData = ship;
        InitializeShipData();
        InitializePlayerStatsHUD();
    }
    public void SetShieldData(ScriptableShield shield)
    {
        shieldData = shield;
    }

    public ScriptableSmallShip GetShipData()
    {
        return shipData;
    }

    public ScriptableShield GetShieldData()
    {
        return shieldData;
    }

    public IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
