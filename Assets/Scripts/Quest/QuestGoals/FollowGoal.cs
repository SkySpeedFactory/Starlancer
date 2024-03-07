using UnityEngine;

public class FollowGoal : QuestGoal
{
    [SerializeField] ScriptableSmallShip target;
    
    //NOT IMPLEMENTED
    //public Enum QuestType;
    //public override Enum GetQuestType(EnumQuestTypes questTypes)
    //{
    //    return QuestType = questTypes;
    //}

    public override string GetDescription()
    {
        return $"Follow this target {target.ShipName}";
    }
    
    private void OnEnable()
    {
        EventSystem.OnFollow += OnFollow;
        EventSystem.OnFail += OnFail;
    }

    private void OnDisable()
    {
        EventSystem.OnFollow -= OnFollow;
        EventSystem.OnFail -= OnFail;
    }

    private void OnFollow(Collider collision)
    {
        CurrentAmount++;
            Evaluate();
    }

    private void OnFail()
    {
        Fail();
    }
}

