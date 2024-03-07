using UnityEngine;

public class GatherGoal : QuestGoal
{

    private void Awake()
    {
        QuestTypes = EnumQuestTypes.GATHER;
    }

    public override EnumQuestTypes GetQuestType()
    {
        return QuestTypes;
    }
    public override string GetDescription()
    {
        return $"Move here ";
    }

    private void OnEnable()
    {
        //EventSystem.OnGather += OnGather;
    }

    private void OnDisable()
    {
        //EventSystem.OnGather -= OnGather;
    }

    private void OnGather(ScriptableItem GatheredItem)
    {

    }
}
