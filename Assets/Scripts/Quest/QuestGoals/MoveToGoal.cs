using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Timeline;

public class MoveToGoal : QuestGoal
{

    //private GameObject MoveToGoalObject;
    [SerializeField] private int SceneBuildIndex = 2;
    [SerializeField] ScriptableMarker markerData;
    [SerializeField] private Vector3 position;
    private Marker MoveToGoalObject;

    private void Awake()
    {
        QuestTypes = EnumQuestTypes.MOVETO;

    }

    public override EnumQuestTypes GetQuestType()
    {
        return QuestTypes;
    }
    public override string GetDescription()
    {
        return $"Move here quest";
    }


    private void OnEnable()
    {
        
    }

    public override void InitializeStage()
    {
        if (IsActiveStage)
        {
            EventSystem.OnArrival += OnArrival;
            SceneLoaderManager.OnNewSceneLoaded += OnNewSceneLoaded;
            SetQuestMarker();
        }
    }

    private void OnNewSceneLoaded(int newSceneID)
    {
        if (!SceneLoaderManager.Instance.IsInWarp)
        {
            SetQuestMarker();
        }
    }

    private void SetMoveToGoal()
    {
        if (SceneBuildIndex == SceneLoaderManager.Instance.GetCurrentSceneIndex())
        {
            GameObject ParentMarker = GameObject.Find("QuestMarkerParent");
            GameObject MoveToGoalGameObject = Instantiate(new GameObject("QuestMarker"), ParentMarker.transform);
            MoveToGoalGameObject.transform.localPosition = position;
            MoveToGoalGameObject.AddComponent<Marker>();
            this.MoveToGoalObject = MoveToGoalGameObject.GetComponent<Marker>();
            Marker marker = MoveToGoalObject;
            marker.AddMarkerData(markerData);
            marker.SetActiveMarker();
            marker.MinGoalDistance = 20;
            marker.isQuestMarker = true;
            marker.QuestGoal = this;
        }
    }

    public override void SetQuestMarker()
    {
        if (SceneBuildIndex == SceneLoaderManager.Instance.GetCurrentSceneIndex())
        {
            SetMoveToGoal();
        }
        else
        {
            GameObject WarpGate = FindGateway(SceneBuildIndex);
            Marker WarpMarker = WarpGate.GetComponent<Marker>();
            WarpMarker.AddMarkerData(markerData);
            WarpMarker.InitializeMarkerData();
            WarpMarker.SetActiveMarker();
        }
    }

    private void OnDisable()
    {
        EventSystem.OnArrival -= OnArrival;
    }

    private GameObject FindGateway(int sceneID)
    {
        List<GameObject> warpGate = GameObject.FindGameObjectsWithTag("Gateway").ToList();
        GameObject targetGate = warpGate.Find(tg => tg.GetComponent<Gateway>().GetNextSceneID() == sceneID);
        return targetGate;
    }

    public void OnArrival()
    {
        if (SceneBuildIndex == SceneLoaderManager.Instance.GetCurrentSceneIndex())
        {
            MoveToGoalObject.DisableMarker();
            CurrentAmount++;
            Evaluate();
        }
    }

    public IEnumerator WaitForTest()
    {

        yield return new WaitForSeconds(5);
    }

}
