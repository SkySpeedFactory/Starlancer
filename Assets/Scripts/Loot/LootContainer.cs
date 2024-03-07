using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour
{
    private List<LootItem> lootContainerItemsList = new List<LootItem>();

    public void AddLootToList(LootItem lootContainerItem)
    {
        lootContainerItemsList.Add(lootContainerItem);
    }

    public List<LootItem> GetLootFromList()
    {
        return lootContainerItemsList;
    }

    public void RemoveContainer()
    {
        Destroy(gameObject);
    }
}
