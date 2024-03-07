using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrencies : MonoBehaviour
{

    [SerializeField] private int credits;

    [SerializeField] private List<int> ships;
    [SerializeField] private List<int> stations;
    
    //Start is called before the first frame update
    void Start()
    {
        PlayerHUD.Instance.UpdateMoneyTextField(credits); 
    }

    public int GetCreditsCurrency()
    {
        return credits;
    }

    public void SetCreditsCurrency(int amount)
    {
        credits += amount;
        PlayerHUD.Instance.UpdateMoneyTextField(credits);
    }

    public void AddShip(int ship)
    {
        ships.Add(ship);
    }

    public List<int> GetShipList()
    {
        return ships;
    }

    public void AddStation(int station)
    {
        stations.Add(station);
    }
    public List<int> GetStationList()
    {
        return stations;
    }
}
