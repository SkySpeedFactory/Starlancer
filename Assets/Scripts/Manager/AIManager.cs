using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public struct AIGroup
    {
        public string GroupName;
        public int GroupID;
        public string QuestName;
        public List<AISpawnData> AISpawnDataList;
        public Transform SpawnPoint;
        public List<Transform> PatrolPoints;
    }

    private static AIManager _instance;
    public static AIManager Instance { get { return _instance; } }

    private int lastSceneIndex;
    private int currentSceneIndex;

    private List<GameObject> AiObjList = new List<GameObject>();
    public List<AIData> AiDataList = new List<AIData>();

    private List<AIGroup> AIGroups = new List<AIGroup>();

    private int LastAIGroupID = 0;

    private bool isPlayerInventoryChecked;

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
        SceneLoaderManager.OnNewSceneLoaded += OnNewSceneLoaded;

    }

    public void AddAi(GameObject AiObject)
    {
        AiObjList.Add(AiObject);
    }

    public void RemoveAI(GameObject AiObject)
    {
        AiObjList.Remove(AiObject);
    }

    public List<AIData> GetAiList()
    {
        foreach (GameObject ai in AiObjList)
        {
            AIData data = new AIData(ai.GetComponent<AIStats>());
            AiDataList.Add(data);
        }
        AiObjList.Clear();
        return AiDataList;
    }

    public void OnNewSceneLoaded(int newSceneID)
    {
        if (newSceneID == lastSceneIndex && currentSceneIndex != 0)
        {
            //SaveManager.Instance.SceneChangeLoad(); // For future updates
        }
       
        lastSceneIndex = currentSceneIndex;
        currentSceneIndex = newSceneID;
    }

    public int CreateSpawnGroup(string groupName, string questName, List<AISpawnData> spawnData)
    {
        AIGroup aIGroup = new AIGroup();
        aIGroup.AISpawnDataList = spawnData;
        aIGroup.GroupName = groupName;
        aIGroup.QuestName = questName;
        aIGroup.GroupID = ++LastAIGroupID;
        
        AIGroups.Add(aIGroup);
        return aIGroup.GroupID;
    }

    public int CreateSpawnGroup(string groupName,  List<AISpawnData> spawnData, Transform spawnPoint, List<Transform> patrolPoints)
    {
        AIGroup aIGroup = new AIGroup();
        aIGroup.AISpawnDataList = spawnData;
        aIGroup.GroupName = groupName;
        aIGroup.SpawnPoint = spawnPoint;
        aIGroup.PatrolPoints = patrolPoints;
        aIGroup.GroupID = ++LastAIGroupID;
        AIGroups.Add(aIGroup);
        
        return aIGroup.GroupID;
    }

    public void SpawnAIGroup(int groupID)
    {
        AIGroup aIGroup = AIGroups.Find(group => group.GroupID == groupID);
        
        foreach (AISpawnData aISpawnData in aIGroup.AISpawnDataList)
        {
            switch (aISpawnData.SpawnType)
            {
                case EnumGroupSpawn.CIRCULAR:
                    List<Vector3> SpawnLocations = calculateCircularSpawn(aIGroup.SpawnPoint.position, aISpawnData.SpawnAmout);
                    foreach (Vector3 locations in SpawnLocations)
                    {
                        spawnAI(locations, aISpawnData, aIGroup);
                    }
                    break;
                case EnumGroupSpawn.TRIANGLEFORM:
                    break;
                default:
                    break;
            }
        }
    }

    private void spawnAI(Vector3 spawnPostion, AISpawnData spawnData, AIGroup aiGroup)
    {
        GameObject newAI = ObjectPoolManager.Instance.GetPooledAIShips(spawnData.Faction);

        newAI.transform.position = spawnPostion;
        newAI.transform.rotation = aiGroup.SpawnPoint.rotation;

        Vector3 offset = spawnPostion - aiGroup.SpawnPoint.position;

        AIStats stats = newAI.GetComponent<AIStats>();
        stats.SetAIFaction(spawnData.Faction);
        stats.SetCanCheckPlayerInventory(spawnData.CanCheckPlayerInventory);
        stats.SetWayPoints(aiGroup.PatrolPoints, offset);
        newAI.SetActive(true);
        newAI.GetComponent<MapObject>().ActivateIcon();
        AddAi(newAI);
    }

    private List<Vector3> calculateCircularSpawn(Vector3 centerSpawnPoint, int spawnAmount)
    {
        List<Vector3> SpawnLocations = new List<Vector3>();
        Vector3 pos;
        Vector3 center = centerSpawnPoint;
        float radius = 40f;
        float ang = 360 / spawnAmount;
        for (int i = 0; i < spawnAmount; i++)
        {
            pos.x = center.x + radius * Mathf.Sin(ang * i * Mathf.Deg2Rad);
            pos.y = center.y + radius * Mathf.Cos(ang * i * Mathf.Deg2Rad);
            pos.z = center.z;
            SpawnLocations.Add(pos);
        }
        return SpawnLocations;
    }

    private IEnumerator CheckInventoryCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        isPlayerInventoryChecked = false;

        foreach (var ai in AiObjList)
        {
            if (ai.GetComponent<AIStats>().GetCanCheckPlayerInventory())
            {
                ai.GetComponent<PoliceAIBehaviour>().SetHasCheckedPlayer(isPlayerInventoryChecked);
            }
        }
    }

    public void PlayerInventoryCheck()
    {
        isPlayerInventoryChecked = true;
        foreach (var ai in AiObjList)
        {
            if (ai.GetComponent<AIStats>().GetCanCheckPlayerInventory())
            {
                ai.GetComponent<PoliceAIBehaviour>().SetHasCheckedPlayer(true);
            }
        }
        StartCoroutine(CheckInventoryCooldown(300f));
    }

    public bool GetIsPlayerInventoryChecked()
    {
        return isPlayerInventoryChecked;
    }
}
