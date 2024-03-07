using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayerScript : MonoBehaviour
{
    [SerializeField] private Station station;
    
    [SerializeField] private ShipsSelector stationShipsSelector;
    [SerializeField] private ShipsSelector playerShipsSelector;

    [SerializeField] private ShipHangarDataUI shipHangarDataUI;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHangarContainer;

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button selectButton;

    [SerializeField] private bool playerSelectorActive = false;

    public List<Transform> selectorObject;
    
    private List<int> playerShipList;
    private List<int> playerStationList;

    private int selectedShip = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerShipList = player.GetComponent<PlayerCurrencies>().GetShipList();
        playerStationList = player.GetComponent<PlayerCurrencies>().GetStationList();

        CheckOwnedShips();
        CheckOwnedStations();

        stationShipsSelector.GetShipList()[0].gameObject.SetActive(true);

        playerHangarContainer.transform.GetComponent<MeshFilter>().mesh = playerShipsSelector.GetActiveMesh();

        playerHangarContainer.transform.GetComponent<MeshCollider>().sharedMesh = playerShipsSelector.GetActiveSharedMesh();

    }

    public void SetSelectorList(List<Transform> selector)
    {
        selectorObject = selector;
    }

    public void SetBoolPlayerSelector(bool active)
    {
        playerSelectorActive = active;
    }

    public void NextObject()
    {
        if (playerSelectorActive)
        {
            //change player ship and shield stats
            player.GetComponent<PlayerStats>().SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            player.GetComponent<PlayerStats>().SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());

            selectorObject[selectedShip].gameObject.SetActive(false);
            selectedShip = (selectedShip + 1) % selectorObject.Count;
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());
            selectorObject[selectedShip].gameObject.SetActive(true);

            //change meshes + colliders
            playerHangarContainer.transform.GetComponent<MeshFilter>().mesh =
                selectorObject[selectedShip].transform.GetComponent<MeshFilter>().mesh;

            playerHangarContainer.transform.GetComponent<MeshCollider>().sharedMesh =
                selectorObject[selectedShip].transform.GetComponent<MeshCollider>().sharedMesh;

            playerHangarContainer.transform.GetComponent<HangarShipData>().SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            playerHangarContainer.transform.GetComponent<HangarShipData>().SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());

            selectButton.gameObject.SetActive(true);

        }
        else
        {
            selectorObject[selectedShip].gameObject.SetActive(false);
            selectedShip = (selectedShip + 1) % selectorObject.Count;
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());
            selectorObject[selectedShip].gameObject.SetActive(true);
            shipHangarDataUI.UpdateShipPriceUI(station.GetStaionOwner());
        }
    }

    public void PreviousObject()
    {
        if (playerSelectorActive)
        {
            //change player ship and shield stats
            player.GetComponent<PlayerStats>().SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            player.GetComponent<PlayerStats>().SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());

            selectorObject[selectedShip].gameObject.SetActive(false);
            selectedShip--;
            if (selectedShip < 0)
            {
                selectedShip += selectorObject.Count;
            }

            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());

            selectorObject[selectedShip].gameObject.SetActive(true);

            //change meshes + colliders
            playerHangarContainer.transform.GetComponent<MeshFilter>().mesh =
                selectorObject[selectedShip].transform.GetComponent<MeshFilter>().mesh;

            playerHangarContainer.transform.GetComponent<MeshCollider>().sharedMesh =
                selectorObject[selectedShip].transform.GetComponent<MeshCollider>().sharedMesh;

            playerHangarContainer.transform.GetComponent<HangarShipData>().SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            playerHangarContainer.transform.GetComponent<HangarShipData>().SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());

            selectButton.gameObject.SetActive(true);
            
        }
        else
        {
            selectorObject[selectedShip].gameObject.SetActive(false);
            selectedShip--;
            if (selectedShip < 0)
            {
                selectedShip += selectorObject.Count;
            }

            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());
            selectorObject[selectedShip].gameObject.SetActive(true);
            shipHangarDataUI.UpdateShipPriceUI(station.GetStaionOwner());
        }

    }

    public void BuyShip()
    {
        float costMultiplier = 1;

        if (station.GetStaionOwner())
        {
            costMultiplier = 0.5F;
        }
        if ((selectorObject[selectedShip].transform.GetComponent<HangarShipData>().GetShip().Price * costMultiplier) <= player.GetComponent<PlayerCurrencies>().GetCreditsCurrency())
        {
            playerShipsSelector.AddShip(selectorObject[selectedShip]);//player selector
            player.GetComponent<PlayerCurrencies>().AddShip(selectorObject[selectedShip].transform.GetComponent<HangarShipData>().GetShip().ShipID);//playercurrencies script

            selectorObject[selectedShip].transform.SetParent(playerShipsSelector.transform);// set parent
            selectorObject[selectedShip].transform.position = playerShipsSelector.transform.position;// transform position + rotation

            playerShipsSelector.gameObject.GetComponentInChildren<MeshRenderer>().gameObject.SetActive(false); //fixed works with more purchases

            //remove money from player
            player.GetComponent<PlayerCurrencies>().SetCreditsCurrency(-(int)(selectorObject[selectedShip].transform.GetComponent<HangarShipData>().GetShip().Price * costMultiplier));

            selectorObject.Remove(selectorObject[selectedShip]);//displayer

            selectedShip = 0;

            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
            selectorObject[selectedShip].parent.GetComponent<ShipsSelector>()
                .SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());
                
            selectorObject[selectedShip].transform.gameObject.SetActive(true);
                
            shipHangarDataUI.UpdateShipPriceUI(station.GetStaionOwner());
            shipHangarDataUI.UpdateShipDataUI();
            ShipBuyQuestTrigger();
        }
        else
        {
            print("No money!!");
        }
    }

    public void SelectShip()
    {
        //change player ship and shield stats
        player.GetComponent<PlayerStats>().SetShipData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShip());
        player.GetComponent<PlayerStats>().SetShieldData(selectorObject[selectedShip].GetComponent<HangarShipData>().GetShield());
        //change meshes + colliders
        player.transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh =
            selectorObject[selectedShip].GetComponent<MeshFilter>().mesh;
       /* player.transform.GetChild(0).GetChild(0).GetComponent<MeshCollider>().sharedMesh =
            selectorObject[selectedShip].GetComponent<MeshCollider>().sharedMesh;*/
        selectButton.gameObject.SetActive(false);
    }

    public void CheckOwnedShips()
    {
        var allShips = stationShipsSelector.GetShipList();

        var toMove = new List<Transform>();
        
        foreach (var stationShips in allShips)
        {
            if (playerShipList.Contains(stationShips.GetComponent<HangarShipData>().GetShip().ShipID))
            {
                toMove.Add(stationShips);
            }
        }

        foreach (var stationShips in toMove)
        {
            playerShipsSelector.AddShip(stationShips);

            stationShips.transform.SetParent(playerShipsSelector.transform);// set parent
            stationShips.transform.position = playerShipsSelector.transform.position;// transform position 

            allShips.Remove(stationShips);//displayer

            selectedShip = 0;
            
            if (stationShips.GetComponent<HangarShipData>().GetShip().ShipID == player.GetComponent<PlayerStats>().GetShipData().ShipID)
            {
                stationShips.transform.gameObject.SetActive(true);
            }
            else
            {
                stationShips.transform.gameObject.SetActive(false);
            }
        }
    }
    public void BuyStation()
    {
        if (station.GetStationData().StationPrice <= player.GetComponent<PlayerCurrencies>().GetCreditsCurrency())// station price needed
        {
            player.GetComponent<PlayerCurrencies>().AddStation(station.GetStationID());// add station id
            player.GetComponent<PlayerCurrencies>().SetCreditsCurrency(-(station.GetStationData().StationPrice));
            station.SetPlayerStation(true);//set bool
        }
        else
        {
            print("No money!!");
        }
    }

    public void ShipBuyQuestTrigger()
    {
        if (player.GetComponent<PlayerCurrencies>().GetShipList().Count < 1)
        {
            EventSystem.RaisOnTrigger();
        }
        
    }

    public void CheckOwnedStations()
    {
        if (playerStationList.Contains(station.GetStationID()))
        {
            station.SetPlayerStation(true);//set bool
            //foreach (var ship in stationShipsSelector.GetShipList())
            //{
            //    ship.GetComponent<HangarShipData>().GetShip().Price =
            //        (int)(ship.GetComponent<HangarShipData>().GetShip().Price * 0.5f);
            //}
        }
    }
}


