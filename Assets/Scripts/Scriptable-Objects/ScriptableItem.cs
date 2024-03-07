using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Item")]
public class ScriptableItem : ScriptableObject
{
    public string ItemName;
    [TextArea(0, 20)] public string Description;
    public Sprite Image;

    public int Price;
    public bool IsIllegal;
}
