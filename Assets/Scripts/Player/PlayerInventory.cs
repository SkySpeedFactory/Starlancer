using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<LootItem> playerInventoryItemsList = new List<LootItem>();

    [SerializeField] private List<ScriptableItem> debugList = new List<ScriptableItem>();
    [SerializeField] private bool debugMode = false;
    [SerializeField] private ScriptableItem debugItemToAdd;

    private void Awake()
    {
        if (debugMode)
        {
            for (int i = 0; i < debugList.Count; i++)
            {
                SetItemToPlayerInventoryItemsList(debugList[i], 10);
            }
        }
    }
    // amount can be + or -
    public void SetItemToPlayerInventoryItemsList(ScriptableItem item, int amount)
    {
        int foundIndex = -1;
        for (int i = 0; i < playerInventoryItemsList.Count; i++)
        {
            if (playerInventoryItemsList[i].Item.ItemName == item.ItemName)
            {
                foundIndex = i;
            }
        }
        if (foundIndex >= 0)
        {
            // add / remove normally
            if (playerInventoryItemsList[foundIndex].Amount + amount > 0)
            {
                LootItem tmpItem = playerInventoryItemsList[foundIndex];
                tmpItem.Amount += amount;
                playerInventoryItemsList[foundIndex] = tmpItem;
            }
            else
            {
                playerInventoryItemsList.RemoveAt(foundIndex);
            }
        }
        else
        {
            // add to list
            playerInventoryItemsList.Add(new LootItem() { Item = item, Amount = amount });
            
        }
    }

    public List<LootItem> GetPlayerInventory()
    {
        return playerInventoryItemsList;
    }

    public List<LootItem> GetAllWeaponsInPlayerInventory()
    {
        return playerInventoryItemsList.FindAll(weapon => weapon.Item.GetType() == typeof(ScriptableWeapon));
    }

    public void SetPlayerInventory(List<LootItem> inventoryList)
    {
        playerInventoryItemsList = inventoryList;
    }

    public void DebugPlayerInventory()
    {
        print("-----Player Inventory-----");
        for (int i = 0; i < playerInventoryItemsList.Count; i++)
        {
            print($"Item: {playerInventoryItemsList[i].Item} Amount: {playerInventoryItemsList[i].Amount}");
        }
        print("--------------------------");
    }
}
