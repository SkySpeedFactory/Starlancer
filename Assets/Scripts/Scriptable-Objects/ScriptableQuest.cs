using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObjects/Quest")]
public class ScriptableQuest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string QuestName;
        public Sprite Icon;
        [TextArea(0, 20)] public string Description;
        public int currentQuestStage;
        public bool isActiveQuest;
        public int QuestID;
        //public GameObject QuestItem;
        // -> Requirements for Moral system goes here <-
    }

    [Header("Quest Info")] public Info Information;
    
    [System.Serializable]
    public struct Stat
    {
        public int Money;
        public int PirateXP;
        public int HandlerXP;
        public GameObject RewardItem;
    }
    
    [Header("Quest Reward")] public Stat Reward =  new Stat{Money =  10, PirateXP = 10, HandlerXP = -10, RewardItem = null};// null needs to be replaced

    public bool Completed { get; private set; }
    public QuestCompletedEvent QuestCompleted;

    public List<QuestGoal> Goals;
    private QuestGoal currentQuestGoal;
    
    public void Initialize(int activeQuestStage)
    {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();
        currentQuestGoal = Goals.Find(g => g.QuestStage == activeQuestStage);
        Information.currentQuestStage = currentQuestGoal.QuestStage;
        startStage(currentQuestGoal);
    }

    private void startStage(QuestGoal goal)
    {
        goal.Initialize();
        goal.InitializeStage();
        goal.GetQuestType();
        goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
    }

    private void CheckGoals()
    {
        if (Information.currentQuestStage != Goals.Count)
        {
            Information.currentQuestStage++;
            AdvanceCurrentQuest();
            return;
        }
        Completed = Goals.All(g => g.Completed);
        if (Completed)
        {
            //give Rewards
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
    }

    public void AdvanceCurrentQuest()
    {
        QuestGoal questGoal = Goals[Information.currentQuestStage -1];
        currentQuestGoal = questGoal;
        startStage(questGoal);
    }

    public QuestGoal GetCurrentQuestGoal()
    {
        return currentQuestGoal;
    }
}

