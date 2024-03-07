using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShipHangarDataUI : MonoBehaviour
{
    [SerializeField] private ShipsSelector shipsSelector;

    [SerializeField]private ScriptableSmallShip selectedShip;
    [SerializeField]private ScriptableShield shieldData;

    [SerializeField] private TMP_Text selectedShipName;
    [SerializeField] private TMP_Text selectedShipType;
    [SerializeField] private TMP_Text selectedShipPrice;
    
    [SerializeField] private Slider maxHealthSlider;
    [SerializeField] private Slider shieldAbsorbCapacitySlider;
    [SerializeField] private Slider shieldRechargeRateSlider;
    [SerializeField] private Slider yawPitchTorqueSlider;
    [SerializeField] private Slider rollTorqueSlider;
    [SerializeField] private Slider maxThrusterSpeedSlider;
    [SerializeField] private Slider mainThrusterAccelerationSlider;
    [SerializeField] private Slider rollAccelerationSlider;

    private int shipPrice;
    
    public void SetShipSelector(ShipsSelector shipSelector)
    {
        shipsSelector = shipSelector;
    }
    public void UpdateShipDataUI()
    {
        selectedShip = shipsSelector.GetSelectedShip();
        if (selectedShip == null)
        {
            selectedShip = PlayerStats.Instance.GetShipData();
            shieldData = PlayerStats.Instance.GetShieldData();
        }
        
        selectedShipName.text = selectedShip.ShipName;
        selectedShipType.text = selectedShip.ShipType.ToString();
        //selectedShipPrice.text = $"Price : {shipPrice.ToString()}";
        shieldAbsorbCapacitySlider.value = shieldData.AbsorbCapacity;
        shieldRechargeRateSlider.value = shieldData.RechargeRate;
        maxHealthSlider.value = selectedShip.MaxHealth;
        yawPitchTorqueSlider.value = selectedShip.YawPitchTorque;
        rollTorqueSlider.value = selectedShip.RollTorque;
        maxThrusterSpeedSlider.value = selectedShip.Thruster;//speed value comes here
        mainThrusterAccelerationSlider.value = selectedShip.MainThrusterAcceleration;
        rollAccelerationSlider.value = selectedShip.RollAcceleration;
        
    }

    public void UpdateShipPriceUI(bool isStationOwner)
    {
        shipPrice = shipsSelector.GetSelectedShip().Price;
        if (isStationOwner)
        {
            selectedShipPrice.text = $"Price : {(shipPrice *0.5).ToString()}";
        }
        else
        {
            selectedShipPrice.text = $"Price : {shipPrice.ToString()}";
        }
    }
}
