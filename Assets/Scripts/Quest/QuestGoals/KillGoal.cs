using System;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : QuestGoal
{
    [SerializeField]
    private ScriptableAIGroup AIListObject;
    public Factions EnemyFaction;
    private int AIGroupID;

    private void Awake()
    {
        QuestTypes = EnumQuestTypes.KILL;
    }

    public override EnumQuestTypes GetQuestType()
    {
        return QuestTypes;
    }


    public override string GetDescription()
    {
        return $"Kill {RequiredAmount} Of the Enemy faction";
    }

    private void OnEnable()
    {
        EventSystem.OnKill += OnKill;
    }

    public override void InitializeStage()
    {
        SpawnAI();
    }

    private void OnDisable()
    {
        EventSystem.OnKill -= OnKill;
    }

    private void SpawnAI()
    {
        AIGroupID = AIManager.Instance.CreateSpawnGroup("Encounter1", "firstQuest", AIListObject.GetAiSpawnData());
        AIManager.Instance.SpawnAIGroup(AIGroupID);
    }

    private void OnKill(Factions killedFaction)
    {
        if (killedFaction == EnemyFaction)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
