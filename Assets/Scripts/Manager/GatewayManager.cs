using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayManager : MonoBehaviour
{

    private static GatewayManager _instance;
    public static GatewayManager Instance { get { return _instance; } }

    private List<Gateway> gateways = new List<Gateway>();
    private int currentUsingGatewayID = -1;
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

    public void AddGatewayToList(Gateway gateway)
    {
        if (!gateways.Contains(gateway))
        {
            gateways.Add(gateway);
        }
    }

    public void SetCurrentUsingGatewayID(int id)
    {
        currentUsingGatewayID = id;
        RemoveAllGateways();
    }

    public int GetCurrentUsingGatewayID() 
    { 
        return currentUsingGatewayID; 
    }

    public Gateway GetGateway(int id)
    {
        return gateways.Find(gateway => gateway.GetGatewayID() == currentUsingGatewayID);
    }

    private void RemoveAllGateways()
    {
        gateways.Clear();
    }
}
