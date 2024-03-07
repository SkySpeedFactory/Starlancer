using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{

    [SerializeField] private ScriptableWeapon weaponData;

    private string weaponName;
    private EnumDamageType damageType;
    private string description;
    private Sprite weaponImage;
    private int price;
    private AudioSource weaponAudioSource;

    private float damage;
    private float fireRate;

    private Transform[] barrels;
    private GameObject bulletPrefab;
    private float primaryFireRateCountdown = 0f;
    private int currentPrimaryWeaponShootIndex = 0;

    private void Awake()
    {
        InitializeWeaponData(weaponData);
        weaponAudioSource = GetComponent<AudioSource>();
    }

    public void InitializeWeaponData(ScriptableWeapon data)
    {
        weaponData = data;
        weaponName = data.ItemName;
        damageType = data.DamageType;
        description = data.Description;
        weaponImage = data.Image;
        price = data.Price;

        damage = data.Damage;
        fireRate = data.FireRate;

        barrels = new Transform[CountSpecificStateOfBarrels(data.Barrels, true)];

        // reset child active state to false
        for (int i = 0; i < data.Barrels.Length; i++)
        {
            if (data.Barrels[i])
            {
                transform.GetChild(i).gameObject.SetActive(true);
                for (int j = 0; j < barrels.Length; j++)
                {
                    if (barrels[j] == null)
                    {
                        barrels[j] = transform.GetChild(i);
                        break;
                    }
                }
            }
        }
        // 1 4 7
        // 2 5 8
        // 3 6 9
        bulletPrefab = data.Bullet;
    }

    public void Shoot(int shipID, bool isPlayer = false)
    {
        if (CheckWeaponCooldown() && barrels != null)
        {
            if (currentPrimaryWeaponShootIndex >= barrels.Length - 1)
            {
                currentPrimaryWeaponShootIndex = 0;
            }
            else
            {
                currentPrimaryWeaponShootIndex++;
            }
            GameObject bullet = ObjectPoolManager.Instance.GetPooledBullet(damageType);
            if (bullet != null)
            {
                bullet.transform.position = barrels[currentPrimaryWeaponShootIndex].position;
                bullet.transform.rotation = barrels[currentPrimaryWeaponShootIndex].rotation;
                bullet.SetActive(true);

                Projectile bulletScript = bullet.GetComponent<Projectile>();
                if (bulletScript)
                {
                    bulletScript.SetShipID(shipID);
                    bulletScript.SetDamage(damage);
                    SoundManager.Instance.PlaySound(weaponAudioSource, bulletScript.GetAudio());
                    if (isPlayer)
                    {
                        bulletScript.SetPlayerOwnerShip();
                    }
                }
            }
            primaryFireRateCountdown = fireRate;
        }
    }

    private bool CheckWeaponCooldown()
    {
        if (primaryFireRateCountdown > 0)
        {
            primaryFireRateCountdown -= Time.deltaTime;
        }
        if (primaryFireRateCountdown <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int CountSpecificStateOfBarrels(bool[] barrelsState,bool state)
    {
        return barrelsState.Count(b => b == state);
    }

    public Sprite GetWeaponIcon()
    {
        return weaponImage;
    }

    public string GetWeaponName()
    {
        return weaponName;
    }

    public ScriptableWeapon GetWeaponCurrentDataInSlot()
    {
        return weaponData;
    }
}
