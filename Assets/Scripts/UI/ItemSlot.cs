using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private StationStorageUI stationStorageUI;
    private ScriptableItem item;
    private int amountOf;
    private int price;
    private bool playerIsOwner = false;

    private PlayerInventoryUI playerInventoryUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemData(ScriptableItem itemData, int itemsCount, int calculatedPrice)
    {
        item = itemData;
        amountOf = itemsCount;
        price = calculatedPrice;
        UpdateUI();
    }

    public ScriptableItem GetItemInSlot()
    {
        return item;
    }

    public int GetItemAmount()
    {
        return amountOf;
    }

    public void SetSelectedItem()
    {
        if (playerIsOwner)
        {
            playerInventoryUI.SelectItem(this);
        }
        else
        {
            stationStorageUI.SelectItem(this);
        }
        
    }

    public ItemSlot GetItemSlot()
    {
        return this;
    }

    public void SetPlayerInventoryUI(PlayerInventoryUI inventoryUI)
    {
        playerInventoryUI = inventoryUI;
    }

    public void SetStationInventoryUI(StationStorageUI stationUI)
    {
        stationStorageUI = stationUI;
    }

    public void SetPlayerIsOwner(bool isOwner)
    {
        playerIsOwner = isOwner;
    }
    public bool GetOwner()
    {
        return playerIsOwner;
    }

    public int GetItemPrice()
    {
        return price;
    }

    private void UpdateUI()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = item.Image;
        transform.GetChild(1).GetComponent<TMP_Text>().text = item.ItemName;
        transform.GetChild(2).GetComponent<TMP_Text>().text = $"Price: {price}";
        transform.GetChild(3).GetComponent<TMP_Text>().text = $"Amount of: {amountOf}";
    }
}
