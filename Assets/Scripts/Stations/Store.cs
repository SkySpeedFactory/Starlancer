using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField] private Station station;

    [SerializeField] private PlayerInventoryUI playerInventoryUI;
    [SerializeField] private StationStorageUI stationStorageUI;

    void Start()
    {
        station = gameObject.GetComponent<Station>();
    }

    public void BuyItem(ScriptableItem item, int amount)
    {
        if (item != null)
        {
            station.SetItemAmount(item, -amount);
            PlayerStats.Instance.GetComponent<PlayerInventory>().SetItemToPlayerInventoryItemsList(item, amount);
            stationStorageUI.UpdateStorageUI();
            playerInventoryUI.UpdateInventoryUI();
        }
    }

    public void SellItem(ScriptableItem item, int amount)
    {
        if (item != null)
        {
            PlayerStats.Instance.GetComponent<PlayerInventory>().SetItemToPlayerInventoryItemsList(item, -amount);
            station.SetItemAmount(item, amount);
            playerInventoryUI.UpdateInventoryUI();
            stationStorageUI.UpdateStorageUI();
        }
    }
}
