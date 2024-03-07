using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Scene", menuName = "ScriptableObjects/Scene")]
public class ScriptableScene : ScriptableObject
{
    public int SceneID;
    public string SceneName;
    public Material Skybox;
}
