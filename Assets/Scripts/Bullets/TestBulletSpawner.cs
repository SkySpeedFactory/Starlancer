using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletSpawner : MonoBehaviour
{
    private Transform[] barrels;
    [SerializeField] private GameObject bulletPrefab;
    private float primaryFireRateCountdown = 0f;
    private int currentPrimaryWeaponShootIndex = 0;
    [SerializeField] private float fireRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        barrels = new Transform[transform.childCount];
        for (int i = 0; i < barrels.Length; i++)
        {
            barrels[i] = transform.GetChild(i);
        }
        primaryFireRateCountdown = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckWeaponCooldown())
        {
            Shoot();
            primaryFireRateCountdown = fireRate;
        }
    }


    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrels[currentPrimaryWeaponShootIndex].position, barrels[currentPrimaryWeaponShootIndex].rotation);
        Projectile bulletScript = bullet.GetComponent<Projectile>();
        if (bulletScript)
        {
            
            bulletScript.SetDamage(100);
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
}
