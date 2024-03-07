using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorialityManager : MonoBehaviour
{
    private static MorialityManager _instance;
    public static MorialityManager Instance { get { return _instance; } }
    [SerializeField] private int morialityValue = 50000;

    private int maxMorality = 100000;
    private MoralityLevel currentMorailtyLevel = MoralityLevel.NEUTRAL;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        SetMoralityLevel(morialityValue);
        Invoke("SetHUDValue",0.5f);
    }

    public void SetMoriality(Factions faction, int amount)
    {
        switch (faction)
        {
            case Factions.POLICE:
                CalcMoralityValue(-amount);
                break;
            case Factions.PIRATES:
                CalcMoralityValue(amount);
                break;
            case Factions.TRADERS:
                break;
            default:
                break;
        }
        PlayerHUD.Instance.UpdateMoralityTextField(maxMorality,morialityValue, currentMorailtyLevel.ToString());
    }

    public MoralityLevel GetMoralityLevel()
    {
        return currentMorailtyLevel;
    }

    private void CalcMoralityValue(int value)
    {
        morialityValue += value;
        morialityValue = Mathf.Clamp(morialityValue, 0, maxMorality);
        SetMoralityLevel(morialityValue);
    }

    private void SetMoralityLevel(int value)
    {
        if (value == maxMorality)
        {
            currentMorailtyLevel = MoralityLevel.MARSHAL;
        }
        else if(value >= maxMorality - 10000)
        {
            currentMorailtyLevel = MoralityLevel.SHERIFF;
        }
        else if (value >= maxMorality - 20000)
        {
            currentMorailtyLevel = MoralityLevel.DEPUTY;
        }
        else if (value >= maxMorality - 30000)
        {
            currentMorailtyLevel = MoralityLevel.CITIZEN;
        }
        /*************************************************/
        else if (value > maxMorality - 60000)
        {
            currentMorailtyLevel = MoralityLevel.NEUTRAL;
        }
        /*************************************************/
        else if (value > maxMorality - 70000)
        {
            currentMorailtyLevel = MoralityLevel.ROWDY;
        }
        else if (value > maxMorality - 80000)
        {
            currentMorailtyLevel = MoralityLevel.PETTYCRIMINAL;
        }
        else if (value > maxMorality - 90000)
        {
            currentMorailtyLevel = MoralityLevel.CRIMINAL;
        }
        else
        {
            currentMorailtyLevel = MoralityLevel.PUBLICENEMY;
        }
    }
    public void SetHUDValue()
    {
        PlayerHUD.Instance.UpdateMoralityTextField(maxMorality,morialityValue, currentMorailtyLevel.ToString());
    }
}
