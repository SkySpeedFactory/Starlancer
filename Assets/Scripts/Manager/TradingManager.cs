using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TradingManager : MonoBehaviour
{
    private static TradingManager _instance;
    public static TradingManager Instance { get { return _instance; } }

    [SerializeField] private List<ScriptableStation> listOfStations = new List<ScriptableStation>();
    private int currentLandedStationID = -1;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public ScriptableStation GetStationDataByID(int id)
    {
        for (int i = 0; i < listOfStations.Count; i++)
        {
            if (listOfStations[i].StationID == id)
            {
                return listOfStations[i];
            }
        }
        return null;
    }

    public void SetCurrentLandedStationID(Station station)
    {
        currentLandedStationID = station.GetStationID();
    }

    public void SetCurrentLandedStationID(int stationID)
    {
        currentLandedStationID = stationID;
    }

    public int GetCurrentLandedStationID()
    {
        return currentLandedStationID;
    }
}
