using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] private ScriptableMarker markerData;
    private WaypointItem waypointItem;
    private int markerDistance;
    public bool isQuestMarker = false;
    public int MinGoalDistance = 20;
    public MoveToGoal QuestGoal;
    private FloatingMarker floatingMarker;

    public Marker(ScriptableMarker markerData)
    {
        this.markerData = markerData;
        waypointItem.Target = this.transform;
        waypointItem.TargetName = markerData.MarkerWaypointInfos.TargetName;
        waypointItem.MarkerOffset = markerData.MarkerWaypointInfos.MarkerOffset;
        waypointItem.MarkerColor = markerData.MarkerWaypointInfos.MarkerColor;
    }

    private void FixedUpdate()
    {
        if (isQuestMarker)
        {
            markerDistance = (int)Vector3.Distance(this.transform.position, Camera.main.transform.position);
            
            if (markerDistance < MinGoalDistance)
            {
                isQuestMarker = false;
                EventSystem.RaisOnArrival();
            }
        }
    }

    public void SetActiveMarker()
    {
        FloatingMarker floatingMarker = Camera.main.GetComponent<FloatingMarker>();
        floatingMarker.SetMarkerTarget(waypointItem);
    }
    
    public void InitializeMarkerData()
    {
        waypointItem = new WaypointItem();
        waypointItem.Target = this.transform;
        waypointItem.TargetName = markerData.MarkerWaypointInfos.TargetName;
        waypointItem.MarkerOffset = markerData.MarkerWaypointInfos.MarkerOffset;
        waypointItem.MarkerColor = markerData.MarkerWaypointInfos.MarkerColor;
    }

    public void AddMarkerData(ScriptableMarker markerData)
    {
        this.markerData = markerData;
        waypointItem.Target = this.transform;
        waypointItem.TargetName = markerData.MarkerWaypointInfos.TargetName;
        waypointItem.MarkerOffset = markerData.MarkerWaypointInfos.MarkerOffset;
        waypointItem.MarkerColor = markerData.MarkerWaypointInfos.MarkerColor;
    }

    public void DisableMarker()
    {
        Camera.main.GetComponent<FloatingMarker>().RemoveMarkerTarget();
    }

    public WaypointItem GetMarkerData()
    {
        return waypointItem;
    }
}
