using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager _instance;
    public static ObjectPoolManager Instance { get { return _instance; } }

    private List<GameObject> pooledProjectileBullets = new List<GameObject>();
    private List<GameObject> pooledLaserBullets = new List<GameObject>();

    private List<GameObject> pooledPirateAI = new List<GameObject>();
    private List<GameObject> pooledPoliceAI = new List<GameObject>();
    private List<GameObject> pooledTraderAI = new List<GameObject>();

    [SerializeField] private GameObject projectileBulletPoolPrefab;
    [SerializeField] private GameObject laserBulletPoolPrefab;
    [SerializeField] private int amountToPool;

    [SerializeField] private GameObject PirateAIPrefab;
    [SerializeField] private int amountPirate;
    [SerializeField] private GameObject PoliceAIPrefab;
    [SerializeField] private int amountPolice;
    [SerializeField] private GameObject TraderAIPrefab;
    [SerializeField] private int amountTrader;


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

    // Start is called before the first frame update
    void Start()
    {
        InitializeBulletObjectPool(pooledProjectileBullets, projectileBulletPoolPrefab);
        InitializeBulletObjectPool(pooledLaserBullets, laserBulletPoolPrefab);

        InitializeAIShips(pooledPirateAI, PirateAIPrefab, amountPirate);
        InitializeAIShips(pooledPoliceAI, PoliceAIPrefab, amountPolice);
        InitializeAIShips(pooledPoliceAI, TraderAIPrefab, amountTrader);
    }

    public GameObject GetPooledBullet(EnumDamageType enumDamage)
    {
        switch (enumDamage)
        {
            case EnumDamageType.ENERGY:
                for (int i = 0; i < pooledLaserBullets.Count; i++)
                {
                    if (pooledLaserBullets[i] != null && !pooledLaserBullets[i].activeInHierarchy)
                    {
                        return pooledLaserBullets[i];
                    }
                }
                AddMoreBulletsToPool(pooledLaserBullets, laserBulletPoolPrefab);
                return pooledLaserBullets[pooledLaserBullets.Count - 1];
            case EnumDamageType.PHYSICAL:
                for (int i = 0; i < pooledProjectileBullets.Count; i++)
                {
                    if (pooledProjectileBullets[i] != null && !pooledProjectileBullets[i].activeInHierarchy)
                    {
                        return pooledProjectileBullets[i];
                    }
                }
                AddMoreBulletsToPool(pooledProjectileBullets, projectileBulletPoolPrefab);
                return pooledProjectileBullets[pooledProjectileBullets.Count - 1];
            default:
                return null;
        }
    }

    private void InitializeBulletObjectPool(List<GameObject> poolList, GameObject bulletPrefab)
    {
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(bulletPrefab);
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    private void AddMoreBulletsToPool(List<GameObject> poolList, GameObject bulletPrefab)
    {
        GameObject tmp;
        tmp = Instantiate(bulletPrefab);
        tmp.SetActive(false);
        poolList.Add(tmp);
    }

    public GameObject GetPooledAIShips(Factions aiFaction)
    {
        switch (aiFaction)
        {
            case Factions.PIRATES:
                for (int i = 0; i < pooledPirateAI.Count; i++)
                {
                    if (pooledPirateAI[i] != null && !pooledPirateAI[i].activeInHierarchy)
                    {
                        return pooledPirateAI[i];
                    }
                }
                AddMoreAIShips(pooledPirateAI, PirateAIPrefab);
                return pooledPirateAI[pooledPirateAI.Count - 1];
            case Factions.POLICE:
                for (int i = 0; i < pooledPoliceAI.Count; i++)
                {
                    if (pooledPoliceAI[i] != null && !pooledPoliceAI[i].activeInHierarchy)
                    {
                        return pooledPoliceAI[i];
                    }
                }
                AddMoreAIShips(pooledPoliceAI, PoliceAIPrefab);
                return pooledPoliceAI[pooledPoliceAI.Count - 1];
            case Factions.TRADERS:
                for (int i = 0; i < pooledTraderAI.Count; i++)
                {
                    if (pooledTraderAI[i] != null && !pooledTraderAI[i].activeInHierarchy)
                    {
                        return pooledTraderAI[i];
                    }
                }
                AddMoreAIShips(pooledTraderAI, TraderAIPrefab);
                return pooledTraderAI[pooledTraderAI.Count - 1];
            default:
                return null;
        }
    }

    private void InitializeAIShips(List<GameObject> poolList, GameObject bulletPrefab, int amount)
    {
        GameObject tmp;
        for (int i = 0; i < amount; i++)
        {
            tmp = Instantiate(bulletPrefab);
            tmp.SetActive(false);
            poolList.Add(tmp);
        }
    }

    private void AddMoreAIShips(List<GameObject> poolList, GameObject bulletPrefab)
    {
        GameObject tmp;
        tmp = Instantiate(bulletPrefab);
        tmp.SetActive(false);
        poolList.Add(tmp);
    }

    public void ResetAIShips()
    {
        for (int i = 0; i < pooledPirateAI.Count; i++)
        {
            if (pooledPirateAI[i] != null && pooledPirateAI[i].activeInHierarchy)
            {
                pooledPirateAI[i].GetComponent<MapObject>().DeactivateIcon();
                pooledPirateAI[i].SetActive(false);
            }
        }

        for (int i = 0; i < pooledPoliceAI.Count; i++)
        {
            if (pooledPoliceAI[i] != null && pooledPoliceAI[i].activeInHierarchy)
            {
                pooledPoliceAI[i].GetComponent<MapObject>().DeactivateIcon();
                pooledPoliceAI[i].SetActive(false);
            }
        }

        for (int i = 0; i < pooledTraderAI.Count; i++)
        {
            if (pooledTraderAI[i] != null && pooledTraderAI[i].activeInHierarchy)
            {
                pooledTraderAI[i].GetComponent<MapObject>().DeactivateIcon();
                pooledTraderAI[i].SetActive(false);
            }
        }
    }
}
