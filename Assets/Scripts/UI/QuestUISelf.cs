using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUISelf : MonoBehaviour
{
    TextMeshProUGUI QuestName;
    TextMeshProUGUI QuestGoal;
    TextMeshProUGUI QuestReward;
    private ScriptableQuest quest;
    private int NecQuestCount;
    private int CurrentQuestCount;
    [SerializeField] private bool isDescription = true;
    [SerializeField] private bool isQuestRewards = true;
    [SerializeField] private bool isQuestName = false;
    [SerializeField] private bool isQuestStage = false;
    [SerializeField] private Transform QuestAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        if (isDescription)
        {
            QuestManager.Instance.QuestUIDescription = this;
        }
        else if (isQuestRewards)
        {
            QuestManager.Instance.QuestUIRewards = this;
        }
        else if (isQuestName)
        {
            QuestManager.Instance.QuestUIName = this;
        }
        else
        {
            QuestManager.Instance.QuestUIGoal = this;
        }
        if (true)
        {
            //UpdateUI();
        }
        //UpdateUI();
    }

    public void UpdateUI()
    {
        quest = QuestManager.Instance.GetActiveQuest();
        if (isDescription)
        {
            QuestName = gameObject.GetComponent<TextMeshProUGUI>();
            QuestName.text = quest.Information.Description;
        }
        else if (isQuestRewards)
        {
            if (quest.Reward.RewardItem != null)
            {
                QuestReward = gameObject.GetComponent<TextMeshProUGUI>();
                QuestReward.text = "Quest Rewards:" + "\n" + "1. Money:" + quest.Reward.Money.ToString() + "\n" +
                                   "2. Handler EXP:" + quest.Reward.HandlerXP.ToString() + "\n" +
                                   "3. Pirate EXP:" + quest.Reward.PirateXP.ToString() +
                                   "\n" + "4. Item:" + quest.Reward.RewardItem.name;//null error
            }
            else
            {
                QuestReward = gameObject.GetComponent<TextMeshProUGUI>();
                QuestReward.text = "Quest Rewards:" + "\n" + "1. Money:" + quest.Reward.Money.ToString() + "\n" +
                                   "2. Handler EXP:" + quest.Reward.HandlerXP.ToString() + "\n" +
                                   "3. Pirate EXP:" + quest.Reward.PirateXP.ToString();
                                   /*+
                                   "\n" + "4. Item: None";// + quest.Reward.RewardItem.name;//null error
                                   */
            }
        }
        else if (isQuestName)
        {
            TextMeshProUGUI QuestName = gameObject.GetComponent<TextMeshProUGUI>();
            QuestName.text = quest.Information.QuestName;
        }
        else
        {
            QuestGoal = gameObject.GetComponent<TextMeshProUGUI>();
            QuestGoal.text = quest.GetCurrentQuestGoal().Description;   
            NecQuestCount = quest.GetCurrentQuestGoal().RequiredAmount;
            if (NecQuestCount > 1)
            {
                CurrentQuestCount = quest.GetCurrentQuestGoal().CurrentAmount;
                //QuestGoal.text = CurrentQuestCount.ToString() + "/" + NecQuestCount.ToString();
                QuestAmount.GetComponent<TextMeshProUGUI>().text = CurrentQuestCount.ToString() + "/" + NecQuestCount.ToString();
                //QuestAmount.GetComponent<TextMeshProUGUI>().text = "hallo";
            }
            else
            {
                QuestAmount.GetComponent<TextMeshProUGUI>().text = "";
            }

        }
    }
}
