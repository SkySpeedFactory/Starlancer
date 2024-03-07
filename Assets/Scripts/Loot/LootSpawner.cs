using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [SerializeField] List<LootDropData> possibleLootItems;
    [SerializeField] GameObject lootContainerPrefab;

    public void SpawnLootDrop()
    {
        GameObject lootContainer = Instantiate(lootContainerPrefab, transform.position, transform.rotation);
        if (lootContainer.GetComponent<LootContainer>() != null)
        {
            GetRandomLootChance(lootContainer.GetComponent<LootContainer>());
        }
    }

    private void GetRandomLootChance(LootContainer container)
    {
        LootItem tempItem = new LootItem();
        for (int i = 0; i < possibleLootItems.Count; i++)
        {
            if (possibleLootItems[i].SpawnChance >= UnityEngine.Random.Range(0, 100))
            {
                tempItem.Item = possibleLootItems[i].Item;
                tempItem.Amount = UnityEngine.Random.Range(1, possibleLootItems[i].DropAmountChance);

                container.AddLootToList(tempItem);
            }
        }
    }
}
