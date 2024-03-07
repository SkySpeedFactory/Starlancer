using UnityEngine;


public class QuestTester : MonoBehaviour
{
    public int kills;
    public GameObject manager;
    public QuestUI managerScript;
    private void Awake()
    {
        managerScript = manager.GetComponent<QuestUI>();
    }

    public void KillTest()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
            kills++;
        }
        
    }
}
