using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
//using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private PlayerHUD playerHUD;
    [SerializeField] private bool lockShipControlls = false;

    private PlayerInventory playerInventory;

    private int shipID;
    private PlayerStats playerStats;
    
    [SerializeField] private GameObject bulletPrefab;
    private Weapon[] weaponSlots;

    private bool isDestroyed = false;

    private float activeMainThrusterSpeed;
    private float activeSideThrusterSpeed;
    private float activeUpDownThrusterSpeed;

    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;

    private Transform selectedTarget;

    private FloatingMarker floatingMarker;

    private bool ShipSoundisPlaying = false;
    
    //landing
    private bool isCloseToStation;
    private PlayableDirector landingCS;
    private Station station;

    void Start()
    {
        InitializeScreenData();
        InitializeShipComponents();
        InitializeWeapons();

        InitializeWeaponHUD();
        
        if (playerHUD != null)
        {
            landingCS = CutSceneManager.Instance.GetCutScene(0);
        }
        CutSceneManager.Instance.AssignAnimators();

        playerInventory = gameObject.GetComponent<PlayerInventory>();
        floatingMarker = Camera.main.GetComponent<FloatingMarker>();
    }

    void Update()
    {
        Targeting();
        Interact();
    }


    void FixedUpdate()
    {
        if (!lockShipControlls)
        {
            Navigation();
            PrimaryAttack();
        }
    }

    private void Navigation()
    {
        Vector3 OriginalPosistion = transform.position;
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), playerStats.GetRollAcceleration() * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * playerStats.GetYawPitchTorque() * Time.deltaTime, mouseDistance.x * playerStats.GetYawPitchTorque() * Time.deltaTime, rollInput * playerStats.GetRollTorque() * Time.deltaTime, Space.Self);

        activeMainThrusterSpeed = Mathf.Lerp(activeMainThrusterSpeed, Input.GetAxisRaw("Vertical") * playerStats.GetMainThrusterSpeed(), playerStats.GetMainThrusterAcceleration() * Time.deltaTime);
        activeSideThrusterSpeed = Mathf.Lerp(activeSideThrusterSpeed, Input.GetAxisRaw("Horizontal") * playerStats.GetSideThrusterSpeed(), playerStats.GetSideThrusterAcceleration() * Time.deltaTime);
        activeUpDownThrusterSpeed = Mathf.Lerp(activeUpDownThrusterSpeed, Input.GetAxisRaw("UpDown") * playerStats.GetUpDownThrusterSpeed(), playerStats.GetUpDownThrusterAcceleration() * Time.deltaTime);

        transform.position += transform.forward * activeMainThrusterSpeed * Time.deltaTime;
        transform.position += (transform.right * activeSideThrusterSpeed * Time.deltaTime) + (transform.up * activeUpDownThrusterSpeed * Time.deltaTime);

        
        if (transform.position != OriginalPosistion && transform.position.sqrMagnitude != OriginalPosistion.sqrMagnitude + 5)
        {
            PlayShipSound(true);
        }
        else
        {
            PlayShipSound(false);
        }
    }

    private void PlayShipSound(bool play)
    {
        if (play && !ShipSoundisPlaying)
        {
            ShipSoundisPlaying = true;
            transform.GetComponent<AudioSource>().Play();
        }
        else if (!play && ShipSoundisPlaying)
        {
            ShipSoundisPlaying = false;
            transform.GetComponent<AudioSource>().Stop();
        }
        
    }

    private void PrimaryAttack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            foreach (var weapon in weaponSlots)
            {
                weapon.Shoot(shipID, true);
            }
        }
        
    }

    private void Targeting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                // add interact policie
                if (hit.transform.GetComponent<Marker>() != null)
                {
                    if (selectedTarget != hit.transform)
                    {
                        selectedTarget = hit.transform;
                    }

                    selectedTarget.GetComponent<Marker>().InitializeMarkerData();
                    if (floatingMarker == null)
                    {
                        floatingMarker = Camera.main.GetComponent<FloatingMarker>();
                    }
                    floatingMarker.SetMarkerTarget(selectedTarget.GetComponent<Marker>().GetMarkerData());
                    
                    if (selectedTarget.CompareTag("StationEntry"))
                    {
                        station = selectedTarget.parent.GetComponent<Station>();
                        TradingManager.Instance.SetCurrentLandedStationID(station.GetStationID());
                        playerHUD.GetInteractText().gameObject.SetActive(true);selectedTarget = hit.transform;
                    }
                }
                else
                {
                    selectedTarget = null;
                    playerHUD.GetInteractText().gameObject.SetActive(false);
                }
            }
            else
            {
                selectedTarget = null;
                playerHUD.GetInteractText().gameObject.SetActive(false);
            }
            if (floatingMarker != null && selectedTarget == null)
            {
                floatingMarker.RemoveMarkerTarget();
            }
        }
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F) && selectedTarget != null)
        {
            if (selectedTarget.tag == "Loot")
            {
                for (int i = 0; i < selectedTarget.GetComponent<LootContainer>().GetLootFromList().Count; i++)
                {
                    playerInventory.SetItemToPlayerInventoryItemsList(selectedTarget.GetComponent<LootContainer>().GetLootFromList()[i].Item, selectedTarget.GetComponent<LootContainer>().GetLootFromList()[i].Amount);
                }
                selectedTarget.GetComponent<LootContainer>().RemoveContainer();
                selectedTarget = null;
            }
            else if (selectedTarget.tag == "StationEntry")
            {
                //StartTimeline(landingCS); //  future updates
                Invoke("Landing", 2);
            }
            else if (selectedTarget.tag == "Gateway")
            {
                selectedTarget.GetComponent<Gateway>().Warp();
            }
            
        }
    }

    public void ChangeDestroyedState(bool state)
    {
        isDestroyed = state;
    }

    public Vector3 GetPlayerVelocity()
    {
        Vector3 velocity = transform.forward * activeMainThrusterSpeed + transform.right * activeSideThrusterSpeed + transform.up * activeUpDownThrusterSpeed;
        return velocity;
    }

    public void ChangeShipLookState(bool state)
    {
        lockShipControlls = state;
    }

    private void InitializeScreenData()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void InitializeShipComponents()
    {
        shipID = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetInstanceID();
        playerStats = gameObject.GetComponent<PlayerStats>();
    }


    private void InitializeWeapons()
    {
        weaponSlots = new Weapon[transform.GetChild(0).GetChild(0).GetChild(0).childCount];
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            //Ship-Container/ShipGraphics/ShipModel/WeaponSlots/WeaponSlot/Weapon
            //print(transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i));
            if (transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).childCount == 1)
            {
                weaponSlots[i] = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).GetChild(0).GetComponent<Weapon>();
            }
            else
            {
                Debug.LogError($"More then 1 or 0 weapon found in slot {i}. Count weapons in slot: {transform.GetChild(0).GetChild(0).GetChild(0).GetChild(i).childCount}");
            }
        }
    }

    private void InitializeWeaponHUD()
    {
        if (PlayerHUD.Instance != null)
        {
            playerHUD = PlayerHUD.Instance;
            
            playerHUD.SetWeaponsSlotsUI(weaponSlots);
            playerHUD.InitializeEvents();
        }
    }

    private void DetachPlayer()
    {
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    public void LeaveWarp()
    {
        DetachPlayer();
        gameObject.GetComponent<PlayerController>().enabled = true;
        Transform gateway = GatewayManager.Instance.GetGateway(GatewayManager.Instance.GetCurrentUsingGatewayID()).transform.GetChild(0);
        transform.position = gateway.position;
        transform.rotation = gateway.rotation;

        floatingMarker = Camera.main.GetComponent<FloatingMarker>();
        InitializeWeaponHUD();
        gameObject.GetComponent<MapObject>().ActivateIcon();
        /*
        if (QuestManager.Instance.GetActiveQuest().GetCurrentQuestGoal().GetQuestType() == EnumQuestTypes.MOVETO)
        {
            SetQuestMarker();
        }
        */
    }

    private void SetQuestMarker()
    {
        QuestManager.Instance.GetActiveQuest().GetCurrentQuestGoal().SetQuestMarker();
    }

    public void LeaveHangar()
    {
        gameObject.GetComponent<PlayerController>().enabled = true;
        floatingMarker = Camera.main.GetComponent<FloatingMarker>();
        playerStats.ResetToBaseValues();
        InitializeWeaponHUD();
        gameObject.GetComponent<MapObject>().ActivateIcon();
    }

    public Weapon[] GetWeaponSlotsData()
    {
        InitializeWeapons();
        return weaponSlots;
    }

    public void SetWeapons(ScriptableWeapon[] weaponsScriptable)
    {
        for(int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlots[i].InitializeWeaponData(weaponsScriptable[i]);
        }
    }
    
    public void StartTimeline(PlayableDirector director)
    {
        director.Play();
    }

    public void Landing()
    {
        TradingManager.Instance.SetCurrentLandedStationID(station);
        ChangeShipLookState(true);
        SceneLoaderManager.Instance.ChangeScene(SceneLoaderManager.Instance.SceneData.CurrentScene.SceneIndex, 5);
    }
}
