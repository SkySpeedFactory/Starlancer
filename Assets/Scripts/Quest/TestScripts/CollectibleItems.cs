using UnityEngine;

public class CollectibleItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log("Triggered");
        
        if (collision.tag == "Player")
        {
            EventSystem.RaiseOnGather(collision);
        }

        if (collision.tag == "Enemy")
        {
            
            EventSystem.RaiseOnFail();
        }
    }   
}
