using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIData
{
    public float Currenthealth;
    public Vector3 position;
    public EnumSmallShipTypes ShipType;

    public AIData(AIStats stats)
    {
        Currenthealth = stats.GetCurrentHealth();
        position = stats.transform.position;
    }

    public void GetData()
    {

    }

    public void SetData()
    {

    }
}
