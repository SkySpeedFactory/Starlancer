using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New shield", menuName = "ScriptableObjects/Shield")]
public class ScriptableShield : ScriptableItem
{
    public int ShieldID;
    public float AbsorbCapacity;
    public float RechargeRate;
}
