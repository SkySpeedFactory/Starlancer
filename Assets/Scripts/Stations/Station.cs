using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Station : MonoBehaviour
{
    private ScriptableStation stationData;

    private TradingManager tradingManager;

    [SerializeField] private int stationID = -1;
    private string stationName;
    //private EnumFaction faction;
    private bool isPlayerStation = false;
    private bool hasLandingBay;

    private List<LootItem> tradingItemsList = new List<LootItem>();

    private List<LootItem> itemsStorageList = new List<LootItem>();

    private List<LootItem> stationSearchForItemsList = new List<LootItem>();
    private List<LootItem> stationSearchForRareItemsList = new List<LootItem>();

    // Start is called before the first frame update
    void Start()
    {
        tradingManager = TradingManager.Instance;
        if (tradingManager.GetCurrentLandedStationID() > 0)
        {
            stationID = tradingManager.GetCurrentLandedStationID();
            InitializeStationData(stationID);
        }
        else
        {
            InitializeStationData(stationID);
        }
        int PlayerStationID = PlayerStats.Instance.GetPlayerStationID();
        if (PlayerStationID > 0 && PlayerStationID == stationID)
        {
            isPlayerStation = true;
        }
    }

    public void SetItemAmount(ScriptableItem item, int amountOf)
    {
        int foundIndex = -1;
        for (int i = 0; i < itemsStorageList.Count; i++)
        {
            if (itemsStorageList[i].Item.ItemName == item.ItemName)
            {
                foundIndex = i;
            }
        }
        if (foundIndex >= 0)
        {
            // add / remove normally
            LootItem tmpItem = itemsStorageList[foundIndex];
            tmpItem.Amount += amountOf;
            itemsStorageList[foundIndex] = tmpItem;
            if (tmpItem.Amount <= 0)
            {
                itemsStorageList.RemoveAt(foundIndex);
            }
        }
        else
        {
            // add to list
            itemsStorageList.Add(new LootItem() { Item = item, Amount = amountOf });
        }
    }

    public int GetCalculatedItemPrice(ScriptableItem item)
    {
        float itemPriceMultiplier = 1f;
        for (int i = 0; i < stationSearchForRareItemsList.Count; i++)
        {
            if (stationSearchForRareItemsList[i].Item == item)
            {
                itemPriceMultiplier = 3f;
            }
        }
        if (itemPriceMultiplier == 1f)
        {
            for (int i = 0; i < stationSearchForItemsList.Count; i++)
            {
                if (stationSearchForItemsList[i].Item == item)
                {
                    itemPriceMultiplier = 1.5f;
                }
            }
        }
        
        return (int)(item.Price * itemPriceMultiplier);
    }

    public List<LootItem> GetItemList()
    {
        return itemsStorageList;
    }

    public int GetStationID()
    {
        return stationID;
    }

    public ScriptableStation GetStationData()
    {
        return stationData;
    }

    public bool GetStaionOwner()
    {
        return isPlayerStation;
    }

    public void SetPlayerStation(bool isPlayerStation)
    {
        this.isPlayerStation = isPlayerStation;
    }

    private void InitializeStationData(int idOfStation)
    {
        stationData = tradingManager.GetStationDataByID(idOfStation);
        stationName = stationData.StationName;
        hasLandingBay = stationData.HasLandingBay;

        tradingItemsList = stationData.ItemsList;
        InitializeTradingItemsList();
    }

    private void InitializeTradingItemsList()
    {
        for (int i = 0; i < tradingItemsList.Count; i++)
        {
            itemsStorageList.Add(tradingItemsList[i]);
        }
        stationSearchForItemsList = stationData.SearchForItemsList;
        stationSearchForRareItemsList = stationData.SearchForRareItemsList;
    }

    private void DebugTradingItemsList()
    {
        print($"----- Begin Tradinglist of Station {stationName} -----");
        for (int i = 0; i < itemsStorageList.Count; i++)
        {
            Debug.Log($"Itemname: {itemsStorageList[i].Item} Item amount: {itemsStorageList[i].Amount}");
        }
        print("-------------------------------------------------------");
    }
}
