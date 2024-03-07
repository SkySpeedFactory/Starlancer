using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class QuestGoal : ScriptableObject
{
    protected EnumQuestTypes QuestTypes;
    
    [SerializeField] public string Description;
    public int CurrentAmount { get; protected set; } = 0;
    public int RequiredAmount = 1;

    public bool IsActiveStage = false;
    public int QuestStage;
    public bool Completed { get; protected set; }
    [HideInInspector] public UnityEvent GoalCompleted;

    public virtual EnumQuestTypes GetQuestType()
    {
        return QuestTypes;
    }

    public virtual string GetDescription()
    {
        return Description;
    }

    public virtual void Initialize()
    {
        Completed = false;
        IsActiveStage = true;
        CurrentAmount = 0;//for testing
        GoalCompleted = new UnityEvent();
        QuestManager.Instance.updateUI();
    }

    public virtual void InitializeStage()
    {
    }

    public virtual void SetQuestMarker()
    {
    }

    protected void Evaluate()
    {
        QuestManager.Instance.updateUI();
        if (CurrentAmount >= RequiredAmount)
        {
            
            switch (QuestTypes)
            {
                case EnumQuestTypes.FETCH:
                    Complete();
                    break;
                case EnumQuestTypes.KILL:
                    Complete();
                    break;
                case EnumQuestTypes.GATHER:
                    Complete();
                    break;
                case EnumQuestTypes.PROTECT:
                    Complete();
                    break;
                case EnumQuestTypes.FOLLOW:
                    Complete();
                    break;
                case EnumQuestTypes.EXPLORE:
                    Complete();
                    break;
                case EnumQuestTypes.COLLECT:
                    Complete();
                    break;
                case EnumQuestTypes.BUY:
                    Complete();
                    break;
                case EnumQuestTypes.TRIGGER:
                    Complete();
                    break;
                case EnumQuestTypes.MOVETO:
                    Complete();
                    break;
                default:
                    
                    break;
            }
        }
    }

    private void Complete()
    {
        Completed = true;
        IsActiveStage = false;
        GoalCompleted.Invoke();
        GoalCompleted.RemoveAllListeners();
    }

    public void Skip()
    {
        Complete();
        //if you want to skip the quest, add sanctions
    }

    public void Fail()
    {
        Complete();
        Debug.Log("failed");
        //if you fail the quest, add consequences
    }
}
