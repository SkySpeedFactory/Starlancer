using UnityEngine;

public class GoalTest : MonoBehaviour
{
    public QuestGoal quest;
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
           EventSystem.RaiseOnFollow(collision);
        }

        if (true)
        {
            quest.Fail();
        }
    }   
}
