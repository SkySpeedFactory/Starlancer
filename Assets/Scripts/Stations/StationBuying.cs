using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationBuying : MonoBehaviour
{
    private Station station;
    private ScriptableStation stationObject;

    private PlayerCurrencies currencies;

    private int stationPrice;

    private void Awake()
    {
        station = GetComponent<Station>();
        stationObject = station.GetStationData();
        stationPrice = stationObject.StationPrice;
    }

    public void BuyStation()
    {
        currencies = PlayerStats.Instance.gameObject.GetComponent<PlayerCurrencies>();
        currencies.SetCreditsCurrency((stationPrice * (-1)));
        PlayerStats.Instance.SetPlayerStationID(stationObject.StationID);
        station.SetPlayerStation(true);
    }
}
