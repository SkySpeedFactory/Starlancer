using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponentInParent<PlayerStats>() != null)
            {
                other.transform.GetComponentInParent<PlayerStats>().SetDamage(10000);
            }
        }
    }
}
