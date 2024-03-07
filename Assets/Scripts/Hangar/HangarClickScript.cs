using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HangarClickScript : MonoBehaviour
{
    [SerializeField] private Station station;
    
    private CutSceneManager cutSceneManager;
    private DisplayerScript displayerScript;
    
    [SerializeField] private ShipsSelector stationShipsSelector;
    [SerializeField] private ShipsSelector playerShipsSelector;

    [SerializeField] private Canvas handlerUI;

    [SerializeField] private ShipHangarDataUI shipHangarDataUI;
    [SerializeField] private GameObject questUI;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button buyButton2;
    [SerializeField] private Button selectButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button backButton2;
    
    [SerializeField] private GameObject garage;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject shipHandler;
    [SerializeField] private GameObject missions;
    [SerializeField] private GameObject bridge;

    private List<Transform> stationShipsSelectorList;
    private List<Transform> playerShipsSelectorList;

    private bool shipCSHasPlayed;
    private bool handlerCSHasPlayed;
    private bool shipHandlerCSHasPlayed;
    private bool missionCSHasPlayed;
    private bool bridgeCSHasPlayed;

    private bool cutsceneHasPlayed;
    
    private bool shipDataPanelIsActive = false;
    private bool handlerUiIsActive = false;
    private bool questUiIsActive = false;

    void Start()
    {
        cutSceneManager = CutSceneManager.Instance;
        cutSceneManager.AssignCamera(); 
        
        stationShipsSelectorList = stationShipsSelector.GetShipList();
        playerShipsSelectorList = playerShipsSelector.GetShipList();
        displayerScript = GetComponent<DisplayerScript>();

        Invoke("FixCam", 0.7f);
    }

    void FixedUpdate()
    {
        
        if (!cutsceneHasPlayed)
        {
            GoToShip();
            GoToHandler();
            GoToShipHandler();
            GoToMissions();
            GoToBridge();
        }
        
    }
    
    public void GoToShip()
    {
        int layerMask = 1 << 9;
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            garage.SetActive(true);
            if (Input.GetMouseButtonDown(0) && !shipCSHasPlayed)
            {
                shipCSHasPlayed = true;
                cutsceneHasPlayed = true;
                StartTimeline(cutSceneManager.GetCutScene(1));
                displayerScript.SetSelectorList(playerShipsSelectorList);
                shipHangarDataUI.SetShipSelector(playerShipsSelector);
                displayerScript.SetBoolPlayerSelector(true);
                garage.SetActive(false);
                Invoke("ActivateDataPanel", 8);
            }
        }
        else
        {
            garage.SetActive(false);
        }
        
    }
    public void GoToHandler()
    {
        int layerMask = 1 << 10;
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            shop.SetActive(true);
            if (Input.GetKey(KeyCode.Mouse0) && !handlerCSHasPlayed)
            {
                handlerCSHasPlayed = true;
                cutsceneHasPlayed = true;
                StartTimeline(cutSceneManager.GetCutScene(2));
                shop.SetActive(false);
                backButton2.gameObject.SetActive(true);
                Invoke("ActivateHandlerUI", 7);
            }
        }
        else
        {
            shop.SetActive(false);
        }
    }
    public void GoToShipHandler()
    {
        int layerMask = 1 << 12;
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            shipHandler.SetActive(true);
            if (Input.GetKey(KeyCode.Mouse0) && !shipHandlerCSHasPlayed)
            {
                shipHandlerCSHasPlayed = true;
                cutsceneHasPlayed = true;
                StartTimeline(cutSceneManager.GetCutScene(4));
                displayerScript.SetSelectorList(stationShipsSelectorList);
                shipHangarDataUI.SetShipSelector(stationShipsSelector);
                shipHandler.SetActive(false);
                Invoke("ActivateDataPanel", 8);
            }
        }
        else
        {
            shipHandler.SetActive(false);
        }
    }
    public void GoToMissions()
    {
        int layerMask = 1 << 11;
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            missions.SetActive(true);
            if (Input.GetKey(KeyCode.Mouse0) && !missionCSHasPlayed)
            {
                missionCSHasPlayed = true;
                cutsceneHasPlayed = true;
                missions.SetActive(false);
                StartTimeline(cutSceneManager.GetCutScene(3));
                Invoke("ActivateQuestUI",5f);
            }
        }
        else
        {
            missions.SetActive(false);
        }
    }
    public void GoToBridge()
    {
        int layerMask = 1 << 13;
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
       
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            bridge.SetActive(true);
            if (Input.GetKey(KeyCode.Mouse0) && !missionCSHasPlayed)
            {
                bridgeCSHasPlayed = true;
                cutsceneHasPlayed = true;
                bridge.SetActive(false);
                
                StartTimeline(cutSceneManager.GetCutScene(5));
                
                //timer needed
                Invoke("ActivateButtons", 7);
            }
        }
        else
        {
            bridge.SetActive(false);
        }
    }

    public void StartTimeline(PlayableDirector director)
    {
        director.Play();
    }

    public void Back()
    {
        
        if (shipCSHasPlayed)
        {
            shipCSHasPlayed = false;
            StartTimeline(cutSceneManager.GetCutScene(7));
            
            displayerScript.SetBoolPlayerSelector(false);
            shipHangarDataUI.transform.parent.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            selectButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            Invoke("SetCutsceneHasPlayed", 5);
        }

        if (handlerCSHasPlayed)
        {
            handlerCSHasPlayed = false;

            StartTimeline(cutSceneManager.GetCutScene(6));
            handlerUI.gameObject.SetActive(false);
            handlerUiIsActive = false;
            backButton2 .gameObject.SetActive(false);
            Invoke("SetCutsceneHasPlayed", 4);
        }

        if (shipHandlerCSHasPlayed)
        {
            shipHandlerCSHasPlayed = false;
            StartTimeline(cutSceneManager.GetCutScene(9));

            shipHangarDataUI.transform.parent.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
            previousButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            Invoke("SetCutsceneHasPlayed", 6);
        }

        if (missionCSHasPlayed)
        {
            missionCSHasPlayed = false;
            StartTimeline(cutSceneManager.GetCutScene(8));
            questUI.gameObject.SetActive(false);
            questUiIsActive=false;
            Invoke("SetCutsceneHasPlayed", 4);
        }
        if (bridgeCSHasPlayed)
        {
            bridgeCSHasPlayed = false;
            StartTimeline(cutSceneManager.GetCutScene(10));
            buyButton2.gameObject.SetActive(false);
            backButton2.gameObject.SetActive(false);
            Invoke("SetCutsceneHasPlayed", 5);
        }
        
    }

    public void Return()
    {
        PlayerStats.Instance.GetComponent<PlayerController>().ChangeShipLookState(false);
        TradingManager.Instance.SetCurrentLandedStationID(-1);
        SceneLoaderManager.Instance.ChangeOutOfHangarScene(SceneLoaderManager.Instance.SceneData.CurrentScene.SceneIndex, SceneLoaderManager.Instance.SceneData.PrevScene.SceneIndex);
    }

    public void ActivateQuestUI()
    {
        if (!questUI.gameObject.activeInHierarchy)
        {
            questUI.gameObject.SetActive(true);
            questUiIsActive= true;
        }
    }
    public void ActivateHandlerUI()
    {
        if (!handlerUI.gameObject.activeInHierarchy)
        {
            handlerUI.gameObject.SetActive(true);
            handlerUiIsActive = true;
            backButton.gameObject.SetActive(true);
        }
    }

    public void ActivateDataPanel()
    {
        if (!shipHangarDataUI.gameObject.activeInHierarchy)
        {
            shipHangarDataUI.transform.parent.gameObject.SetActive(true);
            shipDataPanelIsActive = true;
                
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
            if (shipCSHasPlayed)
            {
                selectButton.gameObject.SetActive(true);
            }
            else if (shipHandlerCSHasPlayed)
            {
                buyButton.gameObject.SetActive(true);
            }
            backButton.gameObject.SetActive(true);
            shipHangarDataUI.UpdateShipDataUI();
            shipHangarDataUI.UpdateShipPriceUI(station.GetStaionOwner());
        }
    }
    // invoke methods
    private void SetCutsceneHasPlayed()
    {
        cutsceneHasPlayed = false;
    }

    private void ActivateButtons()
    {
        buyButton2.gameObject.SetActive(true);
        backButton2.gameObject.SetActive(true);
    }
    
    //to delete
    /*private void FixCam()
    {
        Camera.main.GetComponent<Transform>().position = new Vector3(-217.5f, 13.5f, 113.75f);
        Camera.main.GetComponent<Transform>().rotation = new Quaternion(0,0,0,0);
    }*/
}
