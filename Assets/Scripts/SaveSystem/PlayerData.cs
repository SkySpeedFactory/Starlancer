using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [System.Serializable]
    public struct InventoryItem
    {
        public string Name;
        public int Amount;
    }

    private DataType dataType;
    private float[] position = new float[3];

    private float maxHealth;
    private float currentHealth;
    private int playerStationID;

    private int playerShip;
    private int playerShield;
 
    private List<InventoryItem> inventoryItems;
    private string[] equipedWeapons;

    public PlayerData()
    {
        PlayerStats playerStats = PlayerStats.Instance;
        GameObject player = PlayerStats.Instance.gameObject;
        PlayerInventory playerInventory= player.GetComponent<PlayerInventory>();
        Weapon[] currentWeaponsObject = player.GetComponent<PlayerController>().GetWeaponSlotsData();

        GetCurrentWeapons(currentWeaponsObject);
        dataType = DataType.Player;
        currentHealth = playerStats.GetCurrentHealth();

        position[0] = playerStats.transform.position.x;
        position[1] = playerStats.transform.position.y;
        position[2] = playerStats.transform.position.z;

        ScriptableSmallShip playerShip = playerStats.GetShipData();
        this.playerShip = playerShip.ShipID;
        playerShield = playerStats.GetShieldData().ShieldID;
        playerStationID = playerStats.GetPlayerStationID();
        List<LootItem> inventoryList = playerInventory.GetPlayerInventory();
        PlayerDictionairy(inventoryList);

    }

    public void SetData()
    {
        PlayerStats playerStats= PlayerStats.Instance;
        if (playerStats != null)
        {
            GameObject player = PlayerStats.Instance.gameObject;
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            playerStats.transform.position = new Vector3(position[0], position[1], position[2]);
            playerStats.SetCurrentHealth(currentHealth);
            playerStats.SetPlayerStationID(playerStationID);
            
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerInventory.SetPlayerInventory(PopulateInventory());
            PopulateGuns();
            playerStats.SetShieldData(GetCurrentShield());
            playerStats.SetShipData(GetCurrentShip());
        }

    }

    private ScriptableSmallShip GetCurrentShip()
    {
        ScriptableSmallShip ship = InventoryPool.Instance.getShip(playerShip);
        return ship;
    }

    private ScriptableShield GetCurrentShield()
    {
        ScriptableShield shield = InventoryPool.Instance.getShield(playerShip);
        return shield;
    }

    private void GetCurrentWeapons(Weapon[] weapons)
    {
        equipedWeapons = new string[2];
        for (int i = 0; i < weapons.Length; i++)
        {
            ScriptableWeapon tempWeapon = weapons[i].GetWeaponCurrentDataInSlot();
            equipedWeapons[i] = tempWeapon.ItemName;
        }
    }

    private void PlayerDictionairy(List<LootItem> inventoryList)
    {
        inventoryItems = new List<InventoryItem>();
        foreach (LootItem item in inventoryList)
        {
            InventoryItem newItem = new InventoryItem();
            newItem.Name = item.Item.name;
            newItem.Amount = item.Amount;
            inventoryItems.Add(newItem);
        }
    }

    private List<LootItem> PopulateInventory()
    {
        List<LootItem> inventoryList = new List<LootItem>();
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            LootItem tempItem = new LootItem();
            tempItem.Item = InventoryPool.Instance.getItem(inventoryItems[i].Name);
            tempItem.Amount = inventoryItems[i].Amount;
            inventoryList.Add(tempItem);
        }
        return inventoryList;
    }

    private void PopulateGuns()
    {
        ScriptableWeapon[] weaponlist = new ScriptableWeapon[2];
        for (int i = 0; i < equipedWeapons.Length; i++)
        {
            ScriptableWeapon tempWeapon = InventoryPool.Instance.getWeapon(equipedWeapons[i]);
            weaponlist[i] = tempWeapon;
            
        }
        PlayerController playerController = PlayerStats.Instance.gameObject.GetComponent<PlayerController>();
        playerController.SetWeapons(weaponlist);
    }
}
