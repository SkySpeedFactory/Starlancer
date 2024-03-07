using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPool : MonoBehaviour
{
    private static InventoryPool _instance;
    public static InventoryPool Instance { get { return _instance; } }

    [SerializeField]
    private List<ScriptableItem> itemPool;

    [SerializeField]
    private List<ScriptableSmallShip> smallShipPool;

    [SerializeField]
    private List<ScriptableShield> shieldPool;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<ScriptableItem> getItems()
    {
        return itemPool;
    }

    public ScriptableItem getItem(string ItemName)
    {
        ScriptableItem returnItem = itemPool.First(i => i.name == ItemName);
        return returnItem;
    }

    public ScriptableWeapon getWeapon(string WeaponName)
    {
        List<ScriptableItem> tempList = itemPool.FindAll(w => w.GetType() == typeof(ScriptableWeapon));
        ScriptableWeapon returnItem = tempList.First(w => w.ItemName == WeaponName) as ScriptableWeapon;
        return returnItem;
        
    }

    public ScriptableSmallShip getShip(int shipID)
    {
        ScriptableSmallShip returnItem = smallShipPool.Find(s => s.ShipID == shipID);
        return returnItem;
    }

    public ScriptableShield getShield(int ShieldID)
    {
        ScriptableShield returnItem = shieldPool.Find(s => s.ShieldID == ShieldID);
        return returnItem;
    }
}
