using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Projectile", menuName = "ScriptableObjects/Projectile")]
public class ScriptableProjectile : ScriptableObject
{
    public string ProjectileName;
    public EnumDamageType DamageType;
    public float Speed;
    public bool HasAiming;
}
