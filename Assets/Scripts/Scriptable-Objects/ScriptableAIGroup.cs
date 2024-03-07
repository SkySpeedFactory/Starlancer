using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AI Group", menuName = "ScriptableObjects/AIGroup")]
public class ScriptableAIGroup : ScriptableObject
{
    [SerializeField]
    List<AISpawnData> AISpawnList;

    public List<AISpawnData> GetAiSpawnData()
    {
        return AISpawnList;
    }
}
