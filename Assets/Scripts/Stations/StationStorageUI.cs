using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StationStorageUI : MonoBehaviour
{

    private TradingManager tradingManager;
    private PlayerCurrencies playerCurrencies;

    [SerializeField] private Transform slotPanelPrefab;
    [SerializeField] private GameObject slotPrefab;
    
    [SerializeField] private Slider amountOfSlider;
    [SerializeField] private TMP_Text selectedItemNameTextfield;
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private TMP_Text selectetItemDescriptionTextfield;

    private List<GameObject> itemSlotList = new List<GameObject>();
    private ItemSlot selectedItem;

    [SerializeField] private Station currentStation;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrencies = PlayerStats.Instance.gameObject.GetComponent<PlayerCurrencies>();
        UpdateStorageUI();
    }

    public void SetCurrentStation(Station station)
    {
        currentStation = station;
    }

    public Station GetCurrentStation()
    {
        return currentStation;
    }

    public void UpdateStorageUI()
    {
        for (int i = 0; i < currentStation.GetItemList().Count; i++)
        {
            if (FindIndexOfItemInItemsSlotList(currentStation.GetItemList()[i].Item) >= 0)
            {
                itemSlotList[i].GetComponent<ItemSlot>().SetItemData(currentStation.GetItemList()[i].Item, currentStation.GetItemList()[i].Amount, currentStation.GetCalculatedItemPrice(currentStation.GetItemList()[i].Item));
            }
            else
            {
                GameObject newSlot = Instantiate(slotPrefab, slotPanelPrefab);
                ItemSlot newSlotScript = newSlot.GetComponent<ItemSlot>();
                newSlotScript.SetItemData(currentStation.GetItemList()[i].Item, currentStation.GetItemList()[i].Amount, currentStation.GetCalculatedItemPrice(currentStation.GetItemList()[i].Item));
                newSlotScript.SetStationInventoryUI(this);

                itemSlotList.Add(newSlot);
            }
        }
    }

    public void SelectItem(ItemSlot itemSlot)
    {
        // show details of item in UI (GLOBAL)
        selectedItem = itemSlot.GetItemSlot();
        selectedItemNameTextfield.text = selectedItem.GetItemInSlot().ItemName;
        selectedItemImage.sprite = selectedItem.GetItemInSlot().Image;
        selectetItemDescriptionTextfield.text = $"{selectedItem.GetItemInSlot().Description} Price: {selectedItem.GetItemInSlot().Price}";
        amountOfSlider.maxValue = selectedItem.GetItemAmount();
        amountOfSlider.wholeNumbers = true;
    }

    public void BuyItem()
    {
        if (selectedItem != null)
        {
            if (selectedItem.GetItemPrice() * (int)amountOfSlider.value <= playerCurrencies.GetCreditsCurrency())
            {
                currentStation.GetComponent<Store>().BuyItem(selectedItem.GetItemInSlot(), (int)amountOfSlider.value);
                playerCurrencies.SetCreditsCurrency(-(selectedItem.GetItemPrice() * (int)amountOfSlider.value)); // remove money from player account
            }
            else
            {
                print("No money!!");
            }
            //UpdateStorageUI();
        }
    }

    private void ResetSlots()
    {
        itemSlotList.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private int FindIndexOfItemInItemsSlotList(ScriptableItem itemToSearch)
    {
        for (int i = 0; i < itemSlotList.Count; i++)
        {
            if (itemSlotList[i].GetComponent<ItemSlot>().GetItemInSlot() == itemToSearch)
            {
                return i;
            }
        }
        return -1;
    }
}
