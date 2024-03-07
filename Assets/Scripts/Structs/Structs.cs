using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct LootDropData
{
    public ScriptableItem Item;

    [Range(0, 100)]
    public int SpawnChance;

    [Range(0, 100)]
    public int DropAmountChance;
}

[Serializable]
public struct LootItem
{
    public ScriptableItem Item;
    public int Amount;
}

[Serializable]
public struct WaypointItem
{
    public Image MarkerImage;
    public Transform Target;
    public string TargetName;
    public Vector3 MarkerOffset;
    public Color32 MarkerColor;
}

[Serializable]
public struct AISpawnData
{
    public GameObject ShipPrefab;
    public ScriptableSmallShip ShipObject;
    public ScriptableShield ShipShieldObject;
    public Factions Faction;
    public bool CanCheckPlayerInventory;
    public ScriptableSpawnPoint SpawnPoint;
    public int SpawnAmout;
    public EnumGroupSpawn SpawnType;
}

