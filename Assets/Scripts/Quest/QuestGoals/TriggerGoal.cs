using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGoal : QuestGoal
{
    [SerializeField]
    private GameObject toBuyItem;

    private void Awake()
    {
        QuestTypes = EnumQuestTypes.TRIGGER;
    }

    public override EnumQuestTypes GetQuestType()
    {
        return QuestTypes;
    }

    public override string GetDescription()
    {
        return $"Buy these {toBuyItem.name}";
    }

    private void OnEnable()
    {
        EventSystem.OnTrigger += OnTrigger;
    }

    private void OnDisable()
    {
        EventSystem.OnTrigger -= OnTrigger;
    }


    private void OnTrigger()
    {
        Evaluate();
    }
}
