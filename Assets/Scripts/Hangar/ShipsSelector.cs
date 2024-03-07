using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShipsSelector : MonoBehaviour
{
    [SerializeField] private List<Transform> ships;
    
    [SerializeField] private ScriptableSmallShip selectedShipData;
    [SerializeField] private ScriptableShield shieldData;


    private void Awake()
    {
        CreateShipList();

        if (selectedShipData == null)
        {
            if (ships.Count != 0)
            {
                selectedShipData = ships[0].GetComponent<HangarShipData>().GetShip();
            }
        }

        if (shieldData == null)
        {
            if (ships.Count != 0)
            {
                shieldData = ships[0].GetComponent<HangarShipData>().GetShield();
            }
        }
    }

    
    private void CreateShipList()
    {
        ships = new List<Transform>();
        
        for (int i = 0; i < transform.childCount; i++)
        {
            ships.Add(transform.GetChild(i));
        }
    }

    public void SetShipData(ScriptableSmallShip ship)
    {
        selectedShipData = ship;
    }

    public ScriptableSmallShip GetSelectedShip()
    {
        return selectedShipData;
    }
    public void SetShieldData(ScriptableShield shield)
    {
        shieldData = shield;
    }
    
    public ScriptableShield GetShield()
    {
        return shieldData;
    }
    public List<Transform> GetShipList()
    {
        return ships;
    }

    public void RemoveShipFromList(Transform ship)
    {
        ships.Remove(ship);
    }
    public void AddShip(Transform ship)
    {
        ships.Add(ship);
    }

    public Mesh GetActiveMesh()
    {
        return transform.GetChild(0).GetComponent<MeshFilter>().mesh;
    }

    public Mesh GetActiveSharedMesh()
    {
       return transform.GetChild(0).GetComponent<MeshCollider>().sharedMesh;
    }
}
