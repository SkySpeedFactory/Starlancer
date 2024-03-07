using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnPoint : MonoBehaviour
{
    [SerializeField]
    private ScriptableAIGroup spawnData;
    [SerializeField]
    private string spawnName = "default Name";

    [SerializeField]
    bool spawnOnStart = false;

    private List<Transform> PatrolPoints = new List<Transform>();

    private void Start()
    {
        SetPatrolPoints();
        if (spawnOnStart)
        {
            int groupID;
            groupID = AIManager.Instance.CreateSpawnGroup(spawnName, spawnData.GetAiSpawnData(), transform, PatrolPoints);
            AIManager.Instance.SpawnAIGroup(groupID);
        }
    }

    private void SetPatrolPoints()
    {
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                PatrolPoints.Add(transform.GetChild(i).transform);
            }
            PatrolPoints.Add(transform);
        }
    }
}
