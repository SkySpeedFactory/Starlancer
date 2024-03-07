using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    [SerializeField]
    public readonly string Version = "0.1";
    private PlayerData playerData;
    private SceneData sceneData;
    private QuestData questData;

    public SaveData(PlayerStats playerStats, SceneData sceneData, QuestData questData)
    {
        playerData = new PlayerData();
        this.sceneData = sceneData;
        this.questData = questData;
    }

    public SaveData(PlayerStats playerStats, SceneData sceneData)
    {
        playerData = new PlayerData();
        this.sceneData = sceneData;
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public SceneData GetSceneData()
    {
        return sceneData;
    }

    public QuestData GetQuestData()
    {
        return questData;
    }
}
