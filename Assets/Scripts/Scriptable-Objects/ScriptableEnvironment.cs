using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Environemnt", menuName = "ScriptableObjects/Environemnt")]
public class ScriptableEnvironment : ScriptableObject
{
    public string EnvironmentName;
    public float Health;
}
