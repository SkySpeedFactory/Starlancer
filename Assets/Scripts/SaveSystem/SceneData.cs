using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneData
{
    [System.Serializable]
    public struct SceneStruct
    {
        public int SceneIndex;
        public int CurrentTradingID;
        public List<AIData> AiDataList;
    }

    public DataType dataType;

    public SceneStruct CurrentScene = new SceneStruct();
    public SceneStruct PrevScene = new SceneStruct();
    public List<SceneStruct> SceneStructList = new List<SceneStruct>();

    public SceneData(int sceneIndex)
    {
        CurrentScene.SceneIndex = sceneIndex;
        CurrentScene.CurrentTradingID = TradingManager.Instance.GetCurrentLandedStationID(); 
        dataType = DataType.SceneInfo;
    }

    public void SaveCurrentAiData()
    {
        PrevScene.AiDataList = AIManager.Instance.GetAiList();
    }

    public void LoadAiData()
    {

    }

    public void SetData()
    {
        CurrentScene.CurrentTradingID = TradingManager.Instance.GetCurrentLandedStationID();
    }
}
