using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private Transform goalsContent;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text pirateXpText;
    [SerializeField] private TMP_Text handlerXpText;
    [SerializeField] private TMP_Text moneyText;

    public void Initialize(ScriptableQuest quest)
    {
        titleText.text = quest.Information.QuestName;
        descriptionText.text = quest.Information.Description;

        pirateXpText.text = "Pirate XP:" + quest.Reward.PirateXP.ToString();
        handlerXpText.text = "Handler XP:" + quest.Reward.HandlerXP.ToString();
        moneyText.text ="Money:" + quest.Reward.Money.ToString();


        GameObject goalObj = goalsContent.GetChild(0).gameObject;


        foreach (var goal in quest.Goals)
            {
                GameObject textObj = goalObj.transform.Find("Text").gameObject;
                GameObject countObj = goalObj.transform.Find("Count").gameObject;
                GameObject skipObj = goalObj.transform.Find("Skip").gameObject;

                textObj.GetComponent<TMP_Text>().text = goal.GetDescription();

                if (goal.Completed)
                {
                    countObj.SetActive(false);
                    skipObj.SetActive(false);
                    goalObj.transform.Find("Done").gameObject.SetActive(true);
                }
                else
                {
                    countObj.GetComponent<TMP_Text>().text = goal.CurrentAmount + "/" + goal.RequiredAmount;

                    skipObj.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        goal.Skip();

                        countObj.SetActive(false);
                        skipObj.SetActive(false);
                        goalObj.transform.Find("Done").gameObject.SetActive(true);
                    });
                }
            }

    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < goalsContent.childCount; i++)
        {
            Destroy(goalsContent.GetChild(i).gameObject);
        }
    }
}
