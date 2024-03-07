using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New small ship", menuName = "ScriptableObjects/SmallShip")]
public class ScriptableSmallShip : ScriptableObject
{
    public int ShipID = -1;
    public string ShipName;
    public EnumSmallShipTypes ShipType;
    [TextArea(0, 20)] public string Description;
    public int Price;

    public int MaxHealth;

    public float YawPitchTorque = 100f;
    public float RollTorque = 50f;

    public float Thruster = 20f;
    public float SideThruster = 10f;
    public float UpDownThruster = 10f;

    [Range(0.5f, 10f)] public float MainThrusterAcceleration = 3;
    [Range(0.25f, 10f)] public float SideThrusterAcceleration = 2;
    [Range(0.25f, 10f)] public float UpdDownAcceleration = 2;
    [Range(0.5f, 10f)] public float RollAcceleration = 3;

    public GameObject[] WeaponSlots;
}

