using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    [System.Serializable]
    public struct QuestStruct
    {
        public int QuestID;
        public int ActiveQuestStage;
    }

    public DataType DataType;
    private QuestStruct questStruct;

    public void SetData()
    {
        questStruct.QuestID = QuestManager.Instance.ActiveQuestID;
        questStruct.ActiveQuestStage = QuestManager.Instance.activeQuest.ActiveQuestStage;
    }

    public void LoadData()
    {
        QuestManager.Instance.ActiveQuestID = questStruct.QuestID;
        QuestManager.Instance.activeQuestStage = questStruct.ActiveQuestStage;
        
        QuestManager.Instance.StartQuest(questStruct.QuestID);
    }
}
