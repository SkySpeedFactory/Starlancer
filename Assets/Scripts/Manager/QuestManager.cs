using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager _instance;

    public static QuestManager Instance { get { return _instance; } }

    [Serializable]
    public struct QuestStruct
    {
        public ScriptableQuest QuestObject;
        public int QuestID;
        public int ActiveQuestStage;
        public int QuestIndex;
    }
    [SerializeField]
    public List<QuestStruct> QuestList = new List<QuestStruct>();
    private int activeQuestIndex;
    public int activeQuestStage;
    public int ActiveQuestID = 0;
    public QuestStruct activeQuest;
    [SerializeField] public QuestUISelf QuestUIDescription;
    [SerializeField] public QuestUISelf QuestUIGoal;
    [SerializeField] public QuestUISelf QuestUIName;
    [SerializeField] public QuestUISelf QuestUIRewards;


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
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (ActiveQuestID != 0)
        {
            for (int i = 0; i < QuestList.Count; i++)
            {
                
                QuestStruct temp = QuestList[i];
                temp.QuestID = QuestList[i].QuestObject.Information.QuestID;
                temp.ActiveQuestStage = QuestList[i].ActiveQuestStage;
                temp.QuestObject = QuestList[i].QuestObject;
                temp.QuestIndex = i;
                QuestList[i] = temp;
            }
        }
        else if (!DataManager.LoadSaveGame)
        {
            //StartQuest(1);
        }
        
        //StartQuest(ActiveQuestID);
    }


    public void StartQuest(int questID)
    {
        if (questID != 0)
        {
            ActiveQuestID = questID;
            activeQuest = QuestList.Find(q => q.QuestObject.Information.QuestID == ActiveQuestID);
            activeQuest.ActiveQuestStage = activeQuestStage;
            if (activeQuest.ActiveQuestStage == 0)
            {
                activeQuest.ActiveQuestStage = 1;
            }
            activeQuest.QuestObject.Information.currentQuestStage = activeQuest.ActiveQuestStage;
            activeQuestIndex = activeQuest.QuestIndex;
            activeQuest.QuestObject.Initialize(activeQuest.ActiveQuestStage);
            updateUI();
        }
    }

    public void updateUI()
    {
        if (QuestUIDescription != null)
        {
            QuestUIDescription.UpdateUI();
            QuestUIGoal.UpdateUI();
            QuestUIRewards.UpdateUI();
            
            activeQuest.ActiveQuestStage = activeQuest.QuestObject.Information.currentQuestStage;
            QuestStruct temp = QuestList[activeQuestIndex];
            temp.ActiveQuestStage = activeQuest.ActiveQuestStage;
            activeQuestStage = activeQuest.ActiveQuestStage;
            QuestList[activeQuestIndex] = temp;
        }
    }

    public ScriptableQuest GetActiveQuest()
    {
        return activeQuest.QuestObject;
    }
}
