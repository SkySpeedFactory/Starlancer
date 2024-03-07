using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    [SerializeField] private ScriptableEnvironment environmentData;

    private string environmentName;
    private float maxEnvironmentHealth;
    private float currentEnvironmentHealth;

    private void Awake()
    {
        InitializeData();
    }

    public void SetHealthDamage(float damage)
    {
        currentEnvironmentHealth -= damage;

        if (currentEnvironmentHealth <= 0)
        {
            currentEnvironmentHealth = 0;
            if (gameObject.GetComponent<LootSpawner>() != null)
            {
                gameObject.GetComponent<LootSpawner>().SpawnLootDrop();
            }
            gameObject.SetActive(false);
        }
    }

    private void InitializeData()
    {
        environmentName = environmentData.EnvironmentName;
        maxEnvironmentHealth = environmentData.Health;
        currentEnvironmentHealth = maxEnvironmentHealth;
    }
}
