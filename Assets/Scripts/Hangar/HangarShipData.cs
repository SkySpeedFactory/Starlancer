using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class HangarShipData : MonoBehaviour
{
    [SerializeField] private ScriptableSmallShip shipData;
    [SerializeField] private ScriptableShield shieldData;

    private int shipPrice;

    
    public ScriptableSmallShip GetShip()
    {
        return shipData;
    }
    public void SetShipData(ScriptableSmallShip ship)
    {
        shipData = ship;
    }
    public ScriptableShield GetShield()
    {
        return shieldData;
    }
    
    public void SetShieldData(ScriptableShield shield)
    {
        shieldData = shield;
    }

    public void SetShipPrice(int price)
    {
        shipPrice = price;
    }

    public int GetShipPrice()
    {
        return shipPrice; 
    }
}
