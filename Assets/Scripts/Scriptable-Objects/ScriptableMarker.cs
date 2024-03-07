using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Marker", menuName = "ScriptableObjects/Marker")]
public class ScriptableMarker : ScriptableObject
{
    public WaypointItem MarkerWaypointInfos;
}
