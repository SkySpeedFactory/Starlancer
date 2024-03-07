using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private PlayerCurrencies playerCurrencies;
    [SerializeField] private Transform slotsPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject IsIllegalPanel;

    [SerializeField] private TMP_Text selectedItemName;
    [SerializeField] private TMP_Text selectedItemDescription;
    [SerializeField] private Image defaultItemImage;
    [SerializeField] private Image selectedItemImage;
    [SerializeField] private Slider amountOfSelectedItemsSlider;
    [SerializeField] private TMP_Text amountOfSelectedItemsSliderText;

    [SerializeField] private TMP_Text creditsCurrencyText;

    private List<GameObject> itemSlotList = new List<GameObject>();
    private ItemSlot selectedItem;

    [SerializeField] private Station station;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>();
        
        playerCurrencies = PlayerStats.Instance.gameObject.GetComponent<PlayerCurrencies>();
        UpdateInventoryUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (creditsCurrencyText != null)
        {
            UpdateCurrencyTextfields();
        }
    }

    public void UpdateInventoryUI()
    {
        if (playerInventory == null)
        {
            playerInventory = PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>();
        }

        if (selectedItem == null)
        {
            selectedItemName.text = "";
            selectedItemDescription.text = "";
            selectedItemImage.sprite = defaultItemImage.sprite;
        }

        for (int i = 0; i < playerInventory.GetPlayerInventory().Count; i++)
        {
            if (FindIndexOfItemInItemsSlotList(playerInventory.GetPlayerInventory()[i].Item) >= 0)
            {
                itemSlotList[i].GetComponent<ItemSlot>().SetItemData(playerInventory.GetPlayerInventory()[i].Item, playerInventory.GetPlayerInventory()[i].Amount, playerInventory.GetPlayerInventory()[i].Item.Price); //WIP add player trade debuff
            }
            else
            {
                GameObject newSlot = Instantiate(slotPrefab, slotsPanel);
                ItemSlot newSlotScript = newSlot.GetComponent<ItemSlot>();
                newSlotScript.SetItemData(playerInventory.GetPlayerInventory()[i].Item, playerInventory.GetPlayerInventory()[i].Amount, playerInventory.GetPlayerInventory()[i].Item.Price); //WIP add player trade debuff
                newSlotScript.SetPlayerIsOwner(true);
                newSlotScript.SetPlayerInventoryUI(this);

                itemSlotList.Add(newSlot);
            }
        }
        DeleteSlotArtifacts(playerInventory.GetPlayerInventory().Count, itemSlotList.Count);
    }

    public void SelectItem(ItemSlot itemSlot)
    {
        selectedItem = itemSlot.GetItemSlot();
        selectedItemName.text = selectedItem.GetItemInSlot().ItemName;
        
        selectedItemDescription.text = $"{selectedItem.GetItemInSlot().Description} Price: {selectedItem.GetItemInSlot().Price}";
        selectedItemImage.sprite = selectedItem.GetItemInSlot().Image;
        if (amountOfSelectedItemsSlider != null)
        {
            amountOfSelectedItemsSlider.wholeNumbers = true;
            amountOfSelectedItemsSlider.maxValue = selectedItem.GetItemAmount();
        }
    }

    public void ClearCargo()
    {
        if (selectedItem != null)
        {
            int foundIndex = FindIndexOfItemInItemsSlotList(selectedItem.GetItemInSlot());

            playerInventory.SetItemToPlayerInventoryItemsList(itemSlotList[foundIndex].GetComponent<ItemSlot>().GetItemInSlot(), -selectedItem.GetItemAmount());

            if (foundIndex >= 0)
            {
                itemSlotList.RemoveAt(foundIndex);
                Destroy(selectedItem.gameObject);
                selectedItem = null;
            }
            UpdateInventoryUI();
        }
        
    }

    public void UpdateSliderText()
    {
        amountOfSelectedItemsSliderText.text = $"{amountOfSelectedItemsSlider.value}";
    }

    public void SellItemFromInventory()
    {
        // WIP (Player instance)
        if (selectedItem != null)
        {
            if (!selectedItem.GetItemInSlot().IsIllegal)
            {
                station.GetComponent<Store>().SellItem(selectedItem.GetItemInSlot(), (int)amountOfSelectedItemsSlider.value);
                // add money to player account
                playerCurrencies.SetCreditsCurrency(selectedItem.GetItemPrice() * (int)amountOfSelectedItemsSlider.value);
                //UpdateInventoryUI();
                UpdateCurrencyTextfields();
            }
            else if (selectedItem.GetItemInSlot().IsIllegal && station.GetStationData().CanBuyIllegal)
            {
                station.GetComponent<Store>().SellItem(selectedItem.GetItemInSlot(), (int)amountOfSelectedItemsSlider.value);
                // add money to player account
                playerCurrencies.SetCreditsCurrency(selectedItem.GetItemPrice() * (int)amountOfSelectedItemsSlider.value);
                //UpdateInventoryUI();
                UpdateCurrencyTextfields();
            }
            else
            {
                IsIllegalPanel.SetActive(true);
            }
        }
    }

    public void SellIllegalItemFromInventory()
    {
        // WIP (Player instance)
        if (selectedItem != null)
        {
            station.GetComponent<Store>().SellItem(selectedItem.GetItemInSlot(), (int)amountOfSelectedItemsSlider.value);
            // add money to player account
            playerCurrencies.SetCreditsCurrency(selectedItem.GetItemPrice() * (int)amountOfSelectedItemsSlider.value);
            //UpdateInventoryUI();
            UpdateCurrencyTextfields();
            IsIllegalPanel.SetActive(false);
        }
    }

    public void CloseIllegalPanel()
    {
        IsIllegalPanel.SetActive(false);
    }

    public void UpdateCurrencyTextfields()
    {
        creditsCurrencyText.text = $"Credits: {playerCurrencies.GetCreditsCurrency()}";
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

    private void DeleteSlotArtifacts(int countItemsInInventory, int countSlotsOnUI)
    {
        if (countSlotsOnUI > countItemsInInventory)
        {
            Destroy(itemSlotList[itemSlotList.Count - 1].gameObject);
            itemSlotList.RemoveAt(itemSlotList.Count - 1);
        }
    }
}
