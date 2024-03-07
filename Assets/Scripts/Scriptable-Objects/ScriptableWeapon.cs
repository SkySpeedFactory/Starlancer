using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New weapon", menuName = "ScriptableObjects/Weapon")]
public class ScriptableWeapon : ScriptableItem
{
    public EnumDamageType DamageType;
    public float Damage;
    public float FireRate;

    public GameObject Bullet;

    public bool[] Barrels = new bool[9];
    // 1 4 7
    // 2 5 8
    // 3 6 9
}
