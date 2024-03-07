using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Station", menuName = "ScriptableObjects/Station")]
public class ScriptableStation : ScriptableObject
{
    public int StationID;
    public string StationName;
    public bool HasLandingBay;
    public bool CanBuyIllegal;
    public List<LootItem> ItemsList;
    // want item list => price * 1.5
    public List<LootItem> SearchForItemsList;
    // want item list with rare items => price * 3
    public List<LootItem> SearchForRareItemsList;
    //[Range(0, 100)] public int MaxAmountOfEachItemInStore;
    public int StationPrice;
}
