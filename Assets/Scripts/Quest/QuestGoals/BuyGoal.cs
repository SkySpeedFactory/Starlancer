using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGoal : QuestGoal
{
    [SerializeField]
    private ScriptableObject toBuyItem;

    private void Awake()
    {
        QuestTypes = EnumQuestTypes.BUY;
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
        EventSystem.OnBuy += OnBuy;
    }

    private void OnDisable()
    {
        EventSystem.OnBuy -= OnBuy;
    }

    private void OnBuy(ScriptableObject BoughtObject, int Amount)
    {
        if (BoughtObject.name == toBuyItem.name)
        {
            CurrentAmount += Amount;
            Evaluate();
        }
    }
}
