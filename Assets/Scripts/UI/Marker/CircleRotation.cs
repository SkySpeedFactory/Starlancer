using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRotation : MonoBehaviour
{
    [SerializeField] private float roationSpeed = 25f;
    [SerializeField] private bool clockwise = true;

    void Start()
    {
        if (!clockwise)
        {
            roationSpeed = roationSpeed * -1;
        }
    }
    void FixedUpdate()
    {
        transform.Rotate(0, 0, roationSpeed * Time.deltaTime);
    }
}
