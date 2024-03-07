using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questContent;
    [SerializeField] private GameObject questWindowUI;

    public List<ScriptableQuest> AvailableQuests;

    private void Start()
    {
        foreach (var quest in AvailableQuests)
        {
            quest.Initialize(1);
            quest.QuestCompleted.AddListener(OnQuestCompleted);
            
            GameObject questObj = Instantiate(questPrefab, questContent);
            questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
            questObj.transform.Find("Text").GetComponent<TMP_Text>().text = quest.Information.QuestName;
            
            questObj.GetComponent<Button>().onClick.AddListener(delegate
            {
                questWindowUI.SetActive(true);
                questWindowUI.GetComponent<QuestWindow>().Initialize(quest);
                QuestManager.Instance.StartQuest(quest.Information.QuestID);
            });
        }
    }
    private void OnQuestCompleted(ScriptableQuest quest)
    {
        questContent.GetChild(AvailableQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }

    //
    //public void FollowTarget(Collider collision)
    //{
    //    EventSystem.RaiseOnFollow(collision);
    //}
    //
    //public void KillTargets(int kill)
    //{
    //    EventSystem.RaiseOnKill(Factions.Police);
    //}
    //
    //public void GatherItems(Collider collision)
    //{
    //    EventSystem.RaiseOnGather(collision);
    //}
}
