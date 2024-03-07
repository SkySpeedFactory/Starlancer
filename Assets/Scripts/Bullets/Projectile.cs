using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ScriptableProjectile projectileData;
    [SerializeField] private AudioClip projectileSound;

    private int ownShipID;
    private EnumDamageType damageType;
    private float bulletDamage;
    private float speed;
    private bool hasAiming;
    private bool isPlayerProjectile = false;

    private Transform lockedTarget;

    private float maxLifeTime = 2f;
    private float currentLifeSpan;
    
    
    private void Awake()
    {
        InitializeProjectileData();
        ResetLifeSpan();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAiming && lockedTarget != null)
        {
            MoveAimingProjectile(lockedTarget);
        }
        else
        {
            MoveProjectile();
        }
        if (currentLifeSpan <= 0)
        {
            ResetLifeSpan();
            gameObject.SetActive(false);
        }
        else
        {
            currentLifeSpan -= Time.deltaTime;
        }
    }

    private void InitializeProjectileData()
    {
        damageType = projectileData.DamageType;
        speed = projectileData.Speed;
        hasAiming = projectileData.HasAiming;
    }

    private void MoveProjectile()
    {
        float projectileSpeed = speed * Time.deltaTime;
        transform.Translate(Vector3.forward.normalized * projectileSpeed, Space.Self);
    }

    private void MoveAimingProjectile(Transform target)
    {
        float projectileSpeed = speed * Time.deltaTime;
        transform.Translate(Vector3.forward.normalized * projectileSpeed, Space.Self);
    }

    public void SetDamage(float damageValue)
    {
        bulletDamage = damageValue;
    }

    public void SetShipID(int shipID)
    {
        ownShipID = shipID;
    }

    public void SetPlayerOwnerShip()
    {
        isPlayerProjectile = true;
    }

    public void SetAimingTarget(Transform target)
    {
        lockedTarget = target;
    }

    public AudioClip GetAudio()
    {
        return projectileSound;
    }

    private void ResetLifeSpan()
    {
        currentLifeSpan = maxLifeTime;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.GetComponent<Projectile>() != null) // Check if bullet hit bullet
        {
            return;
        }

        if (ownShipID != collision.gameObject.GetInstanceID())
        {
            if (isPlayerProjectile)
            {
                
                if (collision.transform.GetComponent<AIStats>() != null)
                {
                    collision.transform.GetComponent<AIStats>().SetDamage(bulletDamage, isPlayerProjectile);
                }
                else if (collision.gameObject.CompareTag("Destroyable"))
                {
                    collision.gameObject.GetComponent<Destroyable>().SetHealthDamage(bulletDamage);
                }
            }
            else
            {
                if (collision.transform.GetComponent<AIStats>() != null)
                {
                    collision.transform.GetComponent<AIStats>().SetDamage(bulletDamage, isPlayerProjectile);
                }
                else if (collision.gameObject.CompareTag("Player"))
                {
                    if (collision.transform.GetComponent<PlayerStats>() != null)
                    {
                        collision.transform.GetComponent<PlayerStats>().SetDamage(bulletDamage);
                    }
                    else
                    {
                        collision.transform.parent.parent.GetComponent<PlayerStats>().SetDamage(bulletDamage);
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }
}
