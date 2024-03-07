using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.Shift;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private static PlayerHUD _instance;
    public static PlayerHUD Instance { get { return _instance; } }


    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Slider moralitySlider;

    private PlayerController playerController;
    private PlayerStats playerStats;


    [SerializeField] private TMP_Text healthTextfield;
    [SerializeField] private TMP_Text shieldTextfield;
    [SerializeField] private TMP_Text moralityTextfield;
    [SerializeField] private TMP_Text moneyTextfield;
    [SerializeField] private TMP_Text weaponTextfield;
    
    [SerializeField] private TMP_Text entryText; //delete if not needed
    public TMP_Text GetInteractText() => entryText;

    [SerializeField] private Transform weaponSlotsPanel;
    [SerializeField] private Image weaponUISlotPrefab;
    private Image[] weaponSlotsImages;

    [SerializeField] private GameObject QuestPanel;
    [SerializeField] private GameObject playerSubMenuesContainer;
    [SerializeField] private GameObject shipInfoSubmenuConatiner;
    [SerializeField] private GameObject inventorySubmenuConatiner;

    private bool questPanelIsActive;
    private bool subMenuPanelIsActive;

    [SerializeField] private List<TMP_Dropdown> weponsSlotsDropdowns; // get count of weaponslots (prov with 2)

    private Weapon[] hudWeaponSlots;
    
    //[SerializeField] private Slider testSlider;
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

    private void Start()
    {
        playerStats = PlayerStats.Instance;
        playerController = playerStats.GetComponent<PlayerController>();
        UpdateHealthTextField(playerStats.GetShipData().MaxHealth, playerStats.GetCurrentHealth());
        UpdateShieldTextField(playerStats.GetShieldData().AbsorbCapacity, playerStats.GetShieldStrength());
        MorialityManager.Instance.SetHUDValue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeInventorySubmenuStatus(true);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            QuestPanelButton();
        }
    }

    public void ChangeInventorySubmenuStatus(bool affectParent = false)
    {
        shipInfoSubmenuConatiner.SetActive(false);
        if (affectParent)
        {
            if (!playerSubMenuesContainer.activeInHierarchy)
            {

                inventorySubmenuConatiner.GetComponent<PlayerInventoryUI>().UpdateInventoryUI();
                inventorySubmenuConatiner.SetActive(!inventorySubmenuConatiner.activeInHierarchy);
            }
            playerSubMenuesContainer.SetActive(!playerSubMenuesContainer.activeInHierarchy);
            playerController.ChangeShipLookState(playerSubMenuesContainer.activeInHierarchy);
        }
        else
        {
            inventorySubmenuConatiner.GetComponent<PlayerInventoryUI>().UpdateInventoryUI();
            inventorySubmenuConatiner.SetActive(!inventorySubmenuConatiner.activeInHierarchy);
        }
        SetDropdowns(PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>().GetAllWeaponsInPlayerInventory());
    }

    public void ChangeShipInfoSubmenuStatus()
    {
        inventorySubmenuConatiner.SetActive(false);
        shipInfoSubmenuConatiner.SetActive(!shipInfoSubmenuConatiner.activeInHierarchy);
    }

    public void UpdateHealthTextField(float maxHealth, float currentHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        //background
        healthSlider.gameObject.transform.GetChild(1).GetComponent<Image>().color =
            Color.Lerp(new Color(0.3f,0,0),new Color(0,0.3f,0), currentHealth/maxHealth);
        //fill
        healthSlider.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color =
            Color.Lerp(Color.red, Color.green, currentHealth/maxHealth);
        //text
        healthTextfield.text = $"Health: {currentHealth:N0}/{maxHealth}";
        healthTextfield.color = Color.Lerp(Color.red, new Color(0,0.3f,0), currentHealth/maxHealth);
    }

    public void UpdateShieldTextField(float maxShield, float currentShield)
    {
        shieldSlider.maxValue = maxShield;
        shieldSlider.value = currentShield;
        //background
        shieldSlider.gameObject.transform.GetChild(1).GetComponent<Image>().color =
            Color.Lerp(Color.yellow, Color.blue, currentShield/maxShield);
        //fill
        shieldSlider.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color =
            Color.Lerp(Color.yellow, Color.cyan, currentShield/maxShield);
        //text
        shieldTextfield.text = $"Shield: {currentShield:N0}/{maxShield}";
        shieldTextfield.color = Color.Lerp(new Color(0,0.5f,0.5f), new Color(0,0,0.5f), currentShield/maxShield);
    }
    public void UpdateMoralityTextField(float maxMorality, float currentMorality, string moralityLevel)
    {
        moralitySlider.maxValue = maxMorality;
        moralitySlider.value = currentMorality;
        //background
        moralitySlider.gameObject.transform.GetChild(1).GetComponent<Image>().color = 
            Color.Lerp(Color.red, Color.green, currentMorality/maxMorality);
        //glow
        moralitySlider.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().color =
            Color.Lerp(Color.red, new Color(0,0.3f,0), currentMorality/maxMorality);
        //fill
        moralitySlider.gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().color =
            Color.Lerp(Color.red, Color.green, currentMorality/maxMorality);
        //text
        moralityTextfield.text = $"{moralityLevel}";
        moralityTextfield.color = Color.Lerp(new Color(0.3f,0,0), new Color(0,0.3f,0), currentMorality/maxMorality);
    }

    public void UpdateMoneyTextField(float currentMoney)
    {
        moneyTextfield.text = $"{currentMoney}$";
    }

    public void SetWeaponsSlotsUI(Weapon[] weaponSlots)
    {
        hudWeaponSlots = new Weapon[weaponSlots.Length];
        hudWeaponSlots = weaponSlots;

        weaponSlotsImages = new Image[weaponSlots.Length];
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            weaponSlotsImages[i] = Instantiate(weaponUISlotPrefab, weaponSlotsPanel.position, weaponSlotsPanel.rotation, weaponSlotsPanel);
            if (weaponSlots[i] != null)
            {
                SetWeaponInfos(weaponSlotsImages[i], weaponSlots[i]);
            }
            else
            {
                weaponSlotsImages[i].sprite = null;
            }
        }
    }

    public void SetDropdowns(List<LootItem> weapons)
    {
        for (int i = 0; i < weponsSlotsDropdowns.Count; i++)
        {
            weponsSlotsDropdowns[i].options.Clear();
            for (int j = 0; j < weapons.Count; j++)
            {
                weponsSlotsDropdowns[i].options.Add(new TMP_Dropdown.OptionData() { text = weapons[j].Item.ItemName });
                if (weapons[j].Item.ItemName == hudWeaponSlots[i].GetWeaponName())
                {
                    weponsSlotsDropdowns[i].value = j; // set current weapon
                }
            }
        }
    }

    public void OnWeaponChange(TMP_Dropdown dropdown, int id)
    {
        ScriptableWeapon newWeapon = GetScriptableWeaponFromInventory(dropdown.options[dropdown.value].text);
        ScriptableWeapon oldWeapon = GetScriptableWeaponFromInventory(hudWeaponSlots[id].GetWeaponName());
        if (oldWeapon == null)
        {
            oldWeapon = hudWeaponSlots[id].GetWeaponCurrentDataInSlot();
        }
        //print($"add old weapon to inventroy: {oldWeapon}");
        //print($"remove new old weapon to inventroy: {newWeapon}");
        if (newWeapon != null && newWeapon != oldWeapon)
        {
            hudWeaponSlots[id].InitializeWeaponData(newWeapon);
            PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>().SetItemToPlayerInventoryItemsList(newWeapon, -1); // remove new weapon form inventory
            PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>().SetItemToPlayerInventoryItemsList(oldWeapon, +1); // add old weapon to inventory
            SetWeaponInfos(weaponSlotsImages[id], hudWeaponSlots[id]); // update hud
            
            weaponTextfield.text = hudWeaponSlots[id].GetWeaponName();//needs improvements
        }

    }

    private void SetWeaponInfos(Image weaponUI, Weapon weapon)
    {
        weaponUI.sprite = weapon.GetWeaponIcon();
        weaponUI.transform.GetChild(0).GetComponent<TMP_Text>().text = weapon.GetWeaponName();
    }

    private ScriptableWeapon GetScriptableWeaponFromInventory(string weaponName)
    {
        List<LootItem> tmpList = PlayerStats.Instance.gameObject.GetComponent<PlayerInventory>().GetAllWeaponsInPlayerInventory();
        for (int i = 0; i < tmpList.Count; i++)
        {
            if (tmpList[i].Item.ItemName == weaponName)
            {
                return (ScriptableWeapon)tmpList[i].Item;
            }
        }
        return null;
    }

    public void InitializeEvents()
    {
        for (int i = 0; i < weponsSlotsDropdowns.Count; i++)
        {
            int tmpIndex = i;
            weponsSlotsDropdowns[tmpIndex].onValueChanged.AddListener(delegate { OnWeaponChange(weponsSlotsDropdowns[tmpIndex], tmpIndex); });
        }
    }

    public void QuestPanelButton()
    {
        if (!questPanelIsActive)
        {
            QuestPanel.SetActive(true);
            questPanelIsActive = true;
        }
        else
        {
            QuestPanel.SetActive(false);
            questPanelIsActive = false;
        }
    }

    public void SubMenuPanelButton()
    {
        if (!subMenuPanelIsActive)
        {
            playerSubMenuesContainer.SetActive(true);
            subMenuPanelIsActive = true;
        }
        else
        {
            playerSubMenuesContainer.SetActive(false);
            subMenuPanelIsActive = false;
        }
        
    }
}
